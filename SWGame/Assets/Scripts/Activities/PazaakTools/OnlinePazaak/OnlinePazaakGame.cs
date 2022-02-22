using SWGame.Entities;
using SWGame.Entities.Items.Cards;
using SWGame.Enums;
using SWGame.GlobalConfigurations;
using SWGame.Management;
using SWGame.Management.Repositories;
using SWGame.View.Scenes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace SWGame.Activities.PazaakTools.OnlinePazaak
{
    public class OnlinePazaakGame : MonoBehaviour
    {
        [SerializeField] private List<Image> _playersHandView;
        [SerializeField] private List<Text> _playersHandCardsValues;
        [SerializeField] private Text _playersNameField;
        [SerializeField] private Text _opponentsNameField;
        [SerializeField] private Text _playersSumField;
        [SerializeField] private Text _opponentsSumField;
        [SerializeField] private Text _playersScoreField;
        [SerializeField] private Text _opponentsScoreField;
        [SerializeField] private Deck _playersDeck;
        [SerializeField] private Deck _opponentsDeck;
        [SerializeField] private GameObject _resultMessage;
        [SerializeField] private Text _resultText;
        [SerializeField] private Text _resultTitle;
        [SerializeField] private GameObject _endGameMessage;
        [SerializeField] private Text _endGameTitle;
        [SerializeField] private Text _endGameText;
        [SerializeField] private GameObject _pazaakView;
        [SerializeField] private GameObject _playersDeckSignalPanel;
        [SerializeField] private GameObject _opponentsDeckSignalPanel;
        [SerializeField] private Button _moveButton;
        [SerializeField] private Button _standButton;

        private int _amount;
        private string _opponentsName;
        private int _playersScore;
        private int _opponentsScore;
        private bool _playerStands;
        private bool _opponentStands;
        private bool _canThrowHandCard;
        private int _selectedCardIndex;
        private Player _currentPlayer = CurrentPlayer.Player;
        private Card[] _playersHand;
        private Card _selectedCard;
        private List<Card> _systemCards;
        private bool _movesFirst;

        private ClientManager _clientManager;
        [SerializeField] private MessagesDispatcher _messagesDispatcher;

        private void OnEnable()
        {
            _clientManager = FindObjectOfType<ClientManager>();
            _playersHand = new Card[4];
            ClearGameStatement();
            _systemCards = CardsRepository.SystemCards;
            _playersNameField.text = _currentPlayer.Nickname;
            _opponentsNameField.text = _opponentsName;
            GeneratePlayersHand();
            VisualizePlayersHand();
            if (_movesFirst && !_playerStands)
            {
                AddSystemCard(_playersDeck);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_selectedCard is FlippableCard)
                {
                    FlippableCard flippableCard = _selectedCard as FlippableCard;
                    flippableCard.Flip();
                    _playersHand[_selectedCardIndex] = flippableCard;
                    UpdateCardsView(flippableCard);
                }
                else if (_selectedCard is GoldCard)
                {
                    GoldCard goldCard = _selectedCard as GoldCard;
                    switch (goldCard.Type)
                    {
                        case GoldCardType.PlusMinusOneOrTwo:
                            goldCard.Index++;
                            try
                            {
                                goldCard.Value = goldCard.PossibleValues[goldCard.Index];
                            }
                            catch
                            {
                                goldCard.Index = 0;
                                goldCard.Value = goldCard.PossibleValues[goldCard.Index];
                            }
                            finally
                            {
                                goldCard.ValueInLine = goldCard.Value > 0 ? $"+{goldCard.Value}" : goldCard.Value.ToString();
                                _playersHand[_selectedCardIndex] = goldCard;
                                UpdateCardsView(goldCard);
                            }
                            break;
                        case GoldCardType.TCard:
                            goldCard.Index++;
                            try
                            {
                                goldCard.Value = goldCard.PossibleValues[goldCard.Index];
                            }
                            catch
                            {
                                goldCard.Index = 0;
                                goldCard.Value = goldCard.PossibleValues[goldCard.Index];
                            }
                            finally
                            {
                                goldCard.ValueInLine = goldCard.Value > 0 ? $"+{goldCard.Value}T" : $"{goldCard.Value.ToString()}T";
                                _playersHand[_selectedCardIndex] = goldCard;
                                UpdateCardsView(goldCard);
                            }
                            break;
                    }
                }
            }
        }

        private void ClearGameStatement()
        {
            _selectedCard = null;
            _playersDeck.Clear();
            _opponentsDeck.Clear();
            _playerStands = false;
            _opponentStands = false;
            _canThrowHandCard = true;
            _playersScore = 0;
            _opponentsScore = 0;
            _moveButton.interactable = _movesFirst;
            _standButton.interactable = _movesFirst;
            _playersDeckSignalPanel.SetActive(false);
            _opponentsDeckSignalPanel.SetActive(false);
        }

        private void GeneratePlayersHand()
        {
            List<Card> allPlayersCards = new List<Card>();
            foreach (InventoryCell cell in _currentPlayer.Inventory.Cells)
            {
                if (cell.Content is Card)
                {
                    for (int i = 0; i < cell.Count; i++)
                    {
                        allPlayersCards.Add(cell.Content as Card);
                    }
                }
            }
            for (int i = 0; i < _playersHand.Length; i++)
            {
                int pos = UnityEngine.Random.Range(0, allPlayersCards.Count);
                _playersHand[i] = allPlayersCards[pos];
                allPlayersCards.RemoveAt(pos);
            }
        }
        private void VisualizePlayersHand()
        {
            for (int i = 0; i < _playersHand.Length; i++)
            {
                _playersHandView[i].color = Color.white;
                _playersHandView[i].sprite = _playersHand[i].Image;
                _playersHandCardsValues[i].text = _playersHand[i].ValueInLine;
            }
        }

        private async void AddSystemCard(Deck deck)
        {
            try
            {
                _systemCards = CardsRepository.SystemCards;
                Random random = new Random();
                int pos = random.Next(0, _systemCards.Count);
                Card addition = _systemCards[pos];
                addition.AddServerCardToDeck(deck, _messagesDispatcher);
                _messagesDispatcher.AddMessage(new Action(() =>
                {
                    UpdateView();
                }));
                await _clientManager.SendCardAddition(addition);
                if (_playersDeck.Sum == 20 || _playersDeck.CurrentIndex == 9)
                {
                    SendStandStatement();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void UpdateView()
        {
            try
            {
                _opponentsScoreField.text = _opponentsScore.ToString();
                _playersScoreField.text = _playersScore.ToString();
                _playersSumField.text = _playersDeck.Sum.ToString();
                _opponentsSumField.text = _opponentsDeck.Sum.ToString();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void UpdateCardsView(Card card)
        {
            _playersHandView[_selectedCardIndex].sprite = card.Image;
            _playersHandCardsValues[_selectedCardIndex].text = card.ValueInLine;
        }

        public void SetInitialInfo(int amount, string opponentsName, bool movesFirst)
        {
            _amount = amount;
            _opponentsName = opponentsName;
            _movesFirst = movesFirst;
        }

        public async void AddPlayersCard(int index)
        {
            try
            {
                if (!_playerStands)
                {
                    if (_canThrowHandCard)
                    {
                        Card addition = _playersHand[index];
                        addition.AddToDeck(_playersDeck);
                        _playersHand[index] = null;
                        _playersHandView[index].color = new Color(0.3215686f, 0.3215686f, 0.3215686f);
                        _playersHandView[index].sprite = null;
                        _playersHandCardsValues[index].text = string.Empty;
                        UpdateView();
                        _canThrowHandCard = false;
                        await _clientManager.SendCardAddition(addition);
                        CheckForStandingRequirement();
                    }
                }
            }
            catch { }
        }

        private bool CheckForStandingRequirement()
        {
            bool needToStand = _playersDeck.Sum >= 20 || _playersDeck.CurrentIndex == 9;
            if (needToStand)
            {
                SendStandStatement();
            }
            return needToStand;
        }

        private void CheckForSetFinish()
        {
            if (_playerStands && _opponentStands)
            {
                EvaluateResult();
            }
        }

        public void EvaluateResult()
        {
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                if (_opponentsDeck.CurrentIndex == 9 || _opponentsDeck.HasATiebreaker)
                {
                    _resultText.text = "К сожалению, Вы проиграли.";
                    _opponentsScore++;
                }
                else if ((_playersDeck.HasATiebreaker && !_opponentsDeck.HasATiebreaker && _playersDeck.Sum <= 20) ||
                    (_playersDeck.Sum == 20 && _opponentsDeck.Sum != 20 && _opponentsDeck.CurrentIndex != 9) ||
                    (_opponentsDeck.Sum > 20 && _playersDeck.Sum <= 20) ||
                    (_playersDeck.Sum > _opponentsDeck.Sum && _playersDeck.Sum < 20) ||
                    (_playersDeck.CurrentIndex == 9 && _opponentsDeck.CurrentIndex != 9 && _playersDeck.Sum <= 20))
                {
                    _resultText.text = "Поздравляем, Вы выиграли сет!";
                    _playersScore++;
                }
                else if ((_playersDeck.Sum == _opponentsDeck.Sum) ||
                    (_playersDeck.Sum > 20 && _opponentsDeck.Sum > 20) ||
                    (_playersDeck.CurrentIndex == _opponentsDeck.CurrentIndex && _playersDeck.CurrentIndex == 9))
                {
                    _resultText.text = "Сет завершен вничью!";
                }
                else
                {
                    _resultText.text = "К сожалению, Вы проиграли.";
                    _opponentsScore++;
                }
                _resultTitle.text = "Сет завершен";
                _resultMessage.SetActive(true);
                ProcessToGame();
            }));
        }

        private void ProcessToGame()
        {
            if (_playersScore < 3 && _opponentsScore < 3)
            {
                _playersScoreField.text = _playersScore.ToString();
                _opponentsScoreField.text = _opponentsScore.ToString();
                _playersDeck.Clear();
                _opponentsDeck.Clear();
                _playerStands = false;
                _opponentStands = false;
                _canThrowHandCard = true;
                _moveButton.interactable = _movesFirst;
                _standButton.interactable = _movesFirst;
                _opponentsDeckSignalPanel.SetActive(false);
                _playersDeckSignalPanel.SetActive(false);
                UpdateView();
                if (_movesFirst)
                {
                    AddSystemCard(_playersDeck);
                }
            }
        }

        public void AcceptTheResult()
        {
            _messagesDispatcher.AddMessage(new Action(async () =>
            {
                _resultMessage.SetActive(false);
                if (_playersScore == 3 || _opponentsScore == 3)
                {
                    if (_playersScore == 3)
                    {
                        _currentPlayer.Credits += _amount;
                        _endGameTitle.text = "Успех!";
                        _endGameText.text = $"Поздравляем, Вы победили в матче! Ваш выигрыш составляет {_amount}.";
                    }
                    else
                    {
                        _currentPlayer.Credits -= _amount;
                        _endGameTitle.text = "Неудача!";
                        _endGameText.text = $"К сожалению, Вы проиграли матч. Потеря в кредитах составляет: {_amount}";
                    }
                    _endGameMessage.SetActive(true);
                    if (_movesFirst)
                    {
                        await _clientManager.SendGameFinishing();
                    }
                }
            }));
        }

        public void SelectCard(int pos)
        {
            _selectedCardIndex = pos;
            _selectedCard = _playersHand[_selectedCardIndex];
        }

        public async void StandByPlayer()
        {
            _playerStands = true;
            _playersDeckSignalPanel.SetActive(true);
            await _clientManager.SendStandStatement();
        }

        public void ReceiveStandStatement()
        {
            _opponentStands = true;
            _opponentsDeckSignalPanel.SetActive(true);
        }

        public void ReceiveOpponentsCard(Card addition)
        {
            addition.AddServerCardToDeck(_opponentsDeck, _messagesDispatcher);
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                UpdateView();
            }));
        }

        public void ProcessMoveFinishing()
        {
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                if (!_playerStands)
                {
                    _moveButton.interactable = true;
                    _standButton.interactable = true;
                    AddSystemCard(_playersDeck);
                }
            }));
            CheckForSetFinish();
        }

        public void ProcessStandStatement()
        {
            _opponentStands = true;
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                _opponentsDeckSignalPanel.SetActive(true);
            }));
        }

        public async void SendMoveFinishing()
        {
            if (CheckForStandingRequirement())
                return;
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                if (!_opponentStands)
                {
                    _moveButton.interactable = false;
                    _standButton.interactable = false;
                }
                _canThrowHandCard = true;
            }));
            await _clientManager.SendMoveFinishing();
            if (_opponentStands)
            {
                AddSystemCard(_playersDeck);
            }
        }

        public async void SendStandStatement()
        {
            _playersDeckSignalPanel.SetActive(true);
            _moveButton.interactable = false;
            _standButton.interactable = false;
            _playerStands = true;
            await _clientManager.SendStandStatement();
            await _clientManager.SendMoveFinishing();
            CheckForSetFinish();
        }

        public void FinishGame()
        {
            ClearGameStatement();
            _endGameMessage.SetActive(false);
            _pazaakView.SetActive(false);
        }

        public void ProcessOpponentDisconnection()
        {
            _messagesDispatcher.AddMessage(new Action(() =>
            {
                _currentPlayer.Credits += _amount;
                _endGameTitle.text = "Успех!";
                _endGameText.text = $"Поздравляем, Вы победили в матче, поскольку соперник отключился! Ваш выигрыш составляет {_amount}.";
                _endGameMessage.SetActive(true);
            }));
        }
    }
}