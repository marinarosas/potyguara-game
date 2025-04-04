using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Steamworks;
using System.Net.Sockets;

public class Microtransaction : MonoBehaviour
{
    [SerializeField] private string baseUrl = "https://potysteam.ffcloud.com.br"; // Set this to your API base URL
    [SerializeField] private string appId = "3181940"; // replace with your own appId
    [SerializeField] private List<WWWForm> wWWForms = new List<WWWForm>();

    // finish transaction callback
    protected Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

    private int currentOrderID = 1000;
    private string currentTransactionId = "";

    private bool _isInPurchaseProcess = false;

    //  Singleton stuff
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
        currentOrderID += Random.Range(1000000, 100000000);
    }

    public void InitSale(string itemID, string description, string category)
    {
        this._isInPurchaseProcess = true;
        StartCoroutine(InitializePurchase(itemID, description, category));
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
        form.AddField("orderId", currentOrderID.ToString());
        form.AddField("itemDescription", description);
        form.AddField("category", category);
        form.AddField("appId", appId);

        wWWForms.Add(form);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + "/InitPurchase", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
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
                    wWWForms.Remove(form);
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
                    foreach(WWWForm Form in wWWForms)
                    {
                        if (Form.headers["orderId"] == orderId)
                        {
                            if (Form.headers["itemId"].Equals("1001"))
                            {
                                FindFirstObjectByType<PotyPlayerController>().SetPotycoins(100);
                            }
                            else if (Form.headers["itemId"].Equals("1002"))
                            {
                                FindFirstObjectByType<PotyPlayerController>().SetPotycoins(250);
                            }
                            else if (Form.headers["itemId"].Equals("1003"))
                            {
                                FindFirstObjectByType<PotyPlayerController>().SetPotycoins(500);
                            }
                            else if (Form.headers["itemId"].Equals("1004"))
                            {
                                FindFirstObjectByType<PotyPlayerController>().SetPotycoins(1000);
                            }
                            else if (Form.headers["itemId"].Equals("3002"))
                            {
                                FindFirstObjectByType<MeditationRoomController>().AddButton(1);
                                NetworkManager.Instance.SendSession("3002");
                            }
                            else if (Form.headers["itemId"].Equals("3003"))
                            {
                                FindFirstObjectByType<MeditationRoomController>().AddButton(2);
                                NetworkManager.Instance.SendSession("3003");
                            }
                            else if (Form.headers["itemId"].Equals("4001"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4001");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4002"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4002");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4003"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4003");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4004"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4004");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4005"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4005");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4006"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4006");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4007"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4007");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4008"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4008");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4009"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4009");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4010"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4010");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4011"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4011");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4012"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4012");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4013"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4013");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4014"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4014");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4015"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4015");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4016"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4016");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4017"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4017");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                            else if (Form.headers["itemId"].Equals("4018"))
                            {
                                int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4018");
                                if (index != 0)
                                {
                                    FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                                    NetworkManager.Instance.SendSkin(index);
                                }
                            }
                        }
                    }
                    Debug.Log("Transaction Finished.");
                    FindFirstObjectByType<SalesCenterController>().isPurshing = false;
                    _isInPurchaseProcess = false;
                }
                else if (!string.IsNullOrEmpty(ret.error))
                {
                    Debug.LogError("Error from API: " + ret.error);
                    FindFirstObjectByType<SalesCenterController>().isPurshing = false;
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