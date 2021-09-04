using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;    // ���� Ÿ��

    public float orbitSpeed;    // ���� �ӵ�

    Vector3 offset; // ��ǥ���� �Ÿ�

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        transform.position = target.position + offset;
        // RotateAround() : Ÿ�� ������ ȸ���ϴ� �Լ�, ��ǥ�� �����̸� �ϱ׷����� ������ �ִ�
        // 1.Ÿ�� ��ǥ , 2. ȸ����, 3. �ӵ�
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        // RotateAround()���� ��ġ�� ������ ��ǥ���� �Ÿ��� ������Ų��
        offset = transform.position - target.position;
    }
}
