using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Arrow
{
    Rigidbody rigidbody;

    float angularPower = 5;     // 회전 힘
    float scaleValue = 0.25f;    // 크기

    bool isShoot;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }
    
    // 쏘는 타이밍 관리
    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }

    IEnumerator GainPower()
    {
        while (!isShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue;
            rigidbody.AddTorque(transform.right * angularPower, ForceMode.Impulse);    // 회전력
            yield return null;
        }
    }
}
