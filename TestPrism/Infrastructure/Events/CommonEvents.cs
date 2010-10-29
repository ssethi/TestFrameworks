using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace Infrastructure.Events
{
	public class ChangeTextEvent : CompositePresentationEvent<string>
	{

	}
}
