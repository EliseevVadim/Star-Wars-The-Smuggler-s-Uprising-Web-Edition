using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using SWGame.Entities;
using SWGame.Management;
using SWGame.GlobalConfigurations;
using SWGame.Management.Repositories;
using SWGame.View.Presenters;
using Newtonsoft.Json;
using SWGame.ViewModels;
using SWGame.Activities.PazaakTools.OnlinePazaak;
using SWGame.Entities.Items.Cards;

namespace SWGame.View.Scenes
{
    public class MainScene : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _planetsViews;
        [SerializeField] private List<GameObject> _locationsViews;
        [SerializeField] private GameObject _settings;
        [SerializeField] private GameObject _spaceport;
        [SerializeField] private GameObject _rewardMessage;
        [SerializeField] private GameObject _inventoryView;
        [SerializeField] private GameObject _lackOfCardsMessage;
        [SerializeField] private GameObject _getAmountMessage;
        [SerializeField] private GameObject _tutorial;
        [SerializeField] private NeighbourPlayersVisualizator _playersArea;
        [SerializeField] private PazaakChallengesVisualizator _challengesArea;
        [SerializeField] private ChatVisualizator _chatArea;
        [SerializeField] private SelectedPlayerPresenter _playerPresenter;
        [SerializeField] private GameObject _exitConfirmation;
        [SerializeField] private GameObject _shop;
        [SerializeField] private InputField _messageField;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _errorText;
        [SerializeField] private GameObject _onlinePazaakGame;

        private Player _currentPlayer;
        private ClientManager _clientManager;
        private MessagesDispatcher _messagesDispatcher;
        private OnlinePazaakGame _onlinePazaak;

        private PlanetsRepository _planetsRepository;
        private LocationsRepository _locationsRepository;

        private const int DailyReward = 5000;

        public PlanetsRepository PlanetsRepository { get => _planetsRepository; set => _planetsRepository = value; }
        public ClientManager ClientManager { get => _clientManager; }

        private void Awake()
        {
            _currentPlayer = CurrentPlayer.Player;
            _messagesDispatcher = GetComponent<MessagesDispatcher>();
            _clientManager = FindObjectOfType<ClientManager>().GetComponent<ClientManager>();
            _onlinePazaak = _onlinePazaakGame.GetComponent<OnlinePazaakGame>();
            _clientManager.MainScene = this;
            List<Sprite> lootSprites = LootItemsIconsRepository.ItemsSprites;
            List<Sprite> questSprites = QuestItemsIconsRepository.Icons;
            List<Sprite> cardsSprites = CardsImagesRepository.Cards;
            CardsRepository.LoadAllCards(_clientManager);
            ItemsRepository.LoadAllItems(_clientManager);
        }

        private async void Start()
        {
            _planetsRepository = new PlanetsRepository(_clientManager);
            _locationsRepository = new LocationsRepository(_clientManager);
            _currentPlayer = CurrentPlayer.Player;
            await _planetsRepository.LoadPlanets();
            await _locationsRepository.LoadAllLocations();
            _planetsRepository.Planets.ForEach(planet =>
            {
                planet.Locations.AddRange(_locationsRepository.Locations.
                    Where(location => location.PlanetId == planet.Id));
            });
            CurrentPlayerInfoPresenter presenter = FindObjectOfType<CurrentPlayerInfoPresenter>();
            _currentPlayer.CurrentPlayerInfoPresenter = presenter;
            _currentPlayer.LocationsRepository = _locationsRepository;
            _currentPlayer.PlanetsRepository = _planetsRepository;
            _currentPlayer.ClientManager = _clientManager;
            _currentPlayer.LocationId = _currentPlayer.LocationId;
            _currentPlayer.SetPlanet();
            _currentPlayer.LoadInventory();
            _currentPlayer.Planet.View.SetActive(true);
            _currentPlayer.Location.View.SetActive(true);
            presenter.UpdateView(_currentPlayer);
            if (_currentPlayer.LogoutDateTime < DateTime.Today)
            {
                _currentPlayer.Credits += DailyReward;
                _rewardMessage.SetActive(true);
            }
            if (_currentPlayer.NeedToShowTutorial)
            {
                _tutorial.SetActive(true);
            }
        }

