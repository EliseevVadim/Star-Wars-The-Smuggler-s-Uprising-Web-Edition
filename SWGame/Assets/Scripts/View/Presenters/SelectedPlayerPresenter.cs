using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using SWGame.ViewModels;
using SWGame.Management.Repositories;

namespace SWGame.View.Presenters
{
    public class SelectedPlayerPresenter : MonoBehaviour
    {
        [SerializeField] private Image _faceBox;
        [SerializeField] private Image _onlineIndicator;
        [SerializeField] private Text _nameField;
        [SerializeField] private Text _locationField;
        [SerializeField] private Text _planetField;
        [SerializeField] private Text _prestigeField;
        [SerializeField] private Text _wisdomField;
        [SerializeField] private Image _medalBox;

        public void VisualizePlayer(PlayerViewModel playerViewModel)
        {
            _faceBox.sprite = AvatarsRepository.Avatars[playerViewModel.AvatarIndex];
            _nameField.text = playerViewModel.Name;
            _locationField.text = playerViewModel.LocationName;
            _planetField.text = playerViewModel.PlanetName;
            _prestigeField.text = SplitNumber(playerViewModel.Prestige);
            _wisdomField.text = SplitNumber(playerViewModel.WisdomPoints);
            _medalBox.gameObject.SetActive(playerViewModel.StoryFinished);
            _onlineIndicator.color = (bool)playerViewModel.IsOnline ? Color.green : Color.grey;
            gameObject.SetActive(true);
        }

        private string SplitNumber(int value)
        {
            return string.Format("{0:#,###0.#}", value);
        }
    }
}
