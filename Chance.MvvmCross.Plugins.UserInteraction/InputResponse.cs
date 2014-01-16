using System;

namespace Chance.MvvmCross.Plugins.UserInteraction
{
	public class InputResponse<T>
	{
		public bool Ok { get; set; }
		public T Value { get; set;}
	}
}

