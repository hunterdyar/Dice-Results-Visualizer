﻿namespace Dice.RollCodeParser
{
	public class DiceRollExpression : Expression
	{
		public string Label;
		public Expression NumberDice;
		public Expression NumberFaces;
		public Expression Drop = null;
		public Expression Keep = null;
		public bool Exploding { get; set; }

		public DiceRollExpression()
		{
			
		}

		public DiceRollExpression(Expression numDice, Expression faces)
		{
			this.NumberDice = numDice;
			this.NumberFaces = faces;
		}

		public DiceRollExpression(Expression numDice, Expression faces, Expression drop, Expression keep)
		{
			this.NumberDice = numDice;
			this.NumberFaces = faces;
			this.Drop = drop;
			this.Keep = keep;
		}
		
		public override string ToString()
		{
			if (Drop == null && Keep == null)
			{
				return NumberDice.ToString() + "d" + NumberFaces.ToString() + (Exploding ? "!" : "");
			}
			else
			{
				return NumberDice.ToString() + "d" + NumberFaces.ToString() + (Exploding ? "!" : "");
			}
		}
	}
}