using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    private FMOD.Studio.EventInstance EnemyFallingEvent;
    private bool isDead = false;

    void Start()
    {
        EnemyFallingEvent = FMODUnity.RuntimeManager.CreateInstance("event:/EnemyDieing");
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        setupRigidbodies();
        moveToFloor();
    }
    void FixedUpdate()
    {
        if (navMeshAgent.enabled)
            navMeshAgent.destination = Camera.main.transform.position;

        animator.SetFloat("Speed", this.navMeshAgent.velocity.magnitude);
    }

    private void moveToFloor() {
        
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
        isDead = true;
        this.GetComponent<Animator>().enabled = false;
        this.GetComponent<NavMeshAgent>().enabled = false;
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.velocity = Vector3.zero;
        }

        EnemyFallingEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform.position));
        EnemyFallingEvent.start();
    }

    private void OnCollisionEnter(Collision other)
    {
    }
}
