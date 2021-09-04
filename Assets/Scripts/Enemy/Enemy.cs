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

    public int maxHealth;   // �� ü��
    public int curHealth;   // ���� ü��

    public bool isChase;    // ����
    public bool isAttack = false;
    public bool isDead = false;

    public Transform target;    // ���� ���
    public BoxCollider closeAttackArea; // ���� ����
    public GameObject fireball; // ���̾ & ���� �� ������

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
            nav.SetDestination(target.position);   // SetDestimation() : ������ ��ǥ ��ġ ���� �Լ�
            nav.isStopped = !isChase;
        }

    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            // �������� navAgent �̵��� �������� �ʱ� ����
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

    }

    void Targeting()
    {
        if (!isDead && enemyType != EnemyType.BossSkeleton)
        {
            // ShpereCast()�� ������, ���� ���� ����
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
        // 1. ���� ���� 
        isChase = false;
        // 2.���� ���� &  �ִϸ��̼� Ȱ��ȭ
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
                rigidbody.AddForce(transform.forward * 20, ForceMode.Impulse);  // ����
                if (closeAttackArea != null) closeAttackArea.enabled = true;

                yield return new WaitForSeconds(0.5f);    // ���߱�
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
        // �±� �� ����
        if (other.tag == "Sword")    // ����(��) ������ ��
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;  // ���� ��ġ - �ǰ� ��ġ = ���ۿ� ����
            StartCoroutine(OnDamage(reactVec, false));

            //Debug.Log("Sword : " + curHealth);
        }
        else if (other.tag == "Arrow")
        {
            Arrow arrow = other.GetComponent<Arrow>();
            curHealth -= arrow.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);  // ���� ������ ȭ�� ����
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
            isChase = false;        // ��� �� ���� ����
            nav.enabled = false;    // ��� ���׼� �����ϱ� ���� ��Ȱ��ȭ
            animator.SetTrigger("Death");

            // �˹�
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
