using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace SWGame.Entities
{
    public class Planet
    {
        private int _id;
        private string _name;
        private string _description;
        private int _travellCost;
        private long _treasury;
        private int _iconIdex;
        private List<Location> _locations;
        private GameObject _view;
        private Sprite _smallIcon;

        [JsonConstructor]
        public Planet(int id, string name, string description, int travellCost, long treasury, int iconIdex)
        {
            _id = id;
            _name = name;
            _description = description;
            _travellCost = travellCost;
            _treasury = treasury;
            _locations = new List<Location>();
            _iconIdex = iconIdex;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Descriprion { get => _description; set => _description = value; }
        public int TravellCost { get => _travellCost; set => _travellCost = value; }
        public long Treasury { get => _treasury; set => _treasury = value; }

        [JsonIgnore]
        public List<Location> Locations { get => _locations; set => _locations = value; }
        [JsonIgnore]
        public GameObject View { get => _view; set => _view = value; }
        [JsonIgnore]
        public Sprite SmallIcon { get => _smallIcon; set => _smallIcon = value; }
        public int IconIdex { get => _iconIdex; set => _iconIdex = value; }
    }
}
