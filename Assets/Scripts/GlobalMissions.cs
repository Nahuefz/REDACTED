using UnityEngine;
using System.Collections.Generic;

public static class GlobalMissions
{
    // Al ser una clase estática, el diccionario también debe ser estático
    private static Dictionary<string, bool> _missions = new Dictionary<string, bool>();

    public static void SetMission(string missionName, bool value)
    {
        // Corregí un pequeño detalle de tu lógica anterior: 
        // Si la misión NO existe, ahora la AGREGA. Si ya existe, la MODIFICA.
        if (!_missions.ContainsKey(missionName))
            _missions.Add(missionName, value);
        else
            _missions[missionName] = value;

        Debug.Log($"[GlobalMissions] Modificada: {missionName} = {value}");
    }

    public static bool GetMission(string missionName)
    {
        if (_missions.ContainsKey(missionName))
            return _missions[missionName];
        
        return false; 
    }
}