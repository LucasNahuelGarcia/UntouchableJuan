using System.Collections;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
#endif

public class PunchingSystem : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip[] PunchSounds;
    public GameObject LeftTarget;
    public GameObject LeftReadyPosition;
    public GameObject RightTarget;
    public GameObject RightReadyPosition;
    public GameObject MainCamera;
    public Collider[] punchingColliders;
    public float MaxDIstanceRayCast;
    public float TimeAfterPunchInSeconds = .2f;
    [Space]
    public float PunchForce = 100f;
    [Range(1f, 100f)]
    public float PunchTerminalSpeed = .2f;
    public AnimationCurve PunchAcceleration;
    [Space]


    private bool punching = false;

    void Start()
    {
        MainCamera = MainCamera ?? Camera.main.gameObject;
        DisablePunchingColliders();
        SetArmsReady();
    }

    public void OnFireL(InputValue value)
    {

        if (!punching)
            TryToHit(LeftTarget, LeftReadyPosition);
    }

    public void OnFireR(InputValue value)
    {
        if (!punching)
            TryToHit(RightTarget, RightReadyPosition);
    }

    private void TryToHit(GameObject target, GameObject targetReadyPosition)
    {

        Vector3 camPosition = MainCamera.transform.position;
        Vector3 camForward = MainCamera.transform.forward;

        RaycastHit[] hit = Physics.RaycastAll(camPosition, camForward, MaxDIstanceRayCast);
        foreach (RaycastHit hitInfo in hit)
        {
            StartCoroutine(AnimatePunch(target, hitInfo.point, targetReadyPosition.transform));
        }
    }

    IEnumerator AnimatePunch(GameObject animatedObj, Vector3 endPosition, Transform readyPosition)
    {
        punching = true;

        EnablePunchingColliders();

        yield return AnimateMovement(animatedObj, endPosition, PunchTerminalSpeed, PunchAcceleration);

        DisablePunchingColliders();

        yield return new WaitForSeconds(TimeAfterPunchInSeconds);

        animatedObj.transform.position = readyPosition.position;

        punching = false;
    }

    private void EnablePunchingColliders()
    {
        foreach (Collider col in punchingColliders)
            col.enabled = true;
    }
    private void DisablePunchingColliders()
    {
        foreach (Collider col in punchingColliders)
            col.enabled = false;
    }

    IEnumerator AnimateMovement(GameObject animatedObj, Vector3 endPosition, float targetSpeed, AnimationCurve accelerationCurve)
    {
        Vector3 originalPosition = animatedObj.transform.position;
        float progressPorcentile = 0f;

        while (progressPorcentile < .999f)
        {
            float acceleration = accelerationCurve.Evaluate(progressPorcentile) + 0.1f;
            progressPorcentile += targetSpeed * Time.deltaTime * acceleration;
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
        colisionParent = FindParent(colisionParent);

        if (punching && colisionParent.CompareTag("Enemy"))
        {
            KnockEnemy(col, colisionParent);
            PlayPunchSound();
        }
    }

    private static GameObject FindParent(GameObject colisionParent)
    {
        while (colisionParent.transform.parent != null)
            colisionParent = colisionParent.transform.parent.gameObject;
        return colisionParent;
    }

    private void KnockEnemy(Collision col, GameObject colisionParent)
    {
        colisionParent.GetComponent<Enemy>().Kill();
        col.gameObject.GetComponent<Rigidbody>().AddForceAtPosition((MainCamera.transform.forward) * PunchForce, col.contacts[0].point, ForceMode.Impulse);
    }

    private void PlayPunchSound()
    {
        AudioSource.clip = PunchSounds[Mathf.RoundToInt(Random.Range(0, PunchSounds.Length - 1))];
        AudioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fists"))
        {
            GameManager.Instance.GameOver();
        }
    }

    private void SetArmsReady()
    {
        LeftTarget.transform.position = LeftReadyPosition.transform.position;
        RightTarget.transform.position = RightReadyPosition.transform.position;
    }
}
