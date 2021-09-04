using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public QuestManager questMgr;
    public TalkManager talkMgr;
    public GameObject talkPanel;    // 대화창
    public GameObject menuSet;  // 메뉴창
    public Text talkText;           // 대화 텍스트
    public Text questText;      // 메뉴창에서 퀘스트 진행 사항
    public GameObject player;
    public GameObject scanObject;   // 확인 한 오브젝트

    public int talkIndex;

    public bool isAction;   // 상태 저장용 변수

    private void Start()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            GameLoad();
        }
        questText.text = questMgr.CheckQuest();
        //Debug.Log(questMgr.CheckQuest());
    }

    void Update()
    {
        // ESC 누르면 메뉴 나오게
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf)
            {
                menuSet.SetActive(false);
            }
            else
            {
                menuSet.SetActive(true);
            }
        }
    }

    public void Action(GameObject scanObj)
    {
        // Get Current Object
        scanObject = scanObj;
        ObjectData objectData = scanObject.GetComponent<ObjectData>();
        Talk(objectData.id, objectData.isNpc);

        // Visible Talk for Action
        talkPanel.SetActive(isAction);  // SetActive() 함수로 대화창 숨기기 & 보여주기 구현
    }

    void Talk(int id, bool isNpc)
    {
        // Set Talk Data
        int questTalkIndex = questMgr.GetQuestTalkIndex(id);    // 퀘스트 번호 가져오기
        string talkData = talkMgr.GetTalk(id + questTalkIndex, talkIndex);  // 퀘스트 번호 + NPC Id = 퀘스트 대화 데이터 Id

        // End Talk
        if (talkData == null)
        {
            isAction = false;   // 대화 끝
            talkIndex = 0;
            questText.text = questMgr.CheckQuest(id);
            //Debug.Log(questMgr.CheckQuest(id));
            return; // void 함수에서 return은 강제 종료 역할
        }

        // Continue Talk
        if (isNpc)
        {
            talkText.text = talkData;
        }
        else
        {
            talkText.text = talkData;
        }

        isAction = true;
        talkIndex++;    // 다음 대화를 이어 나가기 위해 index를 더해준다
    }

    public void GameSave()
    {
        // PlayerPrefs : 간단한 데이터 저장 기능을 지원하는 클래스

        // player 좌표
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);

        // quest 정보
        PlayerPrefs.SetInt("QuestId", questMgr.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questMgr.questActionIndex);

        // save
        PlayerPrefs.Save();

        // 메뉴 닫기
        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        // 게임 최초 실행시에는 데이터가 없으니 예외처리
        // 레지스트리 경로 : HKEY_CURRENT_USER/Software
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            return;
        }

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        float z = PlayerPrefs.GetFloat("PlayerZ");

        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, z);

        questMgr.questId = questId;
        questMgr.questActionIndex = questActionIndex;
        questMgr.ControlObject();
    }

    public void GameExit()
    {
        // 에디터에서는 실행되지 않으니 빌드해서 확인해야함
        Application.Quit();
    }
}
