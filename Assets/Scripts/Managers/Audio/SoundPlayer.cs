using UnityEngine;

namespace Managers.Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioData audioToPlay;
        [SerializeField] private bool loop;

        public void PlaySound()
        {
            SoundManager.Instance.Play(audioToPlay, loop);
        }
    }
}
