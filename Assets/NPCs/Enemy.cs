using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        setupRigidbodies();
    }
    void FixedUpdate()
    {
        if (navMeshAgent.enabled)
            navMeshAgent.destination = Camera.main.transform.position;

        animator.SetFloat("Speed", this.navMeshAgent.velocity.magnitude);
    }

    private void setupRigidbodies()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
            rigidbody.isKinematic = true;
    }
    [ContextMenu("Kill")]
    public void kill()
    {
        this.GetComponent<Animator>().enabled = false;
        this.GetComponent<NavMeshAgent>().enabled = false;
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.velocity = Vector3.zero;
        }
    }
}
