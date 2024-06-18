using System.Text;

namespace Dice.RollCodeParser.RollDescription;

public class DiceRoll
{
	private Dictionary<int, Result> _faceMap = new Dictionary<int, Result>();
	public int TotalImaginaryRollCount() => _faceMap.Values.Sum(x => x.Chances);
	
	public void AddResult(Result r)
	{
		
		if(_faceMap.TryGetValue(r.FaceValue, out var existing))
		{
			existing.IncrementChances(r.Chances);
		}else
		{
			_faceMap.Add(r.FaceValue,r);
		}
	}

	public List<Result> GetResults()
	{
		CalculateOdds();
		if (_faceMap.Count == 0)
		{
			return new List<Result>();
		}
		var results = _faceMap.Values.ToList();
		results.Sort((x,y)=>x.FaceValue - y.FaceValue);
		return results;
	}

	public List<Result> GetAtLeastResults()
	{
		var r = GetResults();
		var atleast = new List<Result>();
		//we could speed this up by manually looping through r. it's sorted by face value, so we cna reverse loop through it and add up the rest of i.
		foreach (var result in r)
		{
			var x = new Result(result.FaceValue);
			x.Chances = r.Where(y => x.FaceValue <= y.FaceValue).Sum(y => y.Chances);
			atleast.Add(x);
		}

		int total = TotalImaginaryRollCount();
		foreach (var a in atleast)
		{
			a.Odds = a.Chances/(decimal)total;
		}
		atleast.Sort((x, y) => x.FaceValue - y.FaceValue);
		return atleast;
	}
	private void CalculateOdds()
	{
		int total = TotalImaginaryRollCount();
		foreach (var kvp in _faceMap)
		{
			kvp.Value.Odds = kvp.Value.Chances / (decimal)total;
		}
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
			for (int i = 0; i < or.Chances; i++)
			{
				foreach (var r in myRes)
				{
					for (int j = 0; j < r.Chances; j++)
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
			AddResult(new Result(r.FaceValue+delta,r.Chances));
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

	public void SetTo(DiceRoll other)
	{
		_faceMap = other._faceMap;
	}
}