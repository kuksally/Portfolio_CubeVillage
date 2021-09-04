using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        // 1. Add �Լ��� ����ؼ� ��ȭ ����Ʈ �Է��� �߰��Ѵ� 
        // 2. ���� ������ ���Ƿ� sting[] �迭 ���
        // ���� : 1000, â������ : 2000, ������ : 3000, �������� : 4000 ,���� : 5000, �˻� : 6000
        // �������� : 500, ö : 600

        // talkData
        talkData.Add(1000, new string[] { "�ȳ�?", "�츮 ������ �°� ȯ����." });
        talkData.Add(2000, new string[] { "�� ������?", "�������� ó������ ���ε�." });
        talkData.Add(3000, new string[] { "���߿� ã�ƿ�", "���ϴ� ������ �����ٰ�." });
        talkData.Add(4000, new string[] { "��� �����Ծ�?", "��� ���׷��̵� �Ϸ��� ��Ḧ ������." });
        talkData.Add(5000, new string[] { "���°� ���� �� �־�.", "�� �� �ѷ���." });
        talkData.Add(6000, new string[] { "��������ͳ�?", "�Ʒüҿ� ���� �Ʒ��ϵ�����." });


        // questDate
        // ����Ʈ ��ȣ +  NPC Id�� �ش��ϴ� ��ȭ ������ �ۼ�
        #region quest 1
        talkData.Add(10 + 1000, new string[] { "������ �ݰ��� ���� �� ������ �����̾�.",
                                               "������ �� ������� �� ���� â�����⿡�� ������ �� �?" });
        talkData.Add(11 + 2000, new string[] { "�ȳ�?",
                                               "���� â�������. ������ �� ��Ź��!",
                                               "�׷� �ǹ̷� �� ���� ������ ã���� �� ������?",
                                               "�ֺ��� ������ �����ž�." });
        #endregion

        #region quest 2
        talkData.Add(20 + 1000, new string[] { "â�������� ����?", "�۽� ���� �� �ôµ�" });
        talkData.Add(20 + 2000, new string[] { "�� ã���� �߿��Ѱž�." });
        talkData.Add(20 + 500, new string[] { "���� ������ ã�Ҵ�!", "â�����⿡�� ��������" });
        talkData.Add(21 + 2000, new string[] { "ã���༭ ����!", "��Ż ���� �����翡�� ����." });
        #endregion

        #region quest 3
        talkData.Add(30 + 3000, new string[] { "������ ���ο� ���̰� �ʱ���?", "���߿� ã�ƿ��� ���ϴ� ������ �����ٰ�.", "�������̿��� ����������!" });
        talkData.Add(31 + 4000, new string[] { "������ �ݰ���", "�� �ٻڸ� ���� ���꿡 ���� ö �ϳ��� �������ٷ�?" });

        #endregion

        #region quest 4
        talkData.Add(40 + 4000, new string[] { "������ ���� ���ʿ� �־�." });
        talkData.Add(40 + 600, new string[] { "ö�� �����! �������̿��� ���ư���." });
        talkData.Add(41 + 4000, new string[] { "�����༭ ����!" });
        #endregion
    }

    // ������ ��ȭ ������ ��ȯ�ϴ� �Լ�
    public string GetTalk(int id, int talkIndex)
    {
        // ����Ʈ ������ �� ����ó��
        if (!talkData.ContainsKey(id)) // ContainsKey() : Dictionary�� Key�� ���� �ϴ��� �˻�
        {
            if (!talkData.ContainsKey(id - id % 10))
            {
                // ����Ʈ �� ó�� ��縶�� ���� ���� ����Ʈ ��ȣ���� ���� �� ��Ž��
                // �⺻ ��縦 ������ �´�
                return GetTalk(id - id % 100, talkIndex);   // �Ʒ��� ���� �����ϰ� ����, ��ȯ ���� �ִ� ����Լ��� return���� �� ����
                //if (talkIndex == talkData[id - id % 100].Length)
                //{
                //    return null;
                //}
                //else
                //{
                //    return talkData[id - id % 100][talkIndex];
                //}
            }
            else
            {
                // �ش� ����Ʈ ���� ���� ��簡 ���� ��
                // ����Ʈ �� ó�� ��縦 ������ �´�
                return GetTalk(id - id % 10, talkIndex);    // �Ʒ��� ���� �����ϰ� ����
                //if (talkIndex == talkData[id - id % 10].Length)
                //{
                //    return null;
                //}
                //else
                //{
                //    // Id�� ������ ����Ʈ ��ȭ���� ���� �� ��Ž��
                //    return talkData[id - id % 10][talkIndex];
                //}
            }
        }

        if (talkIndex == talkData[id].Length)    // talkIndex�� ��ȭ�� ���� ������ ���Ͽ� ��ȭ�� ���� Ȯ���Ѵ�
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];
        }
    }
}