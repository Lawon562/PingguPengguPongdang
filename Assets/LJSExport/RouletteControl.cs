using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon;
using Photon.Pun;
using Photon.Realtime;


public class RouletteControl : MonoBehaviour
{
    TextMeshProUGUI rouletteText;

    Animator RandomAni;
    void Start()
    {
        rouletteText = GetComponent<TextMeshProUGUI>();
        RandomAni = GetComponent<Animator>();
        RulletNickname();
    }
    public float timer = 0f;
    public static bool timerFlag = false;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 3f) return;
        if (!timerFlag)
        {
            timerFlag = true;
            RandomSelect();
        }

    }

    public static int randomNum = -1;

    public void RandomSelect()
    {
        print("A"); ;
        randomNum = Random.Range(0, 2);
        if (randomNum == 0)
        {
            RandomAni.SetTrigger("SelectA");
        }
        if (randomNum == 1)
        {
            RandomAni.SetTrigger("SelectB");
        }
    }

    //�����Ʈ��ũ ����Ʈ - �÷��̾� �̸� �޾ƿ��� - 
    public void RulletNickname()
    {
        print("������ ���� �ǳ�?");
        rouletteText.text = PhotonNetwork.PlayerList[0].NickName + "\n" + "Player 2" + "\n"
            + PhotonNetwork.PlayerList[0].NickName + "\n" + "Player 2" + "\n"
            + PhotonNetwork.PlayerList[0].NickName + "\n" + "Player 2" + "\n"
            + PhotonNetwork.PlayerList[0].NickName;
        //PhotonNetwork.PlayerList[0].NickName;
        //PhotonNetwork.PlayerList[1].NickName;
    }


}
