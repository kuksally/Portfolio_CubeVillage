using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameMgr; // GameManager �Լ��� ȣ���� �� �ְ� ���� ����
    public QuestManager questMgr;   // QuestManager
    public Camera followCamera; // ���콺 Ŭ�� ��ǥ�� �����ϱ� ����

    // �÷��̾� ���� ����
    public GameObject[] weapons;
    public GameObject[] grenades;   // �����ϴ� ��ü ��Ʈ��
    public bool[] hasWeapons;
    public int hasGrenades; // ���� ����ź
    public GameObject grenadeObj;   // ����ź ������

    public int arrow;       // ȭ��
    public int coin;        // ����
    public int health;      // ü��
    // �ִ밪 ����
    public int maxArrow;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    public float moveSpeed;
    public float grenadeSpeed = 10;
    public float jumpPower;

    int equipWeaponIndex = -1;

    // Input Axis ���� ���� ��������
    float hAxis;
    float vAxis;

    bool isRun; // run
    bool isJump = false;
    bool isItem = false;    // ���� �Դ� Ű
    bool isSwap = false;    // ���� ��ü �ð���
    bool isSwap1 = false;    // ���� ����
    bool isSwap2 = false;    // ���� ����
    bool isSwap3 = false;    // ���� ����
    bool isSwap4 = false;    // ���� ����
    bool isSwap5 = false;    // ���� ����
    bool isSwap6 = false;    // ���� ����
    bool isReload = false;   // ������ Ű
    bool isReloading = false;   // ������ ����
    bool isGrenade = false; // ����ź

    bool isAttack = false;      // ���� Ű
    bool isAttackReady = true; // ���� �غ�
    float attackDelay;      // ���� ������

    bool isDamage = false;  // ����

    Vector3 moveVec;

    Rigidbody rigidbody;
    Animator animator;
    MeshRenderer[] meshRenderers;
    //SkinnedMeshRenderer[] skinnedMeshes
    GameObject scanObject;
    GameObject nearObject;  // Ʈ���� �� ������ ����
    Weapon equipWeapon;    // ���� �������� ����

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        questMgr = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    private void Update()
    {
        GetInput();
        Move();
        Turn();
        Attack();
        Reload();
        Grenade();
        Interaction();
        Swap();

        // ScanObject
        if (Input.GetMouseButtonDown(0) && scanObject != null)
        {
            gameMgr.Action(scanObject); // ��ȭâ
        }
    }

    // Ű �Է�
    void GetInput()
    {
        // GetAxisRaw() : Axis���� ������ ��ȯ�ϴ� �Լ�
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        isRun = Input.GetButton("Run");
        isJump = Input.GetButtonDown("Jump");
        isAttack = Input.GetButton("Attack");
        isGrenade = Input.GetButtonDown("Grenade");
        isItem = Input.GetButtonDown("Interaction");
        isSwap1 = Input.GetButtonDown("Swap1");
        isSwap2 = Input.GetButtonDown("Swap2");
        isSwap3 = Input.GetButtonDown("Swap3");
        isSwap4 = Input.GetButtonDown("Swap4");
        isSwap5 = Input.GetButtonDown("Swap5");
        isSwap6 = Input.GetButtonDown("Swap6");
        isReload = Input.GetButtonDown("Reload");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0f, vAxis).normalized; // normalized : ���� ���� 1�� ������ ����

        if (isRun)
        {
            // run
            transform.position += moveVec * moveSpeed * 1.5f * Time.deltaTime;
        }
        else
        {
            // walk
            transform.position += moveVec * moveSpeed * Time.deltaTime; // transform �̵��� �� Time.delaTime���� ������� �Ѵ�
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        // swap
        if (isSwap || !isAttackReady || isReloading)   // ���Ұ� ���� �߿��� �̵� �Ұ�
        {
            moveVec = Vector3.zero;
        }

        animator.SetBool("isWalk", moveVec != Vector3.zero);
        animator.SetBool("isRun", isRun);
        animator.SetBool("isJump", isJump);
    }

    void Turn()
    {
        // ȸ�� : ���ư��� ������ �ٶ� �� (Ű����)
        transform.LookAt(transform.position + moveVec); // LookAt() : ������ Vector�� ���ؼ� ȸ�������ִ� �Լ�

        // ȸ�� : ���콺�� ���� ȸ�� (Ŭ������ ����)
        if (isAttack)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))  // out : returnó�� ��ȯ���� �־��� ������ ����
            {
                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 0;  // raycasthit�� ���� ����
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    // ����
    void Attack()
    {
        // ���Ⱑ ���� ���� ����� �� �ֵ��� ���� ��� üũ�Ѵ�
        if (equipWeapon == null) return;

        // ���� �����̿� �ð��� �����ְ� ���� ���� ���θ� Ȯ���Ѵ�
        attackDelay += Time.deltaTime;
        isAttackReady = equipWeapon.rate < attackDelay;

        // ������ �����Ǹ� ���⿡ �ִ� �Լ� ����
        if (isAttack && isAttackReady && !isSwap)
        {
            equipWeapon.Use();
            animator.SetTrigger(equipWeapon.weaponType == Weapon.WeaponType.Sword ? "isAttack" : "isShot");
            attackDelay = 0;    // ���� �����̸� 0 ���� ������ ���� ���ݱ��� ��ٸ����� �ۼ�
        }
    }

    void Reload()
    {
        if (equipWeapon == null) return;    // ���� ������ �ѱ��

        if (equipWeapon.weaponType == Weapon.WeaponType.Sword) return;  // ���� Ÿ���� ���̸� �ѱ��

        if (arrow == 0) return; // ȭ���� 0���� �ѱ��

        if (isReload && !isJump && !isSwap && isAttackReady)
        {
            isReloading = true;

            Invoke("ReloadOut", 1f);
        }
    }

    void Grenade()
    {
        if (hasGrenades == 0) return;

        if (isGrenade && !isReloading && !isSwap)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))  // out : returnó�� ��ȯ���� �־��� ������ ����
            {
                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 5;  // raycasthit�� ���� ����

                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                Rigidbody rigidbodyGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidbodyGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidbodyGrenade.AddTorque(Vector3.back * grenadeSpeed, ForceMode.Impulse);

                hasGrenades--;
                grenades[hasGrenades].SetActive(false);
            }
        }
    }

    void ReloadOut()
    {
        int reArrow = arrow < equipWeapon.maxArrow ? arrow : equipWeapon.maxArrow;
        equipWeapon.curArrow = reArrow;
        arrow -= reArrow;
        isReloading = false;
    }

    // ������ �Լ�
    void Interaction()
    {
        if (isItem && nearObject != null && !isJump)
        {
            if (nearObject.tag == "Weapon")
            {
                Item_Test item = nearObject.GetComponent<Item_Test>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject);
            }
        }
    }

    // ���� ����
    void Swap()
    {
        // ���� �ߺ� ��ü & ���� ���� Ȯ��
        if (isSwap1 && (!hasWeapons[0] || equipWeaponIndex == 0)) return;
        if (isSwap2 && (!hasWeapons[1] || equipWeaponIndex == 1)) return;
        if (isSwap3 && (!hasWeapons[2] || equipWeaponIndex == 2)) return;
        if (isSwap4 && (!hasWeapons[3] || equipWeaponIndex == 3)) return;
        if (isSwap5 && (!hasWeapons[4] || equipWeaponIndex == 4)) return;
        if (isSwap6 && (!hasWeapons[5] || equipWeaponIndex == 5)) return;

        int weaponIndex = -1;

        if (isSwap1) weaponIndex = 0;
        if (isSwap2) weaponIndex = 1;
        if (isSwap3) weaponIndex = 2;
        if (isSwap4) weaponIndex = 3;
        if (isSwap5) weaponIndex = 4;
        if (isSwap6) weaponIndex = 5;

        if ((isSwap1 || isSwap2 || isSwap3 || isSwap4 || isSwap5 || isSwap6) && !isJump && !isSwap)
        {
            // ����� ��� ����ó��
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);       // ��� �ִ� ���� ��Ȱ��ȭ ��
            }

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();  // ���⸦ �ٲ��ְ�
            equipWeapon.gameObject.SetActive(true);                     // �ٲ��� ���⸦ Ȱ��ȭ ��Ų��

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void FreezeRotation()
    {
        rigidbody.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // Raycast
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Object")))
            {
                //Debug.Log(hit.collider.gameObject.name);
            }

            // ScanObject
            if (hit.collider != null)
            {
                scanObject = hit.collider.gameObject;
                questMgr.currentObject = hit.collider.gameObject;
                //if(hit.collider.gameObject.tag == "Item")
                //{
                //    scanObject.SetActive(false);
                //}
            }
            else
            {
                scanObject = null;
            }
        }

        FreezeRotation();
    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Item")
        //{
        //    Item_Test item_test = other.GetComponent<Item_Test>();
        //    switch (item_test.type)
        //    {
        //        case Item_Test.Type.Arrow:
        //            arrow += item_test.value;
        //            if (arrow > maxArrow) arrow = maxArrow;
        //            break;
        //        case Item_Test.Type.Coin:
        //            coin += item_test.value;
        //            if (coin > maxCoin) coin = maxCoin;
        //            break;
        //        case Item_Test.Type.Heart:
        //            health += item_test.value;
        //            if (health > maxHealth) health = maxHealth;
        //            break;
        //        case Item_Test.Type.Grenade:
        //            grenades[hasGrenades].SetActive(true);
        //            hasGrenades += item_test.value;
        //            if (hasGrenades > maxHasGrenades) hasGrenades = maxHasGrenades;
        //            break;
        //    }
        //    Destroy(other.gameObject);
        //}
        if (other.tag == "EnemyAttack")
        {
            if (!isDamage)
            {
                Arrow enemyArrow = other.GetComponent<Arrow>();
                health -= enemyArrow.damage;

                bool isBossAttack = other.name == "JumpAttack Area";

                StartCoroutine(OnDamage(isBossAttack));
            }

            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
        }
    }

    // ���׼� �ڷ�ƾ
    IEnumerator OnDamage(bool isBossAttack)
    {
        isDamage = true;

        //foreach(MeshRenderer mesh in meshRenderers)
        //{
        //    mesh.material.color = Color.yellow;
        //}

        if(isBossAttack)
        {
            rigidbody.AddForce(transform.forward * -5, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1f);

        isDamage = false;
        //foreach (MeshRenderer mesh in meshRenderers)
        //{
        //    mesh.material.color = Color.white;
        //}

        if(isBossAttack)
        {
            rigidbody.velocity = Vector3.zero;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }

        //Debug.Log(nearObject.name);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;
        }
    }
}