using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
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
        this.GetComponent<Animator>().enabled = false;
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
            rigidbody.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
