using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject fire; // ���̾
    public Transform firePortA;
    public Transform firePortB;

    Vector3 lookVec;    // �÷��̾� ������ ����
    Vector3 tauntVec;   // jumpAttack

    public bool isLook = false;        // �÷��̾ �ٶ󺸴� �÷���

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        //material = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        isLook = true;
        nav.isStopped = true;   // ���� ���� �� �� ��ǥ�������� �̵�
        StartCoroutine(Think());
    }


    void Update()
    {
        if(isDead)
        {
            StopAllCoroutines();
            return; // �Ʒ� ���� �������� �ʵ��� ����
        }

        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            lookVec = new Vector3(h, 0, v) * 5f;    // �÷��̾� �Է°����� ���� ���Ͱ� ����
            transform.LookAt(target.position + lookVec);
        }
        else
        {
            nav.SetDestination(tauntVec);   // ���� ���� �� �� ��ǥ�������� �̵�
        }
    }

    // �ൿ ���� �������ֱ�
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.9f);

        int randomAction = Random.Range(0, 5);

        // switch������ break�� �����ϸ� ������ �ø� �� �ִ�
        switch (randomAction)
        {
            case 0:
            case 1:
                // ���̾ �߻� ����
                StartCoroutine(Fireball());
                break;
            case 2:
            case 3:
                // �� ������ ����
                StartCoroutine(RockShot());
                break;
            case 4:
                // ���� ���� ����
                StartCoroutine(JumpAttack());
                break;
        }
    }

    IEnumerator Fireball()
    {
        animator.SetTrigger("isFireball");
        yield return new WaitForSeconds(0.5f);
        GameObject instantFireballA = Instantiate(fire, firePortA.position, firePortA.rotation);   // ���̾ ����
        BossFire bossFireballA = instantFireballA.GetComponent<BossFire>();
        bossFireballA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantFireballB = Instantiate(fire, firePortB.position, firePortB.rotation);   // ���̾ ����
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
        tauntVec = target.position + lookVec;   // ���� ���� �� ��ġ ����

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