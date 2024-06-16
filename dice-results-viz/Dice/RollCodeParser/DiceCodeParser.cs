﻿using System.Text;

namespace Dice.RollCodeParser
{
	public class DiceCodeParser
	{
		private readonly List<Token> _tokens;
		public List<Expression> Expressions;
		private int _pos;
		public DiceCodeParser(List<Token> tokens)
		{
			Expressions = new List<Expression>();
			_tokens = tokens;
		}
		
		public void Parse()
		{
			_pos = 0;
			Expressions.Clear();
			while (_pos < _tokens.Count)
			{
				Expressions.Add(ParseNextToken());
			}
		}

		public override string ToString()
		{
			StringBuilder s = new StringBuilder();
			foreach (var e in Expressions)
			{
				s.AppendLine(e.ToString());
			}

			return s.ToString();
		}

		private Expression ParseNextToken()
		{
			var token = _tokens[_pos];
			switch (token.TType)
			{
				case RollTokenType.Number:
					var n = ParseNumberToken();
					return n;	
				case RollTokenType.Add:
				case RollTokenType.Divide:
				case RollTokenType.Multiply:
				case RollTokenType.Subtract:
					return ParseModifierToken();
				case RollTokenType.DiceSep:
					return ParseDiceToken();
				case RollTokenType.Explode:
					return ParseExplodeToken();
				case RollTokenType.Keep:
					return ParseKeepToken();
				case RollTokenType.LabelOpen:
					return ParseLabelToken();
			}

			return null;
		}

		private Expression ParseLabelToken()
		{
			var left = PopLeftExpressionOrNull();
			_pos++;//consume the [
			
			if (_tokens[_pos].TType != RollTokenType.StringLiteral)
			{
				Console.WriteLine("Empty label? That's not how the tokenizer should report empties.");
			}
			
			if (left is ModifierExpression mod)
			{
				mod.Label = _tokens[_pos].Literal;
			}else if (left is DiceRollExpression dre)
			{
				dre.Label = _tokens[_pos].Literal;
			}else if (left is ExpressionGroup eg)
			{
				eg.Label = _tokens[_pos].Literal;
			}
			else
			{
				Console.WriteLine(("Invalid Label"));
			}

			_pos++;
			//eat or break
			if (_tokens[_pos].TType != RollTokenType.LabelClose)
			{
				Console.WriteLine("Label not closed? Bad.");
			}

			_pos++;
			return left;
		}

		private Expression ParseKeepToken()
		{
			var left = PopLeftExpressionOrNull();
			if (left is DiceRollExpression dre)
			{
				_pos++;
				dre.Keep = ParseNextToken();
				return dre;
			}
			else
			{
				//todo: exploding groups.
			}

			Console.WriteLine("Keep token in bad location. ");
			_pos++;
			return left;
		}

		private Expression ParseExplodeToken()
		{
			var left = PopLeftExpressionOrNull();
			if (left is DiceRollExpression dre)
			{
				dre.Exploding = true;
				_pos++;
				return dre;
			}
			else
			{
				//todo: exploding groups.
				Console.WriteLine("Exploding token in bad location.");
			}

			_pos++;
			return left;
		}

		private Expression PopLeftExpressionOrNull()
		{
			//get previous expression
			Expression left;
			if (Expressions.Count == 0)
			{
				//"d4" should become "1d4".
				left = new NumberExpression()
				{
					Value = 1,
				};
			}
			else
			{
				left = Expressions[^1];
				Expressions.Remove(left);
				return left;
			}

			return null;
		}
		private Expression ParseDiceToken()
		{
			Expression left = PopLeftExpressionOrNull();
			
			if(left == null){
				left = new NumberExpression()
				{
					Value = 1,
				};
			}
			//2d20d2 is roll 2d20 and drop the lowest 2 results.
			if (left is DiceRollExpression existingDRE)
			{
				_pos++;
				existingDRE.Drop = ParseNextToken();
				return existingDRE;
			}
			
			var dre = new DiceRollExpression();
			//consume the sep
			_pos++;
			dre.NumberDice = left;
			dre.NumberFaces = ParseNextToken();
			return dre;
		}

		private Expression ParseModifierToken()
		{
			var token = _tokens[_pos];
			var modifier = new ModifierExpression();
			if (token.TType == RollTokenType.Add)
			{
				modifier.Modifier = Modifier.Add;
			}else if (token.TType == RollTokenType.Multiply)
			{
				modifier.Modifier = Modifier.Multiply;
			}else if (token.TType == RollTokenType.Divide)
			{
				modifier.Modifier = Modifier.Divide;
			}else if (token.TType == RollTokenType.Subtract)
			{
				modifier.Modifier = Modifier.Subtract;
			}
			//consume the token.
			_pos++;
			modifier.Expression = ParseNextToken();
			return modifier;
		}

		private Expression ParseNumberToken()
		{
			var token = _tokens[_pos];
			if (token is NumberToken nt)
			{
				var numberExpression = new NumberExpression();
				numberExpression.Value = nt.Value;
				_pos++;//consume!
				return numberExpression;
			}
			else
			{
				Console.WriteLine($"Unable to parse {token} as token.");
				return null;
			}
		}
	}
}