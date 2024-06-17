﻿namespace Dice.RollCodeParser
{
	public enum RollTokenType
	{
		Invalid,
		Number,
		DiceSep,
		Add,
		Subtract,
		Multiply,
		Divide,
		Keep,
		Explode,
		LabelOpen,
		LabelClose,
		StringLiteral,
		GroupOpen,
		GroupClose,
	}
}