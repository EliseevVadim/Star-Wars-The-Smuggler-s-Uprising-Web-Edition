using Newtonsoft.Json;
using SWGame.Entities;
using SWGame.GlobalConfigurations;
using SWGame.Management;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SWGame.View.Scenes
{
    public class AuthorizationScene : MonoBehaviour
    {
        [SerializeField] private InputField _nicknameField;
        [SerializeField] private InputField _loginField;
        [SerializeField] private InputField _passwordField;
        [SerializeField] private Toggle _passwordToggle;
        [SerializeField] private GameObject _successMessage;
        [SerializeField] private GameObject _errorMessage;
        [SerializeField] private Text _errorText;
        [SerializeField] private GameObject _loadingScreen;
        private ClientManager _clientManager;
        private GameObject _connectionErrorMessage;

        private MessagesDispatcher _messagesDispatcher;

        private void Start()
        {
            _messagesDispatcher = GetComponent<MessagesDispatcher>();
            _clientManager = FindObjectOfType<ClientManager>().GetComponent<ClientManager>();
            _clientManager.AuthorizationScene = this;
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
        public void ChangePasswordFieldStyle()
        {
            if (_passwordToggle.isOn)
            {
                _passwordField.contentType = InputField.ContentType.Standard;
            }
            else
            {
                _passwordField.contentType = InputField.ContentType.Password;
            }
            _passwordField.ForceLabelUpdate();
        }

        public async void CheckUserExistance()
        {
            string name = _nicknameField.text;
            string login = _loginField.text;
            string password = _passwordField.text;
            TextEncryptor encryptor = new TextEncryptor(password);
            await _clientManager.CheckUserExistanceAsync(name, login, encryptor.GetSha1EncryptedLine());
        }

        public void ProcessAuthorizationResult(string response)
        {
            if (string.IsNullOrEmpty(response))
            {
                try
                {
                    _messagesDispatcher.AddMessage(new Action(() =>
                    {
                        _errorText.text = "Ошибка входа! Проверьте введенные данные.";
                        _errorMessage.SetActive(true);
                    }));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            else
            {
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    CurrentPlayer.Player = JsonConvert.DeserializeObject<Player>(response);
                    _successMessage.SetActive(true);
                }));
            }
        }

        public void ProcessToGame()
        {
            _successMessage.SetActive(false);
            _loadingScreen.SetActive(true);
        }
    }
}
