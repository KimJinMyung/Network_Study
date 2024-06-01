using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chatting_UI : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] Text Chat_History;
    [SerializeField] Scrollbar Scrollbar_Chat;
    [SerializeField] InputField Input_ChatMsg;
    [SerializeField] Button Btn_Send;

    internal static string _localPlayerName;

    internal static readonly Dictionary<NetworkConnectionToClient, string> _ConnectNameDic = new Dictionary<NetworkConnectionToClient, string>();

    public void SetLocalPlayer(string name)
    {
        _localPlayerName = name;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        this.gameObject.SetActive(true);
        _ConnectNameDic.Clear();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        this.gameObject.SetActive(true );
        Chat_History.text = string.Empty;
    }

    [Command]
    void CommandSendMsg(string msg, NetworkConnectionToClient sender = null)
    {
        if (!_ConnectNameDic.ContainsKey(sender))
        {
           // var player = sender.identity.GetComponent<User>();
           //_ConnectNameDic.Add(sender, player.Name);
        }

        if(!string.IsNullOrWhiteSpace(msg))
        {
            var sendName = _ConnectNameDic[sender];
            OnRpcMsg(sendName, msg);
        }
    }

    public void RemoveNameOnServerDisconnect(NetworkConnectionToClient conn)
    {
        _ConnectNameDic.Remove(conn);
    }

    [ClientRpc]
    void OnRpcMsg(string sender, string msg)
    {
        string Chatting_Msg = (sender == _localPlayerName) ? $"<color=red> {sender} :</color> {msg}" : $"<color=blue> {sender} :</color> {msg}";

        AppendMsg(Chatting_Msg);
    }

    private void AppendMsg(string msg)
    {
        StartCoroutine(AppendAndScroll(msg));
    }

    IEnumerator AppendAndScroll(string msg)
    {
        Chat_History.text += $"{msg}\n";

        yield return null;
        yield return null;

        Scrollbar_Chat.value = 0;
    }

    public void OnClick_SendMsg()
    {
        var currentChattingMsg = Input_ChatMsg.text;

        if(!string.IsNullOrWhiteSpace(currentChattingMsg))
        {
            CommandSendMsg(currentChattingMsg.Trim());
        }
    }

    public void OnClick_Exit()
    {
        NetworkManager.singleton.StopHost();
    }

    public void OnValueChanged_ToggleButton(string input)
    {
        Btn_Send.interactable = !string.IsNullOrWhiteSpace(input);
    }

    public void OnEndEdit_SendMsg(string input)
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit"))
        {
            OnClick_SendMsg();
        }
    }
}
