using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 무기 타입
    public enum WeaponType
    {
        Axe,
        PickAxe,
        Sword,
        Bow
    };

    public WeaponType weaponType;

    public int damage;  // 데미지

    public int maxArrow;    // 최대 화살
    public int curArrow;    // 현재 보유 화살

    public float rate; // 공격 속도
    public float arrowSpeed = 50;   // 화살 속도

    public BoxCollider weaponArea;  // 공격 범위

    public Transform arrowPos;  // 화살 생성 위치
    public GameObject arrow;    // 프리팹 저장

    //public TrailRenderer trailEffect;   // 공격 효과

    public void Use()
    {
        if(weaponType == WeaponType.Sword)
        {
            StopCoroutine("Swing");     // 코루틴 정지 함수
            StartCoroutine("Swing");    // 코루틴 시작 함수
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
        // 총알 발사 : Instantiate() 함수로 화살 인스턴스화, 프리팹, 좌표, 각도?
        GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigidbody = instantArrow.GetComponent<Rigidbody>();
        arrowRigidbody.velocity = arrowPos.forward * arrowSpeed;
        yield return null;
    }
}
