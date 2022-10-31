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
    void Update()
    {
        animator.SetBool("Jumping", navMeshAgent.isOnOffMeshLink);
    }
    void FixedUpdate()
    {
        if (navMeshAgent.enabled)
            navMeshAgent.destination = Camera.main.transform.position;

        animator.SetFloat("Speed", this.navMeshAgent.velocity.magnitude);
    }

    private void moveToFloor()
    {

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
        StartCoroutine(changeLayer(0.2f));
    }

    IEnumerator changeLayer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.layer = 3;
        foreach (Transform g in transform.GetComponentsInChildren<Transform>())
        {
            g.gameObject.layer = 3;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
    }
}
