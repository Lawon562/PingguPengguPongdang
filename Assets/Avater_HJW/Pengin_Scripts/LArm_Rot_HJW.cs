using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LArm_Rot_HJW : MonoBehaviour
{
    public GameObject object1;  // �����̼� ���� �޾ƿ� ���� ������Ʈ
    public GameObject object2;  // �����̼� ���� ������ ���� ������Ʈ

    private void Update()
    {
        if (object1 == null || object2 == null)
        {
            Debug.LogError("Object references are not assigned!");
            return;
        }

        // object1�� �����̼� ���� object2�� �����մϴ�.
        object2.transform.rotation = object1.transform.rotation;
    }
}
