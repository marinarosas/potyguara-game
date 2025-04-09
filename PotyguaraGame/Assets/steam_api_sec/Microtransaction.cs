// Install http://steamworks.github.io/ to use this script
// This script is just an example but you can use it as you please
using Steamworks;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Concurrent;

public class Microtransaction : MonoBehaviour
{
    [SerializeField] private string baseUrl = "https://potyws.ffcloud.com.br"; // Set this to your API base URL
    [SerializeField] private string appId = "3181940"; // replace with your own appId

    // finish transaction callback
    protected Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

    private int currentOrder = 1000;
    private string currentTransactionId = "";
    private string currentItemId = "";

    private bool _isInPurchaseProcess = false;

    private ConcurrentQueue<int> potycoins = new ConcurrentQueue<int>();
    private ConcurrentQueue<string> skins = new ConcurrentQueue<string>();
    private ConcurrentQueue<int> tickets = new ConcurrentQueue<int>();
    private ConcurrentQueue<int> sessions = new ConcurrentQueue<int>();

    private static Microtransaction _instance;

    public static Microtransaction Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Microtransaction>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("Microtransaction");
                    _instance = obj.AddComponent<Microtransaction>();
                }
            }
            return _instance;
        }
    }

    // Unity Awake function    
    private void Awake() 
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

        // initialize the callback to receive after the purchase
        m_MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(OnMicroTxnAuthorizationResponse); 
        currentOrder += Random.Range(1000000, 100000000);
    }

    public void InitSale(string itemID, string description, string category)
    {
        if (!_isInPurchaseProcess)
        {
            this._isInPurchaseProcess = true;
            StartCoroutine(InitializePurchase(itemID, description, category));
        }
    }

    private void Update()
    {
        while (potycoins.TryDequeue(out int potycoin))
        {
            FindFirstObjectByType<PotyPlayerController>().SetPotycoins(potycoin);
        }

        while (sessions.TryDequeue(out int session))
        {
            FindFirstObjectByType<MeditationRoomController>().AddButton(session);
            NetworkManager.Instance.SendSession(currentItemId);
        }

        while (skins.TryDequeue(out string skinId))
        {
            int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin(skinId);
            if (index != 0)
            {
                FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                NetworkManager.Instance.SendSkin(index);
            }
        }
    }

    // This callback is called when the user confirms the purchase
    // See https://partner.steamgames.com/doc/api/ISteamUser#MicroTxnAuthorizationResponse_t
    private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback) 
    {
        if (pCallback.m_bAuthorized == 1)
        {
            StartCoroutine(FinishPurchase(pCallback.m_ulOrderID.ToString()));
        }
        Debug.Log("[" + MicroTxnAuthorizationResponse_t.k_iCallback + " - MicroTxnAuthorizationResponse] - " + pCallback.m_unAppID + " -- " + pCallback.m_ulOrderID + " -- " + pCallback.m_bAuthorized);
    }

    // To understand how to create products
    // see https://partner.steamgames.com/doc/features/microtransactions/implementation
    public IEnumerator InitializePurchase(string itemID, string description, string category)
    {
        string userId = SteamUser.GetSteamID().ToString();

        WWWForm form = new WWWForm();
        form.AddField("itemId", itemID);
        form.AddField("steamId", userId);
        form.AddField("orderId", currentOrder.ToString());
        form.AddField("itemDescription", description);
        form.AddField("category", category);
        form.AddField("appId", appId);

        currentItemId = itemID;

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + "/InitPurchase", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Full URL: " + baseUrl + "/InitPurchase");
                Debug.LogError("Error initializing purchase: " + www.error);
                Debug.LogError("Response Code: " + www.responseCode);
                Debug.LogError("Response: " + www.downloadHandler.text);
            }
            else
            {
                ApiReturnTransaction ret = JsonUtility.FromJson<ApiReturnTransaction>(www.downloadHandler.text);
                if (!string.IsNullOrEmpty(ret.transid))
                {
                    Debug.Log("Transaction initiated. Id: " + ret.transid);
                    currentTransactionId = ret.transid;
                }
                else if (!string.IsNullOrEmpty(ret.error))
                {
                    Debug.LogError("Error from API: " + ret.error);
                }
            }
        }
    }

    public IEnumerator FinishPurchase(string orderId)
    {
        WWWForm form = new WWWForm();
        form.AddField("orderId", orderId);
        form.AddField("appId", appId);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + "/FinalizePurchase", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error finalizing purchase: " + www.error);
                Debug.LogError("Response Code: " + www.responseCode);
                Debug.LogError("Response: " + www.downloadHandler.text);
            }
            else
            {
                ApiReturn ret = JsonUtility.FromJson<ApiReturn>(www.downloadHandler.text);
                if (ret.success)
                {
                    // after confirmation, give the item to the player
                    if(currentItemId != "")
                    {
                        if (currentItemId.Equals("1001"))
                        {
                            potycoins.Enqueue(100);
                        }
                        else if (currentItemId.Equals("1002"))
                        {
                            potycoins.Enqueue(250);
                        }
                        else if (currentItemId.Equals("1003"))
                        {
                            potycoins.Enqueue(500);
                        }
                        else if (currentItemId.Equals("1004"))
                        {
                            potycoins.Enqueue(1000);
                        }
                        else if (currentItemId.Equals("3002"))
                        {
                            sessions.Enqueue(1);
                        }
                        else if (currentItemId.Equals("3003"))
                        {
                            sessions.Enqueue(2);
                        }
                        else if (currentItemId.Equals("4001"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4002"))
                        {
                            skins.Enqueue("4002");
                        }
                        else if (currentItemId.Equals("4003"))
                        {
                            skins.Enqueue("4003");
                        }
                        else if (currentItemId.Equals("4004"))
                        {
                            skins.Enqueue("4004");
                        }
                        else if (currentItemId.Equals("4005"))
                        {
                            skins.Enqueue("4005");
                        }
                        else if (currentItemId.Equals("4006"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4007"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4008"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4009"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4010"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4011"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4012"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4013"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4014"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4015"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4016"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4017"))
                        {
                            skins.Enqueue("4001");
                        }
                        else if (currentItemId.Equals("4018"))
                        {
                            skins.Enqueue("4001");
                        }
                    }
                    Achievement.Instance.UnclockAchievement("first_purchase");
                    FindFirstObjectByType<SalesCenterController>().isPurshing = false;
                    Debug.Log("Transaction Finished.");
                    _isInPurchaseProcess = false;
                }
                else if (!string.IsNullOrEmpty(ret.error))
                {
                    Debug.LogError("Error from API: " + ret.error);
                }
            }
        }
    }

    public class ApiReturn
    {
        public bool success;
        public string error;
    }

    public class ApiReturnTransaction : ApiReturn
    {
        public string transid;
    }
}
