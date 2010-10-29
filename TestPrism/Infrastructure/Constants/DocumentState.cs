using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Constants
{
	public enum DocumentState
	{ 
		Fetching,
		Invalid,
		Active
	}
	public enum DocumentType
	{
		Job,
		Host,
		Script,
		Info
	}
}
