namespace Dice.RollCodeParser
{
	public class NumberExpression : Expression
	{
		public int Value;

		public NumberExpression(int value)
		{
			Value = value;
		}

		public NumberExpression()
		{
			
		}

		public override string ToString()
		{
			return Value.ToString();
		}
	}
}