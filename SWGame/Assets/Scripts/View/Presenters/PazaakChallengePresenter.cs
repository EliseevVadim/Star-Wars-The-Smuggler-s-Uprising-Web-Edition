using SWGame.Activities.PazaakTools;
using SWGame.GlobalConfigurations;
using SWGame.Management;
using System.Collections;
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
    }
}