using Dice.RollCodeParser.RollDescription;

namespace Dice.RollCodeParser;

public static class ModifierEvaluator
{
	public static void ApplyModifier(this DiceRoll roll, Modifier mod, int value)
	{
		switch (mod)
		{
			case Modifier.Add:
				roll.ShiftAllFaces(value);
				break;
			case Modifier.Subtract:
				roll.ShiftAllFaces(-value);
				break;
			case Modifier.Multiply:
				roll.MultAllFaces(value);
				break;
			case Modifier.Divide:
				roll.MultAllFaces(1/value);
				break;
		}
	}
}