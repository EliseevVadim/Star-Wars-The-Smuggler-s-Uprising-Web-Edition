using SWGame.Entities;
using SWGame.View.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.Presenters
{
    public class NeighbourPlayerPresenter : MonoBehaviour
    {
        [SerializeField] private Text _nameField;
        [SerializeField] private Image _onlineIndicator;

        public void Visualize(Player player)
        {
            _nameField.text = player.Nickname;
            _nameField.transform.GetChild(0).name = player.Id.ToString();
            _onlineIndicator.color = player.IsOnline ? Color.green : Color.grey;
        }

        public void ShowPlayer()
        {
            MainScene scene = FindObjectOfType<MainScene>();
            int id = int.Parse(_nameField.transform.GetChild(0).name);
            scene.GetPlayersInfo(id);
        }
    }
}
