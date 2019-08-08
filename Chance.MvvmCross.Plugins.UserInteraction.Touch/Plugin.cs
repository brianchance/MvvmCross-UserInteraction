using MvvmCross;
using MvvmCross.Plugin;
using System;

namespace Chance.MvvmCross.Plugins.UserInteraction.Touch
{
    [MvxPlugin]
	public class Plugin : IMvxPlugin
	{
		public void Load() 
		{
			Mvx.IoCProvider.RegisterType<IUserInteraction, UserInteraction>();
		}
	}
}

