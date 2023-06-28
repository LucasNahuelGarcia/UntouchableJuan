using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    private bool isDead = false;
    public UnityEvent OnDeath = new UnityEvent();

    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        animator = this.GetComponent<Animator>();
        SetupRigidbodies();
    }
    void Update()
    {
        animator.SetBool("Jumping", navMeshAgent.isOnOffMeshLink);
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }
    private void CheckHittingRotation()
    {
        if (navMeshAgent.velocity.magnitude < .01f)
        {
            Vector3 mainCameraPosition = Camera.main.transform.position;
            Vector3 targetRotation = new Vector3(mainCameraPosition.x, transform.position.y, mainCameraPosition.z);
            transform.LookAt(targetRotation);
        }
    }

    void FixedUpdate()
    {
        CheckHittingRotation();
        if (navMeshAgent.enabled)
            navMeshAgent.destination = Camera.main.transform.position;
    }

    private void SetupRigidbodies()
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
            GetComponent<Animator>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.velocity = Vector3.zero;
            }


            StartCoroutine(changeLayer(0.2f));

            GameManager.Instance.IncreaseEnemyCount();
            OnDeath.Invoke();
            enabled = false;
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
