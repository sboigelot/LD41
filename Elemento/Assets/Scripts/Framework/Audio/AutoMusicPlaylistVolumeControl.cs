using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class AutoMusicPlaylistVolumeControl : MonoBehaviour
    {
        public Slider Slider;
        public Slider SoundSlider;

        public void Update()
        {
            AutoMusicPlaylist.Instance.SetVolume(Slider.value);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SoundVolume = SoundSlider.value;
            }
        }
    }
}