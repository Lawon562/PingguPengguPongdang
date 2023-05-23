using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    CharacterController characterController;

    Vector3 movePower;

    public float Speed;
    public float jumpSpeed; // ĳ���� ���� ��.
    public float gravity;   // ĳ���Ϳ��� �ۿ��ϴ� �߷�.

    Vector3 sendPos;
    Quaternion sendRot;

    Animator playerAni;
    private static Vector3 MoveDir;

    float MyLife = 100;

    public GameObject Bullet;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        playerAni = this.transform.GetChild(0).GetComponent<Animator>();
        MoveDir = Vector3.zero;
        characterController = GetComponent<CharacterController>();
        this.transform.position = sendPos;
        this.transform.rotation = sendRot;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Jump();
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
            this.transform.position = Vector3.Lerp(this.transform.position, sendPos, Time.deltaTime * 20f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, sendRot, Time.deltaTime * 20f);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Move();
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, sendPos, Time.deltaTime * 20f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, sendRot, Time.deltaTime * 20f);
        }
        playerAni.SetFloat("Speed", characterController.velocity.magnitude);
    }


    private void Jump()
    {
        if (characterController.isGrounded)
        {
            // �÷��̾ �ٶ󺸴� �������� ����
            MoveDir = Vector3.zero;

            // ���͸� ���� ��ǥ�� ���ؿ��� ���� ��ǥ�� �������� ��ȯ�Ѵ�.
            MoveDir = transform.TransformDirection(MoveDir);

            // ���ǵ� ����.
            MoveDir *= Speed;

            // ĳ���� ����
            if (Input.GetButton("Jump"))
            {
                MoveDir.y = jumpSpeed;
            }

        }

        // ĳ���Ϳ� �߷� ����.
        MoveDir.y -= gravity * Time.deltaTime;

        // ĳ���� ������.
        characterController.Move(MoveDir * Time.deltaTime);
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0f || v != 0f)
        {
            float y = Camera.main.transform.rotation.eulerAngles.y;
            float targetAngle = y;
            if (h > 0f) // D Ű�� ������ ��
            {
                targetAngle = y + 90f;
                if (v > 0f)
                {
                    targetAngle -= 45f;
                }
                else if (v < 0f)
                {
                    targetAngle += 45f;
                }

            }
            else if (h < 0f) // A Ű�� ������ ��
            {
                targetAngle = y - 90f;
                if (v > 0f)
                {
                    targetAngle += 45f;
                }
                else if (v < 0f)
                {
                    targetAngle -= 45f;
                }
            }
            else if (v < 0f) // S Ű�� ������ ��
            {
                targetAngle = y + 180f;
            }

            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            movePower = transform.forward * Time.deltaTime * Speed;
            characterController.Move(movePower);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // mine�� ���¿��� Remote���� �����͸� �Ѱ��� �� - ���簪�� ����
        if (stream.IsWriting)
        {
            sendPos = transform.position;
            sendRot = transform.rotation;

            stream.SendNext(sendPos);  // ���� ���� ��ü(�̵�/ȸ����)
            stream.SendNext(sendRot);  // ���� ���� ��ü(�̵�/ȸ����)
        }
        // remote ���¿��� mine�� ������ ���� �� - ����� ���� �о�´�.
        else
        {
            // Warning -- SendNext�� ���޵� ������ ������� ���� �� �ִ�.
            sendPos = (Vector3)stream.ReceiveNext();   // �о�� ���� ���� ��ü
            sendRot = (Quaternion)stream.ReceiveNext();   // �о�� ���� ���� ��ü
        }
    }
}
