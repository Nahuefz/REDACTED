using System.Collections;
using UnityEngine;
using Managers.Audio;

namespace Interactuables
{
    public class InodoroInteractuable : MonoBehaviour, IInteractable
    {
        [Header("Efecto Visual (Prefab)")]
        [SerializeField] private GameObject efectoAguaPrefab;
        [SerializeField] private Transform puntoDeSpawnAgua;

        [Header("Audio (Scriptable Object)")]
        [SerializeField] private AudioData sonidoCadena;

        private bool CacaVolando;

        public void Interact(GameObject interactor)
        {
            if (CacaVolando) return;

            StartCoroutine(SecuenciaDescargaRoutine());
        }

        private IEnumerator SecuenciaDescargaRoutine()
        {
            CacaVolando = true;

            if (sonidoCadena != null)
            {
                SoundManager.Instance.Play(sonidoCadena, false);
            }

            if (efectoAguaPrefab != null && puntoDeSpawnAgua != null)
            {
                GameObject aguaInstanciada = Instantiate(efectoAguaPrefab, puntoDeSpawnAgua.position, puntoDeSpawnAgua.rotation);

            }

            yield return new WaitForSeconds(3.5f);
            CacaVolando = false;
        }
    }
}
