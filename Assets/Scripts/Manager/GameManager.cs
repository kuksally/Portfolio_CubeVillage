using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public QuestManager questMgr;
    public TalkManager talkMgr;
    public GameObject talkPanel;    // ��ȭâ
    public GameObject menuSet;  // �޴�â
    public Text talkText;           // ��ȭ �ؽ�Ʈ
    public Text questText;      // �޴�â���� ����Ʈ ���� ����
    public GameObject player;
    public GameObject scanObject;   // Ȯ�� �� ������Ʈ

    public int talkIndex;

    public bool isAction;   // ���� ����� ����

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
        // ESC ������ �޴� ������
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
        talkPanel.SetActive(isAction);  // SetActive() �Լ��� ��ȭâ ����� & �����ֱ� ����
    }

    void Talk(int id, bool isNpc)
    {
        // Set Talk Data
        int questTalkIndex = questMgr.GetQuestTalkIndex(id);    // ����Ʈ ��ȣ ��������
        string talkData = talkMgr.GetTalk(id + questTalkIndex, talkIndex);  // ����Ʈ ��ȣ + NPC Id = ����Ʈ ��ȭ ������ Id

        // End Talk
        if (talkData == null)
        {
            isAction = false;   // ��ȭ ��
            talkIndex = 0;
            questText.text = questMgr.CheckQuest(id);
            //Debug.Log(questMgr.CheckQuest(id));
            return; // void �Լ����� return�� ���� ���� ����
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
        talkIndex++;    // ���� ��ȭ�� �̾� ������ ���� index�� �����ش�
    }

    public void GameSave()
    {
        // PlayerPrefs : ������ ������ ���� ����� �����ϴ� Ŭ����

        // player ��ǥ
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);

        // quest ����
        PlayerPrefs.SetInt("QuestId", questMgr.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questMgr.questActionIndex);

        // save
        PlayerPrefs.Save();

        // �޴� �ݱ�
        menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        // ���� ���� ����ÿ��� �����Ͱ� ������ ����ó��
        // ������Ʈ�� ��� : HKEY_CURRENT_USER/Software
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
        // �����Ϳ����� ������� ������ �����ؼ� Ȯ���ؾ���
        Application.Quit();
    }
}
