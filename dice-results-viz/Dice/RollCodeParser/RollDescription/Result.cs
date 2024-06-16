namespace Dice.RollCodeParser.RollDescription;

public class Result
{
	public int FaceValue;
	public int Probability;
	public decimal Odds;
	public Result(int faceValue)
	{
		FaceValue = faceValue;
		Probability = 1;
	}

	public Result(int faceValue, int probability)
	{
		this.FaceValue = faceValue;
		this.Probability = probability;
	}
	public void IncrementProbabilty(int delta = 1)
	{
		Probability += delta;
	}
}