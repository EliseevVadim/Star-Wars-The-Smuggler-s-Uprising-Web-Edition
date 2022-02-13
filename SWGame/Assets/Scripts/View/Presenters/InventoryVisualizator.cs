using SWGame.Entities;
using SWGame.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SWGame.View.Presenters
{
    public class InventoryVisualizator : MonoBehaviour
    {
        [SerializeField] private InventoryCellPresenter _inventoryCellTemplate;
        [SerializeField] private Transform _container;
        private Player _currentPlayer = CurrentPlayer.Player;
        private Inventory _inventory => _currentPlayer.Inventory;

        public void OnEnable()
        {
            Render(_inventory);
        }
        public void Render(Inventory inventory)
        {
            foreach (Transform cell in _container)
            {
                Destroy(cell.gameObject);
            }
            inventory.Cells.ForEach(cell =>
            {
                var cellView = Instantiate(_inventoryCellTemplate, _container);
                cellView.Visualize(cell);
            });
        }
    }
}
