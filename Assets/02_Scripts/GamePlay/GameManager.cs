using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public enum GameState
{
    idle, ready, roulette, play, finished
}
public class GameManager : MonoBehaviourPun
{
    public Transform iceGround;
    public Transform giantPos;
    public Transform miniPos;
    public Material deadMat;
    List<Transform> ices;
    Transform deadZone;
    GameState gameState;

    bool playingSettings = false;
    int round = 0;



    void Start()
    {
        gameState = GameState.idle;
        ices = new List<Transform>(iceGround.GetComponentsInChildren<Transform>());
        ices.RemoveAt(0);
        foreach (Transform t in ices)
        {
            t.transform.AddComponent<IceAction_KSW>();
        }
    }


    void Update()
    {
        GameFlow();
        GameRule();
    }

    void GameFlow()
    {
        switch (gameState)
        {
            case GameState.idle:
                // idle ���¿��� �÷��̾� ������ üũ
                round++;
                gameState = GameState.ready;
                break;
            case GameState.ready:
                // ���ο� Turn�� �����ϱ� ���� �غ� �ܰ�
                gameState = GameState.roulette;
                break;
            case GameState.roulette:
                // �켱 ������ �� �� ������
                if (playingSettings)
                {
                    DisableBreaking();
                    playingSettings = false;
                    round++;
                }
                // �������� ���ư��� ���� ����

                // �������� ������, ������ ���� ���..
                if (deadZone == null)
                    SetDeadZone();
                Invoke("ChangeToPlay", 3f);
                break;
            case GameState.play:
                // Ư��ȿ�� ���� ����� �� ������ ������ ���� �ܰ�
                // ������ �� �� �ֵ��� ����, �̴���� �̵� ����, 
                if (!playingSettings)
                {
                    SettingsToPlay();
                    playingSettings = true;
                    print("Round " + round);
                    int a = 0;
                    foreach (Transform t in ices)
                    {
                        if (t.GetComponent<IceAction_KSW>().GetLife() > 0)
                            a++;
                    }
                    print("This Ice Count : " + a);
                }
                break;
        }
    }

    void GameRule()
    {
        switch (gameState)
        {
            case GameState.roulette:
                if (deadZone.GetComponent<IceAction_KSW>().GetLife() == 0)
                {
                    DeadZoneKill();
                }
                break;
            case GameState.play:
                if (deadZone.GetComponent<IceAction_KSW>().GetLife() == 0)
                {
                    DeadZoneKill();
                }
                break;
        }
    }

    void ChangeToPlay()
    {
        gameState = GameState.play;
    }

    void SettingsToPlay()
    {
        foreach (Transform t in ices)
        {
            t.transform.GetComponent<IceAction_KSW>().CanBreak();
        }
    }
    void DisableBreaking()
    {
        foreach (Transform t in ices)
        {
            t.transform.GetComponent<IceAction_KSW>().NotBreak();
        }
    }

    void SetDeadZone()
    {
        if (deadZone != null)
        {
            deadZone.GetComponent<IceAction_KSW>().ThisIsNotDeadZone();
            deadZone = null;
        }
        List<Transform> temp = new List<Transform>();
        foreach (Transform t in ices)
        {
            if (t.GetComponent<IceAction_KSW>().GetLife() > 0)
                temp.Add(t);
        }
        deadZone = temp[Random.Range(0, temp.Count)];
        deadZone.GetComponent<IceAction_KSW>().ThisIsDeadZone();
        deadZone.GetComponent<MeshRenderer>().material = deadMat;
    }

    void DeadZoneKill()
    {
        foreach (Transform t in ices)
        {
            t.transform.GetComponent<IceAction_KSW>().LifeZero();
        }
    }
    
    public void Breaked()
    {
        gameState = GameState.ready;
    }

    void IceLifeRollback()
    {
        foreach (Transform t in ices)
        {
            if (t.GetComponent<IceAction_KSW>().GetLife() > 0)
                t.GetComponent<IceAction_KSW>().ResetLife();
        }
    }
}