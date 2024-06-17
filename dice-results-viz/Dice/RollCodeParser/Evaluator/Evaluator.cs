using Dice.RollCodeParser.RollDescription;

namespace Dice.RollCodeParser
{
	public class Evaluator
	{
		public DiceRoll Evaluate(List<Expression> expressions)
		{
			DiceRoll final = new DiceRoll();
			foreach (var expr in expressions)
			{ 
				var newRoll = Evaluate(expr);
				final.CombineWithOtherRoll(newRoll);
			}
			return final;
		}

		public DiceRoll Evaluate(Expression exp)
		{
			if (exp is DiceRollExpression dre)
			{
				var roll = new DiceRoll();
				var numDice = Evaluate(dre.NumberDice);
				var numFace = Evaluate(dre.NumberFaces);
				roll.ApplyNormalDiceRollExpression(numDice, numFace);
				return roll;
			}
			else if (exp is ModifierExpression mod)
			{
				var roll = new DiceRoll();
				roll.ApplyModifier(mod.Modifier,Evaluate(mod.Expression));
				return roll;
			}
			else if (exp is ExpressionGroup group)
			{
				//a lot of (parenthesis) are just for op order disambiguation
				if (group.Expressions.Count == 1)
				{
					return Evaluate(group.Expressions[0]);
				}
				
				DiceRoll subRoll = new DiceRoll();
				foreach (var expression in group.Expressions)
				{
					var newRoll = Evaluate(expression);
					subRoll.CombineWithOtherRoll(newRoll);
				}
				return subRoll;
			}else if (exp is NumberExpression num)
			{
				//Handle if we just wrote "7"
				var roll = new DiceRoll(); 
				roll.ShiftAllFaces(num.Value);
				return roll;
			}
			else
			{
				Console.WriteLine("Invalid Root Expression");
			}

			return null;
		}

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
				//(2+2) should become 4.
				//we need to make some kind of ... thing for this...
				throw new ArgumentException($"Not supporting expressions that aren't numbers, yet. ({exp.ToString()})");
			}
		}
	}
}