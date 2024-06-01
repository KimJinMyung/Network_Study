using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingManager : NetworkManager
{
    [SerializeField] LoginPopUp _loginPopUp;
    [SerializeField] Chatting_UI _chattingUI;
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if(_chattingUI != null)
        {
            _chattingUI.RemoveNameOnServerDisconnect(conn);
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
    
        if(_loginPopUp != null)
        {
            _loginPopUp.SetUIOnClientDisconnected();
        }
    }
}
