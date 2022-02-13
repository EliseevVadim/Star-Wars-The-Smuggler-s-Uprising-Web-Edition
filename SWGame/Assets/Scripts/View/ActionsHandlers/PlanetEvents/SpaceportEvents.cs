using SWGame.Entities;
using SWGame.Management;
using SWGame.Management.Repositories;
using SWGame.View.Scenes;
using SWGame.GlobalConfigurations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.ActionsHandlers.PlanetEvents
{
    public class SpaceportEvents : MonoBehaviour
    {
        [SerializeField] private GameObject _planetInformation;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private GameObject _spaceport;
        [SerializeField] private Text _errorText;
        [SerializeField] private Text _planetName;
        [SerializeField] private Text _planetDescription;
        [SerializeField] private Text _travellButtonText;

        private Player _player;
        private Planet _hutta;
        private Planet _dantooine;
        private Planet _korriban;
        private Planet _tatooine;
        private Planet _narShaddaa;
        private Planet _selectedPlanet;

        private PlanetsRepository _planetsRepository;
        private ClientManager _clientManager;

        private void Start()
        {
            _planetsRepository = GetComponentInParent<MainScene>().PlanetsRepository;
            _player = CurrentPlayer.Player;
            _hutta = _planetsRepository.Planets[0];
            _dantooine = _planetsRepository.Planets[1];
            _korriban = _planetsRepository.Planets[2];
            _tatooine = _planetsRepository.Planets[3];
            _narShaddaa = _planetsRepository.Planets[4];
            _clientManager = GetComponentInParent<MainScene>().ClientManager;
        }
        public void SelectHutta()
        {
            _selectedPlanet = _hutta;
            PreparePlanetInformationView();
        }
        public void SelectKorriban()
        {
            _selectedPlanet = _korriban;
            PreparePlanetInformationView();
        }
        public void SelectDantooine()
        {
            _selectedPlanet = _dantooine;
            PreparePlanetInformationView();
        }
        public void SelectTatooine()
        {
            _selectedPlanet = _tatooine;
            PreparePlanetInformationView();
        }
        public void SelectNarShaddaa()
        {
            _selectedPlanet = _narShaddaa;
            PreparePlanetInformationView();
        }
        public async void Travell()
        {
            if (_planetsRepository.Planets.IndexOf(_selectedPlanet) == _player.GetPlanetIndex())
            {
                _errorText.text = "Перелет невозможен. Вы уже на этой планете.";
                _errorMessage.SetActive(true);
            }
            else if (_player.Credits < _selectedPlanet.TravellCost)
            {
                _errorText.text = "Перелет невозможен. Недостаточно кредитов.";
                _errorMessage.SetActive(true);
            }
            else
            {
                _player.Credits -= _selectedPlanet.TravellCost;
                _selectedPlanet.Treasury += _selectedPlanet.TravellCost;
                await _clientManager.UpdatePlanetsTreasury(_selectedPlanet);
                _player.Planet.View.SetActive(false);
                _player.Location.View.SetActive(false);
                _player.LocationId = _selectedPlanet.Locations[0].Id;
                _player.SetPlanet();
                _spaceport.SetActive(false);
                _planetInformation.SetActive(false);
                _player.Planet.View.SetActive(true);
                _player.Location.View.SetActive(true);
                //PlayerInformationVisualisator.UpdateView();
            }
        }
        public void CancelDeparture()
        {
            _selectedPlanet = null;
            _planetInformation.SetActive(false);
        }
        private void PreparePlanetInformationView()
        {
            _planetName.text = _selectedPlanet.Name;
            _planetDescription.text = _selectedPlanet.Descriprion;
            _planetInformation.SetActive(true);
            _travellButtonText.text = $"Отправиться ({_selectedPlanet.TravellCost} кредитов)";
        }
    }
}

