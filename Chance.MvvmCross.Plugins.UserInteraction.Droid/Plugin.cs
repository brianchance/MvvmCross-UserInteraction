using System;
using Cirrious.CrossCore.Plugins;
using Cirrious.CrossCore;

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

