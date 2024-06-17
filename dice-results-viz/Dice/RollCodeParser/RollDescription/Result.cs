namespace Dice.RollCodeParser.RollDescription;

public class Result
{
	public int FaceValue;
	/// <summary>
	/// The Probabilty. Of all possible dice results, this one appears this many times.
	/// We turn a complex multi-dice roll into an imaginary single dice with the same odds as the given dice. This is the number of times this given face appers on this single big dice.
	/// </summary>
	public int Chances;
	public decimal Odds;
	public Result(int faceValue)
	{
		FaceValue = faceValue;
		Chances = 1;
	}

	public Result(int faceValue, int chances)
	{
		this.FaceValue = faceValue;
		this.Chances = chances;
	}
	public void IncrementChances(int delta = 1)
	{
		Chances += delta;
	}
}