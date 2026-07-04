using System.Collections;
using UnityEngine;

namespace Managers.Audio
{
    public class AmbientSoundController : MonoBehaviour
    {
        [Header("Sonidos de Ambiente Constantes")]
        [Tooltip("Configurá aquí los sonidos que van a loopear de fondo.")]
        [SerializeField] private Sounds[] constantSounds;

        [Header("Pool de Sonidos Aleatorios")]
        [Tooltip("Configurá aquí los sonidos esporádicos.")]
        [SerializeField] private Sounds[] randomSoundPool;

        [Header("Tiempos de Espera (Segundos)")]
        [SerializeField] private float minWaitTime = 5f;
        [SerializeField] private float maxWaitTime = 15f;

        private void Start()
        {
            InitializeSounds(constantSounds);
            InitializeSounds(randomSoundPool);

            foreach (var sound in constantSounds)
            {
                if (sound.source != null)
                {
                    sound.source.loop = true;
                    sound.source.Play();
                }
            }

            if (randomSoundPool != null && randomSoundPool.Length > 0)
            {
                StartCoroutine(PlayRandomSoundsRoutine());
            }
            else
            {
                Debug.LogWarning("El pool de sonidos aleatorios está vacío.");
            }
        }

        // Este método replica la lógica que usa SoundManager para crear los AudioSources
        private void InitializeSounds(Sounds[] soundList)
        {
            foreach (var sound in soundList)
            {
                if (sound.soundClip == null) continue;

                // Le agregamos el AudioSource a ESTE objeto (AmbientSoundController)
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.soundClip;
                sound.source.outputAudioMixerGroup = sound.audioMixer;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
            }
        }

        private IEnumerator PlayRandomSoundsRoutine()
        {
            while (true)
            {
                // Espera aleatoria
                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);

                // Elegir un sonido aleatorio de la lista
                int randomIndex = Random.Range(0, randomSoundPool.Length);
                Sounds selectedSound = randomSoundPool[randomIndex];

                // Reproducir si existe
                if (selectedSound.source != null)
                {
                    selectedSound.source.loop = false; // Disparo único
                    selectedSound.source.Play();
                }
            }
        }
    }
}