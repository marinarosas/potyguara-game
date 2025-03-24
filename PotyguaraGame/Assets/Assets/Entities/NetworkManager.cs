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
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using System.IO;
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
    private int modeGame;

    private ConcurrentQueue<int> potycoins = new ConcurrentQueue<int>();
    private ConcurrentQueue<int> pointingNormalMode = new ConcurrentQueue<int>();
    private ConcurrentQueue<int> pointingZombieMode = new ConcurrentQueue<int>();
    private ConcurrentQueue<string> skin = new ConcurrentQueue<string>();
    private List<string> tickets;

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

    internal void CheckTickets(Transform content)
    {
        if (tickets.Count != 0)
        {
            for (int ii = 0; ii < tickets.Count; ii++)
            {
                if (tickets[ii] != "null")
                    FindFirstObjectByType<MenuShowController>().UnclockShow(tickets[ii]);
            }
        }
        tickets.Clear();
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
    }
    private void Start()
    {
        ConnectToServer();
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

        if (!SteamManager.Initialized) // Verifica se a Steam está inicializada
            return;

        SendConnectionSignal(SteamFriends.GetPersonaName());
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
                    this.playerId = response.parameters["playerID"];
                    pointingNormalMode.Enqueue(int.Parse(response.parameters["pointingNormalMode"]));
                    pointingZombieMode.Enqueue(int.Parse(response.parameters["pointingZombieMode"]));
                    potycoins.Enqueue(int.Parse(response.parameters["potycoins"]));

                    string skinS = response.parameters["skin"];
                    string[] list = skinS.Split('|');
                    Debug.Log("Primeiro item: " + int.Parse(list[0]));
                    Debug.Log(list.Length);
                    if (int.Parse(list[0]) != -1)
                    {
                        isTheFirstAcess = false;
                        modeGame = int.Parse(response.parameters["mode"]);
                        skin.Enqueue(response.parameters["skin"]);
                    }
                    break;
                case "Ticket":
                    tickets.Add(response.parameters["ticket"]);
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

    internal void SendConnectionSignal(string nickname)
    {
        Action action = new Action()
        {
            type = "Connection",
            actor = nickname,
            parameters = new Dictionary<string, string>(){
                { "serverAddress", serverAddress }
            }
        };

        // Enviar a ação para o servidor
        ws.Send(action.ToJson());
    }

    internal void SendSkin(int gender, int index, int material)
    {
        Action action = new Action()
        {
            type = "UpdateSkin",
            actor = playerId,
            parameters = new Dictionary<string, string>(){
                { "gender", gender.ToString() },
                { "index", index.ToString() },
                { "material", material.ToString() }
            }
        };

        // Enviar a ação para o servidor
        ws.Send(action.ToJson());
    }

    internal void SendPontuacionForte(int totalPoints, int mode)
    {
        if (ws != null)
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
    }

    internal void RequestTickets(string id)
    {
        Action action = new Action()
        {
            type = "Ticket",
            actor = this.playerId,
            parameters = new Dictionary<string, string>()
            {
                { "id", id },
            }
        };

        // solicita a atualização dos tickets para o servidor
        if(ws != null)
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
        if(ws != null)
            ws.Send(action.ToJson());
    }

    internal void DeletePerfil()
    {
        Action action = new Action()
        {
            type = "DeletePerfil",
            actor = this.playerId,
            parameters = new Dictionary<string, string>()
            {

            }
        };
        if (ws != null)
        {
            ws.Send(action.ToJson());
            FindFirstObjectByType<TransitionController>().ExitGame();
        }
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
                GameObject playerObject = GameObject.Find(playerId);


                // Se o jogador não existir, instanciar um novo jogador
                if (playerObject == null) {
                    playerObject = Instantiate(RemotePlayerPrefab) as GameObject;
                    playerObject.name = playerId;
                }

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
                FindFirstObjectByType<PotyPlayerController>().GetPotycoinsOfTheServer(potycoin);
            }

            while (skin.TryDequeue(out string skinString))
            {
                FindFirstObjectByType<TechGuaraController>().SetMode(modeGame == 0 ? true : false);
                string[] list = skinString.Split('|');
                int bodyIndex = int.Parse(list[0]);
                int skinIndex = int.Parse(list[1]);
                int variant = int.Parse(list[2]);

                FindFirstObjectByType<PotyPlayerController>().SetSkin(bodyIndex, skinIndex, variant);
            }

            while (pointingNormalMode.TryDequeue(out int pointingNM))
            {
                FindFirstObjectByType<PotyPlayerController>().SetScoreNormalMode(pointingNM);
            }

            while (pointingZombieMode.TryDequeue(out int pointingZM))
            {
                FindFirstObjectByType<PotyPlayerController>().SetScoreZombieMode(pointingZM);
            }

            if (SceneManager.GetActiveScene().buildIndex == 0)
                FindFirstObjectByType<TransitionController>().UpdateMainMenu(isTheFirstAcess);
        }
    }
}
