using SWGame.Core.Enums;

namespace SWGame.Core.Models.Items.Cards
{
    public class GoldCard : Card
    {
        private GoldCardType _type;
        private int _index;
        private int[] _possibleValues;

        public GoldCard(int id, string name, GoldCardType type, int value) : base(id, name, value)
        {
            _type = type;
            switch (_type)
            {
                case GoldCardType.PlusMinusOneOrTwo:
                    _index = 0;
                    _possibleValues = new int[] { 1, -1, 2, -2 };
                    _value = _possibleValues[_index];
                    break;
                case GoldCardType.TCard:
                    _index = 0;
                    _possibleValues = new int[] { 1, -1 };
                    _value = _possibleValues[_index];
                    break;
            }
            GenerateLineFromValue();
        }

        public GoldCardType Type { get => _type; set => _type = value; }
        public int Index { get => _index; set => _index = value; }
        public int[] PossibleValues { get => _possibleValues; set => _possibleValues = value; }

        public override void GenerateLineFromValue()
        {
            switch (_type)
            {
                case GoldCardType.DCard:
                    _valueInLine = "D";
                    break;
                case GoldCardType.TCard:
                    _valueInLine = "+/-1T";
                    break;
                case GoldCardType.ThreeAndSix:
                    _valueInLine = "3&6";
                    break;
                case GoldCardType.TwoAndFour:
                    _valueInLine = "2&4";
                    break;
                case GoldCardType.PlusMinusOneOrTwo:
                    _valueInLine = "+/- 1or2";
                    break;
            }
        }
    }
}
