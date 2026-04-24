using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "Sistema de Dialogo/Dialogo")]
public class DialogoData : ScriptableObject
{
    public string nombrePersonaje;
    [TextArea(3, 5)]
    public string[] lineasDeDialogo;
}
