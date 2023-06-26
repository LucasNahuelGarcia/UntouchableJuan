using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    private bool isDead = false;

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        setupRigidbodies();
    }
    void Update()
    {
        animator.SetBool("Jumping", navMeshAgent.isOnOffMeshLink);
        animator.SetFloat("Speed", this.navMeshAgent.velocity.magnitude);
    }
    void FixedUpdate()
    {
        if (navMeshAgent.enabled)
            navMeshAgent.destination = Camera.main.transform.position;
    }

    private void setupRigidbodies()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
            rigidbody.isKinematic = true;
    }
    [ContextMenu("Kill")]
    public void Kill()
    {
        if (!isDead)
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


            StartCoroutine(changeLayer(0.2f));

            GameManager.Instance.EnemyCount++;
        }
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
}
