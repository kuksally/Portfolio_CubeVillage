using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;                 // 현재 진행중인 퀘스트 id
    public int questActionIndex;        // 퀘스트 대화 순서
    public GameObject[] questObject;    // 퀘스트 오브젝트를 저장
    public GameObject currentObject;

    Dictionary<int, QuestData> questList;  // 퀘스트 데이터를 저장할 Dictionary 변수

    void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        GenerateData();
    }

    void GenerateData()
    {
        // Add 함수로 <퀘스트 id, 퀘스트 data> 저장
        // int[]에는 해당 퀘스트에 연관된 NPC Id를 입력한다
        questList.Add(10, new QuestData("마을 사람들에게 인사하기", new int[] { 1000, 2000 }));
        questList.Add(20, new QuestData("창고지기의 도끼 찾아주기", new int[] { 500, 2000 }));
        questList.Add(30, new QuestData("마법사 만나기", new int[] { 3000, 4000 }));
        questList.Add(40, new QuestData("재료 구해오기", new int[] { 600, 4000 }));

        questList.Add(0, new QuestData("퀘스트 클리어", new int[] { 0 }));
    }

    // NPC Id를 받고 퀘스트 번호를 반환하는 함수
    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    // 대화 진행을 위한 퀘스트 대화 순서를 올리는 함수
    // 퀘스트 이름을 콘솔창에 반환하도록 함수 개조
    public string CheckQuest(int id)
    {
        // 대화가 끝났을 때
        // 순서에 맞게 대화를 했을 때만 퀘스트 대화 순서를 올리게 한다 (다음 퀘스트 타겟)
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }

        // Control Quest Object
        ControlObject();

        // 퀘스트 대화 순서가 끝에 도달했을 때 퀘스트 번호가 증가하도록 한다 (대화 완료 & 다음 퀘스트)
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        // 현재 퀘스트 이름 확인
        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    // 다음 퀘스트를 위한 함수
    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;

        if(questId == 50)
        {
            questId = 0;
        }
    }

    // 퀘스트 오브젝트를 관리할 함수
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
                    questObject[0].SetActive(true); // npc2(창고지기와 대화 끝났을 떄 on)
                }
                break;
            case 20:
                if (questActionIndex == 0)
                {
                    questObject[0].SetActive(true);
                }
                else if (questActionIndex == 1)
                {
                    questObject[0].SetActive(false);    // 퀘스트 아이템을 먹었을 때 false
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
