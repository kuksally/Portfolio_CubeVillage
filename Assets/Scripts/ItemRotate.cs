using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    private float rotateSpeed = 30;   // ȸ�� �ӵ�

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
        if (collision.gameObject.tag == "Ground")
        {
            rigidbody.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }

}
