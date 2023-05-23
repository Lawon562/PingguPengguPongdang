using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RArm_Rot_HJW : MonoBehaviour
{
    public GameObject object3;  // �����̼� ���� �޾ƿ� ���� ������Ʈ
    public GameObject object4;  // �����̼� ���� ������ ���� ������Ʈ

    private void Update()
    {
        if (object3 == null || object4 == null)
        {
            Debug.LogError("Object references are not assigned!");
            return;
        }

        // object1�� �����̼� ���� object2�� �����մϴ�.
        object4.transform.rotation = object3.transform.rotation;
    }
}
