using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class AutoMusicPlaylistVolumeControl : MonoBehaviour
    {
        public Slider Slider;

        public void Update()
        {
            AutoMusicPlaylist.Instance.SetVolume(Slider.value);
        }
    }
}