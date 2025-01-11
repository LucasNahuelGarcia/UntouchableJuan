using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "Audio Event", menuName = "Audio/AudioEvent")]
    public class AudioEvent : ScriptableObject
    {
        [SerializeField] private List<AudioClip> _audioClips;
        [SerializeField] private float _minPitch = 1;
        [SerializeField] private float _maxPitch = 1;
        [SerializeField] private float _minVolume = 1;
        [SerializeField] private float _maxVolume = 1;

        public void PlaySound(AudioSource audioSource)
        {
            if (_audioClips.Count == 0) return;
            audioSource.volume = Random.Range(_minPitch, _maxPitch);
            audioSource.pitch = Random.Range(_minVolume, _maxVolume);
            audioSource.clip = _audioClips[Random.Range(0, _audioClips.Count - 1)];
            audioSource.Play();
        }
    }
}