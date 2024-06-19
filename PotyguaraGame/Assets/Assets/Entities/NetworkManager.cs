using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public GameObject CanvaWellcome;

    // Prefab do jogador local
    public GameObject LocalPlayerPrefab;

    // Prefab do jogador remoto
    public GameObject RemotePlayerPrefab;

    // Endereço do servidor
    public string serverAddress = "ws://192.168.0.2:9000/";
    // WebSocket para comunicação com o servidor
    private WebSocket ws;

    private string ranking;

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

    // Id do jogador local. É definido pelo servidor após a conexão com o evento "Wellcome"
    public string playerId;
    
    // Estado do jogo (posições dos jogadores, etc)
    public GameState gameState;

    // Conecta ao servidor e define os eventos de recebimento de mensagens
    void ConnectToServer() {
        Debug.Log("conectando");
        
        ws = new WebSocket(this.serverAddress);
        CanvaWellcome.transform.GetComponent<FadeController>().FadeIn();
        CanvaWellcome.transform.GetChild(1).GetComponent<Text>().text = this.serverAddress + "";

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

    public string GetRanking()
    {
        return ranking;
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
                    CanvaWellcome.transform.GetComponent<FadeController>().FadeIn();
                    CanvaWellcome.transform.GetChild(1).GetComponent<Text>().text = "Conectado ao Servidor!!!";
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
                    // // mostrar o novo estado do jogo
                    break;
                case "Ranking":
                    ranking = response.parameters["ranking"];
                    Debug.Log(response.parameters["ranking"]);
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

    internal void SendPontuacionForte(int totalPoints)
    {
        Action action = new Action()
        {
            type = "GameForte",
            actor = this.playerId,
            parameters = new Dictionary<string, string>()
            {
                { "pointing", totalPoints.ToString() }
            }
        };

        // envia a pontuação final no jogo do Forte para o servidor
        ws.Send(action.ToJson());
    }

    /// <summary>
    /// Atualiza a posição dos jogadores na cena, de acordo com o gameState.
    /// Se um jogador não existir na cena, ele é instanciado.
    /// Se um jogador existir na cena, sua posição é atualizada.
    /// </summary>
    void Update() {
        // A cada frame, verificar se precisa instanciar quantos RemotePlayerPrefab e
        // LocalPlayerPrefab forem necessários
        //Debug.Log("Avaliando UPDATE" + gameState.ToString());
        if (gameState != null) {

            // Para cada jogador no gameState verificar se o jogador já existe na cena
            // Se não existir, instanciar um novo jogador
            // Se existir, atualizar a posição do jogador
            foreach (var playerId in gameState.players.Keys) {
                Debug.Log("Avaliando" + playerId);

                // Se o jogador for o jogador local, não fazer nada
                if (playerId == this.playerId) {
                    Debug.Log(playerId + " SOU EU!");
                    // é o jogador local
                    //TODO: talvez precisa atualiza a minha posição se isso puder acontecer
                    // com alguma ação do servidor.
                    continue;
                }


                Debug.Log("Avaliando" + playerId + " - " + gameState.players[playerId]);
                // Buscar o jogador na cena pelo playerId
                GameObject playerObject = GameObject.Find(playerId);


                // Se o jogador não existir, instanciar um novo jogador
                if (playerObject == null) {
                    playerObject = Instantiate(RemotePlayerPrefab) as GameObject;
                    playerObject.name = playerId;
                }

                // Atualizar a posição do jogador
                // TODO: implementar interpolação de movimento
                playerObject.transform.position = new Vector3(
                    gameState.players[playerId].position_x,
                    gameState.players[playerId].position_y,
                    gameState.players[playerId].position_z
                );
            }
        }
    }
}
