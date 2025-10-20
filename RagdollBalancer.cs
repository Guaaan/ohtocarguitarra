using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollBalancer : MonoBehaviour
{
    //TODO: en este codigo hay que agregar balanceo de la cabeza al ritmo de la drum machine
    [Header("Ragdoll Parts")]
    public Rigidbody head;
    public Rigidbody pelvis;
    public Rigidbody leftFoot;
    public Rigidbody rightFoot;

    [Header("Forces")]
    public float upwardForce = 50f;
    public float downwardForce = 50f;
    public float wanderForce = 10f;
    public float wanderInterval = 2f;

    public bool isWandering = false;
    private float nextWanderTime = 0f;

    private void FixedUpdate()
    {
        if (head && pelvis && leftFoot && rightFoot)
        {
            ApplyBalancingForces();

            if (isWandering && Time.time >= nextWanderTime)
            {
                ApplyWanderForces();
                nextWanderTime = Time.time + wanderInterval;
            }
        }
    }

    private void ApplyBalancingForces()
    {
        head.AddForce(Vector3.up * upwardForce);
        pelvis.AddForce(Vector3.up * upwardForce);
        leftFoot.AddForce(Vector3.down * downwardForce);
        rightFoot.AddForce(Vector3.down * downwardForce);
    }

    private void ApplyWanderForces()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 force = randomDirection * wanderForce;

        pelvis.AddForce(force, ForceMode.Acceleration);
        leftFoot.AddForce(force * 0.5f, ForceMode.Acceleration);
        rightFoot.AddForce(force * 0.5f, ForceMode.Acceleration);
    }
    public void ApplySpasmForce()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Vector3 force = randomDirection * wanderForce;

        pelvis.AddForce(force, ForceMode.Impulse);
        leftFoot.AddForce(force * 1.5f, ForceMode.Impulse);
        rightFoot.AddForce(force * 1.5f, ForceMode.Impulse);
    }

    public void ToggleWandering(bool wandering)
    {
        isWandering = wandering;
    }
}
