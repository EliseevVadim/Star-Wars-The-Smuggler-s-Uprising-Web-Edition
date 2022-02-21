using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Linq;
using SWGame.Entities;
using SWGame.View.Scenes;
using UnityEngine.SceneManagement;
using SWGame.ViewModels;
using SWGame.Management.ItemCellsCreators;
using SWGame.Entities.Items.Cards;
using SWGame.Management.Repositories;
using SWGame.Entities.Items;
using SWGame.Activities.PazaakTools.OnlinePazaak;

namespace SWGame.GlobalConfigurations
{
    public class ClientManager : MonoBehaviour
    {
        private static ClientManager _instance;

        private string _url = "https://localhost:44376/sw";
        private static HubConnection _hubConnection;

        private RegistrationScene _registrationScene;
        private AuthorizationScene _authorizationScene;
        private MainScene _mainScene;

        private MessagesDispatcher _messagesDispatcher;

        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _errorText;

        public RegistrationScene RegistrationScene { get => _registrationScene; set => _registrationScene = value; }
        public AuthorizationScene AuthorizationScene { get => _authorizationScene; set => _authorizationScene = value; }
        public GameObject ErrorMessage { get => _errorMessage; set => _errorMessage = value; }
        public Text ErrorText { get => _errorText; set => _errorText = value; }
        public MainScene MainScene { get => _mainScene; set => _mainScene = value; }

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            if (_hubConnection == null)
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(_url)                  
                    .Build();
                _hubConnection.ServerTimeout = TimeSpan.FromMinutes(5);
                _hubConnection.Closed += async (error) =>
                {
                    await Task.Delay(1000);
                    Debug.Log(error);
                    UpdateMessagesDispatcher();
                    _messagesDispatcher.AddMessage(new Action(() =>
                    {
                        _messagesDispatcher.ConnectionErrorText.text = "Ошибка подключения. Проверьте ваше соединение с Интернетом" +
                        " и перезапустите приложение.";
                        _messagesDispatcher.ConnectionErrorMessage.SetActive(true);
                    }));

                    await _hubConnection.StartAsync();
                };
                Connect();
            }
        }

        private async void Connect()
        {
            _hubConnection.On<string>("ReceiveRegistrationResult", (response) =>
            {                               
                _registrationScene.ProcessRegistrationResult(response);
            });
            _hubConnection.On<string>("ReceiveAuthorizationResult", (response) =>
            {
                _authorizationScene.ProcessAuthorizationResult(response);
            });
            _hubConnection.On<List<Planet>>("ReceiveAllPlanets", (planets) =>
            {
                _mainScene.ProcessReceivedPlanets(planets);
            });
            _hubConnection.On<List<Location>>("ReceiveAllLocations", (response) =>
            {
                _mainScene.ProcessReceivedLocations(response);
            });
            _hubConnection.On<List<Player>>("RedrawSubPanels", (players) =>
            {
                _mainScene.RedrawSubPanels(players);
            });
            _hubConnection.On<PlayerViewModel>("ProcessRequestedPlayer", (playerViewModel) =>
            {
                _mainScene.ShowPlayersInfo(playerViewModel);
            });
            _hubConnection.On<List<string>>("ProcessInventory", (response) =>
            {
                List<Dictionary<string, object>> cells = new List<Dictionary<string, object>>();
                try
                {
                    response.ForEach(cell =>
                    {
                        Dictionary<string, object> pair = JsonConvert.DeserializeObject<Dictionary<string, object>>(cell);
                        cells.Add(pair);
                    });
                    IItemCellsCreator<InventoryCell> cellsCreator = new InventoryCellsCreator();
                    _mainScene.SetInventoryCellsToPlayer(cellsCreator.ProcessCellsData(cells));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            });
            _hubConnection.On<Shop>("ProcessReceivedShop", (shop) =>
            {
                if (shop != null)
                {
                    _mainScene.AttachShopToLocation(shop);
                }
            });
            _hubConnection.On<List<string>>("ReceiveShopItems", (response) =>
            {
                List<Dictionary<string, object>> slots = new List<Dictionary<string, object>>();
                try
                {
                    response.ForEach(slot =>
                    {
                        Dictionary<string, object> pair = JsonConvert.DeserializeObject<Dictionary<string, object>>(slot);
                        slots.Add(pair);
                    });
                    IItemCellsCreator<ShopSlot> creator = new ShopSlotsCreator();
                    _mainScene.SetGoodsToCurrentShop(creator.ProcessCellsData(slots));
                }
                catch(Exception ex)
                {
                    Debug.LogException(ex);
                }

            });
            _hubConnection.On<List<Card>, List<ClassicalCard>, List<FlippableCard>, List<GoldCard>>("ProcessPazaakCards", (systemCards, classicalCards, flippableCards, goldCards) =>
            {
                CardsRepository.SystemCards = systemCards;
                CardsRepository.ClassicalCards = classicalCards;
                CardsRepository.FlippableCards = flippableCards;
                CardsRepository.GoldCards = goldCards;
            });
            _hubConnection.On<List<LootItem>, List<QuestItem>>("ProcessRequestedItems", (lootItems, questItems) =>
            {
                ItemsRepository.QuestItems = questItems;
                ItemsRepository.LootItems = lootItems;
                ItemsRepository.SithItems = lootItems.Where(item => item.FactionId == 1).ToList();
                ItemsRepository.JediItems = lootItems.Where(item => item.FactionId == 2).ToList();
            });
            _hubConnection.On<Chat>("RedrawChat", (chat) =>
            {
                _mainScene.RedrawChatPanel(chat);
            });
            _hubConnection.On<List<PazaakChallenge>>("RedrawChallengesPanel", (challenges) =>
            {
                _mainScene.RedrawChallengesArea(challenges);
            });
            _hubConnection.On("DisplayChallengeCreationError", () =>
            {
                _mainScene.DisplayChallengeCreationError();
            });
            _hubConnection.On<PazaakChallenge, bool>("StartOnlinePazaakGame", (challenge, movesFirst) =>
            {
                _mainScene.StartOnlinePazaakGame(challenge, movesFirst);
            });
            _hubConnection.On<string>("ProcessCard", (response) =>
            {
                _mainScene.ProcessCard(response);
            });
            _hubConnection.On("ProcessMoveFinishing", () =>
            {
                _mainScene.ProcessMoveFinishing();
            });
            _hubConnection.On("ProcessStandStatement", () =>
            {
                _mainScene.ProcessStandStatement();
            });
            _hubConnection.On("ProcessOpponentDisconnection", () =>
            {
                _mainScene.ProcessOpponentDisconnection();
            });
            try
            {
                await _hubConnection.StartAsync();
                Debug.Log("Connection started");
            }
            catch (Exception ex)
            {
                UpdateMessagesDispatcher();
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    _messagesDispatcher.ConnectionErrorText.text = "Ошибка подключения. Сервер не отвечает на запрос.";
                    _messagesDispatcher.ConnectionErrorMessage.SetActive(true);
                }));
                Debug.Log(ex.Message);
            }
        }
        public async Task RegisterPlayerAsync(Player player)
        {
            string data = JsonConvert.SerializeObject(player, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,               
            });
            await _hubConnection.InvokeAsync("RegisterPlayer", data);
        }
        public async Task CheckUserExistanceAsync(string name, string login, string password)
        {
            await _hubConnection.InvokeAsync("CheckPlayerExistance", name, login, password);
        }

        public void UpdateMessagesDispatcher()
        {
            var scene = SceneManager.GetActiveScene();
            var canvas = scene.GetRootGameObjects()[1];
            _messagesDispatcher = canvas.GetComponent<MessagesDispatcher>();
            _messagesDispatcher.ConnectionErrorMessage = _errorMessage;
            _messagesDispatcher.ConnectionErrorText = _errorText;
        }

        public async Task LoadAllPlanets()
        {
            await _hubConnection.InvokeAsync("LoadAllPlanets");
        }

        public async Task LoadAllLocations()
        {
            await _hubConnection.InvokeAsync("LoadAllLocations");
        }

        public async Task ChangePlayersLocation(Player player, int newLocationId)
        {
            string data = JsonConvert.SerializeObject(player);
            await _hubConnection.InvokeAsync("ChangeLocation", data, newLocationId);
        }

        public async Task UpdatePlayersCredits(Player player)
        {
            string data = JsonConvert.SerializeObject(player);
            await _hubConnection.InvokeAsync("UpdateCredits", data);
        }

        public async Task UpdatePlayersTutorialDisplayment(Player player)
        {
            string data = JsonConvert.SerializeObject(player);
            await _hubConnection.InvokeAsync("UpdateTutorialDisplayment", data);
        }

        public async Task UpdatePlayersPrestige(Player player)
        {
            string data = JsonConvert.SerializeObject(player);
            await _hubConnection.InvokeAsync("UpdatePrestige", data);
        }

        public async Task UpdatePlayersWisdom(Player player)
        {
            string data = JsonConvert.SerializeObject(player);
            await _hubConnection.InvokeAsync("UpdateWisdom", data);
        }

        public async Task GetPlayersNeighbours(int locationId)
        {
            await _hubConnection.InvokeAsync("GetNeighbours", locationId);
        }

        public async Task UpdatePlanetsTreasury(Planet planet)
        {
            string data = JsonConvert.SerializeObject(planet);
            await _hubConnection.InvokeAsync("UpdateTreasury", data);
        }

        public async Task GetPlayersInfo(int playersId)
        {
            await _hubConnection.InvokeAsync("GetPlayersInfo", playersId);
        }

        public async Task AddPlayerToChallengesViewers()
        {
            await _hubConnection.InvokeAsync("AddPlayerToChallengesViewers");
        }

        public async Task LoadInventoryItems(int inventoryId)
        {
            await _hubConnection.InvokeAsync("LoadInventoryItems", inventoryId);
        }

        public async Task LoadShopInfo(int locationId)
        {
            await _hubConnection.InvokeAsync("LoadShopInfo", locationId);
        }

        public async Task LoadAllShopItems(int shopId)
        {
            await _hubConnection.InvokeAsync("LoadAllShopItems", shopId);
        }

        public async Task LoadChatInfo(int locationId)
        {
            await _hubConnection.InvokeAsync("LoadChatInfo", locationId);
        }

        public async Task LoadMessages(int chatId)
        {
            await _hubConnection.InvokeAsync("LoadMessages", chatId);
        }

        public async Task CreateInventoryCell(InventoryCellDataModel cell)
        {
            await _hubConnection.InvokeAsync("CreateInventoryCell", cell);
        }

        public async Task UpdateInventoryCell(InventoryCellDataModel cell)
        {
            await _hubConnection.InvokeAsync("UpdateInventoryCell", cell);
        }

        public async Task RemoveInventoryCell(InventoryCellDataModel cell)
        {
            await _hubConnection.InvokeAsync("RemoveInventoryCell", cell);
        }

        public async Task LoadPazaakCards()
        {
            await _hubConnection.InvokeAsync("LoadPazaakCards");
        }

        public async Task LoadItems()
        {
            await _hubConnection.InvokeAsync("LoadItems");
        }

        public async Task SendMessage(ChatMessage message)
        {
            await _hubConnection.InvokeAsync("SendMessage", message);
        }

        public async Task CreatePazaakChallenge(string creator, int amount)
        {
            await _hubConnection.InvokeAsync("CreatePazaakChallenge", creator, amount);
        }

        public async Task RemoveChallenge()
        {
            await _hubConnection.InvokeAsync("RemovePazaakChallenge");
        }

        public async Task AcceptChallenge(string acceptor, string creator)
        {
            await _hubConnection.InvokeAsync("AcceptChallenge", acceptor, creator);
        }

        public async Task SendCardAddition(Card addition)
        {
            string request = JsonConvert.SerializeObject(addition);
            await _hubConnection.InvokeAsync("SendCardAddition", request);
        }
        
        public async Task SendMoveFinishing()
        {
            await _hubConnection.InvokeAsync("SendMoveFinishing");
        }

        public async Task SendStandStatement()
        {
            await _hubConnection.InvokeAsync("SendStandStatement");
        }

        public async Task SendGameFinishing()
        {
            await _hubConnection.InvokeAsync("FinishPazaakGame");
        }
    }
}
