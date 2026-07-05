using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Managers.Audio
{
    [RequireComponent(typeof(Slider))]
    public class SoundSettingSlider : MonoBehaviour
    {
        private Slider Slider => GetComponent<Slider>();
        [SerializeField] private AudioMixerGroup mixerGroup;

        private void Start()
        {
            if (!SoundManager.Instance || mixerGroup == null) return;

            if (!SoundManager.Instance.MixerSettings.ContainsKey(mixerGroup))
            {
                SoundManager.Instance.MixerSettings[mixerGroup] = Slider.value;
            }

            LoadVolumeValues();
            Slider.onValueChanged.AddListener(SetVolume);
        }

        public void SetVolume(float value)
        {
            if (mixerGroup == null) return;

            mixerGroup.audioMixer.SetFloat(mixerGroup.name, Mathf.Log10(value) * 20);
            SoundManager.Instance.MixerSettings[mixerGroup] = value;
        }

        public void LoadVolumeValues()
        {
            Slider.value = SoundManager.Instance.MixerSettings[mixerGroup];
            mixerGroup.audioMixer.SetFloat(mixerGroup.name, Mathf.Log10(Slider.value) * 20);
        }
    }
}