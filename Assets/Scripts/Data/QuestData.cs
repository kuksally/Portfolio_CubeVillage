using System.Collections;
using System.Collections.Generic;

public class QuestData 
{
    public string questName;    // ����Ʈ �̸�
    public int[] npcId;         // ����Ʈ�� �����Ǿ��ִ� npc id�� �����ϴ� int �迭

    // Ŭ���� ������ ���� �Ű����� ������ �ۼ�
    public QuestData(string name, int[] npc)
    {
        questName = name;
        npcId = npc;
    }
}