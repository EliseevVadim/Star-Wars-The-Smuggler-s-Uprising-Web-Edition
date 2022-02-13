using SWGame.Entities.Items;
using SWGame.Enums;
using SWGame.Management.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SWGame.Activities
{
    public class ExchangePoint : MonoBehaviour
    {
        [SerializeField] private ExchangePointType _type;
        private List<LootItem> _items;

        public List<LootItem> Items { get => _items; set => _items = value; }

        private void Awake()
        {
            _items = new List<LootItem>();
            switch (_type)
            {
                case ExchangePointType.JediEnclave:
                    _items = ItemsRepository.JediItems;
                    break;
                case ExchangePointType.SithAcademy:
                    _items = ItemsRepository.SithItems;
                    break;
            }
        }
    }
}
