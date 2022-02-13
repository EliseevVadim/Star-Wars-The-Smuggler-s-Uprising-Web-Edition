using SWGame.Entities;
using SWGame.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.Activities.PazaakTools
{
    public class PazaakAmountGetter : MonoBehaviour
    {
        [SerializeField] private InputField _amountField;
        [SerializeField] private GameObject _amountMessage;
        [SerializeField] private GameObject _pazzaakGameView;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _errorText;

        private Player _currentPlayer = CurrentPlayer.Player;
        private int _amount;

        private void OnEnable()
        {
            _amountField.text = string.Empty;
        }

        public void StartPazaakGame()
        {
            try
            {
                _amount = int.Parse(_amountField.text);
                if (_amount <= 0)
                {
                    _errorText.text = "Ставка не может быть отрициательной, либо равной нулю.";
                    _errorMessage.SetActive(true);
                }
                else if (_amount > _currentPlayer.Credits)
                {
                    _errorText.text = "Недостаточно кредитов.";
                    _errorMessage.SetActive(true);
                }
                else
                {
                    _pazzaakGameView.GetComponent<PazaakGame>().SetAmount(_amount);
                    _pazzaakGameView.SetActive(true);
                }
                _amountMessage.SetActive(false);
            }
            catch
            {
                _amountMessage.SetActive(false);
                _errorText.text = "Проверьте ввод.";
                _errorMessage.SetActive(true);
            }
        }
    }
}
