using Dice.RollCodeParser.RollDescription;

namespace Dice.RollCodeParser;

public static class DiceRollExpressionEvaluator
{
	public static void ApplyDiceRollExpression(this DiceRoll roll, DiceRollExpression dre)
	{
		
	}

	//assumes faces 1 to highestFace
	public static void ApplyEvenDistributionFacesForNormalDice(this DiceRoll roll, int highestFace)
	{
		for (int i = 1; i <= highestFace; i++)
		{
			roll.AddResult(new Result(i,1));
		}
	}

	public static void ApplyNormalDiceRollExpression(this DiceRoll roll, int numberDice, int highestFace)
	{
		for (int i = 1; i <= numberDice; i++)
		{
			DiceRoll nRoll = new DiceRoll();
			nRoll.ApplyEvenDistributionFacesForNormalDice(highestFace);
			roll.CombineWithOtherRoll(nRoll);
		}
	}
}