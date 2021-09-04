using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;    // 공전 타겟

    public float orbitSpeed;    // 공전 속도

    Vector3 offset; // 목표와의 거리

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        transform.position = target.position + offset;
        // RotateAround() : 타겟 주위를 회전하는 함수, 목표가 움직이면 일그러지는 단점이 있다
        // 1.타겟 좌표 , 2. 회전축, 3. 속도
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        // RotateAround()후의 위치를 가지고 목표와의 거리를 유지시킨다
        offset = transform.position - target.position;
    }
}
