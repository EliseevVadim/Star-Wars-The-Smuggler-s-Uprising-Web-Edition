using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using SWGame.GlobalConfigurations;

namespace SWGame.Entities
{
    public class Location
    {
        private int _id;
        private string _name;
        private GameObject _view;
        private int _planetId;

        private Shop _shop;
        private Chat _chat;

        private ClientManager _clientManager;

        [JsonConstructor]
        public Location(int id, string name, GameObject view, int planetId)
        {
            _id = id;
            _name = name;
            _view = view;
            _planetId = planetId;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }

        [JsonIgnore]
        public GameObject View { get => _view; set => _view = value; }
        public int PlanetId { get => _planetId; set => _planetId = value; }
        [JsonIgnore]
        public Shop Shop { get => _shop; set => _shop = value; }
        [JsonIgnore]
        public ClientManager ClientManager { get => _clientManager; set => _clientManager = value; }
        [JsonIgnore]
        public Chat Chat { get => _chat; set => _chat = value; }

        public async void TryToLoadShop()
        {
            await _clientManager.LoadShopInfo(_id);
        }

        public async void LoadChat()
        {
            await _clientManager.LoadChatInfo(_id);
        }
    }
}
