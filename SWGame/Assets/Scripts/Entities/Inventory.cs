using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWGame.GlobalConfigurations;
using Newtonsoft.Json;
using SWGame.Entities.Items;

namespace SWGame.Entities
{
    public class Inventory
    {
        private List<InventoryCell> _cells;
        private int _id;
        private ClientManager _clientManager;

        public Inventory(int id, ClientManager clientManager)
        {
            _id = id;
            _clientManager = clientManager;
            _cells = new List<InventoryCell>();
            LoadAllItems();
        }

        public List<InventoryCell> Cells { get => _cells; set => _cells = value; }

        public int Id { get => _id; set => _id = value; }

        public async void LoadAllItems()
        {
            await _clientManager.LoadInventoryItems(_id);
        }

        public bool Contains(Item item)
        {
            foreach (InventoryCell cell in _cells)
            {
                if (cell.Content.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task AddItem(Item item)
        {
            if (Contains(item))
            {
                var cell = _cells.Where(cell => cell.Content.Equals(item)).First();
                cell.Count++;
                InventoryCellDataModel model = new InventoryCellDataModel(cell);
                await model.UpdateInDatabase(_clientManager);
            }
            else
            {
                InventoryCell cell = new InventoryCell(1, item, _id);
                _cells.Add(cell);
                InventoryCellDataModel model = new InventoryCellDataModel(cell);
                await model.AddToDatabase(_clientManager);
            }
        }

        public async Task RemoveItem(Item item)
        {
            var cell = _cells.Where(cell => cell.Content.Equals(item)).First();
            if (cell.Count > 1)
            {
                cell.Count--;
                InventoryCellDataModel model = new InventoryCellDataModel(cell);
                await model.UpdateInDatabase(_clientManager);
            }
            else
            {
                _cells.Remove(cell);
                InventoryCellDataModel model = new InventoryCellDataModel(cell);
                await model.RemoveFromDatabase(_clientManager);
            }
        }
    }
}
