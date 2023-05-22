using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class LobbyCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject generateCanvas;
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject roomCanvas;
    [SerializeField] private TextMeshProUGUI LobbyNickName;
    [SerializeField] private GameObject statePannel;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject viewContent;
    private bool showName = false;
    private bool createRoom = false;
    private bool joinRoom = false;

    public void OnClickOptions()
    {
        optionsCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);
        OptionsCanvasManager.prevCanvas = lobbyCanvas;

    }

    public void OnClickBackBtn()
    {
        showName = false;
        lobbyCanvas.SetActive(false);
        generateCanvas.SetActive(true);
    }

    public void OnClickCreateRoom()
    {
        PhotonNetworkManager.Instance.CreateRoom();
        createRoom = true;
    }

    public void onClickPannelExit()
    {
        statePannel.SetActive(false);
        createRoom = false;
    }

    public void OnClickRefresh()
    {
        //PhotonNetworkManager.Instance.RoomUpdate();
        RoomUpdate();
    }

    private void RoomUpdate()
    {
        int roomCount = PhotonNetworkManager.Instance.GetRoomListCount();
        for (int idx = 0; idx < viewContent.transform.childCount; idx++)
        {
            Destroy(viewContent.transform.GetChild(idx).gameObject);
        }

        for (int idx = 0; idx < roomCount; idx++)
        {
            GameObject room = Instantiate(roomPrefab);
            Transform obj = room.transform.GetChild(0).transform;

            //Info info = (Info)PhotonNetworkManager.Instance.GetRoomMasterPlayerName();
            //if (info.roomName == "") { return;}

            obj.Find("TextHost").GetComponent<TextMeshProUGUI>().text = "Player1";
            obj.Find("TextRoomName").GetComponent<TextMeshProUGUI>().text = "AbZv4";
            obj.Find("TextCount").GetComponent<TextMeshProUGUI>().text = "1/2";
            SetRoomListPosition(room, idx);
            //Debug.Log(info.ToString());
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



    public void OnClickJoin()
    {
        PhotonNetworkManager.Instance.FastJoinRoom();
        joinRoom = true;
    }

    public void OnClickFastJoin()
    {
        PhotonNetworkManager.Instance.FastJoinRoom();
        joinRoom = true;
    }

    private bool LobbyStart = false;
    // Update is called once per frame
    void Update()
    {
        if (!LobbyStart)
        {
            RoomUpdate();
            LobbyStart = true;
            return;
        }
        if (lobbyCanvas.activeSelf)
        {
            Invoke("RoomUpdate", 5f);
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            print("Player : " + player.NickName);
        }
        if (!showName && lobbyCanvas.activeSelf)
        {
            LobbyNickName.text = GameData.name;
            showName = true;
        }

        if (createRoom || joinRoom)
        {
            switch (PhotonNetworkManager.network)
            {
                case NETWORK_STATE.CreatingRoom:
                    // �� ���� �� �г� ����
                    stateChange(true, "�� ���� �� . . .");
                    break;
                case NETWORK_STATE.CreatedRoom:
                    // �� ���� ���� �� ������ �г� ����
                    stateChange(true, "�� ���� ����!");
                    break;
                case NETWORK_STATE.JoiningRoom:
                    // �� ���� ���� �� ������ �г� ����
                    stateChange(true, "�� ���� �� . . .");
                    break;
                case NETWORK_STATE.JoinedRoom:
                    lobbyCanvas.SetActive(false);
                    roomCanvas.SetActive(true);
                    // ����� �г� ����
                    statePannel.SetActive(false);
                    createRoom = false;
                    joinRoom = false;
                    break;
                case NETWORK_STATE.FailedCreatedRoom:
                    // �� ���� ���� �г� ����
                    stateChange(true, "�� ���� ����!\n�ٽ� �õ��� �ּ���.");
                    createRoom = false;
                    joinRoom = false;
                    break;
                case NETWORK_STATE.FailedJoiningRoom:
                    // �� ���� ���� �г� ����
                    stateChange(true, "�� ���� ����!\n�ٽ� �õ��� �ּ���.");
                    createRoom = false;
                    joinRoom = false;
                    break;
            }
        }
        
    }

    private void stateChange(bool pannelOn, string message)
    {
        statePannel.SetActive(pannelOn);
        statePannel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
    }
}
