using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // ���� Ÿ��
    public enum WeaponType
    {
        Axe,
        PickAxe,
        Sword,
        Bow
    };

    public WeaponType weaponType;

    public int damage;  // ������

    public int maxArrow;    // �ִ� ȭ��
    public int curArrow;    // ���� ���� ȭ��

    public float rate; // ���� �ӵ�
    public float arrowSpeed = 50;   // ȭ�� �ӵ�

    public BoxCollider weaponArea;  // ���� ����

    public Transform arrowPos;  // ȭ�� ���� ��ġ
    public GameObject arrow;    // ������ ����

    //public TrailRenderer trailEffect;   // ���� ȿ��

    public void Use()
    {
        if(weaponType == WeaponType.Sword)
        {
            StopCoroutine("Swing");     // �ڷ�ƾ ���� �Լ�
            StartCoroutine("Swing");    // �ڷ�ƾ ���� �Լ�
        }
        else if(weaponType == WeaponType.Bow && curArrow > 0)
        {
            curArrow--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        weaponArea.enabled = true;
        //trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        weaponArea.enabled = false;

        //yield return new WaitForSeconds(0.3f);
        //trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // �Ѿ� �߻� : Instantiate() �Լ��� ȭ�� �ν��Ͻ�ȭ, ������, ��ǥ, ����?
        GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigidbody = instantArrow.GetComponent<Rigidbody>();
        arrowRigidbody.velocity = arrowPos.forward * arrowSpeed;
        yield return null;
    }
}