        private void Update()
        {
            if (_messageField.isFocused)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_spaceport.activeSelf)
                {
                    _spaceport.SetActive(false);
                }
                else
                {
                    _exitConfirmation.SetActive(true);
                }
            }
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.L))
            {
                _settings.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                _inventoryView.SetActive(!_inventoryView.activeSelf);
            }
            if (Input.GetKey(KeyCode.T))
            {
                _tutorial.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                GetInfoAboutCurrentPlayer();
            }
        }

        public void ProcessReceivedPlanets(List<Planet> planets)
        {
            _planetsRepository.Planets = planets;
            int i = 0;
            _planetsRepository.Planets.ForEach(planet =>
            {
                planet.View = _planetsViews[i];
                i++;
            });
        }

        public void ProcessReceivedLocations(List<Location> locations)
        {
            _locationsRepository.Locations = locations;
            int i = 0;
            _locationsRepository.Locations.ForEach(location =>
            {
                location.View = _locationsViews[i];
                location.ClientManager = _clientManager;
                location.TryToLoadShop();
                i++;
            });
        }

        public void RedrawSubPanels(List<Player> players)
        {
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                _playersArea.Render(players);
            }));
        }

        public void RedrawChatPanel(Chat chat)
        {
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                _chatArea.Render(chat);
            }));
        }

        public void RedrawChallengesArea(List<PazaakChallenge> challenges)
        {
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                _challengesArea.Render(challenges);
            }));
        }

        public async void GetPlayersInfo(int id)
        {
            await _clientManager.GetPlayersInfo(id);
        }

        public async void GetInfoAboutCurrentPlayer()
        {
            await _clientManager.GetPlayersInfo(_currentPlayer.Id);
        }

        public void ShowPlayersInfo(PlayerViewModel playerViewModel)
        {
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                _playerPresenter.VisualizePlayer(playerViewModel);
            }));
        }

        public void SetInventoryCellsToPlayer(List<InventoryCell> cells)
        {
            _currentPlayer.Inventory.Cells = cells;
        }

        public void AttachShopToLocation(Shop shop)
        {
            _locationsRepository.Locations.Where(location => location.Id == shop.LocationId).FirstOrDefault().Shop = shop;
        }

        public async void LoadAllShopItems()
        {
            await _clientManager.LoadAllShopItems(_currentPlayer.Location.Shop.Id);
        }

        public void SetGoodsToCurrentShop(List<ShopSlot> slots)
        {
            _currentPlayer.Location.Shop.Slots = slots;
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                _shop.SetActive(true);
            }));
        }

        public void StartPazaakGame()
        {
            if (_currentPlayer.CanPlayPazaak())
            {
                _getAmountMessage.SetActive(true);
            }
            else
            {
                _lackOfCardsMessage.SetActive(true);
            }
        }

        public void DisplayChallengeCreationError()
        {
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                _errorText.text = "Невозможно создать больше одной игры";
                _errorMessage.SetActive(true);
            }));
        }

        public void StartOnlinePazaakGame(PazaakChallenge challenge, bool movesFirst)
        {
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                _onlinePazaak.SetInitialInfo(challenge.Amount, challenge.Creator, movesFirst);
                _onlinePazaakGame.SetActive(true);
            }));
        }

        public void ProcessCard(string response)
        {
            PazaakCardsCreator creator = new PazaakCardsCreator(response);
            Card card = creator.CreateCard();
            _onlinePazaak.ReceiveOpponentsCard(card);
        }

        public void ProcessMoveFinishing()
        {
            _onlinePazaak.ProcessMoveFinishing();
        }

        public void ProcessStandStatement()
        {
            _onlinePazaak.ProcessStandStatement();
        }

        public void ProcessOpponentDisconnection()
        {
            _onlinePazaak.ProcessOpponentDisconnection();
        }
    }
}
