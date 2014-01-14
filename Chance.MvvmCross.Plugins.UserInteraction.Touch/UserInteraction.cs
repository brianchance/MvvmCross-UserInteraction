using System;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace Chance.MvvmCross.Plugins.UserInteraction.Touch
{
	public class UserInteraction : IUserInteraction
	{
        public void Confirm(string message, Action okClicked = null, string title = "", string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			Confirm(message, confirmed =>
			{
				if (confirmed)
					okClicked();
			},
			title, okButton, cancelButton);
		}

        public void Confirm(string message, Action<bool> answer = null, string title = "", string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var confirm = new UIAlertView(title ?? string.Empty, message,
											  null, cancelButton, okButton);
				if (answer != null)
				{
					confirm.Clicked +=
						(sender, args) =>
							answer(confirm.CancelButtonIndex != args.ButtonIndex);
				}
				confirm.Show();
			});
		}

        public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var tcs = new TaskCompletionSource<bool>();
			Confirm(message, tcs.SetResult, title, okButton, cancelButton);
			return tcs.Task;
		}

        public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer = null, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe", TimeSpan? duration = null)
		{
			var confirm = new UIAlertView(title ?? string.Empty, message, null, negative, positive, neutral);
			if (answer != null)
			{
				confirm.Clicked +=
					(sender, args) =>
					{
						var buttonIndex = args.ButtonIndex;
						if (buttonIndex == confirm.CancelButtonIndex)
							answer(ConfirmThreeButtonsResponse.Negative);
						else if (buttonIndex == confirm.FirstOtherButtonIndex)
							answer(ConfirmThreeButtonsResponse.Positive);
						else
							answer(ConfirmThreeButtonsResponse.Neutral);
					};
				confirm.Show();
			}
		}

        public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe", TimeSpan? duration = null)
		{

			var tcs = new TaskCompletionSource<ConfirmThreeButtonsResponse>();
			ConfirmThreeButtons(message, tcs.SetResult, title, positive, negative, neutral);
			return tcs.Task;
		}

        public void Alert(string message, Action done = null, string title = "", string okButton = "OK", TimeSpan? duration = null)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var alert = new UIAlertView(title ?? string.Empty, message, null, okButton);
				if (done != null)
				{
					alert.Clicked += (sender, args) => done();
				}
				alert.Show();
			});

		}

        public Task AlertAsync(string message, string title = "", string okButton = "OK", TimeSpan? duration = null)
		{
			var tcs = new TaskCompletionSource<object>();
			Alert(message, () => tcs.SetResult(null), title, okButton);
			return tcs.Task;
		}

		public void Input(string message, Action<string> okClicked = null, string placeholder = null, string title = null, string okButton = "OK",
                        string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			Input(message, (ok, text) =>
			{
				if (ok)
					okClicked(text);
			},
			placeholder, title, okButton, cancelButton);
		}

        public void Input(string message, Action<bool, string> answer = null, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				var input = new UIAlertView(title ?? string.Empty, message, null, cancelButton, okButton);
				input.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
				var textField = input.GetTextField(0);
				textField.Placeholder = placeholder;
				if (answer != null)
				{
					input.Clicked +=
						(sender, args) =>
							answer(input.CancelButtonIndex != args.ButtonIndex, textField.Text);
				}
				input.Show();
			});
		}

        public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var tcs = new TaskCompletionSource<InputResponse>();
			Input(message, (ok, text) => tcs.SetResult(new InputResponse {Ok = ok, Text = text}),	placeholder, title, okButton, cancelButton);
			return tcs.Task;
		}

        public void ChooseSingle(string message, string[] options, int? chosenItem = null, Action<int?> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			throw new NotImplementedException();
		}

        public Task<int?> ChooseSingleAsync(string message, string[] options, int? chosenItem = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			throw new NotImplementedException();
		}

        public void ChooseMultiple(string message, string[] options, int[] selectedOptions, Action<int[]> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			throw new NotImplementedException();
		}

        public Task<int[]> ChooseMultipleAsync(string message, string[] options, int[] selectedOptions, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			throw new NotImplementedException();
		}
	}
}

