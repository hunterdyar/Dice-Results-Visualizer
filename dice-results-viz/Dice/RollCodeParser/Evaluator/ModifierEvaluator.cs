using Dice.RollCodeParser.RollDescription;

namespace Dice.RollCodeParser;

public static class ModifierEvaluator
{
	public static void ApplyModifier(this DiceRoll roll, Modifier mod, DiceRoll modroll)
	{
		foreach (var face in modroll.GetResults())
		{
			for (int i = 0; i < face.Chances; i++)
			{
				switch (mod)
				{
					case Modifier.Add:
						roll.ShiftAllFaces(face.FaceValue);
						break;
					case Modifier.Subtract:
						roll.ShiftAllFaces(-face.FaceValue);
						break;
					case Modifier.Multiply:
						roll.MultAllFaces(face.FaceValue);
						break;
					case Modifier.Divide:
						roll.MultAllFaces(1 / face.FaceValue);
						break;
				}
			}
		}
		
	}
}