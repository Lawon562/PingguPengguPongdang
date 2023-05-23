using Oculus.Interaction.Input;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateCanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject generateCanvas;
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject optionsCanvas;

    [SerializeField] private TextMeshProUGUI DuplicationText;
    [SerializeField] private TMP_InputField PlayerName;


    private bool characterFlag = false;


    private void Start()
    {
        characterFlag = false;
        OnClickGenerateKoreanName();
    }
    public void OnClickCreateCharacter()
    {
        if(EmptyCheck())
        {
            DuplicationText.text = "�г��� ���� ����ֽ��ϴ�.";
            return;
        }

        DuplicationText.text = "ĳ���� ������ �Ϸ�Ǿ����ϴ�.";
        GameData.name = PlayerName.text;
        PhotonNetworkManager.Instance.SetNickName(PlayerName.text);
        characterFlag = true;

        generateCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);
    }

    public void OnClickGenerateKoreanName()
    {
        KoreanNameGenerator nameGenerator = new KoreanNameGenerator();
        string koreanName = nameGenerator.GenerateKoreanName();
        PlayerName.text = koreanName;
    }

    public bool EmptyCheck()
    {
        if (PlayerName.text.Equals(""))
        {
            return true;
        }
        return false;
    }

    public void OnClickOptions()
    {
        generateCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        OptionsCanvasManager.prevCanvas = generateCanvas;

    }

    public void OnClickBackBtn()
    {
        PhotonNetworkManager.Instance.DisConnectingServer();
        generateCanvas.SetActive(false);
        titleCanvas.SetActive(true);

    }

    private void Update()
    {
       
    }

}
