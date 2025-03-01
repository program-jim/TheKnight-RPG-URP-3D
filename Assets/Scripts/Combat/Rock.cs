using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Basic Settings")]
    public float force;
    public GameObject target;
    private Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        FlyToTarget();
    }

    public void FlyToTarget()
    {
        direction = (target.transform.position - transform.position).normalized;

    }
}
