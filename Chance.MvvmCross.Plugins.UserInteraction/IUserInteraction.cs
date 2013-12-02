using System;
using System.Threading.Tasks;

namespace Chance.MvvmCross.Plugins.UserInteraction
{
	public interface IUserInteraction
	{
		void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel");
		void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel");

		void Alert(string message, Action done = null, string title = "", string okButton = "OK");

		void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");
		void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");

		Task AlertAsync(string message, string title = "", string okButton = "OK");
		Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel");
		Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");

	    void ConfirmThreeButtons(string message, Action<ConfirmResponse> answer, string title = null, string positive = "Yes", string negative = "No",
	        string neutral = "Maybe");

	    Task<ConfirmResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No",
	        string neutral = "Maybe");
	}

    public enum ConfirmResponse
    {
        Positive,
        Negative,
        Neutral
    }
}

