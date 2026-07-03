using UnityEngine;

namespace Managers.Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        // CAMBIO: Ahora es un array de strings en lugar de SoundTypes
        [SerializeField] private string[] soundPlayNames;
        [SerializeField] private bool loop;

        private void Start()
        {
            foreach(var soundName in soundPlayNames)
            {
                SoundManager.Instance.Play(soundName, loop);
            }
        }
    }
}