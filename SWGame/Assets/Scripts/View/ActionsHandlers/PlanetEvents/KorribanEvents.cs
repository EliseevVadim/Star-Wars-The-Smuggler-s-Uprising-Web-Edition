using SWGame.Entities;
using SWGame.Management.Repositories;
using SWGame.View.Scenes;
using SWGame.Management;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SWGame.Enums;
using SWGame.Entities.Items;
using SWGame.Activities;
using SWGame.Exceptions;

namespace SWGame.View.ActionsHandlers.PlanetEvents
{
    public class KorribanEvents : MonoBehaviour
    {
        [SerializeField] private GameObject _requestMessage;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _errorText;
        [SerializeField] private GameObject _successMessage;
        [SerializeField] private GameObject _defeatMessage;
        [SerializeField] private GameObject _failMessage;
        [SerializeField] private Text _failText;
        [SerializeField] private GameObject _successLootMessage;
        [SerializeField] private Image _extractionIcon;
        [SerializeField] private Text _extractionNameField;
        [SerializeField] private GameObject _errorAcademyMessage;
        [SerializeField] private GameObject _errorTombsMessage;
        [SerializeField] private GameObject _academyExchangePoint;

        private Planet _currentPlanet;
        private Player _currentPlayer;

        private PlanetsRepository _planetsRepository;

        private const int RequiredPrestige = 2500;
        private const int BalanceGap = 250;
        private const float Cooldown = 3f;

        private void Start()
        {
            _planetsRepository = GetComponentInParent<MainScene>().PlanetsRepository;
            _currentPlanet = _planetsRepository.Planets[2];
            _currentPlayer = CurrentPlayer.Player;

            if (_currentPlayer.Location == _currentPlanet.Locations[2])
            {
                StartCoroutine(AcolytesAttack());
            }
        }
        public void GoToTombs()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[1].Id;
            _currentPlanet.Locations[0].View.SetActive(false);
            _currentPlanet.Locations[1].View.SetActive(true);
            //PlayerInformationVisualisator.UpdateView();
        }
        public void ReturnToAcademyEntrance()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[0].Id;
            _currentPlanet.Locations[1].View.SetActive(false);
            _currentPlanet.Locations[0].View.SetActive(true);
            //PlayerInformationVisualisator.UpdateView();
        }
        public void GoToAcademy()
        {
            if (_currentPlayer.HasA(ItemsRepository.QuestItems[2]))
            {
                _currentPlayer.LocationId = _currentPlanet.Locations[2].Id;
                _currentPlanet.Locations[0].View.SetActive(false);
                _currentPlanet.Locations[2].View.SetActive(true);
                //PlayerInformationVisualisator.UpdateView();
                StartCoroutine(AcolytesAttack());
            }
            else
            {
                _errorAcademyMessage.SetActive(true);
            }
        }
        public void GoOutOfTheAcademy()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[0].Id;
            _currentPlanet.Locations[2].View.SetActive(false);
            _currentPlanet.Locations[0].View.SetActive(true);
            //PlayerInformationVisualisator.UpdateView();
            StopAllCoroutines();
        }
        public void AskForProtection()
        {
            _requestMessage.SetActive(true);
        }
        public void AcceptProtection()
        {
            _requestMessage.SetActive(false);
            if (_currentPlayer.StoryFinished)
            {
                _errorText.text = "Сюжет игры уже завершен.";
                _errorMessage.SetActive(true);
            }
            else if (_currentPlayer.Prestige < RequiredPrestige)
            {
                _errorText.text = "Недостаточно очков престижа.";
                _errorMessage.SetActive(true);
            }
            else
            {
                _currentPlayer.StoryFinished = true;
                _successMessage.SetActive(true);
                //PlayerInformationVisualisator.UpdateView();
            }
        }
        IEnumerator AcolytesAttack()
        {
            while (true)
            {
                yield return new WaitForSeconds(Cooldown);
                int attackCoefficient = Random.Range(-BalanceGap, RequiredPrestige + BalanceGap);
                if (attackCoefficient > _currentPlayer.Prestige)
                {
                    bool isDefeat = Random.Range(0, 2) == 1;
                    if (isDefeat)
                    {
                        _currentPlayer.Inventory.RemoveItem(ItemsRepository.QuestItems[2]);
                        GoOutOfTheAcademy();
                        _defeatMessage.SetActive(true);
                    }
                }
            }
        }
        public async void Loot()
        {
            if (_currentPlayer.HasA(ItemsRepository.SithItems[0]))
            {
                LootPlace lootPlace = new LootPlace(LootPlaceType.SithTomb);
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
                    await _currentPlayer.Inventory.RemoveItem(ItemsRepository.SithItems[0]);
                    _failText.text = ex.Message;
                    _failMessage.SetActive(true);
                }
            }
            else
            {
                _errorTombsMessage.SetActive(true);
            }
        }
        public void OpenExchangePoint()
        {
            _academyExchangePoint.SetActive(true);
        }
    }
}
