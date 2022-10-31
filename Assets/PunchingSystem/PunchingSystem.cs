using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

public class PunchingSystem : MonoBehaviour
{

    public GameObject LeftTarget;
    public GameObject LeftReadyPosition;
    public GameObject RightTarget;
    public GameObject RightReadyPosition;
    public GameObject MainCamera;
    [NonReorderable]
    public Collider[] punchingColliders;
    public AudioSource rightHandPunchAudioSource;
    public AudioSource LeftHandPunchAudioSource;
    public float maxDIstanceRayCast;
    [Range(0.00001f, 0.00203f)]
    public float animationSpeed = .2f;
    public float timeAfterPunchInSeconds = .2f;
    private bool punching = false;
    private FMOD.Studio.EventInstance FMODEventInstance;

    void Start()
    {
        FMODEventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Hitting");
        MainCamera = MainCamera ?? Camera.main.gameObject;
        disablePunchingColliders();
        setArmsReady();
    }

    public void OnFireL(InputValue value)
    {
        if (!punching)
            tryToHit(LeftTarget, LeftReadyPosition);
    }

    public void OnFireR(InputValue value)
    {
        if (!punching)
            tryToHit(RightTarget, RightReadyPosition);
    }

    private void tryToHit(GameObject target, GameObject targetReadyPosition)
    {
        Vector3 camPosition = MainCamera.transform.position;
        Vector3 camForward = MainCamera.transform.forward;

        RaycastHit[] hit = Physics.RaycastAll(camPosition, camForward, maxDIstanceRayCast);
        foreach (RaycastHit hitInfo in hit)
        {
            StartCoroutine(animatePunch(target, hitInfo.point, targetReadyPosition.transform));
        }
    }

    IEnumerator animatePunch(GameObject animatedObj, Vector3 endPosition, Transform readyPosition)
    {
        this.punching = true;

        enablePunchingColliders();

        yield return animateMovement(animatedObj, endPosition);
        // yield return animateMovement(animatedObj, readyPosition.position);
        disablePunchingColliders();
        yield return new WaitForSeconds(timeAfterPunchInSeconds);
        animatedObj.transform.position = readyPosition.position;


        this.punching = false;
    }

    private void enablePunchingColliders()
    {
        foreach (Collider col in punchingColliders)
            col.enabled = true;
    }
    private void disablePunchingColliders()
    {
        foreach (Collider col in punchingColliders)
            col.enabled = false;
    }

    IEnumerator animateMovement(GameObject animatedObj, Vector3 endPosition)
    {
        Vector3 originalPosition = animatedObj.transform.position;
        float progressPorcentile = 0f;
        float distance = Vector3.Distance(animatedObj.transform.position, endPosition);
        float porcentileIncrement = animationSpeed * 100 / distance;

        while (progressPorcentile < .999f)
        {
            progressPorcentile += porcentileIncrement;
            progressPorcentile = Mathf.Clamp(progressPorcentile, 0f, 1f);
            Vector3 actualPosition = Vector3.Lerp(originalPosition, endPosition, progressPorcentile);

            animatedObj.transform.position = actualPosition;

            yield return new WaitForFixedUpdate();
        }
        animatedObj.transform.position = endPosition;
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject colisionParent = col.gameObject;
        while (colisionParent.transform.parent != null)
            colisionParent = colisionParent.transform.parent.gameObject;

        if (colisionParent.CompareTag("Enemy"))
        {
            FMODEventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(col.contacts[0].point));
            FMODEventInstance.start();
            Debug.Log("Hitting enemy");
            colisionParent.GetComponent<Enemy>().kill();
            col.gameObject.GetComponent<Rigidbody>().AddForceAtPosition((MainCamera.transform.forward) * 100, col.contacts[0].point, ForceMode.Impulse);
        }
    }

    private void setArmsReady()
    {
        LeftTarget.transform.position = LeftReadyPosition.transform.position;
        RightTarget.transform.position = RightReadyPosition.transform.position;
    }
}
