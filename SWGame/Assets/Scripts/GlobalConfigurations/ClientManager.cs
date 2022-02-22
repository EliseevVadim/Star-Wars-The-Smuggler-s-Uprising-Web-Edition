using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using SWGame.Activities.PazaakTools.OnlinePazaak;
using SWGame.Entities;
using SWGame.Entities.Items;
using SWGame.Entities.Items.Cards;
using SWGame.Management.ItemCellsCreators;
using SWGame.Management.Repositories;
using SWGame.View.Scenes;
using SWGame.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            if (_instance == null)
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
                catch (Exception ex)
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
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
                Debug.Log(ex.Message);
            }
        }
        public async Task RegisterPlayerAsync(Player player)
        {
            try
            {
                string data = JsonConvert.SerializeObject(player, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                await _hubConnection.InvokeAsync("RegisterPlayer", data);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }
        public async Task CheckUserExistanceAsync(string name, string login, string password)
        {
            try
            {
                await _hubConnection.InvokeAsync("CheckPlayerExistance", name, login, password);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
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
            try
            {
                await _hubConnection.InvokeAsync("LoadAllPlanets");
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task LoadAllLocations()
        {
            try
            {
                await _hubConnection.InvokeAsync("LoadAllLocations");
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task ChangePlayersLocation(Player player, int newLocationId)
        {
            try
            {
                string data = JsonConvert.SerializeObject(player);
                await _hubConnection.InvokeAsync("ChangeLocation", data, newLocationId);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task UpdatePlayersCredits(Player player)
        {
            try
            {
                string data = JsonConvert.SerializeObject(player);
                await _hubConnection.InvokeAsync("UpdateCredits", data);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task UpdatePlayersTutorialDisplayment(Player player)
        {
            try
            {
                string data = JsonConvert.SerializeObject(player);
                await _hubConnection.InvokeAsync("UpdateTutorialDisplayment", data);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task UpdateStoryFinishing(Player player)
        {
            try
            {
                string data = JsonConvert.SerializeObject(player);
                await _hubConnection.InvokeAsync("UpdateStoryFinishing", data);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task UpdatePlayersPrestige(Player player)
        {
            try
            {
                string data = JsonConvert.SerializeObject(player);
                await _hubConnection.InvokeAsync("UpdatePrestige", data);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task UpdatePlayersWisdom(Player player)
        {
            try
            {
                string data = JsonConvert.SerializeObject(player);
                await _hubConnection.InvokeAsync("UpdateWisdom", data);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task GetPlayersNeighbours(int locationId)
        {
            try
            {
                await _hubConnection.InvokeAsync("GetNeighbours", locationId);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task UpdatePlanetsTreasury(Planet planet)
        {
            try
            {
                string data = JsonConvert.SerializeObject(planet);
                await _hubConnection.InvokeAsync("UpdateTreasury", data);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task GetPlayersInfo(int playersId)
        {
            try
            {
                await _hubConnection.InvokeAsync("GetPlayersInfo", playersId);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task AddPlayerToChallengesViewers()
        {
            try
            {
                await _hubConnection.InvokeAsync("AddPlayerToChallengesViewers");
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task LoadInventoryItems(int inventoryId)
        {
            try
            {
                await _hubConnection.InvokeAsync("LoadInventoryItems", inventoryId);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task LoadShopInfo(int locationId)
        {
            try
            {
                await _hubConnection.InvokeAsync("LoadShopInfo", locationId);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task LoadAllShopItems(int shopId)
        {
            try
            {
                await _hubConnection.InvokeAsync("LoadAllShopItems", shopId);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task LoadChatInfo(int locationId)
        {
            try
            {
                await _hubConnection.InvokeAsync("LoadChatInfo", locationId);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task LoadMessages(int chatId)
        {
            try
            {
                await _hubConnection.InvokeAsync("LoadMessages", chatId);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task CreateInventoryCell(InventoryCellDataModel cell)
        {
            try
            {
                await _hubConnection.InvokeAsync("CreateInventoryCell", cell);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task UpdateInventoryCell(InventoryCellDataModel cell)
        {
            try
            {
                await _hubConnection.InvokeAsync("UpdateInventoryCell", cell);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task RemoveInventoryCell(InventoryCellDataModel cell)
        {
            try
            {
                await _hubConnection.InvokeAsync("RemoveInventoryCell", cell);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task LoadPazaakCards()
        {
            try
            {
                await _hubConnection.InvokeAsync("LoadPazaakCards");
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task LoadItems()
        {
            try
            {
                await _hubConnection.InvokeAsync("LoadItems");
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task SendMessage(ChatMessage message)
        {
            try
            {
                await _hubConnection.InvokeAsync("SendMessage", message);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task CreatePazaakChallenge(string creator, int amount)
        {
            try
            {
                await _hubConnection.InvokeAsync("CreatePazaakChallenge", creator, amount);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task RemoveChallenge()
        {
            try
            {
                await _hubConnection.InvokeAsync("RemovePazaakChallenge");
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task AcceptChallenge(string acceptor, string creator)
        {
            try
            {
                await _hubConnection.InvokeAsync("AcceptChallenge", acceptor, creator);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task SendCardAddition(Card addition)
        {
            try
            {
                string request = JsonConvert.SerializeObject(addition);
                await _hubConnection.InvokeAsync("SendCardAddition", request);
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task SendMoveFinishing()
        {
            try
            {
                await _hubConnection.InvokeAsync("SendMoveFinishing");
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task SendStandStatement()
        {
            try
            {
                await _hubConnection.InvokeAsync("SendStandStatement");
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        public async Task SendGameFinishing()
        {
            try
            {
                await _hubConnection.InvokeAsync("FinishPazaakGame");
            }
            catch
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    DisplayErrorMessage("Ошибка подключения. Сервер не отвечает на запрос.");
                }));
            }
        }

        private void DisplayErrorMessage(string message)
        {
            _messagesDispatcher.ConnectionErrorText.text = message;
            _messagesDispatcher.ConnectionErrorMessage.SetActive(true);
        }
    }
}
