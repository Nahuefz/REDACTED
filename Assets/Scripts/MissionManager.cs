using UnityEngine;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour
{
    // Creamos una estructura que agrupa todo lo que necesita UNA sola misión
    [System.Serializable]
    public struct ConfiguracionMision
    {
        [Tooltip("Nombre de la misión en GlobalMissions.")]
        public MissionNames nombreMision;

        [Header("Modificaciones del Mapa")]
        public GameObject[] objetosADeshabilitar;
        public GameObject[] objetosAHabilitar;

        [Header("Reposicionamiento (Ratas, NPCs, etc.)")]
        public Transform entidadAMover;
        public Transform puntoDestino;
    }

    [Header("Listado de Misiones de esta Escena")]
    [Tooltip("Añade aquí todas las misiones que alteran este mapa.")]
    [SerializeField] private List<ConfiguracionMision> misionesDelMapa = new List<ConfiguracionMision>();

    private void Start()
    {
        // Recorremos la lista de misiones configuradas una por una
        foreach (var mision in misionesDelMapa)
        {
            // Validamos que no esté vacío el nombre de la misión
            if (string.IsNullOrEmpty(mision.nombreMision.ToString())) continue;

            // Le preguntamos a tu clase estática si esta misión en específico ya se cumplió
            if (GlobalMissions.GetMission(mision.nombreMision.ToString()))
            {
                ActualizarEscenarioMision(mision);
            }
        }
    }

    private void ActualizarEscenarioMision(ConfiguracionMision mision)
    {
        // 1. Deshabilitar objetos de esta misión
        DisableObjects(mision);
        // 2. Habilitar objetos de esta misión
        EnableObjects(mision);        
        // 3. Mover entidad de esta misión
        if (mision.entidadAMover == null || mision.puntoDestino == null) return;
        mision.entidadAMover.position = mision.puntoDestino.position;
        mision.entidadAMover.rotation = mision.puntoDestino.rotation;
    }

    void DisableObjects(ConfiguracionMision mision)
    {
        if (mision.objetosADeshabilitar == null) return;
        foreach (var obj in mision.objetosADeshabilitar)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    private void EnableObjects(ConfiguracionMision mision)
    {
        if (mision.objetosAHabilitar == null) return;
        foreach (var obj in mision.objetosAHabilitar)
        {
            if (obj != null) obj.SetActive(true);
        }
    }
}