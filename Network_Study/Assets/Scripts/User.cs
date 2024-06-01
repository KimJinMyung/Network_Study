using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : NetworkBehaviour
{
    [SyncVar]
    public string Name;

    public override void OnStartServer()
    {
        //base.OnStartServer();
        Name = (string)connectionToClient.authenticationData;
    }

    public override void OnStartLocalPlayer()
    {
        //base.OnStartLocalPlayer();
        var chatUI = GameObject.Find("Chatting_UI");
        if(chatUI != null)
        {
            var ChatUIComponent = chatUI.GetComponent<Chatting_UI>();
            if (ChatUIComponent != null)
            {
                ChatUIComponent.SetLocalPlayer(Name);
            }
        }
    }
}
