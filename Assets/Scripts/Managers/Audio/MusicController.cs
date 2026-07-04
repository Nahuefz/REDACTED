using UnityEngine;

namespace Managers.Audio
{
    public class MusicController : MonoBehaviour
    {
        [Header("Música del Nivel")]
        [SerializeField] private AudioData backgroundMusic;

        private void Start()
        {
            if (backgroundMusic != null)
            {
                SoundManager.Instance.Play(backgroundMusic, true);
            }
        }
    }
}
