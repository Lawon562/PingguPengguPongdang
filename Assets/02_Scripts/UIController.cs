using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class UIController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject TitleCanvas;
    [SerializeField] private GameObject GenerateCanvas;
    [SerializeField] private TextMeshProUGUI DuplicationText;
    [SerializeField] private TMP_InputField PlayerName;
    [SerializeField] private GameObject LobbyCanvas;
    [SerializeField] private TextMeshProUGUI LobbyNickName;
    [SerializeField] private GameObject RoomCanvas;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject viewContent;
    [SerializeField] private GameObject optionCanvas;

    private GameObject OnCanvas;

    //----------------------------------------------------------------------//
    // 



    /// <summary>
    /// ���� ��Ʈ��ũ ����<br/>
    /// ���� ���� : 0.1
    /// ������ �ۼ��� Ƚ�� �����Ӵ� 60ȸ
    /// </summary>
    private void NetworkSettings()
    {
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.SerializationRate = 60;
        PhotonNetwork.SendRate = 60;
    }

    // ������ ���۵Ǹ� ��Ʈ��ũ ������ ���� ��
    public void Start()
    {
        OnCanvas = TitleCanvas;
        NetworkSettings();
    }

    //----------------------------------------------------------------------//
    // title

    /// <summary>
    /// Title Canvas�� Start ��ư�� ������ �� �۵�<br />
    /// - �κ� ���� ���� �õ�
    /// </summary>
    public void Title_OnClickStart()
    {
        Debug.Log("���� ���� ����");
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ���ӿ� ���� �ݹ� �޼���
    // - Title Canvas�� ���� Lobby Canvas�� ������<br />
    // - ĳ���� ����
    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ���� �Ϸ�");
        TitleCanvas.SetActive(false);
        GenerateCanvas.SetActive(true);
        OnCanvas = GenerateCanvas;

        PhotonNetwork.JoinLobby();
    }

    // ���� ���ӿ� ���� �ݹ� �޼���
    // ���� ������ ����� ��� ĵ������ ���� TitleCanvas�� ���ƿ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("���� ����");

        TitleCanvas.SetActive(true);
        GenerateCanvas.SetActive(false);
        LobbyCanvas.SetActive(false);

        OnCanvas = TitleCanvas;
    }

    //----------------------------------------------------------------------//
    // Generate

    public void Generate_OnClickCreate()
    {
        Debug.Log("�г����� : " + PlayerName.text);
        if (PlayerName.text.Equals(""))
        {
            print("�г��� ���� ����ֽ��ϴ�.");
            DuplicationText.text = "�г��� ���� ����ֽ��ϴ�.";
            return;
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (PlayerName.text.Equals(player.NickName))
            {
                print("�г����� �ߺ��Ǿ����ϴ�");
                DuplicationText.text = "�г����� �ߺ��Ǿ����ϴ�";
                return;
            }
        }
        DuplicationText.text = "ĳ���� ������ �Ϸ�Ǿ����ϴ�.";
        PhotonNetwork.NickName = PlayerName.text;

        GenerateCanvas.SetActive(false);
        LobbyCanvas.SetActive(true);
        OnCanvas = LobbyCanvas;
    }

    //----------------------------------------------------------------------//
    // Lobby


    public override void OnJoinedLobby()
    {
        print("�κ񼭹� Ȱ��ȭ");
        LobbyNickName.text = PhotonNetwork.NickName;
        RoomUpdate();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("�� ����Ʈ �ʱ�ȭ");
    }



    /// <summary>
    /// �� ����Ʈ ��������
    /// </summary>
    public void GetRoomList()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            return;
        }

        TypedLobby lobby = new TypedLobby("Default", LobbyType.Default);
        PhotonNetwork.GetCustomRoomList(lobby, string.Empty);
    }



    public void Lobby_OnClickRefresh()
    {
        Debug.Log("���ΰ�ħ");
        RoomUpdate();
    }

    private void RoomUpdate()
    {
        int roomCount = PhotonNetwork.CountOfRooms;
        print("���� �� ���� : " + roomCount);
        print("���� viewCotent �ڽ� ���� : " + roomCount);
        for (int idx = 0; idx < viewContent.transform.childCount; idx++)
        {
            Destroy(viewContent.transform.GetChild(idx).gameObject);
        }

        for (int idx = 0; idx < roomCount; idx++)
        {
            GameObject room = Instantiate(roomPrefab);
            SetRoomListPosition(room, idx);
        }

        // Content ���� ���� ����
        RectTransform contentRect = viewContent.GetComponent<RectTransform>();
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 10 * 50);
    }

    private void SetRoomListPosition(GameObject room, int idx)
    {
        RectTransform rect = room.GetComponent<RectTransform>();

        room.transform.SetParent(viewContent.transform, false);
        rect.anchoredPosition = new Vector2(0, idx * -21);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, 20f);
        rect.localScale = Vector3.one;
    }


    public void CreateRoom()
    {
        print("�� �����");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreatedRoom()
    {
        print("�� ���� ����");
        LobbyCanvas.SetActive(false);
        RoomCanvas.SetActive(true);
        OnCanvas = RoomCanvas;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("�� ���� ����");
    }

    public void Lobby_OnClickFastJoinBtn()
    {
        print("���� �� ����");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        print("�� ���� ����");
        LobbyCanvas.SetActive(false);
        RoomCanvas.SetActive(true);
        OnCanvas = RoomCanvas;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("�� ���� ����");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("���� �� ���� ����");
    }


    // ---------------------------------------------------------------- //
    public void OnClickBtnOption()
    {
        optionCanvas.SetActive(true);
        OnCanvas = optionCanvas;
    }

    public void OnClickExitBtn()
    {
        OnCanvas.SetActive(false);
    }

}
