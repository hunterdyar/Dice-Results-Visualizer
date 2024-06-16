namespace Dice.RollCodeParser
{
	public class NumberToken : Token
	{
		public int Value;

		public NumberToken(string literal)
		{
			this.Literal = literal;
			this.TType = RollTokenType.Number;
			RecalculateValue();
		}

		public void RecalculateValue()
		{
			Value = int.Parse(Literal);
		}
	}
}