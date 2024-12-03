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

    [Header("UI 속성")]
    [SerializeField] private GameObject survivorChatPanel;
    [SerializeField] private GameObject traitorChatPanel;
    [SerializeField] private GameObject traitorButton;
    [SerializeField] private InputField survivorInputField;
    [SerializeField] private InputField traitorInputField;
    [SerializeField] private Text survivorChatLog;
    [SerializeField] private Text traitorChatLog;

    private string userName;
    private bool isTraitor;

    // 싱글톤으로 설정
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
        // 3초 텀을 둠
        yield return new WaitForSeconds(3f);

        // 닉네임 설정
        userName = PhotonNetwork.LocalPlayer.NickName;
        // 배신자 여부 판단
        isTraitor = PhotonNetwork.LocalPlayer.ActorNumber == 1;

        // 챗 서버 연결
        ConnectToChat();
        // UI 활성화 (배신자 여부)
        UpdateUI();
    }

    private void Update()
    {
        // 챗 클라이언트 업데이트
        chatClient?.Service();
    }
    private void ConnectToChat()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(userName));
    }

    // 배신자 여부에 따라 traitorChatPanel 활성화
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
        Debug.Log("채팅 연결");
        chatClient.Subscribe(new string[] { "Survivor", "Traitor" });
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        Debug.Log("채팅 연결 해제");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"채팅 상태 변경 : {state}");
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
        Debug.Log($" {sender}가 {message}라는 귓속말");
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
        Debug.Log($"유저명: {user}, 상태: {status}, 메세지: {message}");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.Log($"{user} 가 {channel} 에 연결");
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.Log($"{user} 가 {channel} 을 해지");
    }

    // 생존자 채팅 목록으로 전송 (버튼 온클릭)
    public void SendMessageToSurvivorChat()
    {
        string message = survivorInputField.text;
        if (!string.IsNullOrEmpty(message))
        {
            chatClient.PublishMessage("Survivor", message);
            survivorInputField.text = "";
        }
    }

    // 배신자 채팅 목록으로 전송 (버튼 온클릭)
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
