using UnityEngine;

namespace Interactuables
{
    public class CanillaInteractuable : MonoBehaviour, IInteractable
    {
        [Header("Componentes")]
        [SerializeField] private ParticleSystem chorroAgua;
        [SerializeField] private AudioSource audioAgua; // Ya que estaba declarado en el código, lo usamos [cite: 2]

        private bool _estaAbierta; 

        private void Start()
        {
            if (chorroAgua != null)
            {
                // Es más seguro usar Stop() para que las partículas arranquen de cero
                chorroAgua.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _estaAbierta = false; // 
            }

            if (audioAgua != null) 
            {
                audioAgua.Stop();
            }
        }

        public void Interact(GameObject interactor)
        {
            // 1. INVERTIMOS EL ESTADO (La magia que faltaba)
            // Si era false se vuelve true, y viceversa.
            _estaAbierta = !_estaAbierta;

            // 2. Ejecutamos la acción correspondiente
            if (_estaAbierta)
            {
                AbrirGrifo();
            }
            else
            {
                CerrarGrifo();
            }
        }

        private void AbrirGrifo()
        {
            if (chorroAgua != null) chorroAgua.Play(); // 
            if (audioAgua != null) audioAgua.Play();
        }

        private void CerrarGrifo()
        {
            if (chorroAgua != null) chorroAgua.Stop(); // 
            if (audioAgua != null) audioAgua.Stop();
        }
    }
}