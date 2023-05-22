using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject generateCanvas;
    [SerializeField] private GameObject guideCanvas;
    [SerializeField] private GameObject optionCanvas;

    /// <summary>
    /// Title Canvas�� Start ��ư�� ������ �� �۵�<br />
    /// - �κ� ���� ���� �õ�
    /// </summary>
    public void OnClickStart()
    {
        Debug.Log("�κ� ���� ���� ����");
        PhotonNetworkManager.Instance.ConnectingLobby();
    }

    public void OnClickOptions()
    {
        optionCanvas.SetActive(true);
        titleCanvas.SetActive(false);
        OptionsCanvasManager.prevCanvas = titleCanvas;
    }

    public void OnClickGuide()
    {
        guideCanvas.SetActive(true);
        titleCanvas.SetActive(false);
        GuideCanvasManager.prevCanvas = titleCanvas;

    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    private void Update()
    {
        if (PhotonNetworkManager.network == NETWORK_STATE.Connecting)
        {
            titleCanvas.SetActive(false);
            generateCanvas.SetActive(true);
        }
    }
}
