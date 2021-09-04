using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Arrow
{
    Rigidbody rigidbody;

    float angularPower = 5;     // ȸ�� ��
    float scaleValue = 0.25f;    // ũ��

    bool isShoot;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }
    
    // ��� Ÿ�̹� ����
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
            rigidbody.AddTorque(transform.right * angularPower, ForceMode.Impulse);    // ȸ����
            yield return null;
        }
    }
}
