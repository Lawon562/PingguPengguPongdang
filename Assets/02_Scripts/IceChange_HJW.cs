using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceChange_HJW : MonoBehaviour
{

    public Material material1;
    public Material material2;
    public GameObject[] gameObjects;

    private int currentIndex = 0;
    private float waitTime = 1f;

    private bool isChangingMaterials = false;

    void Start()
    {
        // Material ���� �ڷ�ƾ
        StartCoroutine(ChangeMaterialsCoroutine());
    }

    IEnumerator ChangeMaterialsCoroutine()
    {
        while (currentIndex < gameObjects.Length) 
        {
            if (!isChangingMaterials)
            {
                isChangingMaterials = true;
                StartCoroutine(ChangeMaterials());
            }

            yield return null;
        }
    }

    IEnumerator ChangeMaterials()
    {
        List<Renderer> childRenderers = new List<Renderer>(); // �ڽ� ��ü���� Renderer ������Ʈ�� ������ ����Ʈ
        List<Transform> childTransforms = new List<Transform>(); // �ڽ� ��ü���� Transform ������Ʈ�� ������ ����Ʈ



        // �ڽ� ��ü���� Renderer ������Ʈ�� Transform ������Ʈ ��������
        foreach (Transform childTransform in gameObjects[currentIndex].transform)
        {
            Renderer renderer = childTransform.GetComponent<Renderer>();
            childRenderers.Add(renderer);
            childTransforms.Add(childTransform);
        }

        // Material1�� ����
        foreach (Renderer renderer in childRenderers)
        {
            renderer.material = material1;
        }

        yield return new WaitForSeconds(waitTime); // ���׸���1 ���� �� ���׸���2 ������� 1�� ��ٸ�

        // Material2�� ����
        foreach (Renderer renderer in childRenderers)
        {
            renderer.material = material2;
        }

        // ȸ�� ����
        float numRotations = 3f; // �����̼� �ݺ���
        float duration = 0.3f; // �ִϸ��̼� �����ϴ� �ð�
        Quaternion[] startRotations = new Quaternion[childTransforms.Count];        //�ڽİ�ü�� rotation������
        Quaternion[] targetRotations = new Quaternion[childTransforms.Count];       //�ڽİ�ü�� rotation�� target��

        for (int i = 0; i < childTransforms.Count; i++)
        {
            startRotations[i] = childTransforms[i].rotation;        //�ڽİ�ü ������ rotation��
            targetRotations[i] = Quaternion.Euler(-70f, childTransforms[i].rotation.eulerAngles.y, childTransforms[i].rotation.eulerAngles.z);
            //�ڽİ�ü �� Ÿ�� �����̼� ���� x -70f y,z ���� �״��
        }

        for (int i = 0; i < numRotations; i++) //�����̼� Ƚ��
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                for (int j = 0; j < childTransforms.Count; j++)
                {
                    childTransforms[j].rotation = Quaternion.Lerp(startRotations[j], targetRotations[j], t);
                    //�ڽİ�ü ������ ��ŸƮ���� Ÿ���� ������
                }

                yield return null;
            }

            // ȸ���� �Ϸ�Ǹ� ���� ȸ���� ��ǥ ȸ���� ��ȯ�Ͽ� ���� ȸ���� �غ�
            for (int j = 0; j < childTransforms.Count; j++)
            {
                Quaternion tempRotation = startRotations[j];
                startRotations[j] = targetRotations[j];
                targetRotations[j] = tempRotation;
            }
        }

        // �ڽ� ��ü���� ������ �� ����
        Vector3[] startPositions = new Vector3[childTransforms.Count];
        Vector3[] targetPositions = new Vector3[childTransforms.Count];

        for (int i = 0; i < childTransforms.Count; i++)
        {
            startPositions[i] = childTransforms[i].position;
            targetPositions[i] = new Vector3(childTransforms[i].position.x, -20f, childTransforms[i].position.z);
        }

        float positionDuration = 1f;
        float positionElapsedTime = 0f;

        while (positionElapsedTime < positionDuration)
        {
            positionElapsedTime += Time.deltaTime;
            float t = positionElapsedTime / positionDuration;

            for (int i = 0; i < childTransforms.Count; i++)
            {
                childTransforms[i].position = Vector3.Lerp(startPositions[i], targetPositions[i], t);
            }

            yield return null;
        }

        currentIndex++;
        isChangingMaterials = false;
    }
}

