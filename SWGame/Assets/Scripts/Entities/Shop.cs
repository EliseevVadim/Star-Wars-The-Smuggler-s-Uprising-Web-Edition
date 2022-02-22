using Newtonsoft.Json;
using System.Collections.Generic;

namespace SWGame.Entities
{
    public class Shop
    {
        private int _id;
        private string _name;
        private int _locationId;
        private int _revenue;
        private List<ShopSlot> _slots;

        [JsonConstructor]
        public Shop(int id, string name, int locationId, int revenue)
        {
            _id = id;
            _name = name;
            _locationId = locationId;
            _revenue = revenue;
            _slots = new List<ShopSlot>();
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public int LocationId { get => _locationId; set => _locationId = value; }
        public int Revenue { get => _revenue; set => _revenue = value; }
        public List<ShopSlot> Slots { get => _slots; set => _slots = value; }
    }
}
