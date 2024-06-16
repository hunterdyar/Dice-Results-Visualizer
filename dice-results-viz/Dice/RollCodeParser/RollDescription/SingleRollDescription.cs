using System.Collections.Generic;
using System.Linq;

namespace Dice.RollCodeParser.RollDescription
{
	[System.Serializable]
	public class SingleRollDescription
	{
		//Serializable Roll Description
		public int numberTimesToRoll;
		public int numberSides;
		public ExplodeBehaviour exploding;
		
		public SingleRollDescription(int times, int sides, ExplodeBehaviour exploding = ExplodeBehaviour.DontExplode)
		{
			numberTimesToRoll = times;
			numberSides = sides;
			this.exploding = exploding;
		}
	}
}