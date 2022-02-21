using SWGame.Entities;
using SWGame.Entities.Items;
using SWGame.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.Presenters
{
    public class ExchangeSlotPresenter : MonoBehaviour
    {
        [SerializeField] private Text _nameField;
        [SerializeField] private Image _iconField;
        [SerializeField] private Text _descriptionField;
        [SerializeField] private Text _buttonText;
        [SerializeField] private GameObject _successMessage;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _successText;

        private LootItem _item;
        private Player _currentPlayer = CurrentPlayer.Player;

        public void Visualize(Item item)
        {
            LootItem lootItem = item as LootItem;
            _item = lootItem;
            _nameField.text = lootItem.Name;
            _iconField.sprite = lootItem.Image;
            _descriptionField.text = lootItem.Descriprion;
            _buttonText.text =
                lootItem.PrestigeValue > 0 ? $"Сдать ({lootItem.PrestigeValue} очков престижа)"
                : $"Сдать ({lootItem.WisdomValue} очков мудрости)";
        }
        public async void Deliver()
        {
            if (_currentPlayer.HasA(_item))
            {
                await _currentPlayer.Inventory.RemoveItem(_item);
                if (_item.PrestigeValue > 0)
                {
                    _currentPlayer.Prestige += _item.PrestigeValue;
                    _successText.text = $"Получено {_item.PrestigeValue} очков престижа.";
                }
                else
                {
                    _currentPlayer.WisdomPoints += _item.WisdomValue;
                    _successText.text = $"Получено {_item.WisdomValue} очков мудрости.";
                }
                _successMessage.SetActive(true);
            }
            else
            {
                _errorMessage.SetActive(true);
            }
        }
    }
}
