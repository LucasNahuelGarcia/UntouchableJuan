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
    public float maxDIstanceRayCast;
    [Range(0.00001f, 0.00203f)]
    public float animationSpeed = .2f;
    public float timeAfterPunchInSeconds = .2f;

    void Start()
    {
        MainCamera = MainCamera ?? Camera.main.gameObject;
        setArmsReady();
    }

    void Update()
    {

    }

    public void OnFireL(InputValue value)
    {
        tryToHit(LeftTarget, LeftReadyPosition);
    }

    public void OnFireR(InputValue value)
    {
        tryToHit(RightTarget, RightReadyPosition);
    }

    private void tryToHit(GameObject target, GameObject targetReadyPosition)
    {
        Vector3 camPosition = MainCamera.transform.position;
        Vector3 camForward = MainCamera.transform.forward;
        RaycastHit hitInfo;

        if (Physics.Raycast(camPosition, camForward, out hitInfo, maxDIstanceRayCast))
        {
            StartCoroutine(animatePunch(target, hitInfo.point, targetReadyPosition.transform));
        }
    }

    IEnumerator animatePunch(GameObject animatedObj, Vector3 endPosition, Transform readyPosition)
    {
        yield return animateMovement(animatedObj, endPosition);
        // yield return animateMovement(animatedObj, readyPosition.position);
        animatedObj.transform.position = readyPosition.position;
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

            yield return new WaitForEndOfFrame();
        }
        animatedObj.transform.position = endPosition;
        yield return new WaitForSeconds(timeAfterPunchInSeconds);
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject colisionParent = col.gameObject;
        while (colisionParent.transform.parent != null)
            colisionParent = colisionParent.transform.parent.gameObject;

        if (colisionParent.CompareTag("Enemy"))
        {
            colisionParent.GetComponent<Enemy>().kill();
            col.gameObject.GetComponent<Rigidbody>().AddForceAtPosition((MainCamera.transform.forward) * 200, col.contacts[0].point, ForceMode.Impulse);
        }
    }

    private void setArmsReady()
    {
        LeftTarget.transform.position = LeftReadyPosition.transform.position;
        RightTarget.transform.position = RightReadyPosition.transform.position;

    }
}
