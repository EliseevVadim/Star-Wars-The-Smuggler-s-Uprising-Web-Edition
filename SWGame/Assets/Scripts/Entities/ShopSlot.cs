using SWGame.Entities.Items;

namespace SWGame.Entities
{
    public class ShopSlot
    {
        private int _shopId;
        private int _price;
        private int _revenue;
        private Item _stuff;

        public ShopSlot(int shopId, int price, Item stuff)
        {
            _shopId = shopId;
            _price = price;
            _stuff = stuff;
            _revenue = 0;
        }

        public int ShopId { get => _shopId; set => _shopId = value; }
        public int Price { get => _price; set => _price = value; }
        public Item Stuff { get => _stuff; set => _stuff = value; }
        public int Revenue { get => _revenue; set => _revenue = value; }
    }
}
