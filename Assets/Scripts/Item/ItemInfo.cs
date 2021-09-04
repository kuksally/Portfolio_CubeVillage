using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public int hp;  //  ����, �� �� �� �� �ִ� �����۵� ü��

    public float destroyTime;   // ���� �ð�

    public GameObject item; // �⺻ ������
    public GameObject dropItem; // ����, �� �� �� �� �ִ� �����۵� ���
    public GameObject itemPrefab;   // ������ ������
    public BoxCollider boxCollider; // ������ �ݶ��̴�


    public void Mining()
    {
        hp--;

        if(hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        boxCollider.enabled = false;
        Destroy(item);

        dropItem.SetActive(true);
        Destroy(dropItem, destroyTime);

        int count = Random.Range(1, 3);

        for(int i = 0; i< count; i++)
        {
            Instantiate(itemPrefab, itemPrefab.transform.position, Quaternion.identity);
        }
    }
}
