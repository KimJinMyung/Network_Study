using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LoginPopUp : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private InputField _Input_ID;
    [SerializeField] private InputField _Input_Password;

    [SerializeField] private Button _Btn_Host;
    [SerializeField] private Button _Btn_Client;

    public static LoginPopUp instance;

    private string _originalNetworkAddress;
    private bool isReadyToAccess_ID;
    private bool isReadyToAccess_Password;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetDefaultNetworkAddress();
    }

    private void SetDefaultNetworkAddress()
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = "localhost";
        }

        _originalNetworkAddress = NetworkManager.singleton.networkAddress;
    }

    private void OnEnable()
    {
        _Input_ID.onValueChanged.AddListener(OnValueChanged_ID);
        _Input_Password.onValueChanged.AddListener(OnValueChanged_Password);
    }

    private void OnDisable()
    {
        _Input_ID.onValueChanged.RemoveListener(OnValueChanged_ID);
        _Input_Password.onValueChanged.RemoveListener(OnValueChanged_Password);
    }

    //ID와 Password 모두 입력하면 버튼 활성화
    public void OnValueChanged_ID(string str)
    {
        bool isNotInputFieldNull = !string.IsNullOrWhiteSpace(str);
        isReadyToAccess_ID = isNotInputFieldNull;
        OnValueChanged_ToggleButton();
    }

    public void OnValueChanged_Password(string str)
    {
        bool isNotInputFieldNull = !string.IsNullOrWhiteSpace(str);
        isReadyToAccess_Password = isNotInputFieldNull;
        OnValueChanged_ToggleButton();
    }

    private void OnValueChanged_ToggleButton()
    {
        bool isReadToAccess = isReadyToAccess_ID && isReadyToAccess_Password;
        _Btn_Host.interactable = isReadToAccess;
        _Btn_Client.interactable= isReadToAccess;
    }

    private void Update()
    {
        if (string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = _originalNetworkAddress;
        }        
    }

    public void OnClick_StartHost()
    {
        NetworkManager.singleton.StartHost();
        this.gameObject.SetActive(false);
    }
    public void OnClick_StartClient()
    {
        NetworkManager.singleton.StartClient();
        this.gameObject.SetActive(false);
    }

    public void SetUIOnClientDisconnected()
    {
        this.gameObject.SetActive(true);
        _Input_ID.text = string.Empty;
        _Input_Password.text = string.Empty;
        _Input_ID.ActivateInputField();
        _Input_Password.ActivateInputField();
    }
}
