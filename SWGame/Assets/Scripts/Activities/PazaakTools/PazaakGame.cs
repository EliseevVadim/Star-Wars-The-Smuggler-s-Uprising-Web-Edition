using SWGame.Entities;
using SWGame.Entities.Items.Cards;
using SWGame.Enums;
using SWGame.Management;
using SWGame.Management.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace SWGame.Activities.PazaakTools
{
    public class PazaakGame : MonoBehaviour
    {
        [SerializeField] private List<Image> _playersHandView;
        [SerializeField] private List<Text> _playersHandCardsValues;
        [SerializeField] private Text _playersNameField;
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

        private int _amount;
        private int _playersScore;
        private int _opponentsScore;
        private bool _playerStands;
        private bool _computerStands;
        private bool _canThrowHandCard;
        private int _selectedCardIndex;
        private Player _currentPlayer = CurrentPlayer.Player;
        private Card[] _playersHand;
        private Card[] _computersHand;
        private Card _selectedCard;
        private List<Card> _systemCards;

        private const int PivotalValue = 18;
        private const int MiddleValue = 6;

        private void OnEnable()
        {
            _playersHand = new Card[4];
            _computersHand = new Card[4];
            ClearGameStatement();
            _systemCards = CardsRepository.SystemCards;
            _playersNameField.text = _currentPlayer.Nickname;
            GeneratePlayersHand();
            GenerateComputersHand();
            VisualizePlayersHand();
            if (Random.Range(0, 2) == 1)
            {
                AddSystemCard(_playersDeck);
            }
            else
            {
                AddSystemCard(_opponentsDeck);
                CalculateComputersDecision();
                FinishComputersMove();
            }
        }

        private void ClearGameStatement()
        {
            _selectedCard = null;
            _playersDeck.Clear();
            _opponentsDeck.Clear();
            _playerStands = false;
            _computerStands = false;
            _canThrowHandCard = true;
            _playersScore = 0;
            _opponentsScore = 0;
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
                int pos = Random.Range(0, allPlayersCards.Count);
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
        private void GenerateComputersHand()
        {
            PazaakOpponent opponent;
            if (_currentPlayer.Location.Name == "Кантина Нал-Хатты")
            {
                opponent = new PazaakOpponent(PazaakOpponentsLevel.Easy);
            }
            else
            {
                opponent = new PazaakOpponent(PazaakOpponentsLevel.Medium);
            }
            for (int i = 0; i < _computersHand.Length; i++)
            {
                int pos = Random.Range(0, opponent.AllCards.Count);
                _computersHand[i] = opponent.AllCards[pos];
                opponent.AllCards.RemoveAt(pos);
            }
        }
        public void SelectCard(int pos)
        {
            _selectedCardIndex = pos;
            _selectedCard = _playersHand[_selectedCardIndex];
        }
        public void FinishPlayersMove()
        {
            if (_playersDeck.Sum >= 20 || _playersDeck.CurrentIndex == 9)
            {
                _playerStands = true;
            }
            if (!_computerStands)
            {
                AddSystemCard(_opponentsDeck);
                CalculateComputersDecision();
                FinishComputersMove();
            }
            else if (!_playerStands)
            {
                AddSystemCard(_playersDeck);
            }
            else
            {
                CheckForSetFinish();
                _canThrowHandCard = true;
            }
            _canThrowHandCard = true;
        }
        public void StandByPlayer()
        {
            _playerStands = true;
            if (_playerStands && _computerStands)
            {
                EvaluateResult();
            }
            else
            {
                while (!_computerStands)
                {
                    AddSystemCard(_opponentsDeck);
                    CalculateComputersDecision();
                    FinishComputersMove();
                }
            }
        }
        private void FinishComputersMove()
        {
            if (_opponentsDeck.Sum >= 20 || _opponentsDeck.CurrentIndex == 9)
            {
                _computerStands = true;
            }
            if (!_playerStands)
            {
                AddSystemCard(_playersDeck);
            }
            else if (!_computerStands)
            {
                AddSystemCard(_opponentsDeck);
                CalculateComputersDecision();
                FinishComputersMove();
            }
            else
            {
                CheckForSetFinish();
            }
        }
        private void CalculateComputersDecision()
        {
            if ((_playerStands && _opponentsDeck.Sum > _playersDeck.Sum && _opponentsDeck.Sum <= 20)
                || (_playerStands && _playersDeck.Sum > 20))
            {
                _computerStands = true;
                return;
            }
            Dictionary<int, int> possibleStatementsWithRespectiveCards = new Dictionary<int, int>();
            for (int i = 0; i < _computersHand.Length; i++)
            {
                if (_computersHand[i] is ClassicalCard)
                {
                    try
                    {
                        possibleStatementsWithRespectiveCards.Add(_opponentsDeck.Sum + _computersHand[i].Value, i);
                    }
                    catch { }
                }
                else if (_computersHand[i] is FlippableCard)
                {
                    try
                    {
                        possibleStatementsWithRespectiveCards.Add(_opponentsDeck.Sum + _computersHand[i].Value, i);
                    }
                    catch { }
                    try
                    {
                        possibleStatementsWithRespectiveCards.Add(_opponentsDeck.Sum - _computersHand[i].Value, i);
                    }
                    catch { }
                }
            }
            possibleStatementsWithRespectiveCards = possibleStatementsWithRespectiveCards
                .Where(record => record.Key <= 20)
                .ToDictionary(record => record.Key, record => record.Value);
            int[] keys = possibleStatementsWithRespectiveCards.Keys.ToArray();
            try
            {
                int bestStatement = ClosestKeyToTwenty(keys);
                if (bestStatement == 20 ||
                    (_playerStands && bestStatement > _playersDeck.Sum) ||
                    _opponentsDeck.Sum > 20 ||
                    _opponentsDeck.CurrentIndex == 8)
                {
                    if (_computersHand[possibleStatementsWithRespectiveCards[bestStatement]] is ClassicalCard)
                    {
                        _computersHand[possibleStatementsWithRespectiveCards[bestStatement]].AddToDeck(_opponentsDeck);
                    }
                    else
                    {
                        if (_computersHand[possibleStatementsWithRespectiveCards[bestStatement]].Value > 0
                            && bestStatement - _opponentsDeck.Sum < 0)
                        {
                            FlippableCard flippableCard = _computersHand[possibleStatementsWithRespectiveCards[bestStatement]] as FlippableCard;
                            flippableCard.Flip();
                            flippableCard.AddToDeck(_opponentsDeck);
                        }
                        else
                        {
                            _computersHand[possibleStatementsWithRespectiveCards[bestStatement]].AddToDeck(_opponentsDeck);
                        }
                    }
                    _computersHand[possibleStatementsWithRespectiveCards[bestStatement]] = null;
                    UpdateView();
                }
            }
            catch (IndexOutOfRangeException)
            {
                if (_opponentsDeck.Sum >= 20 || _opponentsDeck.CurrentIndex == 9)
                {
                    _computerStands = true;
                }
            }
            finally
            {
                possibleStatementsWithRespectiveCards.Clear();
            }
            int moreInfluentDecrement = FindMostInfluentDecrementInHand(); // looking for biggest negative card value in the hand (biggest by absolute value)            
            if (_opponentsDeck.Sum + MiddleValue + moreInfluentDecrement > 20 ||
                _opponentsDeck.Sum > _playersDeck.Sum && _playerStands && _opponentsDeck.Sum <= 20)
            {
                _computerStands = true;
            }
        }
        private int ClosestKeyToTwenty(int[] keys)
        {
            int difference = 20;
            int closest = keys[0];
            foreach (int element in keys)
            {
                if (difference > 20 - element)
                {
                    difference = 20 - element;
                    closest = element;
                }
            }
            return closest;
        }
        private int FindMostInfluentDecrementInHand()
        {
            int moreInfluentDecrement = 0;
            foreach (Card card in _computersHand)
            {
                if (card is ClassicalCard)
                {
                    if (card.Value < moreInfluentDecrement)
                    {
                        moreInfluentDecrement = card.Value;
                    }
                }
                else if (card is FlippableCard)
                {
                    int value = -Math.Abs(card.Value);
                    if (value < moreInfluentDecrement)
                    {
                        moreInfluentDecrement = value;
                    }
                }
            }
            return moreInfluentDecrement;
        }
        private void AddSystemCard(Deck deck)
        {
            int pos = Random.Range(0, _systemCards.Count);
            _systemCards[pos].AddToDeck(deck);
            UpdateView();
            if (!_playerStands)
            {
                if (_playersDeck.Sum == 20 || _playersDeck.CurrentIndex == 9)
                {
                    _playerStands = true;
                    FinishPlayersMove();
                }
            }
            if (!_computerStands)
            {
                if (_opponentsDeck.Sum == 20 || _opponentsDeck.CurrentIndex == 9)
                {
                    _computerStands = true;
                }
            }
        }

        private void CheckForSetFinish()
        {
            if (_playerStands && _computerStands)
            {
                EvaluateResult();
            }
        }

        public void AddPlayersCard(int index)
        {
            try
            {
                if (!_playerStands)
                {
                    if (_canThrowHandCard)
                    {
                        _playersHand[index].AddToDeck(_playersDeck);
                        _playersHand[index] = null;
                        _playersHandView[index].color = new Color(0.3215686f, 0.3215686f, 0.3215686f);
                        _playersHandView[index].sprite = null;
                        _playersHandCardsValues[index].text = string.Empty;
                        UpdateView();
                        _canThrowHandCard = false;
                        if (_playersDeck.Sum >= 20)
                        {
                            FinishPlayersMove();
                        }
                    }
                }
            }
            catch { }
        }
        private void UpdateView()
        {
            _opponentsScoreField.text = _opponentsScore.ToString();
            _playersScoreField.text = _playersScore.ToString();
            _playersSumField.text = _playersDeck.Sum.ToString();
            _opponentsSumField.text = _opponentsDeck.Sum.ToString();
        }
        private void UpdateCardsView(Card card)
        {
            _playersHandView[_selectedCardIndex].sprite = card.Image;
            _playersHandCardsValues[_selectedCardIndex].text = card.ValueInLine;
        }
        public void EvaluateResult()
        {
            if ((_playersDeck.HasATiebreaker && !_opponentsDeck.HasATiebreaker && _playersDeck.Sum <= 20) ||
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
        }
        public void AcceptTheResult()
        {
            if (_playersScore < 3 && _opponentsScore < 3)
            {
                _playersScoreField.text = _playersScore.ToString();
                _opponentsScoreField.text = _opponentsScore.ToString();
                _playersDeck.Clear();
                _opponentsDeck.Clear();
                _playerStands = false;
                _computerStands = false;
                _canThrowHandCard = true;
                _resultMessage.SetActive(false);
                UpdateView();
            }
            else
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
                UpdateView();
                _resultMessage.SetActive(false);
                _endGameMessage.SetActive(true);
            }
        }
        public void SetAmount(int amount)
        {
            _amount = amount;
        }
        public void FinishGame()
        {
            ClearGameStatement();
            _endGameMessage.SetActive(false);
            _pazaakView.SetActive(false);
        }
    }
}
