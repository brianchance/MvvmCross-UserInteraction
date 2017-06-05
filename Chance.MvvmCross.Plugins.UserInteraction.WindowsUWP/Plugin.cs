using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace Chance.MvvmCross.Plugins.UserInteraction.Uwp
{
	public class Plugin : IMvxPlugin
	{
		public void Load() 
		{
			Mvx.RegisterType<IUserInteraction, Uwp.UserInteraction>();
		}
	}
}

