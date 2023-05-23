using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RArmFix_Pos_HJW : MonoBehaviour
{
    public GameObject targetObject;  // ������ų ���� ������Ʈ

    private Vector3 fixedPosition = new Vector3(0f, 0.533f, -0.037f);  // ������ ��ġ ��

    private void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned!");
            return;
        }

        // Ÿ�� ������Ʈ�� ��ġ ���� ������ ������ �����մϴ�.
        targetObject.transform.position = fixedPosition;
    }
}
