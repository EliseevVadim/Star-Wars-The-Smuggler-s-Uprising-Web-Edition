using SWGame.Entities;
using SWGame.Management.Repositories;
using SWGame.Management;
using UnityEngine;
using UnityEngine.UI;
using SWGame.View.Scenes;

namespace SWGame.View.ActionsHandlers.PlanetEvents
{
    public class NarShaddaaEvents : MonoBehaviour
    {
        [SerializeField] private GameObject _requestMessage;
        [SerializeField] private GameObject _successMessage;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _errorText;
        [SerializeField] private InputField _amountField;
        [SerializeField] private GameObject _errorCasinoMessage;
        [SerializeField] private Text _errorCasinoText;
        [SerializeField] private GameObject _resultCasinoMessage;
        [SerializeField] private Text _resultCasinoText;

        private Planet _currentPlanet;
        private Player _currentPlayer;

        private PlanetsRepository _planetsRepository;

        private const int RequiredCredits = 1000000;

        private void Start()
        {
            _planetsRepository = GetComponentInParent<MainScene>().PlanetsRepository;
            _currentPlanet = _planetsRepository.Planets[4];
            _currentPlayer = CurrentPlayer.Player;
        }
        public void GoToLowLevels()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[1].Id;
            _currentPlanet.Locations[0].View.SetActive(false);
            _currentPlanet.Locations[1].View.SetActive(true);
        }
        public void ReturnToUpperLevels()
        {
            _currentPlayer.LocationId = _currentPlanet.Locations[0].Id;
            _currentPlanet.Locations[1].View.SetActive(false);
            _currentPlanet.Locations[0].View.SetActive(true);
        }
        public void CancelBounty()
        {
            _requestMessage.SetActive(false);
            if (_currentPlayer.StoryFinished)
            {
                _errorText.text = "����� ���� ��� ��������.";
                _errorMessage.SetActive(true);
            }
            else if (_currentPlayer.Credits < RequiredCredits)
            {
                _errorText.text = "������������ ��������!";
                _errorMessage.SetActive(true);
            }
            else
            {
                _currentPlayer.Credits -= RequiredCredits;
                _currentPlayer.StoryFinished = true;
                _successMessage.SetActive(true);
            }
        }
        public void RunTheCasino()
        {
            try
            {
                int amount = int.Parse(_amountField.text);
                if (amount <= 0)
                {
                    _errorCasinoText.text = "������ �� ����� ���� �������������, ���� ������ ����.";
                    _errorCasinoMessage.SetActive(true);
                }
                else if (amount > _currentPlayer.Credits)
                {
                    _errorCasinoText.text = "������������ ��������.";
                    _errorCasinoMessage.SetActive(true);
                }
                else
                {
                    _currentPlayer.Credits -= amount;
                    int result = Random.Range(1, 101);
                    if (result <= 25)
                    {
                        _resultCasinoText.text = $"�� ���������! ���� ������ ��������� {amount}";
                    }
                    else if (result > 75)
                    {
                        _resultCasinoText.text = $"�� ��������! ��� ������� �������� {amount * 2}";
                        _currentPlayer.Credits += amount * 2;
                    }
                    else
                    {
                        _resultCasinoText.text = $"�����, �� �� ���������! ������ ����������!";
                        _currentPlayer.Credits += amount;
                    }
                    _resultCasinoMessage.SetActive(true);
                }
            }
            catch
            {
                _errorCasinoText.text = "��������� ����!";
                _errorCasinoMessage.SetActive(true);
            }
            finally
            {
                _amountField.text = string.Empty;
            }
        }
    }
}
