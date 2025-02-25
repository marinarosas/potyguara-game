using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using System.Collections.Concurrent;
using Steamworks;

public class NetworkManager : MonoBehaviour
{
    // Prefab do jogador local
    public GameObject LocalPlayerPrefab;

    // Prefab do jogador remoto
    public GameObject RemotePlayerPrefab;

    // Endereço do servidor
    public string serverAddress = "wss://potyws.ffcloud.com.br";

    // WebSocket para comunicação com o servidor
    private WebSocket ws;

    private string rankingZ = "";
    private string rankingB = "";

    public bool isTheFirstAcess = true;
    private bool playerIsConnected = false;
    private ConcurrentQueue<int> potycoins = new ConcurrentQueue<int>();
    private string currentDay;
    private bool ticketsUpdated = false;

    //  Singleton stuff
    private static NetworkManager _instance;

    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<NetworkManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("NetworkManager");
                    _instance = obj.AddComponent<NetworkManager>();
                }
            }
            return _instance;
        }
    }

    // Awake é chamado antes do Start, e é chamado apenas uma vez
    // conecta ao servidor, e mantém a conexão aberta, assim como não destrói o objeto ao mudar de cena
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        ConnectToServer();
    }
    private void Start()
    {
        currentDay= DateTime.Today.DayOfWeek.ToString();
    }

    // Id do jogador local. É definido pelo servidor após a conexão com o evento "Wellcome"
    public string playerId;
    
    // Estado do jogo (posições dos jogadores, etc)
    public GameState gameState;

    // Conecta ao servidor e define os eventos de recebimento de mensagens
    void ConnectToServer() {
        Debug.Log("conectando");
        
        ws = new WebSocket(this.serverAddress);
        ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

        // OnMessage é chamado sempre que uma mensagem é recebida
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received: " + e.Data);
            // Processar a mensagem recebida
            ProcessServerMessage(e.Data);
        };                         

        // Depois de definir os eventos, conectar ao servidor
        ws.Connect();
    }

    public string GetRankingZombieMode()
    {
        return rankingZ;
    }

    public string GetRankingBatalhaMode()
    {
        return rankingB;
    }

    NetworkManager() {
        gameState = new GameState();
    }
    
    /// <summary>
    /// Processa a mensagem recebida do servidor
    /// </summary>
    /// <param name="message"></param>
    private void ProcessServerMessage(string message)
    {
        Debug.Log("Processing message from server");
        try {
            // converter a mensagem recebida(JSON) para um objeto ServerResponse
            var response = JsonConvert.DeserializeObject<ServerResponse>(message);
            Debug.Log("response: " + response);
            Debug.Log("response.type: " + response.type);
            Debug.Log("response.parameters: " + response.parameters);
            Debug.Log("response.gameState: " + response.gameState);

            // verificar o tipo da mensagem recebida
            switch (response.type)
            {
                case "Wellcome":
                    // Se for uma mensagem "Wellcome", o servidor enviou o id do jogador que será usado para
                    // identificar o jogador local. Esse id é gerado pelo servidor.
                    Debug.Log("::: WELCOME RECEIVED" + response.parameters);
                    this.playerId = response.parameters["playerId"];
                    break;
                case "GameState":
                    // Aqui o servidor enviou o estado atual do jogo, com as posições dos jogadores
                    Debug.Log(":: GAMESTATE RECEIVED:: ");
                    Debug.Log(response.gameState);
                    // Note que aqui não estamos atualizando o gameState, mas sim substituindo o objeto
                    // pelo novo estado do jogo. Isso é feito para que o objeto gameState seja sempre o
                    // estado atual do jogo.
                    // Entretanto, isso pode causar problemas se houver alguma referência ao objeto gameState
                    // Então, é importante sempre usar o gameState para acessar o estado do jogo, e não manter
                    // referências ao objeto gameState que pode ser substituído a qualquer momento.
                    // Note também que não estamos trocando a posição dos jogadores, mas sim atualizando
                    // o objeto gameState com as novas posições dos jogadores.          
                    // A mudança de posição dos jogadores é feita no método Update() que irá consultar o
                    // gameState para saber a posição dos jogadores e atualizar a posição dos objetos na cena.
                    gameState = response.gameState;
                    // mostrar o novo estado do jogO
                    if (response.parameters["connection"].Equals("online"))
                        playerIsConnected = true;
                    else
                        playerIsConnected = false;
                    break;
                case "RankingZ":
                    rankingZ = response.parameters["ranking"];
                    Debug.Log("Recebi Zumbi: " + rankingZ);
                    break;
                case "RankingB":
                    Debug.Log("Recebi: " + response.parameters["ranking"]);
                    rankingB = response.parameters["ranking"];
                    break;
                case "Reconnection":
                    this.playerId = response.parameters["playerId"];
                    isTheFirstAcess = false;
                    Debug.Log("Reconnectou!!!");
                    break;
                case "Potycoins":
                    potycoins.Enqueue(int.Parse(response.parameters["potycoins"]));
                    break;
                case "Tickets":
                    ticketsUpdated = true;
                    break;
                default:
                    break;
            }
        } catch (Exception e) {
            Debug.Log("Erro ao deserializar: " + e);
        }
    }

    void OnDestroy()
    {
        if (ws != null)
        {
            ws.Close();
            ws = null;
        }
    }


    /// <summary>
    /// Envia a posição do jogador para o servidor
    /// </summary>
    /// <param name="position"></param>
    internal void SendPosition(Vector3 position)
    {
        // Antes é necessário criar um objeto Action, que contém o tipo de ação, o ator e os parâmetros
        // esses parâmetros são um dicionário que contem a nova posição do jogador.
        // é necessário usar o InvariantCulture para garantir que o ponto seja usado como separador decimal
        Action action = new Action() {
            type = "PositionUpdate",
            actor = this.playerId,
            parameters = new Dictionary<string, string>(){
                {"position_x", position.x.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)},
                {"position_y", position.y.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)},
                {"position_z", position.z.ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)}
            }
        };

        // Enviar a ação para o servidor
        ws.Send(action.ToJson());
    }

    internal void SendPontuacionForte(int totalPoints, int mode)
    {
        if (mode == 0)
        {
            Action action = new Action()
            {
                type = "GameForteZ",
                actor = this.playerId,
                parameters = new Dictionary<string, string>()
            {
                { "nickname", PotyPlayerController.Instance.nickname },
                { "pointing", totalPoints.ToString() }
            }
            };

            // envia a pontuação final no jogo do Forte para o servidor
            ws.Send(action.ToJson());
        }
        else
        {
            Action action = new Action()
            {
                type = "GameForteB",
                actor = this.playerId,
                parameters = new Dictionary<string, string>()
            {
                { "nickname", PotyPlayerController.Instance.nickname },
                { "pointing", totalPoints.ToString() }
            }
            };

            // envia a pontuação final no jogo do Forte para o servidor
            ws.Send(action.ToJson());
        }
    }

    internal void RequestTickets()
    {
        Action action = new Action()
        {
            type = "Tickets",
            actor = this.playerId,
            parameters = new Dictionary<string, string>()
            {
            }
        };

        // solicita a atualização dos tickets para o servidor
        ws.Send(action.ToJson());
    }

    internal void SendUpdateSkin(int skinGender, int skinIndex, int skinMaterial)
    {
        Action action = new Action()
        {
            type = "UpdateSkin",
            actor = this.playerId,
            parameters = new Dictionary<string, string>()
            {
                { "gender", skinGender.ToString()},
                { "index", skinIndex.ToString() },
                { "material", skinMaterial.ToString() }
            }
        };

        // envia a atualização da skin para o servidor
        ws.Send(action.ToJson());
    }

    internal void UpdatePotycoins(int potycoins)
    {
        Action action = new Action()
        {
            type = "UpdatePotycoins",
            actor = this.playerId,
            parameters = new Dictionary<string, string>()
            {
                {"potycoins", potycoins.ToString() },
            }
        };

        // envia a pontuação final no jogo do Forte para o servidor
        ws.Send(action.ToJson());
    }

    internal void DeletePerfil(string playerId)
    {
        Action action = new Action()
        {
            type = "DeletePerfil",
            actor = this.playerId,
            parameters = new Dictionary<string, string>()
            {
            }
        };
        ws.Send(action.ToJson());
        FindFirstObjectByType<TransitionController>().ExitGame();
    }

    /// <summary>
    /// Atualiza a posição dos jogadores na cena, de acordo com o gameState.
    /// Se um jogador não existir na cena, ele é instanciado.
    /// Se um jogador existir na cena, sua posição é atualizada.
    /// </summary>
    void Update() {
        // A cada frame, verificar se precisa instanciar quantos RemotePlayerPrefab e
        // LocalPlayerPrefab forem necessários
        if (gameState != null) {

            // Para cada jogador no gameState verificar se o jogador já existe na cena
            // Se não existir, instanciar um novo jogador
            // Se existir, atualizar a posição do jogador
            foreach (var playerId in gameState.players.Keys) {

                // Se o jogador for o jogador local, não fazer nada
                if (playerId == this.playerId) {
                    Debug.Log(playerId + " SOU EU!");
                    // é o jogador local
                    //TODO: talvez precisa atualiza a minha posição se isso puder acontecer
                    // com alguma ação do servidor.
                    continue;
                }

                // Buscar o jogador na cena pelo playerId
                //GameObject playerObject = GameObject.Find(playerId);


                // Se o jogador não existir, instanciar um novo jogador
                /*if (playerObject == null) {
                    playerObject = Instantiate(LocalPlayerPrefab);
                    playerObject.name = playerId;
                }*/

                // Atualizar a posição do jogador
                // TODO: implementar interpolação de movimento
                /*playerObject.transform.position = new Vector3(
                    gameState.players[playerId].position_x,
                    gameState.players[playerId].position_y,
                    gameState.players[playerId].position_z
                );*/
            }

            if (!rankingZ.Equals("")) {
                FindObjectOfType<RankingController>().UpdateRanking(0);
                rankingZ = "";
            }
            if (!rankingB.Equals(""))
            {
                FindObjectOfType<RankingController>().UpdateRanking(1);
                rankingB = "";
            }

            while (potycoins.TryDequeue(out int potycoin))
            {
                PotyPlayerController.Instance.SetPotycoins(potycoin);
            }

            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                string day = DateTime.Today.DayOfWeek.ToString();
                if (isTheFirstAcess)
                {
                    GameObject canva = GameObject.FindWithTag("MainCamera").transform.GetChild(5).gameObject;
                    Button button = canva.transform.GetChild(5).GetComponent<Button>();

                    if (button != null && canva != null)
                    {
                        canva.SetActive(true);
                        canva.GetComponent<FadeController>().FadeIn();
                        button.onClick.AddListener(() => FindFirstObjectByType<PotyPlayerController>().UpdatePotycoins(50, button, canva));
                        isTheFirstAcess = false;
                    }
                }
                else if (currentDay != day)
                {
                    GameObject canva = GameObject.FindWithTag("MainCamera").transform.GetChild(5).gameObject;
                    Button button = canva.transform.GetChild(5).GetComponent<Button>();

                    if (button != null && canva != null)
                    {
                        canva.SetActive(true);
                        canva.GetComponent<FadeController>().FadeIn();
                        button.onClick.AddListener(() => PotyPlayerController.Instance.UpdatePotycoins(50, button, canva));
                    }
                    currentDay = day;
                }
            }

            if (SceneManager.GetActiveScene().buildIndex == 0)
                FindFirstObjectByType<TransitionController>().UpdateMainMenu(isTheFirstAcess);
        }
    }
}
