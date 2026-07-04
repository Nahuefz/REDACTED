using UnityEngine;
using UnityEngine.Audio;

namespace Managers.Audio
{
    [CreateAssetMenu(fileName = "NuevoSonido", menuName = "Audio/Audio Data")]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private AudioClip soundClip;
        [SerializeField] private AudioMixerGroup audioMixer;
        [Range(0f, 1f)][SerializeField] private float volume = 1f;
        [SerializeField] private SoundTypes soundCategory;

        public AudioClip Clip => soundClip;
        public AudioMixerGroup Mixer => audioMixer;
        public float Volume => volume;
        public SoundTypes Category => soundCategory;
    }
}
