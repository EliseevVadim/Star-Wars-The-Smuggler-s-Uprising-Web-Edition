namespace SWGame.Core.Models.Items
{
    public class QuestItem : Item
    {
        private int _iconIndex;
        public QuestItem(int id, string name, string description, int iconIndex)
        {
            _id = id;
            _name = name;
            _descriprion = description;
            _iconIndex = iconIndex;
        }

        public int IconIndex { get => _iconIndex; set => _iconIndex = value; }
    }
}
