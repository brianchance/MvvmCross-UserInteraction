using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Chance.MvvmCross.Plugins.UserInteraction.Droid
{
	public class Plugin : IMvxPlugin
	{
		public void Load() 
		{
			Mvx.RegisterType<IUserInteraction, UserInteraction>();
		}
	}
}

