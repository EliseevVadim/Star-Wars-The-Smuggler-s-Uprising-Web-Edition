using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SWGame.Core.Exceptions;
using SWGame.Core.Management;
using SWGame.Core.Models;
using SWGame.Core.Models.Items;
using SWGame.Core.Models.Items.Cards;
using SWGame.Core.Repositories;
using SWGame.Core.Services;
using SWGame.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWGame.Core.Hubs
{
    public class GameHub : Hub
    {
        private readonly LocationsRepository _locationsRepository;
        private Dictionary<int, string> _locationsNames;
        private string _viewersGroupName = "ChallengesViewers";

        public GameHub(LocationsRepository locationsRepository)
        {
            _locationsRepository = locationsRepository;
            _locationsNames = _locationsRepository.LoadLocationsNames();
        }

        public async Task RegisterPlayer(string playerData)
        {
            try
            {
                Player player = JsonConvert.DeserializeObject<Player>(playerData);
                await player.AddToDatabase();
                await Clients.Caller.SendAsync("ReceiveRegistrationResult", string.Empty);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveRegistrationResult", ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            int id = OnlineUsers.GetUserIdByConnectionId(Context.ConnectionId);
            IRepository<Player> repository = new PlayersRepository();
            Player player = repository.LoadById(id);
            if (player != null)
            {
                await player.UpdateLogoutDateTime();
            }
            OnlineUsers.RemoveUserByConnectionId(Context.ConnectionId);
            string groupName = PlayingPazaakChallenges.GetChallengeNameByConnectionId(Context.ConnectionId);
            if (groupName != null)
            {
                await Clients.OthersInGroup(groupName).SendAsync("ProcessOpponentDisconnection");
                PlayingPazaakChallenges.RemoveChallengeByGroupName(groupName);
            }
            await RemovePazaakChallenge();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _viewersGroupName);
            await GetNeighbours(player.LocationId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task CheckPlayerExistance(string name, string login, string password)
        {
            try
            {
                AuthorizationModel model = new AuthorizationModel(name, login, password);
                Player player = model.GetCorrespondingUser();
                string response = string.Empty;
                if (player != null)
                {
                    response = JsonConvert.SerializeObject(player, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });
                    OnlineUsers.AddUserId(new PlayerIdentitiesModel(player.Id, Context.ConnectionId));
                }
                await Clients.Caller.SendAsync("ReceiveAuthorizationResult", response);
            }
            catch
            {
                await Clients.Caller.SendAsync("ReceiveAuthorizationResult", string.Empty);
            }
        }

        public async Task LoadAllPlanets()
        {
            IRepository<Planet> _planetsRepository = new PlanetsRepository();
            List<Planet> planets = _planetsRepository.LoadAll();
            await Clients.Caller.SendAsync("ReceiveAllPlanets", planets);
        }

        public async Task LoadAllLocations()
        {
            IRepository<Location> _locationsRepository = new LocationsRepository();
            List<Location> locations = _locationsRepository.LoadAll();
            await Clients.Caller.SendAsync("ReceiveAllLocations", locations);
        }

        public async Task ChangeLocation(string data, int oldLocationId)
        {
            Player player = JsonConvert.DeserializeObject<Player>(data);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _locationsNames[oldLocationId]);
            await Groups.AddToGroupAsync(Context.ConnectionId, _locationsNames[player.LocationId]);
            await player.SaveNewLocationId();
            await GetNeighbours(oldLocationId);
            await GetNeighbours(player.LocationId);
            await LoadChat(player.LocationId);
        }

        public async Task GetNeighbours(int locationId)
        {
            PlayersRepository repository = new PlayersRepository();
            List<Player> correspondingLocationPlayers = repository.LoadByLocation(locationId);
            await Clients.Group(_locationsNames[locationId]).SendAsync("RedrawSubPanels", correspondingLocationPlayers);
        }

        public async Task UpdateCredits(string data)
        {
            Player player = JsonConvert.DeserializeObject<Player>(data);
            await player.SaveNewCreditsAmount();
        }

        public async Task UpdateTutorialDisplayment(string data)
        {
            Player player = JsonConvert.DeserializeObject<Player>(data);
            await player.SaveNewTutorialDisplayment();
        }

        public async Task UpdateStoryFinishing(string data)
        {
            Player player = JsonConvert.DeserializeObject<Player>(data);
            await player.SaveNewStoryFinishing();
        }

        public async Task UpdatePrestige(string data)
        {
            Player player = JsonConvert.DeserializeObject<Player>(data);
            await player.SaveNewPrestigeScore();
        }

        public async Task UpdateWisdom(string data)
        {
            Player player = JsonConvert.DeserializeObject<Player>(data);
            await player.SaveNewWisdomPoints();
        }

        public async Task SendMessage(ChatMessage message)
        {
            await message.AddToDatabase();
            IRepository<Chat> repository = new ChatRepository();
            Chat response = repository.LoadById(message.ChatId);
            await Clients.Group(_locationsNames[message.ChatId]).SendAsync("RedrawChat", response);
        }

        public async Task UpdateTreasury(string data)
        {
            Planet planet = JsonConvert.DeserializeObject<Planet>(data);
            await planet.SaveNewTreasury();
        }

        public async Task GetPlayersInfo(int id)
        {
            PlayersRepository repository = new PlayersRepository();
            PlayerViewModel playerViewModel = repository.LoadPlayersInfoById(id);
            playerViewModel.IsOnline = OnlineUsers.IsUserOnline(id);
            await Clients.Caller.SendAsync("ProcessRequestedPlayer", playerViewModel);
        }

        public async Task LoadInventoryItems(int inventoryId)
        {
            Inventory inventory = new Inventory(inventoryId);
            inventory.LoadAllItems();
            List<string> response = new List<string>();
            inventory.Cells.ForEach(cell =>
            {
                response.Add(JsonConvert.SerializeObject(cell));
            });
            await Clients.Caller.SendAsync("ProcessInventory", response);
        }

        public async Task LoadShopInfo(int locationId)
        {
            IRepository<Shop> repository = new ShopsRepository();
            Shop result = repository.LoadById(locationId);
            await Clients.Caller.SendAsync("ProcessReceivedShop", result);
        }

        public async Task LoadAllShopItems(int shopId)
        {
            Shop shop = new Shop
            {
                Id = shopId
            };
            List<ShopSlot> goods = shop.GetAllItems();
            List<string> response = new List<string>();
            goods.ForEach(item =>
            {
                response.Add(JsonConvert.SerializeObject(item));
            });
            await Clients.Caller.SendAsync("ReceiveShopItems", response);
        }

        public async Task CreateInventoryCell(InventoryCellDataModel addition)
        {
            await addition.AddToDatabase();
        }

        public async Task UpdateInventoryCell(InventoryCellDataModel target)
        {
            await target.UpdateInDatabase();
        }

        public async Task RemoveInventoryCell(InventoryCellDataModel cell)
        {
            await cell.RemoveFromDatabase();
        }

        public async Task LoadPazaakCards()
        {
            ItemsRepository repository = new ItemsRepository();
            List<Card> systemCards = repository.LoadSystemCards();
            List<ClassicalCard> classicalCards = repository.LoadClassicalCards();
            List<FlippableCard> flippableCards = repository.LoadFlippableCards();
            List<GoldCard> goldCards = repository.LoadGoldCards();
            await Clients.Caller.SendAsync("ProcessPazaakCards", systemCards, classicalCards, flippableCards, goldCards);
        }

        public async Task LoadItems()
        {
            ItemsRepository repository = new ItemsRepository();
            List<LootItem> lootItems = repository.LoadLootItems();
            List<QuestItem> questItems = repository.LoadQuestItems();
            await Clients.Caller.SendAsync("ProcessRequestedItems", lootItems, questItems);
        }

        public async Task LoadChat(int locationId)
        {
            IRepository<Chat> repository = new ChatRepository();
            Chat response = repository.LoadById(locationId);
            await Clients.Caller.SendAsync("RedrawChat", response);
        }

        public async Task AddPlayerToChallengesViewers()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, _viewersGroupName);
            await Clients.Caller.SendAsync("RedrawChallengesPanel", ActivePazaakChallenges.PazaakChallenges);
        }

        public async Task RemovePlayerFromChallengesViewers()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _viewersGroupName);
        }

        public async Task CreatePazaakChallenge(string creator, int amount)
        {
            try
            {
                ActivePazaakChallenges.AddChallenge(creator, amount, Context.ConnectionId);
                await Clients.Group(_viewersGroupName).SendAsync("RedrawChallengesPanel", ActivePazaakChallenges.PazaakChallenges);
            }
            catch (ChallengeAlreadyExistException)
            {
                await Clients.Caller.SendAsync("DisplayChallengeCreationError");
            }
        }

        public async Task RemovePazaakChallenge()
        {
            ActivePazaakChallenges.RemoveChallengeByConnectionId(Context.ConnectionId);
            List<PazaakChallenge> test = ActivePazaakChallenges.PazaakChallenges;
            await Clients.Group(_viewersGroupName).SendAsync("RedrawChallengesPanel", ActivePazaakChallenges.PazaakChallenges);
        }

        public async Task AcceptChallenge(string acceptor, string creator)
        {
            PazaakChallenge challenge = ActivePazaakChallenges.PazaakChallenges.Where(challenge => challenge.Creator == creator).FirstOrDefault();
            if (challenge != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, challenge.Name);
                string creatorsId = ActivePazaakChallenges.GetConnectionIdByCreatorsName(creator);
                await Groups.AddToGroupAsync(creatorsId, challenge.Name);
                ChallengeIdentitiesModel game = new ChallengeIdentitiesModel()
                {
                    ChallengeName = challenge.Name,
                    PlayersIds = new List<string> { creatorsId, Context.ConnectionId }
                };
                PlayingPazaakChallenges.AddGame(game);
                Random random = new Random();
                bool playerMovesFirst = random.Next(0, 2) == 1;
                await Clients.Caller.SendAsync("StartOnlinePazaakGame", challenge, playerMovesFirst);
                PazaakChallenge secondVariant = new PazaakChallenge()
                {
                    Creator = acceptor,
                    Amount = challenge.Amount,
                    Name = challenge.Name
                };
                await Clients.OthersInGroup(challenge.Name).SendAsync("StartOnlinePazaakGame", secondVariant, !playerMovesFirst);
                ActivePazaakChallenges.RemoveChallengeByConnectionId(creatorsId);
                await Clients.Group(_viewersGroupName).SendAsync("RedrawChallengesPanel", ActivePazaakChallenges.PazaakChallenges);
            }
        }

        public async Task SendCardAddition(string request)
        {
            string groupName = PlayingPazaakChallenges.GetChallengeNameByConnectionId(Context.ConnectionId);
            PazaakCardsCreator creator = new PazaakCardsCreator(request);
            Card addition = creator.CreateCard();
            string response = JsonConvert.SerializeObject(addition);
            await Clients.OthersInGroup(groupName).SendAsync("ProcessCard", response);
        }

        public async Task SendMoveFinishing()
        {
            string groupName = PlayingPazaakChallenges.GetChallengeNameByConnectionId(Context.ConnectionId);
            await Clients.OthersInGroup(groupName).SendAsync("ProcessMoveFinishing");
        }

        public async Task SendStandStatement()
        {
            string groupName = PlayingPazaakChallenges.GetChallengeNameByConnectionId(Context.ConnectionId);
            await Clients.OthersInGroup(groupName).SendAsync("ProcessStandStatement");
        }

        public void FinishPazaakGame()
        {
            string groupName = PlayingPazaakChallenges.GetChallengeNameByConnectionId(Context.ConnectionId);
            PlayingPazaakChallenges.RemoveChallengeByGroupName(groupName);
        }
    }
}
