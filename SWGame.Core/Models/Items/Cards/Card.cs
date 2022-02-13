namespace SWGame.Core.Models.Items.Cards
{
    public class Card : Item
    {
        protected int _value;
        protected string _valueInLine;

        public int Value { get => _value; set => _value = value; }
        public string ValueInLine { get => _valueInLine; set => _valueInLine = value; }


        public Card(int id, string name, int value)
        {
            _id = id;
            _name = name;
            _value = value;
            GenerateLineFromValue();
        }

        public virtual void GenerateLineFromValue()
        {
            _valueInLine = _value.ToString();
        }
    }
}
