using System;
using System.Threading.Tasks;

namespace Chance.MvvmCross.Plugins.UserInteraction
{
	public interface IUserInteraction
	{
		void Confirm(string message, Action okClicked = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");
		void Confirm(string message, Action<bool> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");

		void Alert(string message, Action done = null, string title = "", string okButton = "OK");

		void Input(string message, Action<string> okClicked = null, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");
		void Input(string message, Action<bool, string> answer = null, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");

		Task AlertAsync(string message, string title = "", string okButton = "OK");
		Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel");
		Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");

		void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer = null, string title = null, string positive = "Yes", string negative = "No",
			string neutral = "Maybe");

		Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No",
			string neutral = "Maybe");

		void ChooseSingle(string message, string[] options, int? chosenItem = null, Action<int?> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");

		Task<int?> ChooseSingleAsync(string message, string[] options, int? chosenItem = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");

		void ChooseMultiple(string message, string[] options, int[] selectedOptions, Action<int[]> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel");

		Task<int[]> ChooseMultipleAsync(string message, string[] options, int[] selectedOptions, string title = null, string okButton = "OK", string cancelButton = "Cancel");
	}
}

