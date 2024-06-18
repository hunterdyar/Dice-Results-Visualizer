using Dice.RollCodeParser;
using Sprache;

namespace DiceParserTests;

public class DiceEvaluatorTests
{
	[Test]
	public void Test1()
	{
		var e = DiceCodeParserSprache.ExpressionGroup.Parse("3d6");

		Assert.IsTrue(e.Expressions.Count == 1);
		if (e.Expressions[0] is ModifierExpression mod)
		{
			Assert.That(Modifier.Add, Is.EqualTo(mod.Modifier));
			if (mod.Expression is DiceRollExpression dre)
			{
				Assert.AreEqual(dre.NumberDice.ToString(), "3");
				Assert.AreEqual(dre.NumberFaces.ToString(), "6");
			}
			else
			{
				Assert.Fail();
			}
		}
		else
		{
			Assert.Fail();
		}
	}
}