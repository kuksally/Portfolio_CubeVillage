using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;
    public bool isClose;    // �����ΰ�?
    public bool isRock;

    // �浹
    private void OnCollisionEnter(Collision collision)
    {
        if(!isRock && collision.gameObject.tag == "Grond")
        {
            Destroy(gameObject, 1f);
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Grond" && !isClose)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
