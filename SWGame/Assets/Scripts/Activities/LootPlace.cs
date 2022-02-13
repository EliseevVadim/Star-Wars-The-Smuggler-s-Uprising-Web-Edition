using SWGame.Entities.Items;
using SWGame.Enums;
using SWGame.Exceptions;
using SWGame.Management.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SWGame.Activities
{
    public class LootPlace
    {
        private List<LootItem> _items;
        private LootPlaceType _type;
        private string _explanation;

        public LootPlace(LootPlaceType type)
        {
            _items = new List<LootItem>();
            _type = type;
            switch (_type)
            {
                case LootPlaceType.SithTomb:
                    _items = ItemsRepository.SithItems;
                    _explanation = "Дух темного Лорда напал на Вас, вы проиграли. Вы потеряли голокрон.";
                    break;
                case LootPlaceType.JediRuins:
                    _items = ItemsRepository.JediItems;
                    _explanation = "Древний дроид-страж напал на вас, вы проиграли. Вы потеряли медальон.";
                    break;
            }
        }
        public Item Loot()
        {
            int pos = Random.Range(0, _items.Count * 2 + 1);
            try
            {
                return _items[pos];
            }
            catch
            {
                throw new FailedLootException(_explanation);
            }
        }
    }
}
