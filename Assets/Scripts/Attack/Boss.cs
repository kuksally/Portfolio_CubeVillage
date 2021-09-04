using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject fire; // 파이어볼
    public Transform firePortA;
    public Transform firePortB;

    Vector3 lookVec;    // 플레이어 움직임 예측
    Vector3 tauntVec;   // jumpAttack

    public bool isLook = false;        // 플레이어를 바라보는 플래그

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        //material = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        isLook = true;
        nav.isStopped = true;   // 점프 공격 할 때 목표지점으로 이동
        StartCoroutine(Think());
    }


    void Update()
    {
        if(isDead)
        {
            StopAllCoroutines();
            return; // 아래 로직 실행하지 않도록 리턴
        }

        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            lookVec = new Vector3(h, 0, v) * 5f;    // 플레이어 입력값으로 예측 벡터값 생성
            transform.LookAt(target.position + lookVec);
        }
        else
        {
            nav.SetDestination(tauntVec);   // 점프 공격 할 때 목표지점으로 이동
        }
    }

    // 행동 패턴 결정해주기
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.9f);

        int randomAction = Random.Range(0, 5);

        // switch문에서 break를 생략하면 조건을 늘릴 수 있다
        switch (randomAction)
        {
            case 0:
            case 1:
                // 파이어볼 발사 패턴
                StartCoroutine(Fireball());
                break;
            case 2:
            case 3:
                // 돌 굴리기 패턴
                StartCoroutine(RockShot());
                break;
            case 4:
                // 점프 공격 패턴
                StartCoroutine(JumpAttack());
                break;
        }
    }

    IEnumerator Fireball()
    {
        animator.SetTrigger("isFireball");
        yield return new WaitForSeconds(0.5f);
        GameObject instantFireballA = Instantiate(fire, firePortA.position, firePortA.rotation);   // 파이어볼 생성
        BossFire bossFireballA = instantFireballA.GetComponent<BossFire>();
        bossFireballA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantFireballB = Instantiate(fire, firePortB.position, firePortB.rotation);   // 파이어볼 생성
        BossFire bossFireballB = instantFireballB.GetComponent<BossFire>();
        bossFireballB.target = target;

        yield return new WaitForSeconds(2f);
        Destroy(instantFireballA);
        Destroy(instantFireballB);

        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        isLook = false;
        animator.SetTrigger("isRockShot");
        Instantiate(fireball, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator JumpAttack()
    {
        tauntVec = target.position + lookVec;   // 점프 공격 할 위치 저장

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        animator.SetTrigger("isJumpAttack");

        yield return new WaitForSeconds(1.5f);
        if (closeAttackArea != null)closeAttackArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        if (closeAttackArea != null) closeAttackArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;

        StartCoroutine(Think());
    }
}