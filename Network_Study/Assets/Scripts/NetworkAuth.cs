using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class NetworkAuth : NetworkAuthenticator
{
    readonly HashSet<NetworkConnection> _ConnectionsPendingDisconnect = new HashSet<NetworkConnection>();
    static readonly HashSet<string> _playerName = new HashSet<string>();

    public struct AuthReqMsg : NetworkMessage 
    {
        public string authUserName;
    }

    public struct AuthRespMsg : NetworkMessage
    {
        public byte code;
        public string msg;
    }

    [RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {

    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<AuthReqMsg>(OnAuthRequestMessage, false);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        NetworkServer.UnregisterHandler<AuthRespMsg>();
    }

    public void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthReqMsg msg)
    {
        if (_ConnectionsPendingDisconnect.Contains(conn)) return;

        if (!_playerName.Contains(msg.authUserName))
        {
            _playerName.Add(msg.authUserName);
            conn.authenticationData = msg.authUserName;

            AuthRespMsg authRespMsg = new AuthRespMsg
            {
                code = 100,
                msg = "Auth Success"
            };

            conn.Send(authRespMsg);
            //conn.isAuthenticated = true;
            ServerAccept(conn);
        }
        else
        {
            _ConnectionsPendingDisconnect.Add(conn);

            AuthRespMsg authRespMsg = new AuthRespMsg
            {
                code = 200,
                msg = "User Name already in use! Try again!"
            };

            conn.Send(authRespMsg);
            conn.isAuthenticated = false;

            StartCoroutine(DelayDisconnect(conn, 1.5f));
        }
    }

    IEnumerator DelayDisconnect(NetworkConnectionToClient conn, float TIme)
    {
        yield return new WaitForSeconds(TIme);
        ServerReject(conn);

        yield return null;
        _ConnectionsPendingDisconnect.Remove(conn);
    }
}
