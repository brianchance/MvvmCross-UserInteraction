using System;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;

namespace Chance.MvvmCross.Plugins.UserInteraction.Ios
{
	public class Plugin : IMvxPlugin
	{
		public void Load() 
		{
			Mvx.RegisterType<IUserInteraction, UserInteraction>();
		}
	}
}

