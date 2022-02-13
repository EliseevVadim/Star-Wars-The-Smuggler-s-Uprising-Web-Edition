using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWGame.Entities;
using SWGame.Entities.Items;
using SWGame.Entities.Items.Cards;
using UnityEngine;
using Newtonsoft.Json;

namespace SWGame.Management.ItemCellsCreators
{
    public class InventoryCellsCreator : IItemCellsCreator<InventoryCell>
    {
        public List<InventoryCell> ProcessCellsData(List<Dictionary<string, object>> cellsData)
        {
            List<InventoryCell> cells = new List<InventoryCell>();
            cellsData.ForEach(cellDataUnit =>
            {
                Item addition = null;
                string itemString = cellDataUnit["Content"].ToString();
                int count = int.Parse(cellDataUnit["Count"].ToString());
                int inventoryId = int.Parse(cellDataUnit["InventoryId"].ToString());
                Dictionary<string, object> itemInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(itemString);
                switch (itemInfo["TypeName"])
                {
                    case "LootItem":
                        addition = JsonConvert.DeserializeObject<LootItem>(itemString);
                        break;
                    case "QuestItem":
                        addition = JsonConvert.DeserializeObject<QuestItem>(itemString);
                        break;
                    case "GoldCard":
                        addition = JsonConvert.DeserializeObject<GoldCard>(itemString);
                        break;
                    case "FlippableCard":
                        addition = JsonConvert.DeserializeObject<FlippableCard>(itemString);
                        break;
                    case "ClassicalCard":
                        addition = JsonConvert.DeserializeObject<ClassicalCard>(itemString);
                        break;
                }
                try
                {
                    cells.Add(new InventoryCell(count, addition, inventoryId));
                }
                catch(NullReferenceException)
                {
                    Debug.Log("пусто");
                }
            });
            return cells;
        }
    }
}
