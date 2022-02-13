using SWGame.Core.Models.Items;

namespace SWGame.Core.Models
{
    public class InventoryCell
    {
        private int _count;
        private Item _content;
        private int _inventoryId;

        public InventoryCell(int count, Item content, int inventoryId)
        {
            _count = count;
            _content = content;
            _inventoryId = inventoryId;
        }

        public int Count { get => _count; set => _count = value; }
        public int InventoryId { get => _inventoryId; set => _inventoryId = value; }
        public Item Content { get => _content; set => _content = value; }
    }
}
