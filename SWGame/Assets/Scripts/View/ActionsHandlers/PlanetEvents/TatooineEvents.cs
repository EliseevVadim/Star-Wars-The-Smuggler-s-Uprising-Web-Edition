using SWGame.Entities;
using SWGame.Management;
using SWGame.Management.Repositories;
using SWGame.View.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View.ActionsHandlers.PlanetEvents
{
    public class TatooineEvents : MonoBehaviour
    {
        [SerializeField] private GameObject _successHuntingMessage;
        [SerializeField] private GameObject _failHuntingMessage;
        [SerializeField] private GameObject _errorHuntingMessage;
        [SerializeField] private Image _perlIconField;

        private Planet _currentPlanet;
        private Player _currentPlayer;

        private PlanetsRepository _planetsRepository;

        private const int Chance = 10;

        private void Start()
        {
            _planetsRepository = GetComponentInParent<MainScene>().PlanetsRepository;
            _currentPlanet = _planetsRepository.Planets[3];
            _currentPlayer = CurrentPlayer.Player;
        }
        public void GoToDesert()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[1].Id;
            _currentPlanet.Locations[0].View.SetActive(false);
            _currentPlanet.Locations[1].View.SetActive(true);
        }
        public void ReturnToMosEisley()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[0].Id;
            _currentPlanet.Locations[1].View.SetActive(false);
            _currentPlanet.Locations[0].View.SetActive(true);
        }
        public async void HuntTheDragon()
        {
            if (_currentPlayer.HasA(ItemsRepository.QuestItems[0]))
            {
                await _currentPlayer.Inventory.RemoveItem(ItemsRepository.QuestItems[0]);
                int coefficient = Random.Range(0, 100);
                if (coefficient <= Chance)
                {
                    await _currentPlayer.Inventory.AddItem(ItemsRepository.QuestItems[1]);
                    _perlIconField.sprite = ItemsRepository.QuestItems[1].Image;
                    _successHuntingMessage.SetActive(true);
                }
                else
                {
                    _failHuntingMessage.SetActive(true);
                }
            }
            else
            {
                _errorHuntingMessage.SetActive(true);
            }
        }
    }
}
