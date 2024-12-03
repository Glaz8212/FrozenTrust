using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public static ChatManager Instance;


    private ChatClient chatClient;

    [Header("UI �Ӽ�")]
    [SerializeField] private GameObject survivorChatPanel;
    [SerializeField] private GameObject traitorChatPanel;
    [SerializeField] private GameObject traitorButton;
    [SerializeField] private InputField survivorInputField;
    [SerializeField] private InputField traitorInputField;
    [SerializeField] private Text survivorChatLog;
    [SerializeField] private Text traitorChatLog;

    private string userName;
    private bool isTraitor;

    // �̱������� ����
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(InitializeAfterDelay());
    }

    private IEnumerator InitializeAfterDelay()
    {
        // 3�� ���� ��
        yield return new WaitForSeconds(3f);

        // �г��� ����
        userName = PhotonNetwork.LocalPlayer.NickName;
        // ����� ���� �Ǵ�
        isTraitor = PhotonNetwork.LocalPlayer.ActorNumber == 1;

        // ê ���� ����
        ConnectToChat();
        // UI Ȱ��ȭ (����� ����)
        UpdateUI();
    }

    private void Update()
    {
        // ê Ŭ���̾�Ʈ ������Ʈ
        chatClient?.Service();
    }
    private void ConnectToChat()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(userName));
    }

    // ����� ���ο� ���� traitorChatPanel Ȱ��ȭ
    private void UpdateUI()
    {
        Debug.LogWarning(isTraitor);
        survivorChatPanel.SetActive(true);
        traitorButton.SetActive(isTraitor);
    }

    public void ChangeToTraitorChat()
    {
        survivorChatPanel.SetActive(false);
        traitorChatPanel.SetActive(true);
    }

    public void ChangeToSurvivorChat()
    {
        survivorChatPanel.SetActive(true);
        traitorChatPanel.SetActive(false);
    }


    public void OnConnected()
    {
        Debug.Log("ä�� ����");
        chatClient.Subscribe(new string[] { "Survivor", "Traitor" });
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        Debug.Log("ä�� ���� ����");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"ä�� ���� ���� : {state}");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Text logText = channelName == "Survivor" ? survivorChatLog : traitorChatLog;

        for (int i = 0; i < senders.Length; i++)
        {
            logText.text += $"\n{senders[i]}: {messages[i]}";
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log($" {sender}�� {message}��� �ӼӸ�");
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log($"Subscribed to channel(s): {string.Join(", ", channels)}");
        chatClient.PublishMessage("Survivor", $"{userName} joined the Survivor Chat.");
        if (isTraitor)
        {
            chatClient.PublishMessage("Traitor", $"{userName} joined the Traitor Chat.");
        }
    }

    public void OnUnsubscribed(string[] channels)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log($"������: {user}, ����: {status}, �޼���: {message}");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log($"{user} �� {channel} �� ����");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log($"{user} �� {channel} �� ����");
    }

    // ������ ä�� ������� ���� (��ư ��Ŭ��)
    public void SendMessageToSurvivorChat()
    {
        string message = survivorInputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            chatClient.PublishMessage("Survivor", message);
            survivorInputField.text = "";
        }
    }

    // ����� ä�� ������� ���� (��ư ��Ŭ��)
    public void SendMessageToTraitorChat()
    {
        if (!isTraitor) return;

        string message = traitorInputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            chatClient.PublishMessage("Traitor", message);
            traitorInputField.text = "";
        }
    }

    public void DebugReturn(DebugLevel debugLevel, string user)
    {

    }
}
