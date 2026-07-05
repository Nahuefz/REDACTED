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

        [Header("Configuración de Reproducción")]
        [SerializeField] private bool isLoop;
        [Tooltip("0 es 2D, 1 es 3D")]
        [Range(0f, 1f)][SerializeField] private float spatialBlend = 0f;

        public AudioClip Clip => soundClip;
        public AudioMixerGroup Mixer => audioMixer;
        public float Volume => volume;
        public SoundTypes Category => soundCategory;
        public bool IsLoop => isLoop;
        public float SpatialBlend => spatialBlend;
    }
}