namespace SWGame.Core.Models.Items.Cards
{
    public class FlippableCard : Card
    {
        public FlippableCard(int id, string name, int value) : base(id, name, value)
        {
            GenerateLineFromValue();
        }
        public override void GenerateLineFromValue()
        {
            _valueInLine = $"+/-{_value}";
        }
    }
}
