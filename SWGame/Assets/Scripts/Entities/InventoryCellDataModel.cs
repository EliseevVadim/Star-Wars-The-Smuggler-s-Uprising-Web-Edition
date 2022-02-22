using SWGame.GlobalConfigurations;
using System.Threading.Tasks;

namespace SWGame.Entities
{
    public class InventoryCellDataModel
    {
        private int _id;
        private int _count;
        private int _itemId;

        public int Id { get => _id; set => _id = value; }
        public int Count { get => _count; set => _count = value; }
        public int ItemId { get => _itemId; set => _itemId = value; }

        public InventoryCellDataModel(InventoryCell cell)
        {
            _id = cell.InventoryId;
            _count = cell.Count;
            _itemId = cell.Content.Id;
        }

        public async Task AddToDatabase(ClientManager manager)
        {
            await manager.CreateInventoryCell(this);
        }

        public async Task UpdateInDatabase(ClientManager manager)
        {
            await manager.UpdateInventoryCell(this);
        }

        public async Task RemoveFromDatabase(ClientManager manager)
        {
            await manager.RemoveInventoryCell(this);
        }
    }
}
