using UnityEngine;

namespace Managers.Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioData audioToPlay;

        public void PlaySound()
        {
            SoundManager.Instance.Play(audioToPlay, transform.position);
        }
    }
}
