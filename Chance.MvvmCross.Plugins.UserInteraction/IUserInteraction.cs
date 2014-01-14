using System;
using System.Threading.Tasks;

namespace Chance.MvvmCross.Plugins.UserInteraction
{
	public interface IUserInteraction
	{
		void Confirm(string message, Action okClicked = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);
        void Confirm(string message, Action<bool> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);

        void Alert(string message, Action done = null, string title = "", string okButton = "OK", TimeSpan? duration = null);

        void Input(string message, Action<string> okClicked = null, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);
        void Input(string message, Action<bool, string> answer = null, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);

        Task AlertAsync(string message, string title = "", string okButton = "OK", TimeSpan? duration = null);
        Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);
        Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);

		void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer = null, string title = null, string positive = "Yes", string negative = "No",
            string neutral = "Maybe", TimeSpan? duration = null);

		Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No",
            string neutral = "Maybe", TimeSpan? duration = null);

        void ChooseSingle(string message, string[] options, int? chosenItem = null, Action<int?> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);

        Task<int?> ChooseSingleAsync(string message, string[] options, int? chosenItem = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);

        void ChooseMultiple(string message, string[] options, int[] selectedOptions, Action<int[]> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);

        Task<int[]> ChooseMultipleAsync(string message, string[] options, int[] selectedOptions, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null);
	}
}

