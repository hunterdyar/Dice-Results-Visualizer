using Sprache;

namespace Dice.RollCodeParser;

public class DiceCodeParserSprache
{
	private static readonly Parser<char> DiceSep = Parse.Chars(new []{'d', 'D'});

	public static readonly Parser<NumberExpression> Number =
		from x in Parse.Number
		select new NumberExpression(int.Parse(x));

	public static readonly Parser<DiceRollExpression> NormalDiceRoll =
		from d in Number
		from sep in DiceSep
		from f in Number
		select new DiceRollExpression(d,f);
	
}