﻿namespace Dice.RollCodeParser.RollDescription;

public class DiceRoll
{
	private Dictionary<int, Result> _faceMap = new Dictionary<int, Result>();

	public void AddResult(Result r)
	{
		if(_faceMap.TryGetValue(r.FaceValue, out var existing))
		{
			existing.IncrementProbabilty(r.Probability);
		}else
		{
			_faceMap.Add(r.FaceValue,r);
		}
	}

	public List<Result> GetResults()
	{
		if (_faceMap.Count == 0)
		{
			return new List<Result>();
		}
		var results = _faceMap.Values.ToList();
		results.Sort((x,y)=>x.FaceValue - y.FaceValue);
		return results;
	}
	
	public void MergeDiceRoll(DiceRoll other)
	{
		//Turn two lists into one list of the sums.
		foreach (var r in other.GetResults())
		{
			AddResult(r);
		}
	}

	public void CombineWithOtherRoll(DiceRoll other, Modifier mod = Modifier.Add)
	{
		var myRes = GetResults();
		if (myRes.Count == 0)
		{
			foreach (var r in other.GetResults())
			{
				AddResult(r);
			}
			return;
		}
		var otherRes = other.GetResults();
		if (otherRes.Count == 0)
		{
			return;
		}
		_faceMap.Clear();
		foreach (var or in otherRes)
		{
			for (int i = 0; i < or.Probability; i++)
			{
				foreach (var r in myRes)
				{
					for (int j = 0; j < r.Probability; j++)
					{
						switch (mod)
						{
							case Modifier.Add:
								var n = new Result(r.FaceValue + or.FaceValue);
								AddResult(n);
								break;
							case Modifier.Subtract:
								n = new Result(r.FaceValue - or.FaceValue);
								AddResult(n);
								break;
							case Modifier.Divide:
								n = new Result(r.FaceValue / or.FaceValue);
								AddResult(n);
								break;
							case Modifier.Multiply:
								n = new Result(r.FaceValue * or.FaceValue);
								AddResult(n);
								break;
						}
					}
				}
			}
		}
	}

	public void ShiftAllFaces(int delta)
	{
		if (delta == 0)
		{
			return;
		}
		var myRes = GetResults();
		//0d0+3 = 3?
		if (myRes.Count == 0)
		{
			AddResult(new Result(delta));
			return;
		}
		_faceMap.Clear();
		foreach (var r in myRes)
		{
			//todo: wait, didn't i turn results into classes instead of structs?
			//ah well this is silly slow.
			AddResult(new Result(r.FaceValue+delta));
		}
	}

	public void MultAllFaces(int value)
	{
		if (value == 0)
		{
			//remove all results.
			_faceMap.Clear();
			return;
		}

		var myRes = GetResults();
		if (myRes.Count == 0)
		{
			return;
		}

		_faceMap.Clear();
		foreach (var r in myRes)
		{
			//todo: wait, didn't i turn results into classes instead of structs?
			//ah well this is silly slow.
			AddResult(new Result(r.FaceValue * value));
		}
	}
}