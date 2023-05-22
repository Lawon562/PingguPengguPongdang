using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking.Types;


public enum NETWORK_STATE 
{
    Disconnected,               // ���� �ȵ�
    Connecting,                 // ������
    Connected,                  // ����Ϸ�
    Disconnecting,              // ������ �Ǿ����� ������ ������ ���� ����
    JoinedLobby,                // �κ� ���ӵ�
    CreatingRoom,               // �� ���� ��
    CreatedRoom,                // �� ���� ��
    FailedCreatedRoom,          // �� ���� ����
    JoiningRoom,                // �濡 ���� ��
    FailedJoiningRoom,          // �濡 ���� ����
    JoinedRoom                  // �뿡 ���ӵ�
}

public class PhotonNetworkManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private static PhotonNetworkManager instance;
    public static PhotonNetworkManager Instance
    {
        get
        {
            if (instance == null) return null;
            return instance;
        }
    }

    public static NETWORK_STATE network = NETWORK_STATE.Disconnected;

    private void Awake()
    {
        network = NETWORK_STATE.Disconnected;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

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
        NetworkSettings();
    }


    // ���� ���� �޼ҵ�
    public void ConnectingLobby()
    {
        print("���� ���� �� . . .");
        network = NETWORK_STATE.Connecting;
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ���� ���� �޼ҵ�
    public void DisConnectingServer()
    {
        print("���� ���� ����");
        network = NETWORK_STATE.Disconnecting;
        PhotonNetwork.Disconnect();
    }

    // �г��� �ߺ��˻� �޼ҵ�
    public bool DuplicateCheck(string name)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (name.Equals(player.NickName))
            {
                return true;
            }
        }
        PhotonNetwork.NickName = name;
        GameData.name = PhotonNetwork.NickName;
        return false;
    }




    // ���� ���� �ݹ� �޼ҵ� -------------------------------


    // ���� ���ӿ� ���� �ݹ� �޼���
    // - Title Canvas�� ���� Lobby Canvas�� ������<br />
    // - ĳ���� ����
    public override void OnConnectedToMaster()
    {
        print("���� ���� �Ϸ�");
        network = NETWORK_STATE.Connected;
        PhotonNetwork.JoinLobby();
        print("�κ� ���� �� . . .");
    }

    // ���� ���ӿ� ���� �ݹ� �޼���
    // ���� ������ ����� ��� ĵ������ ���� TitleCanvas�� ���ƿ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("���� ���� ����");
        network = NETWORK_STATE.Disconnected;
    }

    // �κ� ���� ���� �޼��� ----------------------------

    public override void OnJoinedLobby()
    {
        print("�κ� ���� �Ϸ�");
        network = NETWORK_STATE.JoinedLobby;
    }

    // �� ���� �޼��� -----------------------------------

    private string CreateRoomRandomName()
    {
        RandomStringGenerator generator = new RandomStringGenerator();
        string randomString = generator.GenerateRandomString(5);
        return randomString;
    }

    public void CreateRoom()
    {
        network = NETWORK_STATE.CreatingRoom;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "MasterPlayer", PhotonNetwork.NickName }
        };

        PhotonNetwork.CreateRoom(
            CreateRoomRandomName(),
            options
        );
    }

    public void FastJoinRoom()
    {
        network = NETWORK_STATE.JoiningRoom;
        PhotonNetwork.JoinRandomRoom();
    }

    public int GetRoomListCount()
    {
        return PhotonNetwork.CountOfRooms;
    }

    public override void OnCreatedRoom()
    {
        network = NETWORK_STATE.CreatedRoom;
        print("�� ���� ����");
        PhotonNetwork.JoinRoom(CreateRoomRandomName());
        network = NETWORK_STATE.JoiningRoom;
    }

    public override void OnJoinedRoom()
    {
        print("�� ����");
        network = NETWORK_STATE.JoinedRoom;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        network = NETWORK_STATE.FailedCreatedRoom;
        print("�� ���� ����" + message);
        network = NETWORK_STATE.JoinedLobby;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        network = NETWORK_STATE.FailedJoiningRoom;

        print("�� ���� ����");
        network = NETWORK_STATE.JoinedLobby;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        network = NETWORK_STATE.FailedJoiningRoom;

        print("���� �� ���� ����");
        network = NETWORK_STATE.JoinedLobby;
    }


    public string GetRoomMasterPlayerName()
    {
        if (PhotonNetwork.InRoom)
        {
            Room currentRoom = PhotonNetwork.CurrentRoom;

            string roomName = currentRoom.Name;
            int playerCount = currentRoom.PlayerCount;
            int maxPlayers = currentRoom.MaxPlayers;
            string masterPlayer = "";
            if (currentRoom.CustomProperties != null && currentRoom.CustomProperties.Count > 0)
            {
                // CustomProperties���� ���ϴ� Ű�� �ش��ϴ� ���� ������
                if (currentRoom.CustomProperties.ContainsKey("MasterPlayer"))
                {
                    masterPlayer = currentRoom.CustomProperties["MasterPlayer"].ToString();
                }
            }
            

            Debug.Log("Current Room Name: " + roomName);
            Debug.Log("Current Player Count: " + playerCount);
            Debug.Log("Max Players: " + maxPlayers);
            Debug.Log("Master Player: " + masterPlayer);

            return masterPlayer;
        }
        return "";
    }

    public void LeftRoom()
    {
        print("�� ������ �õ�");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        print("�� ���ͼ� �κ� ������");
        network = NETWORK_STATE.JoinedLobby; 
        
        

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        switch(network)
        {
            //case NETWORK_STATE.JoinedRoom:
        }
    }
}