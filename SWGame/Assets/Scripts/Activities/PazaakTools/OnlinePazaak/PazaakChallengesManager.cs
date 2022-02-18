using System;
using System.Collections;
using SWGame.GlobalConfigurations;
using UnityEngine;
using UnityEngine.UI;
using SWGame.Management;
using SWGame.Entities;

namespace SWGame.Activities.PazaakTools.OnlinePazaak
{
    public class PazaakChallengesManager : MonoBehaviour
    {
        [SerializeField] private InputField _amountField;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _errorText;
        [SerializeField] private GameObject _creatingChallengeForm;

        private ClientManager _manager;
        private Player _currentPlayer = CurrentPlayer.Player;

        private void Awake()
        {
            _manager = FindObjectOfType<ClientManager>();
        }
        private async void OnEnable()
        {
            await _manager.AddPlayerToChallengesViewers();
        }

        public async void CreateChallenge()
        {
            if (String.IsNullOrWhiteSpace(_amountField.text))
            {
                PrepareErrorPanel("Ставка не должна быть пустой.");
                return;
            }
            int amount = int.MinValue;
            bool canParse = int.TryParse(_amountField.text, out amount);
            if (!canParse)
            {
                PrepareErrorPanel("Ошибка ввода");
                return;
            }
            if (amount < 0)
            {
                PrepareErrorPanel("Ставка не должна быть отрицательной.");
                return;
            }
            if (amount > _currentPlayer.Credits)
            {
                PrepareErrorPanel("Не хватает кредитов.");
                return;
            }
            if (!_currentPlayer.CanPlayPazaak())
            {
                PrepareErrorPanel("Не хватает карт для игры в Пазаак.");
                return;
            }
            _amountField.text = String.Empty;
            await _manager.CreatePazaakChallenge(_currentPlayer.Nickname, amount);
            _creatingChallengeForm.SetActive(false);
        }

        private void PrepareErrorPanel(string message)
        {
            _amountField.text = String.Empty;
            _errorText.text = message;
            _errorMessage.SetActive(true);
        }
    }
}