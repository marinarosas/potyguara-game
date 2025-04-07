// Install http://steamworks.github.io/ to use this script
// This script is just an example but you can use as you please
using Steamworks;
using UnityEngine;
using Jazz.http;
using System.Collections;
using System.Collections.Generic;

public class Microtransaction : MonoBehaviour
{
    [SerializeField] private HttpSettingsEditor clientSettings;
    private HttpApi m_internalHttpApi;

    [SerializeField] private string appId = "3181940"; // replace by your own appId

    // finish transaction callback
    protected Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;

    private int currentOrder = 1000;

    private string currentTransactionId = "";

    private string currentItemId;
    private string currentDescription;
    private string currentCategory;

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


    // unity awake function    
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

       m_internalHttpApi = new HttpApi(clientSettings.GenerateSettings());

       currentOrder += Random.Range(1000000,100000000);
    }

    // unity update function
    private void Update()
    {
        if(m_internalHttpApi != null)
        {
            m_internalHttpApi.Update();
        }
    }

    bool _isInPurchaseProcess = false;

    public void InitSale(string itemID, string description, string category)
    {
        this._isInPurchaseProcess = true;

        currentItemId = itemID;
        currentDescription = description;
        currentCategory = category;

        InitializePurchase();
    }


    // This callback is called when the user confirms the purchase
    // See https://partner.steamgames.com/doc/api/ISteamUser#MicroTxnAuthorizationResponse_t
    private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback) 
    {
        if(pCallback.m_bAuthorized == 1)
        {
            this.FinishPurchase(pCallback.m_ulOrderID.ToString());
        }
        Debug.Log("[" + MicroTxnAuthorizationResponse_t.k_iCallback + " - MicroTxnAuthorizationResponse] - " + pCallback.m_unAppID + " -- " + pCallback.m_ulOrderID + " -- " + pCallback.m_bAuthorized);
    }

    // To understand how to create products
    // see https://partner.steamgames.com/doc/features/microtransactions/implementation
    public void InitializePurchase()
    {
        string userId = SteamUser.GetSteamID().ToString();

        InitPurchaseArgs argsRequest = new InitPurchaseArgs();
        argsRequest.itemId = currentItemId;
        argsRequest.steamId = userId;
        argsRequest.orderId = currentOrder.ToString();
        argsRequest.itemDescription = currentDescription;
        argsRequest.category = currentCategory;

        // you can use your own library to call the API if you want to.
        this.MakeApiCall("InitPurchase",argsRequest, (HttpJsonResponse response) => 
            {
                ApiReturnTransaction ret = JsonUtility.FromJson<ApiReturnTransaction>(response.rawResponse);
                if(ret.transid != "")
                {
                    Debug.Log("Transaction initiated. Id:" + ret.transid);
                    this.currentTransactionId = ret.transid;
                }

            },(HttpRequestError error) => {
                Debug.Log(error.message);
            },true,HttpRequestContainerType.POST);
    }

    public void FinishPurchase(string orderId)
    {
        PurchaseArgs argsRequest = new PurchaseArgs();
        argsRequest.orderId = orderId.ToString();

        this.MakeApiCall("FinalizePurchase",argsRequest, (HttpJsonResponse response) => 
            {
                ApiReturn ret = JsonUtility.FromJson<ApiReturn>(response.rawResponse);
                if(ret.success)
                {
                    // after confirmation, you can give the item for the player
                    if (currentItemId.Equals("1001"))
                    {
                        FindFirstObjectByType<PotyPlayerController>().SetPotycoins(100);
                    }else if (currentItemId.Equals("1002"))
                    {
                        FindFirstObjectByType<PotyPlayerController>().SetPotycoins(250);
                    }
                    else if (currentItemId.Equals("1003"))
                    {
                        FindFirstObjectByType<PotyPlayerController>().SetPotycoins(500);
                    }
                    else if (currentItemId.Equals("1004"))
                    {
                        FindFirstObjectByType<PotyPlayerController>().SetPotycoins(1000);
                    }else if (currentItemId.Equals("3002"))
                    {
                        FindFirstObjectByType<MeditationRoomController>().AddButton(1);
                        NetworkManager.Instance.SendSession("3002");
                    }else if (currentItemId.Equals("3003"))
                    {
                        FindFirstObjectByType<MeditationRoomController>().AddButton(2);
                        NetworkManager.Instance.SendSession("3003");
                    }else if (currentItemId.Equals("4001"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4001");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }else if (currentItemId.Equals("4002"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4002");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4003"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4003");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4004"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4004");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4005"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4005");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4006"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4006");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4007"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4007");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4008"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4008");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4009"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4009");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4010"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4010");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4011"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4011");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4012"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4012");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4013"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4013");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4014"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4014");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4015"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4015");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4016"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4016");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4017"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4017");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    else if (currentItemId.Equals("4018"))
                    {
                        int index = FindFirstObjectByType<SalesCenterController>().GetIndexSkin("4018");
                        if (index != 0)
                        {
                            FindFirstObjectByType<PotyPlayerController>().AddSkin(index);
                            NetworkManager.Instance.SendSkin(index);
                        }
                    }
                    Achievement.Instance.UnclockAchievement("first_purchase");
                    FindFirstObjectByType<SalesCenterController>().isPurshing = false;
                    Debug.Log("Transaction Finished.");
                    this._isInPurchaseProcess = false;
                }
            },(HttpRequestError error) => {
                Debug.Log(error.message);
            },true,HttpRequestContainerType.POST);
    }

    // call the api    
    private void MakeApiCall(string apiEndPoint, HttpRequestArgs args, HttpRequestContainer.ActionSuccessHandler successCallback, HttpRequestContainer.ActionErrorHandler errorCallback, bool allowQueueing = false,string requestType = HttpRequestContainerType.POST)
    {
        if(m_internalHttpApi != null)
        {
            Dictionary<string, string> extraHeaders = new Dictionary<string, string>();

            args = args ?? new HttpRequestArgs();

            // steam app id
            args.appId = this.appId;

            m_internalHttpApi.MakeApiCall(apiEndPoint, args, successCallback,errorCallback,extraHeaders,requestType,allowQueueing);
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

    public class PurchaseArgs : HttpRequestArgs {
        public string orderId;
        public string transId;
    } 

    public class InitPurchaseArgs : PurchaseArgs {
        public string itemId;
        public string steamId;
        public string category;
        public string itemDescription;
    }
}