using Dice.RollCodeParser;
using Sprache;
using Result = Dice.RollCodeParser.RollDescription.Result;

namespace DiceParserTests;

public class Tests
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void Test1()
	{
		var r = new RollCode("1d6");
		var results = r.Result.GetResults();
		for (int i = 1; i <= 6; i++)
		{
			Assert.AreEqual(results[i - 1], new Result(i, 1));
		}
	}
}