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
using Unity.VisualScripting;
using UnityEngine.Analytics;

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
    private int modeTutorial;
    private int modeWeather;

    private ConcurrentQueue<int> potycoins = new ConcurrentQueue<int>();
    private ConcurrentQueue<int> pointingNormalMode = new ConcurrentQueue<int>();
    private ConcurrentQueue<int> pointingZombieMode = new ConcurrentQueue<int>();
    private ConcurrentQueue<string> skin = new ConcurrentQueue<string>();
    private ConcurrentQueue<int> skins = new ConcurrentQueue<int>();
    private ConcurrentQueue<string> tickets = new ConcurrentQueue<string>();
    private int qntTickets = 0;
    private int count = 0;
    public bool isNewDay = false;

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
                    isNewDay = true;
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
                case "Skins":
                    string skinsString = response.parameters["skins"];
                    if (skinsString.Contains("|"))
                    {
                        string[] indexList = skinsString.Split('|');
                        foreach (var index in indexList)
                            skins.Enqueue(int.Parse(index));
                    }
                    else
                        skins.Enqueue(0);

                    Debug.Log(skinsString + "o tamanho da lista: ");
                    break;
                case "Reconnection":
                    this.playerId = response.parameters["playerID"];
                    pointingNormalMode.Enqueue(int.Parse(response.parameters["pointingNormalMode"]));
                    pointingZombieMode.Enqueue(int.Parse(response.parameters["pointingZombieMode"]));
                    potycoins.Enqueue(int.Parse(response.parameters["potycoins"]));

                    if (response.parameters["nDay"] == "true")
                        isNewDay = true;
                    else
                        isNewDay = false;

                    string skinS = response.parameters["skin"];
                    string[] list = skinS.Split('|');
                    if (int.Parse(list[0]) != -1)
                    {
                        isTheFirstAcess = false;
                        modeTutorial = int.Parse(response.parameters["modeTutorial"]);
                        modeWeather = int.Parse(response.parameters["modeWeather"]);
                        skin.Enqueue(response.parameters["skin"]);
                    }
                    break;
                case "Tickets":
                    string ticketsS = response.parameters["tickets"];
                    string[] ticketList = ticketsS.Split('|');
                    qntTickets = ticketList.Length;
                    foreach (var ticket in ticketList)
                        tickets.Enqueue(ticket);
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

    internal void SendModeTutorial(string mode)
    {
        Action action = new Action()
        {
            type = "UpdateModeTutorial",
            actor = playerId,
            parameters = new Dictionary<string, string>(){
                { "mode", mode }
            }
        };

        if (ws != null)
            // Enviar a ação para o servidor
            ws.Send(action.ToJson());
    }

    internal void SendModeWeather(string mode)
    {
        Action action = new Action()
        {
            type = "UpdateModeWeather",
            actor = playerId,
            parameters = new Dictionary<string, string>(){
                { "mode", mode }
            }
        };

        if (ws != null)
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

        if (ws != null)
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

        if (ws != null)
            // Enviar a ação para o servidor
            ws.Send(action.ToJson());
    }

    internal void SendSkin(int index)
    {
        Action action = new Action()
        {
            type = "NewSkin",
            actor = playerId,
            parameters = new Dictionary<string, string>(){
                { "index", index.ToString() }
            }
        };

        if (ws != null)
            // Enviar a ação para o servidor
            ws.Send(action.ToJson());
    }
    internal void RequestSkins()
    {
        Action action = new Action()
        {
            type = "RequestSkins",
            actor = playerId,
            parameters = new Dictionary<string, string>(){
            }
        };

        if (ws != null)
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

            if (ws != null)
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

            if (ws != null)
                // envia a pontuação final no jogo do Forte para o servidor
                ws.Send(action.ToJson());
        }
    }

    internal void RequestTickets()
    {
        Action action = new Action()
        {
            type = "RequestTickets",
            actor = this.playerId,
            parameters = new Dictionary<string, string>()
            {
            }
        };

        // solicita a atualização dos tickets para o servidor
        if(ws != null)
            ws.Send(action.ToJson());
    }

    internal void SendTicket(string id)
    {
        Action action = new Action()
        {
            type = "NewTicket",
            actor = this.playerId,
            parameters = new Dictionary<string, string>()
            {
                { "id", id },
            }
        };

        // solicita a atualização dos tickets para o servidor
        if (ws != null)
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
                FindFirstObjectByType<TechGuaraController>().SetModeOfTheServer(modeTutorial == 0 ? true : false);
                FindFirstObjectByType<DayController>().SetModeOfTheServer(modeWeather == 0 ? true : false);
                string[] list = skinString.Split('|');
                int bodyIndex = int.Parse(list[0]);
                int skinIndex = int.Parse(list[1]);
                int variant = int.Parse(list[2]);

                FindFirstObjectByType<PotyPlayerController>().SetSkin(bodyIndex, skinIndex, variant);
            }

            while (skins.TryDequeue(out int skin))
            {
                FindFirstObjectByType<PotyPlayerController>().AddSkin(skin);
            }

            while (tickets.TryDequeue(out string ticket))
            {
                FindFirstObjectByType<PotyPlayerController>().AddTicket(ticket);
                count++;
                if (count == qntTickets) {
                    FindFirstObjectByType<MenuShowController>().CheckTickets();
                    count = 0;
                }
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
