using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGClient.Infra
{
	public interface IMessageService
	{
		Task Show(string msg);
		Task ShowContent(string msg);
		Task HideContent();
		//Task<string> ShowAcceptDialog(string msg);
	}
}
