using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public int hp;  //  나무, 돌 등 깰 수 있는 아이템들 체력

    public float destroyTime;   // 제거 시간

    public GameObject item; // 기본 아이템
    public GameObject dropItem; // 나무, 돌 등 깰 수 있는 아이템들 모양
    public GameObject itemPrefab;   // 아이템 프리팹
    public BoxCollider boxCollider; // 아이템 콜라이더


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
