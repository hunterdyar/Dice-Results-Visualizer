using Sprache;

namespace Dice.RollCodeParser;

public class DiceCodeParserSprache
{
	private static readonly Parser<char> DiceSep = Parse.Chars(new []{'d', 'D'});

	public static readonly Parser<NumberExpression> Number =
		from x in Parse.Number
		select new NumberExpression(int.Parse(x));

	public static readonly Parser<Expression> NormalDiceRoll =
		from d in Number
		from sep in DiceSep
		from f in Number
		//label
		select new DiceRollExpression(d,f);

	public static readonly Parser<Expression> DiceOrNumber =
		from d in NormalDiceRoll.Or(Number)
		select d;
	// public static readonly Parser<ExpressionGroup> ExpressionGroup =
	// 	from e in NormalDiceRoll.Many()
	// 	select new ExpressionGroup(e.GetEnumerator());

}