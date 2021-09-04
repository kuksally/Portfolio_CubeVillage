using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Test : MonoBehaviour
{
    // ���� Ÿ��
    public enum Type
    {
        Arrow,
        Coin,
        Grenade,
        Heart,
        Weapon
    };

    public Type type;

    public int value;

    public float rotateSpeed = 0;   // ȸ�� �ӵ�

    Rigidbody rigidbody;
    SphereCollider sphereCollider;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        // ������ ȸ��
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            rigidbody.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
