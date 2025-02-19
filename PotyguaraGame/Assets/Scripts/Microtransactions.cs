using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.Networking;

public class Microtransaction : MonoBehaviour
{
    [SerializeField] private string baseUrl = "http://127.0.0.1:3000"; // Set this to your API base URL
    [SerializeField] private string appId = "3181940"; // replace with your own appId
    [SerializeField] private List<WWWForm> wWWForms;

    // finish transaction callback
    protected Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

    private int currentOrderID = 1000;
    private string currentTransactionId = "";

    private bool _isInPurchaseProcess = false;
    private int currentCoins;

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
                    currentCoins += 1000;
                    foreach(WWWForm Form in wWWForms)
                    {
                        if (Form.headers["orderId"] == orderId)
                        {
                            if (Form.headers["itemId"].Equals("1001"))
                            {
                                PotyPlayerController.Instance.potyPlayer.SetPotycoins(100);
                            }else if (Form.headers["itemId"].Equals("1002")){
                                PotyPlayerController.Instance.potyPlayer.SetPotycoins(250);
                            }else if (Form.headers["itemId"].Equals("1003"))
                            {
                                PotyPlayerController.Instance.potyPlayer.SetPotycoins(500);
                            }else if (Form.headers["itemId"].Equals("1004"))
                            {
                                PotyPlayerController.Instance.potyPlayer.SetPotycoins(1000);
                            }else if (Form.headers["itemId"].Equals("2002"))
                            {

                            }else if (Form.headers["itemId"].Equals("3002"))
                            {

                            }
                        }
                    }
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