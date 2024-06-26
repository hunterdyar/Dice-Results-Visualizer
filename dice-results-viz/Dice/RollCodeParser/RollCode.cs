﻿using Dice.RollCodeParser.RollDescription;
using Sprache;

namespace Dice.RollCodeParser
{
	public class RollCode
	{
		private string code;
		public Expression Expression; 
		private List<Token> _tokens;
		private Evaluator _evaluator;
		public DiceRoll Result;
		delegate int RollDiceDelegate(DiceRollExpression dre);
		
		public RollCode(string code)
		{
			this.code = code;
			var tokens = Tokenize(code);
			Expression = DiceCodeParserSprache.ExpressionGroup.Parse(code);
			_evaluator = new Evaluator();
			Result = _evaluator.Evaluate(Expression);
		}
		
		private List<Token> Tokenize(string s)
		{
			s = s.Trim().ToLower();
			var tokens = new List<Token>();
			for (int i = 0; i < s.Length; i++)
			{
				var c = s[i];
				//, is just a seperator. You can comma-separate rolls "1d6,2d20" or "1d6+2d20" or "1d6 2d20" will all work (although the + will parse differently)
				if (c == ' ' || c == '\n' || c == '\t' || c == '\r' || c == ',')
				{
					continue;
				}else if ("0123456789".Contains(c))
				{
					if (tokens.Count > 0 && tokens[^1].TType == RollTokenType.Number)
					{
						tokens[^1].Literal += c;
						if (tokens[^1] is NumberToken nt)
						{
							nt.RecalculateValue();
						}

						continue;
					}
					else
					{
						var t = new NumberToken(c.ToString());
						tokens.Add(t);
						continue;
					}
				}else if (c == '[')
				{
					//Eat up a label! yum.
					tokens.Add(new Token(RollTokenType.LabelOpen,c));
					string literal = "";
					i++;//consume the [
					for (; s[i] != ']' && i < s.Length; i++)
					{
						literal += s[i];
					}
					tokens.Add(new Token(RollTokenType.StringLiteral,literal));
					tokens.Add(new Token(RollTokenType.LabelClose,s[i]));
					continue;
				}

				switch (c){
					case 'd':
					{
						var t = new Token(RollTokenType.DiceSep, c);
						tokens.Add(t);
						continue;
					} 
					case '+':
						tokens.Add(new Token(RollTokenType.Add,c));
						continue;
						
					case '-':
					tokens.Add(new Token(RollTokenType.Subtract,c));
					continue;
				case'x': 
				case '*':
					tokens.Add(new Token(RollTokenType.Multiply,c));
					continue;
				case '/':
					tokens.Add(new Token(RollTokenType.Divide,c));
					continue;
				case 'k':
					tokens.Add(new Token(RollTokenType.Keep,c));
					continue;
				case '!':	
					tokens.Add(new Token(RollTokenType.Explode, c));
					continue;
				case '[':
					tokens.Add(new Token(RollTokenType.LabelOpen,c));
					continue;
				case ']':
					tokens.Add(new Token(RollTokenType.LabelClose,c));
					continue;
				case '(':
					tokens.Add(new Token(RollTokenType.GroupOpen, c));
					continue;
				case ')':
					tokens.Add(new Token(RollTokenType.GroupClose, c));
					continue;
				default:
					Console.Error.WriteLine($"Unexpected character {c}");
					continue;
				}
			}
			return tokens;
		}
	}
}