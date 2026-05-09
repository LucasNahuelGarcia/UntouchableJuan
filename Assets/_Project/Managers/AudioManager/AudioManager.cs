using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        public static AudioManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    GameObject go = new GameObject(nameof(AudioManager));
                    _instance = go.AddComponent<AudioManager>();
                }

                return _instance;
            }
        }

        [SerializeField] private AudioMixerSnapshot defaultSnapshot;
        private AudioMixerSnapshot _previousSnapshot;
        private AudioMixerSnapshot _currentSnapshot;

        private void Awake()
        {
            if (_instance is null)
            {
                _instance = this;
                transform.SetParent(null);
                _previousSnapshot ??= defaultSnapshot;
                _currentSnapshot ??= defaultSnapshot;
            }
            else
                Destroy(gameObject);
        }

        public void TransitionTo(AudioMixerSnapshot snapshot, float time)
        {
            snapshot.TransitionTo(time);
            _previousSnapshot = _currentSnapshot;
            _currentSnapshot = snapshot;
        }

        public void ReturnToPreviousSnapshot(float time)
        {
            TransitionTo(_previousSnapshot, time);
        }
    }
}