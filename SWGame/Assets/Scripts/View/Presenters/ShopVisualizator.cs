using SWGame.Entities;
using SWGame.Management;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.Presenters
{
    public class ShopVisualizator : MonoBehaviour
    {
        [SerializeField] private ShopSlotPresenter _shopSlotPresenterTemplate;
        [SerializeField] private Transform _parent;
        [SerializeField] private Text _title;

        private Player _currentPlayer = CurrentPlayer.Player;
        private Shop _shop => _currentPlayer.Location.Shop;

        public void OnEnable()
        {
            Render(_shop);
        }

        public void Render(Shop shop)
        {
            try
            {
                foreach (Transform cell in _parent)
                {
                    Destroy(cell.gameObject);
                }
                _title.text = shop.Name;
                shop.Slots.ForEach(slot =>
                {
                    var slotView = Instantiate(_shopSlotPresenterTemplate, _parent);
                    slotView.Visualize(slot);
                });
            }
            catch
            {

            }
        }
    }
}
