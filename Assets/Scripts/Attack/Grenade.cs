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

    // �ð��� ������ ���� �ڷ�ƾ ����
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2f);
        rigidbody.velocity = Vector3.zero;  // �ӵ� �ʱ�ȭ
        rigidbody.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effctObj.SetActive(true);

        // �ǰ� ȿ��
        RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));    // SphereCastAll : ��ü ����� �����ɽ��� (��� ������Ʈ)
        foreach(RaycastHit hitObj in raycastHits)
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5f);    
    }
}
