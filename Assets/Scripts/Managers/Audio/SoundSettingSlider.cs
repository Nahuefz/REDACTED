using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Managers.Audio
{
    public class SoundSettingSlider : MonoBehaviour
    {
        private Slider Slider => GetComponent<Slider>();
        public string mixerName;
        [SerializeField] private AudioMixerGroup mixerGroup;
        private SoundManager _soundManager;

        private void Start()
        {
            if (!SoundManager.Instance)
            {
                Debug.Log("No hay manager de audio!");
                return;
            }

            _soundManager = SoundManager.Instance;
            mixerName = mixerGroup.name;

            if (!_soundManager.MixerValue.ContainsKey(mixerName))
            {
                _soundManager.MixerValue[mixerName] = Slider.value;
            }
            LoadVolumeValues();
        }

        public void SetVolume(float value)
        {
            mixerGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(value) * 20);
        }

        public void SetVolumeValues()
        {
            _soundManager.MixerValue[mixerName] = Slider.value;
        }

        public void LoadVolumeValues()
        {
            Slider.value = _soundManager.MixerValue[mixerName];
            mixerGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(_soundManager.MixerValue[mixerName]) * 20);
        }

        private void OnDisable()
        {
            SetVolumeValues();
        }
    }
}