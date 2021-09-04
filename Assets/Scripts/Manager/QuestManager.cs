using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;                 // ���� �������� ����Ʈ id
    public int questActionIndex;        // ����Ʈ ��ȭ ����
    public GameObject[] questObject;    // ����Ʈ ������Ʈ�� ����
    public GameObject currentObject;

    Dictionary<int, QuestData> questList;  // ����Ʈ �����͸� ������ Dictionary ����

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        // Add �Լ��� <����Ʈ id, ����Ʈ data> ����
        // int[]���� �ش� ����Ʈ�� ������ NPC Id�� �Է��Ѵ�
        questList.Add(10, new QuestData("���� ����鿡�� �λ��ϱ�", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("â�������� ���� ã���ֱ�", new int[] { 500, 2000 }));
        questList.Add(30, new QuestData("������ ������", new int[] { 3000, 4000 }));
        questList.Add(40, new QuestData("��� ���ؿ���", new int[] { 600, 4000 }));

        questList.Add(0, new QuestData("����Ʈ Ŭ����", new int[] { 0 }));
    }

    // NPC Id�� �ް� ����Ʈ ��ȣ�� ��ȯ�ϴ� �Լ�
    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    // ��ȭ ������ ���� ����Ʈ ��ȭ ������ �ø��� �Լ�
    // ����Ʈ �̸��� �ܼ�â�� ��ȯ�ϵ��� �Լ� ����
    public string CheckQuest(int id)
    {
        // ��ȭ�� ������ ��
        // ������ �°� ��ȭ�� ���� ���� ����Ʈ ��ȭ ������ �ø��� �Ѵ� (���� ����Ʈ Ÿ��)
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }

        // Control Quest Object
        ControlObject();

        // ����Ʈ ��ȭ ������ ���� �������� �� ����Ʈ ��ȣ�� �����ϵ��� �Ѵ� (��ȭ �Ϸ� & ���� ����Ʈ)
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        // ���� ����Ʈ �̸� Ȯ��
        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    // ���� ����Ʈ�� ���� �Լ�
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;

        if(questId == 50)
        {
            questId = 0;
        }
    }

    // ����Ʈ ������Ʈ�� ������ �Լ�
    public void ControlObject()
    {
        switch (questId)
        {
            case 10:
                //if (questActionIndex == 0)
                //{
                //    questObject[0].SetActive(true);
                //}
                if (questActionIndex == 2)
                {
                    questObject[0].SetActive(true); // npc2(â������� ��ȭ ������ �� on)
                }
                break;
            case 20:
                if (questActionIndex == 0)
                {
                    questObject[0].SetActive(true);
                }
                else if (questActionIndex == 1)
                {
                    questObject[0].SetActive(false);    // ����Ʈ �������� �Ծ��� �� false
                }
                break;
            case 30:
                if (questActionIndex == 0)
                {
                    questObject[1].SetActive(false);
                }
                if (questActionIndex == 1)
                {
                    questObject[1].SetActive(true);
                }
                break;
            case 40:
                if (questActionIndex == 0)
                {
                    questObject[1].SetActive(true);
                }
                else if (questActionIndex == 1)
                {
                    questObject[1].SetActive(false);
                }
                break;
        }
    }
}
