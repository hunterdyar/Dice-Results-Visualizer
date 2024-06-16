using Dice.RollCodeParser.RollDescription;

namespace Dice.RollCodeParser
{
	public class Evaluator
	{
		public DiceRoll Evaluate(List<Expression> expressions)
		{
			DiceRoll roll = new DiceRoll();
			foreach (var expr in expressions)
			{ 
				Evaluate(expr, ref roll);
			}
			return roll;
		}

		public void Evaluate(Expression exp, ref DiceRoll roll) 
		{
			if (exp is DiceRollExpression dre)
			{
				int numDice = GetValueFromExpression(dre.NumberDice);
				int numFace = GetValueFromExpression(dre.NumberFaces);
				roll.ApplyNormalDiceRollExpression(numDice, numFace);
			}else if (exp is ModifierExpression mod)
			{
				int val = GetValueFromExpression(mod.Expression);
				roll.ApplyModifier(mod.Modifier,val);
			}else if (exp is ExpressionGroup group)
			{
				DiceRoll subRoll = new DiceRoll();
				foreach (var expression in group.Expressions)
				{
					Evaluate(expression, ref subRoll);
				}
				roll.CombineWithOtherRoll(subRoll, Modifier.Add);
			}
			else
			{
				Console.WriteLine("Invalid Root Expression");
			}
		}

		// private static int DefaultRoll(int numFaces)
		// {
		// 	return Random.Range(1, numFaces + 1);
		// }

		public int GetValueFromExpression(Expression exp)
		{
			if (exp == null)
			{
				//This is valid for drop and keep items - if we don't have them, they'll be null!
				return 0;
			}
			
			if (exp is NumberExpression ne)
			{
				return ne.Value;
			}
			else
			{
				//todo: We have to yield here!
				//we need to make some kind of ... thing for this...
				throw new ArgumentException($"Not supporting expressions that aren't numbers, yet. ({exp.ToString()})");
			}
		}
	}
}