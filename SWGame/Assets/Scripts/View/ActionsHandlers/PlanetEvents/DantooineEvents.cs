using SWGame.Entities;
using SWGame.Management.Repositories;
using SWGame.View.Scenes;
using SWGame.Management;
using UnityEngine;
using UnityEngine.UI;
using SWGame.Activities;
using SWGame.Enums;
using SWGame.Entities.Items;
using SWGame.Exceptions;

namespace SWGame.View.ActionsHandlers.PlanetEvents
{
    public class DantooineEvents : MonoBehaviour
    {
        [SerializeField] private GameObject _successLootMessage;
        [SerializeField] private Text _extractionNameField;
        [SerializeField] private Image _extractionIcon;
        [SerializeField] private GameObject _failMessage;
        [SerializeField] private Text _failText;
        [SerializeField] private GameObject _lootErrorMessage;
        [SerializeField] private GameObject _requestDefenceMessage;
        [SerializeField] private GameObject _successDefenceMessage;
        [SerializeField] private GameObject _errorDefenceMessage;
        [SerializeField] private Text _errorText;

        private Planet _currentPlanet;
        private Player _currentPlayer;

        private PlanetsRepository _planetsRepository;

        private const int RequiredWisdom = 15000;

        private void Start()
        {
            _planetsRepository = GetComponentInParent<MainScene>().PlanetsRepository;
            _currentPlanet = _planetsRepository.Planets[1];
            _currentPlayer = CurrentPlayer.Player;
        }
        public void GoToRuins()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[1].Id;
            _currentPlanet.Locations[0].View.SetActive(false);
            _currentPlanet.Locations[1].View.SetActive(true);
        }
        public void GoToTheTemple()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[2].Id;
            _currentPlanet.Locations[0].View.SetActive(false);
            _currentPlanet.Locations[2].View.SetActive(true);
        }
        public void ReturnFromTheTemple()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[0].Id;
            _currentPlanet.Locations[2].View.SetActive(false);
            _currentPlanet.Locations[0].View.SetActive(true);
        }
        public void ReturnToEnclaveEntrance()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[0].Id;
            _currentPlanet.Locations[1].View.SetActive(false);
            _currentPlanet.Locations[0].View.SetActive(true);
        }

        public async void Loot()
        {
            if (_currentPlayer.HasA(ItemsRepository.JediItems[1]))
            {
                LootPlace lootPlace = new LootPlace(LootPlaceType.JediRuins);
                try
                {
                    Item extraction = lootPlace.Loot();
                    await _currentPlayer.Inventory.AddItem(extraction);
                    _extractionIcon.sprite = extraction.Image;
                    _extractionNameField.text = extraction.Name;
                    _successLootMessage.SetActive(true);
                }
                catch (FailedLootException ex)
                {
                    await _currentPlayer.Inventory.RemoveItem(ItemsRepository.JediItems[1]);
                    _failText.text = ex.Message;
                    _failMessage.SetActive(true);
                }
            }
            else
            {
                _lootErrorMessage.SetActive(true);
            }
        }
        public void AcceptProtection()
        {
            _requestDefenceMessage.SetActive(false);
            if (_currentPlayer.StoryFinished)
            {
                _errorText.text = "Сюжет игры уже завершен.";
                _errorDefenceMessage.SetActive(true);
            }
            else if (_currentPlayer.WisdomPoints < RequiredWisdom)
            {
                _errorText.text = "Недостаточно очков мудрости.";
                _errorDefenceMessage.SetActive(true);
            }
            else
            {
                _currentPlayer.StoryFinished = true;
                _successDefenceMessage.SetActive(true);
            }
        }
    }
}