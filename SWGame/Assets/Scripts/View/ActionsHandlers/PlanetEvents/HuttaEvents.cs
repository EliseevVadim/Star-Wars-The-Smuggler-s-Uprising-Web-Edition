using SWGame.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SWGame.Management.Repositories;
using SWGame.Management;
using SWGame.View.Scenes;

namespace SWGame.View.ActionsHandlers.PlanetEvents 
{
    public class HuttaEvents : MonoBehaviour
    {
        [SerializeField] private GameObject _requestMessage;
        [SerializeField] private GameObject _successMessage;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _errorText;

        private Planet _currentPlanet;
        private Player _currentPlayer;

        private PlanetsRepository _planetsRepository;

        private const int RequiredCredits = 1000000;

        private void Start()
        {
            _planetsRepository = GetComponentInParent<MainScene>().PlanetsRepository;
            _currentPlanet = _planetsRepository.Planets[0];
            _currentPlayer = CurrentPlayer.Player;
        }
        public void GoToCity()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[1].Id;
            _currentPlanet.Locations[0].View.SetActive(false);
            _currentPlanet.Locations[1].View.SetActive(true);
        }
        public void ReturnToCantina()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[0].Id;
            _currentPlanet.Locations[1].View.SetActive(false);
            _currentPlanet.Locations[0].View.SetActive(true);
        }
        public void AskForThePayment()
        {
            _requestMessage.SetActive(true);
        }
        public void PayTheDebt()
        {
            _requestMessage.SetActive(false);
            if (_currentPlayer.StoryFinished)
            {
                _errorText.text = "Сюжет игры уже завершен.";
                _errorMessage.SetActive(true);
            }
            else if (_currentPlayer.Credits < RequiredCredits)
            {
                _errorText.text = "Недостаточно кредитов!";
                _errorMessage.SetActive(true);
            }
            else
            {
                _currentPlayer.Credits -= RequiredCredits;
                _currentPlayer.StoryFinished = true;
                _successMessage.SetActive(true);
            }
        }
    }
}

