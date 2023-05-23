using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaterBody_rotFixed_HJW : MonoBehaviour
{
    private float fixedXRotation;  // ������ X �����̼� ��
    private float fixedZRotation;  // ������ Z �����̼� ��

    private void Start()
    {
        // ������ �� ���� �����̼� ���� ����մϴ�.
        fixedXRotation = transform.rotation.eulerAngles.x;
        fixedZRotation = transform.rotation.eulerAngles.z;
    }

    private void LateUpdate()
    {
        // �����̼� ���� ������ ������ �����մϴ�.
        Quaternion fixedRotation = Quaternion.Euler(fixedXRotation, transform.rotation.eulerAngles.y, fixedZRotation);
        transform.rotation = fixedRotation;
    }
}
