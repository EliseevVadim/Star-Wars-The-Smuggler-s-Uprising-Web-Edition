using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWGame.Management.Repositories;
using Newtonsoft.Json;
using SWGame.GlobalConfigurations;
using SWGame.Entities.Items;
using SWGame.Entities.Items.Cards;

namespace SWGame.Entities
{
    public class Player
    {
        private int _id;
        private string _nickname;
        private string _login;
        private string _password;
        private Sprite _avatar;
        private long _credits;
        private int _prestige;
        private int _wisdomPoints;
        private int _locationId;
        private bool _storyFinished;
        private bool _needToShowTutorial;
        private int _avatarIndex;
        private Location _location;
        private Planet _planet;
        private DateTime _logoutDateTime;
        private bool _isOnline;
        private Inventory _inventory;

        private LocationsRepository _locationsRepository;
        private PlanetsRepository _planetsRepository;
        private ClientManager _clientManager;
        private CurrentPlayerInfoPresenter _currentPlayerInfoPresenter;

        [JsonConstructor]
        public Player(int id, string nickname, string login, string password, long credits, int prestige, int wisdomPoints, int locationId, int avatarIndex, bool storyFinished, bool showTutorial, DateTime logoutDateTime)
        {
            _id = id;
            _nickname = nickname;
            _login = login;
            _password = password;
            _avatarIndex = avatarIndex;
            _credits = credits;
            _prestige = prestige;
            _wisdomPoints = wisdomPoints;
            _locationId = locationId;
            _storyFinished = storyFinished;
            _avatar = AvatarsRepository.Avatars[_avatarIndex];
            _needToShowTutorial = showTutorial;
            _logoutDateTime = logoutDateTime;
        }

        public Player(int id, string name, bool isOnline)
        {
            _id = id;
            _nickname = name;
            _isOnline = isOnline;
        }

        public string Nickname { get => _nickname; set => _nickname = value; }
        public string Login { get => _login; set => _login = value; }
        public string Password { get => _password; set => _password = value; }

        [JsonIgnore]
        public Sprite Avatar { get => _avatar; set => _avatar = value; }

        public long Credits 
        {
            get => _credits;
            set
            {
                _credits = value;
                UpdateCredits();
                UpdateSideView();
            }
        }

        public int Prestige 
        { 
            get => _prestige; 
            set
            {
                _prestige = value;
                UpdatePrestige();
                UpdateSideView();
            }
        }
        public int WisdomPoints 
        { 
            get => _wisdomPoints; 
            set
            {
                _wisdomPoints = value;
                UpdateWisdom();
                UpdateSideView();
            }
        }
        public int AvatarIndex { get => _avatarIndex; set => _avatarIndex = value; }
        public int LocationId 
        {
            get => _locationId;
            set
            {
                int oldLocationId = _locationId;
                _locationId = value;
                UpdateLocation(oldLocationId);
            }
        }
        public bool StoryFinished 
        { 
            get => _storyFinished; 
            set
            {
                _storyFinished = value;
                UpdateStoryFinishing();
            }
        }
        public bool NeedToShowTutorial 
        { 
            get => _needToShowTutorial; 
            set
            {
                _needToShowTutorial = value;
                UpdateTutorialDisplayment();
            }
        }
        [JsonIgnore]
        public Location Location { get => _location; set => _location = value; }
        [JsonIgnore]
        public Planet Planet { get => _planet; set => _planet = value; }
        [JsonIgnore]
        public DateTime LogoutDateTime { get => _logoutDateTime; set => _logoutDateTime = value; }
        public int Id { get => _id; set => _id = value; }

        [JsonIgnore]
        public LocationsRepository LocationsRepository { get => _locationsRepository; set => _locationsRepository = value; }
        [JsonIgnore]
        public PlanetsRepository PlanetsRepository { get => _planetsRepository; set => _planetsRepository = value; }
        [JsonIgnore]
        public ClientManager ClientManager { get => _clientManager; set => _clientManager = value; }
        public bool IsOnline { get => _isOnline; set => _isOnline = value; }
        [JsonIgnore]
        public Inventory Inventory { get => _inventory; set => _inventory = value; }
        [JsonIgnore]
        public CurrentPlayerInfoPresenter CurrentPlayerInfoPresenter { get => _currentPlayerInfoPresenter; set => _currentPlayerInfoPresenter = value; }

        public void SetLocation()
        {
            _location = _locationsRepository.Locations[_locationId - 1];
        }

        public async void UpdateLocation(int oldLocationId)
        {
            SetLocation();
            await _clientManager.ChangePlayersLocation(this, oldLocationId);
        }

        private async void UpdateCredits()
        {
            await _clientManager.UpdatePlayersCredits(this);
        }

        private async void UpdateTutorialDisplayment()
        {
            try
            {
                await _clientManager.UpdatePlayersTutorialDisplayment(this);
            }
            catch
            {

            }
        }

        private async void UpdateStoryFinishing()
        {
            await _clientManager.UpdateStoryFinishing(this);
        }

        private async void UpdatePrestige()
        {
            await _clientManager.UpdatePlayersPrestige(this);
        }

        private async void UpdateWisdom()
        {
            await _clientManager.UpdatePlayersWisdom(this);
        }

        public void SetPlanet()
        {
            _planet = _planetsRepository.Planets[GetPlanetIndex()];
        }

        public int GetPlanetIndex()
        {
            return _locationsRepository.Locations[_locationId - 1].PlanetId - 1;
        }

        public void LoadInventory()
        {
            _inventory = new Inventory(_id, _clientManager);
        }

        public bool HasA(Item item)
        {
            return _inventory.Contains(item);
        }

        public bool CanPlayPazaak()
        {
            int cardsCount = 0;
            foreach (InventoryCell cell in _inventory.Cells)
            {
                if (cell.Content is Card)
                {
                    cardsCount += cell.Count;
                }
            }
            return cardsCount >= 4;
        }

        private void UpdateSideView()
        {
            try
            {
                _currentPlayerInfoPresenter.UpdateView(this);
            }
            catch { }
        }
    }
}

