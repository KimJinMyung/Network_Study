using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkAuth
{
    [Header("Client UserName")]
    public string _thisClentPlayerName;

    public void OnInputValueChanged_SetPlayerName(string username)
    {
        _thisClentPlayerName = username;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<AuthRespMsg>(OnAuthResponseMessage, false);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        NetworkClient.UnregisterHandler<AuthRespMsg>();
    }

    public override void OnClientAuthenticate()
    {
        base.OnClientAuthenticate();
        NetworkClient.Send(new AuthReqMsg { authUserName = _thisClentPlayerName });
    }

    public void OnAuthResponseMessage(AuthRespMsg msg)
    {
        if(msg.code == 100)
        {
            ClientAccept();
        }
        else
        {
            NetworkManager.singleton.StopHost();
        }
    }
}
