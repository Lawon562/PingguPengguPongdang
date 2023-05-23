using UnityEngine;
using System.Globalization;

public class KoreanNameGenerator : MonoBehaviour
{
    private static readonly string[] frontWords = { "�ż���", "��û��", "��ο�", "����", "�밨��", "������", "�Ƹ��ٿ�", "�Ŵ���", "����", "ȭ����", "�밨��", "����" };
    private static readonly string[] backWords = { "Ź��", "����", "����", "��ǻ��", "����", "�Ź�", "��", "�ð�", "�Ȱ�", "ȭ��ǰ", "ī�޶�", "å", "��Ʈ��", "�����", "��Ź��", "���", "����" };

    
    public string GenerateKoreanName()
    {
        int syllableCount = Random.Range(2, 4); // �̸��� 2~3������ ����
        string name = frontWords[Random.Range(0, frontWords.Length)];
        name += backWords[Random.Range(0, frontWords.Length)];
        return name;
    }
}