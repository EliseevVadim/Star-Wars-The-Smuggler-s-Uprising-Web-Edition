using SWGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SWGame.View.Presenters
{
    public class NeighbourPlayersVisualizator : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private NeighbourPlayerPresenter _template;

        public void Render(List<Player> neighbours)
        {
            try
            {
                foreach (Transform player in _container)
                {
                    Destroy(player.gameObject);
                }
                neighbours.ForEach(player =>
                {
                    var playerView = Instantiate(_template, _container);
                    playerView.Visualize(player);
                });
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
