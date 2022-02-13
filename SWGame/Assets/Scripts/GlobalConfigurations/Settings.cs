using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.Client.GlobalConfigurations
{
    public class Settings : MonoBehaviour
    {
        private bool _screenMaximized = true;
        private Resolution[] _resolutions;
        private List<string> _resolutionsNames;
        [SerializeField] private Dropdown _dropdown;
        [SerializeField] private Toggle _fullscreenChanger;

        public void Start()
        {
            _resolutionsNames = new List<string>();
            _resolutions = Screen.resolutions;
            foreach (var i in Screen.resolutions)
            {
                _resolutionsNames.Add($"{i.width}x{i.height}");
            }
            _dropdown.ClearOptions();
            _dropdown.AddOptions(_resolutionsNames);
            try
            {
                SetQuality(PlayerPrefs.GetInt("Quality"));
            }
            catch { }
            try
            {
                int resolutionIndex = PlayerPrefs.GetInt("Resolution");
                _dropdown.value = resolutionIndex;
                SetResolution(resolutionIndex);
            }
            catch { }
            if (PlayerPrefs.HasKey("FullScreen"))
            {
                bool fullscreenMode = PlayerPrefs.GetInt("FullScreen") == 1;
                _fullscreenChanger.isOn = fullscreenMode;
                Screen.fullScreen = fullscreenMode;
            }
        }

        public void ChangeScreen()
        {
            _screenMaximized = !_screenMaximized;
            Screen.fullScreen = _screenMaximized;
            PlayerPrefs.SetInt("FullScreen", _screenMaximized ? 1 : 0);
            PlayerPrefs.Save();
        }
        public void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);
            PlayerPrefs.SetInt("Quality", index);
            PlayerPrefs.Save();
        }
        public void SetResolution(int index)
        {
            Screen.SetResolution(_resolutions[index].width, _resolutions[index].height, _screenMaximized);
            PlayerPrefs.SetInt("Resolution", index);
            PlayerPrefs.Save();
        }
    }
}
