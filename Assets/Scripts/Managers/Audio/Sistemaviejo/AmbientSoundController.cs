using System.Collections;
using UnityEngine;

namespace Managers.Audio
{
    public class AmbientSoundController : MonoBehaviour
    {
        [Header("Sonidos de Ambiente Constantes")]
        [Tooltip("Nombres de los sonidos que van a loopear de fondo.")]
        [SerializeField] private string[] constantSounds;

        [Header("Pool de Sonidos Aleatorios")]
        [Tooltip("Nombres de los sonidos esporádicos.")]
        [SerializeField] private string[] randomSoundList;

        [Header("Tiempos de Espera (Segundos)")]
        [SerializeField] private float minWaitTime = 5f;
        [SerializeField] private float maxWaitTime = 15f;

        private void Start()
        {
            foreach (var soundName in constantSounds) SoundManager.Instance.Play(soundName, true);

            // Corutine de sonidos aleatorios.
            if (randomSoundList != null && randomSoundList.Length > 0)
            {
                StartCoroutine(PlayRandomSoundsRoutine());
            }
            else
            {
                Debug.LogWarning("El pool de sonidos aleatorios está vacío en AmbientSoundController.");
            }
        }

        private IEnumerator PlayRandomSoundsRoutine()
        {
            while (true)
            {
                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);

                int randomIndex = Random.Range(0, randomSoundList.Length);
                string selectedSound = randomSoundList[randomIndex];

                SoundManager.Instance.Play(selectedSound, false);
            }
        }
    }
}
