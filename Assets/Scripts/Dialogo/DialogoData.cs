using UnityEngine;

[System.Serializable]
public struct LineaDeDialogo
{
    public string nombrePersonaje;
    [TextArea(3, 5)]
    public string texto;
}

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "Sistema de Dialogo/Dialogo")]
public class DialogoData : ScriptableObject
{
    public string nombrePorDefecto;
    public LineaDeDialogo[] dialogos;
}
