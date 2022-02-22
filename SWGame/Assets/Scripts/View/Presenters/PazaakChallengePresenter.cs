using SWGame.Activities.PazaakTools.OnlinePazaak;
using SWGame.GlobalConfigurations;
using SWGame.Management;
using UnityEngine;
using UnityEngine.UI;

namespace SWGame.View.Presenters
{
    public class PazaakChallengePresenter : MonoBehaviour
    {
        [SerializeField] private Text _creatorName;
        [SerializeField] private Text _amountField;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _errorText;

        private PazaakChallenge _challenge;

        public void Visualize(PazaakChallenge challenge)
        {
            _challenge = challenge;
            _creatorName.text = challenge.Creator;
            _amountField.text = string.Format("{0:#,###0.#}", challenge.Amount);
            _cancelButton.gameObject.SetActive(challenge.Creator == CurrentPlayer.Player.Nickname);
            _acceptButton.gameObject.SetActive(!_cancelButton.gameObject.activeSelf);
        }

        public async void CancelChallenge()
        {
            await FindObjectOfType<ClientManager>().RemoveChallenge();
        }

        public async void AcceptChallenge()
        {
            if (!CurrentPlayer.Player.CanPlayPazaak())
            {
                PrepareErrorPanel("Не хватает карт для игры в Пазаак.");
                return;
            }
            if (CurrentPlayer.Player.Credits < _challenge.Amount)
            {
                PrepareErrorPanel("Не хватает кредитов.");
                return;
            }
            await FindObjectOfType<ClientManager>().AcceptChallenge(CurrentPlayer.Player.Nickname, _challenge.Creator);
        }

        private void PrepareErrorPanel(string message)
        {
            _errorText.text = message;
            _errorMessage.SetActive(true);
        }
    }
}