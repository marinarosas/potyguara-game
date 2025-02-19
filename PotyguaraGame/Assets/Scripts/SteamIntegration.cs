using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Unity.VisualScripting;

public class SteamIntegration : MonoBehaviour
{
    [Header("Steam Configuration")]
    private uint appID = 480;
    private ulong itemID = 1;

    private Callback<GetTicketForWebApiResponse_t> m_TicketForWebApiResponse;

    private void Start()
    {
        if (!SteamManager.Initialized)
            return;

        SendUsernameSteam();

    }

    public void SendUsernameSteam()
    {
        HAuthTicket ticket =  SteamUser.GetAuthTicketForWebApi("0AE12415B02F2D1A7FBC0093AE27FC2B");
     
        Debug.Log(SteamFriends.GetPersonaName());
        CSteamID steamID = SteamUser.GetSteamID();
        
    }

    public void Purchase(int itemID, ulong itemPrice)
    {
        if (!SteamUser.BLoggedOn())
        {
            Debug.Log("Usuario não conectado ao steam.");
            return;
        }

        ulong transactionID;
        SteamInventoryResult_t resultHandle;
    }
}
