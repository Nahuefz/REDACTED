using UnityEngine;
using System.Collections.Generic;

[SelectionBase]
public class ObjetoModular : MonoBehaviour
{
    [Tooltip("Arrastrá acá los prefabs del objeto")]
    public List<GameObject> variantes = new List<GameObject>();

    [HideInInspector]
    public int indiceActual = 0;
}
