using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Chicken,
        Wolf,
        Lizard,
        BossSkeleton
    };
    public EnemyType enemyType;

    public int maxHealth;   // 총 체력
    public int curHealth;   // 현재 체력

    public bool isChase;    // 추적
    public bool isAttack = false;
    public bool isDead = false;

    public Transform target;    // 추적 대상
    public BoxCollider closeAttackArea; // 공격 범위
    public GameObject fireball; // 파이어볼 & 보스 돌 굴리기

    public Rigidbody rigidbody;
    public BoxCollider boxCollider;
    //Material material;
    public NavMeshAgent nav;
    public Animator animator;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        //material = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (enemyType != EnemyType.BossSkeleton) Invoke("ChaseStart", 2f);
    }

    void ChaseStart()
    {
        isChase = true;
        animator.SetBool("isWalk", true);
    }

    private void Update()
    {
        if (nav.enabled && enemyType != EnemyType.BossSkeleton)
        {
            nav.SetDestination(target.position);   // SetDestimation() : 도착할 목표 위치 지정 함수
            nav.isStopped = !isChase;
        }

    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            // 물리력이 navAgent 이동을 방해하지 않기 위해
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

    }

    void Targeting()
    {
        if (!isDead && enemyType != EnemyType.BossSkeleton)
        {
            // ShpereCast()의 반지름, 길이 조정 변수
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case EnemyType.Chicken:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case EnemyType.Wolf:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;
                case EnemyType.Lizard:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (raycastHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        // 1. 먼저 정지 
        isChase = false;
        // 2.공격 범위 &  애니메이션 활성화
        isAttack = true;
        animator.SetBool("isAttack", true);

        switch (enemyType)
        {
            case EnemyType.Chicken:
                yield return new WaitForSeconds(0.1f);
                if (closeAttackArea != null) closeAttackArea.enabled = true;
                //Debug.Log(closeAttackArea.enabled);

                yield return new WaitForSeconds(0.6f);
                if (closeAttackArea != null) closeAttackArea.enabled = false;
                //Debug.Log(closeAttackArea.enabled);

                yield return new WaitForSeconds(0.5f);
                break;
            case EnemyType.Wolf:
                yield return new WaitForSeconds(0.1f);
                rigidbody.AddForce(transform.forward * 20, ForceMode.Impulse);  // 돌격
                if (closeAttackArea != null) closeAttackArea.enabled = true;

                yield return new WaitForSeconds(0.5f);    // 멈추기
                rigidbody.velocity = Vector3.zero;
                if (closeAttackArea != null) closeAttackArea.enabled = false;

                yield return new WaitForSeconds(1.5f);
                break;
            case EnemyType.Lizard:
                yield return new WaitForSeconds(0.5f);
                GameObject instantFireball = Instantiate(fireball, transform.position, transform.rotation);
                Rigidbody rigidbodyFireball = instantFireball.GetComponent<Rigidbody>();
                rigidbodyFireball.velocity = transform.forward * 20;

                yield return new WaitForSeconds(1.5f);
                break;
        }

        isChase = true;
        isAttack = false;
        animator.SetBool("isAttack", false);
    }


    private void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)
    {
        // 태그 비교 조건
        if (other.tag == "Sword")    // 근접(검) 공격일 때
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;  // 현재 위치 - 피격 위치 = 반작용 방향
            StartCoroutine(OnDamage(reactVec, false));

            //Debug.Log("Sword : " + curHealth);
        }
        else if (other.tag == "Arrow")
        {
            Arrow arrow = other.GetComponent<Arrow>();
            curHealth -= arrow.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);  // 적과 닿으면 화살 삭제
            StartCoroutine(OnDamage(reactVec, false));
            //Debug.Log("Bow : " + curHealth);
        }
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        //material.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            //material.color = Color.white;
        }
        else
        {
            //material.color = Color.gray;
            gameObject.layer = 14;
            isDead = true;
            isChase = false;        // 사망 후 추적 금지
            nav.enabled = false;    // 사망 리액션 유지하기 위해 비활성화
            animator.SetTrigger("Death");

            // 넉백
            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                rigidbody.freezeRotation = false;
                rigidbody.AddForce(reactVec * 2, ForceMode.Impulse);
                rigidbody.AddTorque(reactVec * 5, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigidbody.AddForce(reactVec * 5, ForceMode.Impulse);
            }

            if (enemyType != EnemyType.BossSkeleton) Destroy(gameObject, 5f);
        }
    }
}
