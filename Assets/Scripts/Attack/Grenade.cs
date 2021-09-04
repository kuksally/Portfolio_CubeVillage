using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effctObj;
    public Rigidbody rigidbody;

    void Start()
    {
        StartCoroutine(Explosion());
    }

    // 시간차 폭발을 위한 코루틴 선언
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2f);
        rigidbody.velocity = Vector3.zero;  // 속도 초기화
        rigidbody.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effctObj.SetActive(true);

        // 피격 효과
        RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));    // SphereCastAll : 구체 모양의 레이케스팅 (모든 오브젝트)
        foreach(RaycastHit hitObj in raycastHits)
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5f);    
    }
}
