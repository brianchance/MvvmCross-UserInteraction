using System;

namespace Chance.MvvmCross.Plugins.UserInteraction
{
	public interface IUserInteraction
	{
		void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel");
		void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel");

		void Alert(string message, Action done = null, string title = "");

		void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");
		void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");
	}
}

