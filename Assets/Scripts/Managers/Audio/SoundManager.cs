using System.Collections.Generic;
using UnityEngine;

namespace Managers.Audio
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        public Sounds[] sounds;
        // Cambiá esto en SoundManager.cs:
        public Dictionary<string, float> MixerValue = new Dictionary<string, float>();

        private void Awake()
        {
            if (!Instance) Instance = this;
            else Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            InitialSet();
        }

        private void InitialSet()
        {
            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.soundClip;
                sound.source.outputAudioMixerGroup = sound.audioMixer;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.isLoop;
            }
        }
        public void Play(string soundName, bool loop)
        {
            Sounds sound = FindSound(soundName);
            if (sound == null)
            {
                Debug.LogWarning($"No se encontró el sonido con el nombre: {soundName}");
                return;
            }

            sound.source.loop = loop;
            sound.source.Play();
        }
        public void Pause(string soundName)
        {
            Sounds sound = FindSound(soundName);
            if (sound == null)
            {
                Debug.LogWarning($"No se encontró el sonido para pausar: {soundName}");
                return;
            }
            sound.source.Pause();
        }
        public void PauseAll()
        {
            foreach (var sound in sounds) sound.source.Pause();
        }
        private Sounds FindSound(string soundName)
        {
            foreach (var sound in sounds)
            {
                if (sound.name == soundName) return sound;
            }
            return null;
        }
    }
}
