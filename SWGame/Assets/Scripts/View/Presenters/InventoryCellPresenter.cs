using SWGame.Entities;
using SWGame.Entities.Items.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.Presenters
{
    public class InventoryCellPresenter : MonoBehaviour
    {
        [SerializeField] private Text _nameField;
        [SerializeField] private Text _descriptionField;
        [SerializeField] private Text _priceField;
        [SerializeField] private Text _countField;
        [SerializeField] private Image _iconField;
        [SerializeField] private Text _cardValueField;

        public void Visualize(InventoryCell cell)
        {
            _nameField.text = cell.Content.Name;
            _descriptionField.text = cell.Content.Descriprion;
            _priceField.text = $"Цена продажи: {cell.Content.SalePrice}";
            _countField.text = cell.Count.ToString();
            _iconField.sprite = cell.Content.Image;
            if (cell.Content is Card)
            {
                Card card = cell.Content as Card;
                _cardValueField.text = card.ValueInLine;
            }
        }
    }
}
