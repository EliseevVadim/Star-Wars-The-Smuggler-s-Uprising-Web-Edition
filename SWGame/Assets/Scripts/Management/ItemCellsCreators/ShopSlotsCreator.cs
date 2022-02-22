using Newtonsoft.Json;
using SWGame.Entities;
using SWGame.Entities.Items;
using SWGame.Entities.Items.Cards;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWGame.Management.ItemCellsCreators
{
    public class ShopSlotsCreator : IItemCellsCreator<ShopSlot>
    {
        public List<ShopSlot> ProcessCellsData(List<Dictionary<string, object>> cellsData)
        {
            List<ShopSlot> slots = new List<ShopSlot>();
            cellsData.ForEach(cellDataUnit =>
            {
                Item addition = null;
                string itemString = cellDataUnit["Stuff"].ToString();
                int price = int.Parse(cellDataUnit["Price"].ToString());
                int shopId = int.Parse(cellDataUnit["ShopId"].ToString());
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
                    slots.Add(new ShopSlot(shopId, price, addition));
                }
                catch (NullReferenceException)
                {
                    Debug.Log("пусто");
                }
            });
            return slots;
        }
    }
}
