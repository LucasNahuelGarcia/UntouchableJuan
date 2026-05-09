using UnityEngine;

namespace StarterAssets
{
    public class PlayerDeathManager : MonoBehaviour
    {
        [SerializeField] MonoBehaviour[] MonoBehavioursToDisable;
        [SerializeField] MonoBehaviour[] MonoBehavioursToEnable;

   

        private void OnEnable()
        {
            GameManager.Instance?.OnGameOver.AddListener(OnGameOver);
        }
        private void OnDisable()
        {
            GameManager.Instance?.OnGameOver.RemoveListener(OnGameOver);
        }
        private void OnGameOver()
        {
            foreach (MonoBehaviour monoBehaviour in MonoBehavioursToDisable)
                monoBehaviour.enabled = false;
            foreach (MonoBehaviour monoBehaviour in MonoBehavioursToEnable)
                monoBehaviour.enabled = true;
            ThrowPlayer();

        }

        private void ThrowPlayer()
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.AddTorque(Vector3.one, ForceMode.Impulse);
        }

    }
}