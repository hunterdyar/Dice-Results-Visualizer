using Sprache;

namespace Dice.RollCodeParser;

public class DiceCodeParserSprache
{
	private static readonly Parser<char> DiceSep = Parse.Chars(new []{'d', 'D'});
	private static readonly Parser<Modifier> PlusSign = from x in Parse.Char('+') select Modifier.Add;

	private static readonly Parser<Modifier> TimesSign =
		from x in Parse.Chars(new[] { '*', 'x', 'X' }) select Modifier.Multiply;

	private static readonly Parser<Modifier> MinusSign = from x in Parse.Char('-') select Modifier.Subtract;
	private static readonly Parser<Modifier> DivideSign = from x in Parse.Char('/') select Modifier.Divide;
	private static readonly Parser<Modifier> ModifierSign = PlusSign.Or(MinusSign.Or(TimesSign.Or(DivideSign)));
	
	public static readonly Parser<NumberExpression> Number =
		from x in Parse.Number
		select new NumberExpression(int.Parse(x));

	public static readonly Parser<Expression> NormalDiceRoll =
		from d in Number.Optional()
		from sep in DiceSep
		from f in Number
		//label
		select d.IsDefined ? new DiceRollExpression(d.Get(), f) : new DiceRollExpression(f);

	public static readonly Parser<Expression> DiceOrNumber =
		from d in NormalDiceRoll.Or(Number)
		select d;

	private static readonly Parser<Expression> ModifiedExpression =
		from w in Parse.WhiteSpace.Many()
		from x in ModifierSign.Optional()
		from d in DiceOrNumber
		select x.IsDefined ? new ModifierExpression(x.Get(), d) : new ModifierExpression(Modifier.Add, d);
	
	public static readonly Parser<ExpressionGroup> ExpressionGroup =
		from e in ModifiedExpression.Many()
		select new ExpressionGroup(e);
	
}