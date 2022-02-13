using SWGame.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SWGame.View.Presenters
{
    public class ExchangePointVisualizator : MonoBehaviour
    {
        [SerializeField] private ExchangeSlotPresenter _exchangeSlotTemplate;
        [SerializeField] private Transform _container;
        [SerializeField] private ExchangePoint _exchangePoint;

        public void Start()
        {
            Render(_exchangePoint);
        }
        public void Render(ExchangePoint exchangePoint)
        {
            foreach (Transform slot in _container)
            {
                Destroy(slot.gameObject);
            }
            exchangePoint.Items.ForEach(item =>
            {
                var slotView = Instantiate(_exchangeSlotTemplate, _container);
                slotView.Visualize(item);
            });
        }
    }
}
