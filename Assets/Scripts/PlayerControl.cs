using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.Asteroids;

public class PlayerControl : MonoBehaviourPun, IPunObservable
{
    float dir_rot;
    float dir_fb;
    //public Slider MyEnergy;
    float MyLife = 100;
    // �̵� ȸ�� ���庯��
    public Vector3 setPos;      // �̵�
    public Quaternion setRot;   // ȸ��
    public GameObject Bullet;
    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        // ó���� �ʱ�ȭ�� ��ġ�� ����(0,0,0)
        this.transform.position = setPos;
        this.transform.rotation = setRot;
    }

    // Update is called once per frame
    void Update()
    {
        // �� ĳ���ʹ� ��Ʈ��ũ���� �޾ƿ��� ��ġ������ �ƴ϶�,
        // ���� ���氡���ϹǷ� photoView.IsMine���� ���� �˻�� �������� ǥ������.
        if (photonView.IsMine)
        {
            MoveRot();    // �̵� �� ȸ��
            Jump();       // ����

            // ���߿� �� ���� �߻� �ڵ�
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                print("����");
                    GameObject temp = PhotonNetwork.Instantiate(Bullet.name, firePoint.position, firePoint.rotation);
                    //temp.GetComponent<Rigidbody>().AddForce(firePoint.forward * 600f);
            }
        }
        else
        {
            // �װ� �ƴ϶��, �� �� ��ǻ�Ϳ��� ���ʷ� ������ ĳ���Ͱ� �ƴ϶��
            // ���� ��ü�� ��ġ�� ������ ��ġ(setPos)���� �����Ͽ� �̵����Ѷ�
            this.transform.position = Vector3.Lerp(this.transform.position, setPos, Time.deltaTime * 20f);
            // ���� ��ü�� ��ġ�� ������ ����(setRot)���� �����Ͽ� ȸ�����Ѷ�
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, setRot, Time.deltaTime * 20f);
        }


    }

    void MoveRot()
    {
        dir_rot = Input.GetAxis("Horizontal");
        dir_fb = Input.GetAxis("Vertical");

        this.transform.Translate(Vector3.forward * dir_fb * Time.deltaTime * 5f);
        this.transform.Rotate(Vector3.up * dir_rot * Time.deltaTime * 110f);

    }

    // PhotonSerializeView ó��(�ݹ� �޼���)
    /* PhotonStream: �� �Ű������� �����͸� �аų� ���� �� ���Ǵ� 
     * Photon ��Ʈ�� ��ü�Դϴ�. 
     * �����͸� ���� ���ؼ��� PhotonStream�� Write �޼��带 ȣ���Ͽ� 
     * �����͸� ��Ʈ���� �� �� �ְ�, 
     * �����͸� �б� ���ؼ��� PhotonStream�� Read �޼��带 ����Ͽ� 
     * �����͸� ��Ʈ������ ���� �� �ֽ��ϴ�.
     */

    /* PhotonMessageInfo: �� �Ű������� Photon �޽����� ���� ������ �����ϴ� ��ü�Դϴ�. 
     * �޽����� ���� �÷��̾��� ID, Ÿ�ӽ����� ���� ������ Ȯ���� �� �ֽ��ϴ�.
     */

    // ���� ��ġ�� ������ remote���� ����
    // ����Ʈ���� // mine�� ������ ���� ���̴�.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // mine�� ���¿��� Remote���� �����͸� �Ѱ��� �� - ���簪�� ����
        if (stream.IsWriting)
        {
            setPos = transform.position;
            setRot = transform.rotation;

            stream.SendNext(setPos);  // ���� ���� ��ü(�̵�/ȸ����)
            stream.SendNext(setRot);  // ���� ���� ��ü(�̵�/ȸ����)
        }
        // remote ���¿��� mine�� ������ ���� �� - ����� ���� �о�´�.
        else
        {
            // Warning -- SendNext�� ���޵� ������ ������� ���� �� �ִ�.
            setPos = (Vector3)stream.ReceiveNext();   // �о�� ���� ���� ��ü
            setRot = (Quaternion)stream.ReceiveNext();   // �о�� ���� ���� ��ü
        }
    }


    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 15f, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            Dammaged();
        }
    }

    private void Dammaged()
    {
        if (MyLife <= 0f) return;
        this.MyLife -= 3f;
        //MyEnergy.value = MyLife;
        if (MyLife <= 0)
        {
            MyLife = 0;
        }
    }
}