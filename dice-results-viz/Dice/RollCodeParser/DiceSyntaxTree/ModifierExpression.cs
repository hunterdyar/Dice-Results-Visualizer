namespace Dice.RollCodeParser
{
	
	
	public class ModifierExpression: Expression
	{
		public string Label;
		public Modifier Modifier;
		public Expression Expression;

		public ModifierExpression()
		{
			Modifier = Modifier.Add;
			Label = "";
		}

		public ModifierExpression(Modifier mod, Expression exp)
		{
			Modifier = mod;
			Expression = exp;
			Label = "";
		}

		public override string ToString()
		{
			return ModToString(Modifier) + Expression.ToString();
		}

		public static string ModToString(Modifier m)
		{
			switch (m)
			{
				case Modifier.Add:
					return "+";
				case Modifier.Divide:
					return "/";
				case Modifier.Multiply:
					return "x";
				case Modifier.Subtract:
					return "-";
				default:
					return "";
			}
		}
	}
}