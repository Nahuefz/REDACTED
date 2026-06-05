using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjetoModular))]
[CanEditMultipleObjects]
public class ObjetoModularEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjetoModular script = (ObjetoModular)target;

        DrawDefaultInspector();
        EditorGUILayout.Space();

        if (script.variantes.Count > 0)
        {
            string[] opcionesDelMenu = new string[script.variantes.Count];
            for (int i = 0; i < script.variantes.Count; i++)
                opcionesDelMenu[i] = script.variantes[i] != null ? script.variantes[i].name : "Vacío";

            GUILayout.Label("Diseño Visual", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            int eleccion = EditorGUILayout.Popup("Elegir Variante:", script.indiceActual, opcionesDelMenu);

            if (EditorGUI.EndChangeCheck())
            {
                foreach (var objetoSeleccionado in targets)
                {
                    ObjetoModular modulo = (ObjetoModular)objetoSeleccionado;
                    Undo.RecordObject(modulo, "Cambiar modelo modular");
                    modulo.indiceActual = eleccion;
                    ActualizarModelo(modulo);
                }
            }
        }
        else EditorGUILayout.HelpBox("Agregá prefabs a la lista para verlos", MessageType.Info);
    }

    void ActualizarModelo(ObjetoModular script)
    {
        while (script.transform.childCount > 0)
        {
            DestroyImmediate(script.transform.GetChild(0).gameObject);
        }

        GameObject prefabElegido = script.variantes[script.indiceActual];
        if (prefabElegido != null)
        {
            GameObject nuevoModelo = (GameObject)PrefabUtility.InstantiatePrefab(prefabElegido);

            nuevoModelo.transform.SetParent(script.transform, false);

            Undo.RegisterCreatedObjectUndo(nuevoModelo, "Aparecer nuevo modelo");
        }
    }
}
