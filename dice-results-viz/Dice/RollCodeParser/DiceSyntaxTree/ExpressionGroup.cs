using System.Collections.Generic;

namespace Dice.RollCodeParser
{
	public class ExpressionGroup : Expression
	{
		public string Label;
		public List<Expression> Expressions = new List<Expression>();

		public ExpressionGroup()
		{
			Expressions = new List<Expression>();
			Label = "";
		}
		public ExpressionGroup(IEnumerable<Expression> exp)
		{
			Expressions = new List<Expression>();
			foreach (var e in exp)
			{
				Expressions.Add(e);
			}
			Label = "";
		}
		
		public override string ToString()
		{
			var s = "(";
			foreach (var e in Expressions)
			{
				s += e.ToString();
				s += ",";
			}

			s += ")";
			return s;
		}
	}
}