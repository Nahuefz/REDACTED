using System.Collections;
using UnityEngine;

namespace Managers.Audio
{
    public class AmbientSoundController : MonoBehaviour
    {
        [Header("Sonidos de Ambiente Constantes")]
        [SerializeField] private AudioData[] constantSounds;

        [Header("Pool de Sonidos Aleatorios")]
        [SerializeField] private AudioData[] randomSoundPool;

        [Header("Tiempos de Espera (Segundos)")]
        [SerializeField] private float minWaitTime = 5f;
        [SerializeField] private float maxWaitTime = 15f;

        private void Start()
        {
            // Reproducir loops constantes
            foreach (var audio in constantSounds)
            {
                SoundManager.Instance.Play(audio, true);
            }

            // Iniciar corutina de disparos esporádicos
            if (randomSoundPool != null && randomSoundPool.Length > 0)
            {
                StartCoroutine(PlayRandomSoundsRoutine());
            }
        }

        private IEnumerator PlayRandomSoundsRoutine()
        {
            while (true)
            {
                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);

                int randomIndex = Random.Range(0, randomSoundPool.Length);
                AudioData selectedAudio = randomSoundPool[randomIndex];

                SoundManager.Instance.Play(selectedAudio, false);
            }
        }
    }
}