using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public GameObject target;
    public Vector3 pos;

    private void Start()
    {
        target = GameObject.Find("Player");
    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = target.transform.position + pos;
    }
}
