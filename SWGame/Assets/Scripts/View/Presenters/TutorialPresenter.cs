using SWGame.Entities;
using SWGame.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.Presenters
{
    public class TutorialPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _tutorial;
        [SerializeField] private Toggle _hideForever;
        [SerializeField] private Text _infoField;
        [SerializeField] private Scrollbar _scrollbar;

        private Player _currentPlayer = CurrentPlayer.Player;

        public void Hide()
        {
            if (_hideForever.isOn)
            {
                _currentPlayer.NeedToShowTutorial = false;
            }
            _tutorial.SetActive(false);
        }
        public void ExpandRecord(Button sender)
        {
            _infoField.text = TutorialDataHandler.TutorialChapters[int.Parse(sender.name)];
            _scrollbar.value = 1;
        }
    }
}
