using BGClient.Infra;
using BGClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Services
{
	public class GameService: IGameService
	{
		private static Random random = new Random();
		public void ThrowDice(Dice dice)
		{
			dice.Active = true;
			dice.Number = random.Next(1, 7);
		}
	}
}
