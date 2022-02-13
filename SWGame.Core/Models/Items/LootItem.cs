namespace SWGame.Core.Models.Items
{
    public class LootItem : Item
    {
        private int _prestigeValue;
        private int _wisdomValue;
        private int _imageIndex;
        private int _factionId;

        public LootItem(int id, string name, string description, int prestigeValue, int wisdomValue, int imageIndex, int factionId)
        {
            _id = id;
            _name = name;
            _descriprion = description;
            _prestigeValue = prestigeValue;
            _wisdomValue = wisdomValue;
            _imageIndex = imageIndex;
            _factionId = factionId;
        }

        public int PrestigeValue { get => _prestigeValue; set => _prestigeValue = value; }
        public int WisdomValue { get => _wisdomValue; set => _wisdomValue = value; }
        public int ImageIndex { get => _imageIndex; set => _imageIndex = value; }
        public int FactionId { get => _factionId; set => _factionId = value; }
    }
}
