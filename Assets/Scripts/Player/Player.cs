using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameMgr; // GameManager 함수를 호출할 수 있게 변수 생성
    public QuestManager questMgr;   // QuestManager
    public Camera followCamera; // 마우스 클릭 좌표로 공격하기 위해

    // 플레이어 무기 관련
    public GameObject[] weapons;
    public GameObject[] grenades;   // 공전하는 물체 컨트롤
    public bool[] hasWeapons;
    public int hasGrenades; // 버섯 수류탄
    public GameObject grenadeObj;   // 수류탄 프리팹

    public int arrow;       // 화살
    public int coin;        // 동전
    public int health;      // 체력
    // 최대값 저장
    public int maxArrow;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenades;

    public float moveSpeed;
    public float grenadeSpeed = 10;
    public float jumpPower;

    int equipWeaponIndex = -1;

    // Input Axis 값을 받을 전역변수
    float hAxis;
    float vAxis;

    bool isRun; // run
    bool isJump = false;
    bool isItem = false;    // 무기 먹는 키
    bool isSwap = false;    // 무기 교체 시간차
    bool isSwap1 = false;    // 무기 스왑
    bool isSwap2 = false;    // 무기 스왑
    bool isSwap3 = false;    // 무기 스왑
    bool isSwap4 = false;    // 무기 스왑
    bool isSwap5 = false;    // 무기 스왑
    bool isSwap6 = false;    // 무기 스왑
    bool isReload = false;   // 재장전 키
    bool isReloading = false;   // 재장전 여부
    bool isGrenade = false; // 수류탄

    bool isAttack = false;      // 공격 키
    bool isAttackReady = true; // 공격 준비
    float attackDelay;      // 공격 딜레이

    bool isDamage = false;  // 무적

    Vector3 moveVec;

    Rigidbody rigidbody;
    Animator animator;
    MeshRenderer[] meshRenderers;
    //SkinnedMeshRenderer[] skinnedMeshes
    GameObject scanObject;
    GameObject nearObject;  // 트리거 된 아이템 저장
    Weapon equipWeapon;    // 현재 장착중인 무기

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
            gameMgr.Action(scanObject); // 대화창
        }
    }

    // 키 입력
    void GetInput()
    {
        // GetAxisRaw() : Axis값을 정수로 반환하는 함수
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
        moveVec = new Vector3(hAxis, 0f, vAxis).normalized; // normalized : 방향 값이 1로 보정된 백터

        if (isRun)
        {
            // run
            transform.position += moveVec * moveSpeed * 1.5f * Time.deltaTime;
        }
        else
        {
            // walk
            transform.position += moveVec * moveSpeed * Time.deltaTime; // transform 이동은 꼭 Time.delaTime까지 곱해줘야 한다
        }

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        // swap
        if (isSwap || !isAttackReady || isReloading)   // 스왑과 공격 중에는 이동 불가
        {
            moveVec = Vector3.zero;
        }

        animator.SetBool("isWalk", moveVec != Vector3.zero);
        animator.SetBool("isRun", isRun);
        animator.SetBool("isJump", isJump);
    }

    void Turn()
    {
        // 회전 : 나아가는 방향을 바라볼 것 (키보드)
        transform.LookAt(transform.position + moveVec); // LookAt() : 지정된 Vector를 향해서 회전시켜주는 함수

        // 회전 : 마우스에 의한 회전 (클릭했을 때만)
        if (isAttack)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))  // out : return처럼 반환값을 주어진 변수에 저장
            {
                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 0;  // raycasthit의 높이 무시
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    // 공격
    void Attack()
    {
        // 무기가 있을 때만 실행될 수 있도록 현재 장비를 체크한다
        if (equipWeapon == null) return;

        // 공격 딜레이에 시간을 더해주고 공격 가능 여부를 확인한다
        attackDelay += Time.deltaTime;
        isAttackReady = equipWeapon.rate < attackDelay;

        // 조건이 충족되면 무기에 있는 함수 실행
        if (isAttack && isAttackReady && !isSwap)
        {
            equipWeapon.Use();
            animator.SetTrigger(equipWeapon.weaponType == Weapon.WeaponType.Sword ? "isAttack" : "isShot");
            attackDelay = 0;    // 공격 딜레이를 0 으로 돌려서 다음 공격까지 기다리도록 작성
        }
    }

    void Reload()
    {
        if (equipWeapon == null) return;    // 무기 없으면 넘기기

        if (equipWeapon.weaponType == Weapon.WeaponType.Sword) return;  // 무기 타입이 검이면 넘기기

        if (arrow == 0) return; // 화살이 0개면 넘기기

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
            if (Physics.Raycast(ray, out hit, 100))  // out : return처럼 반환값을 주어진 변수에 저장
            {
                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 5;  // raycasthit의 높이 무시

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

    // 아이템 입수
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

    // 무기 스왑
    void Swap()
    {
        // 무기 중복 교체 & 없는 무기 확인
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
            // 빈손일 경우 예외처리
            if (equipWeapon != null)
            {
                equipWeapon.gameObject.SetActive(false);       // 들고 있는 무기 비활성화 후
            }

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();  // 무기를 바꿔주고
            equipWeapon.gameObject.SetActive(true);                     // 바꿔준 무기를 활성화 시킨다

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

    // 리액션 코루틴
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