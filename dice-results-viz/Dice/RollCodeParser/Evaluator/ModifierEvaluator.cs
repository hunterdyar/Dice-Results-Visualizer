using Dice.RollCodeParser.RollDescription;

namespace Dice.RollCodeParser;

public static class ModifierEvaluator
{
	public static void ApplyModifier(this DiceRoll roll, Modifier mod)
	{
		if (mod == Modifier.Add)
		{
			return;
		}

		if (mod == Modifier.Subtract)
		{
			roll.MultAllFaces(-1);
		}else if (mod == Modifier.Multiply)
		{
			throw new Exception("Can't multiply nothing by something");
		}
		else if (mod == Modifier.Divide)
		{
			throw new Exception("Can't divide nothing by something");
		}
	}
	public static void ApplyModifier(this DiceRoll roll, Modifier mod, DiceRoll modroll)
	{
		if (roll.GetResults().Count == 0)
		{
			if (mod == Modifier.Add)
			{
				roll.SetTo(modroll);
				return;
			}
			else
			{
				roll.SetTo(modroll);
				roll.ApplyModifier(mod);
			}
		}
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