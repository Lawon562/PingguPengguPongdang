using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour
{
    //[SerializeField]
    //private Transform Player;
    public Transform Player;
    float xAxis;
    // Start is called before the first frame update
    void Start()
    {
        xAxis = Camera.main.transform.rotation.eulerAngles.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            this.transform.position = Player.position + new Vector3(0, 0.45f, 0.3f);
        }
        else
        {
            this.transform.position = Vector3.zero;
        }
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        RotateCamera(mouseX, mouseY);

    }

    void RotateCamera(float mouseX, float mouseY)
    {
        //// ȸ�� �ӵ� ���� ����
        float rotationSpeed = 2.0f;
        xAxis -= mouseY * rotationSpeed; // Y�� ȸ�� �� ����
        xAxis = Mathf.Clamp(xAxis, -70f, 80f); // ���� ���� (0��~40��)
        transform.rotation = Quaternion.Euler(xAxis, transform.rotation.eulerAngles.y + mouseX * rotationSpeed, 0f); // ȸ�� �� ����
    }
}