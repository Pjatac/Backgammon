using BGClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Infra
{
	public interface IGameService
	{
		void ThrowDice(Dice dice);
	}
}
