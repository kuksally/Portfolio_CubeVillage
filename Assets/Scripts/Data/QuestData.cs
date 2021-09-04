using System.Collections;
using System.Collections.Generic;

public class QuestData 
{
    public string questName;    // 퀘스트 이름
    public int[] npcId;         // 퀘스트와 연관되어있는 npc id를 저장하는 int 배열

    // 클래스 생성을 위한 매개변수 생성자 작성
    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }
}