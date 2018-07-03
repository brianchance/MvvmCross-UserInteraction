using System;
using MvvmCross;
using MvvmCross.Plugin;

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

