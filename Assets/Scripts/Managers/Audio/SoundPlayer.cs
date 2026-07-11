using System;
using UnityEngine;

namespace Managers.Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioData audioToPlay;

        private void Start()
        {
            PlaySound();
        }

        private void PlaySound()
        {
            SoundManager.Instance.Play(audioToPlay, transform.position);
        }
    }
}
