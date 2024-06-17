using Dice.RollCodeParser.RollDescription;

namespace Dice.RollCodeParser;

public static class DiceRollExpressionEvaluator
{

	//assumes faces 1 to highestFace
	public static void ApplyEvenDistributionFacesForNormalDice(this DiceRoll roll, DiceRoll highestFace)
	{
		foreach (var face in highestFace.GetResults())
		{
			for (int j = 0; j < face.Chances; j++)
			{
				for (int i = 1; i <= face.FaceValue; i++)
				{
					roll.AddResult(new Result(i, 1));
				}
			}
		}
		// for (int i = 1; i <= highestFace; i++)
		// {
		// 	roll.AddResult(new Result(i,1));
		// }
	}

	public static void ApplyNormalDiceRollExpression(this DiceRoll roll, DiceRoll numberDice, DiceRoll highestFace)
	{
		foreach (var dice in numberDice.GetResults())
		{
			for (int j = 0; j < dice.Chances; j++)
			{
				for (int i = 1; i <= dice.FaceValue; i++)
				{
					DiceRoll nRoll = new DiceRoll();
					nRoll.ApplyEvenDistributionFacesForNormalDice(highestFace);
					roll.CombineWithOtherRoll(nRoll);
				}
			}
		}
	}
}