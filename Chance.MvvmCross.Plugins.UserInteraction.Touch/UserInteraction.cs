using System;
using UIKit;
using System.Threading.Tasks;

namespace Chance.MvvmCross.Plugins.UserInteraction.Touch
{
	public class UserInteraction : IUserInteraction
	{
		public void Confirm(string message, Action okClicked, string title = "", string okButton = "OK", string cancelButton = "Cancel")
		{
			Confirm(message, confirmed =>
			{
				if (confirmed)
					okClicked();
			},
			title, okButton, cancelButton);
		}

		public void Confirm(string message, Action<bool> answer, string title = "", string okButton = "OK", string cancelButton = "Cancel")
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
                var confirm = UIAlertController.Create(title ?? string.Empty, message, UIAlertControllerStyle.Alert);
                confirm.AddAction(UIAlertAction.Create(okButton, UIAlertActionStyle.Default, alert => {
                    answer(true);
                }));
                confirm.AddAction(UIAlertAction.Create(cancelButton, UIAlertActionStyle.Cancel, alert => {
                    answer(false);
                }));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(confirm, true, null);
			});
		}

		public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
		{
			var tcs = new TaskCompletionSource<bool>();
			Confirm(message, (r) => tcs.TrySetResult(r), title, okButton, cancelButton);
			return tcs.Task;
        }

        public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var confirm = UIAlertController.Create(title ?? string.Empty, message, UIAlertControllerStyle.Alert);
                confirm.AddAction(UIAlertAction.Create(positive, UIAlertActionStyle.Default, alert => {
                    answer(ConfirmThreeButtonsResponse.Positive);
                }));
                confirm.AddAction(UIAlertAction.Create(neutral, UIAlertActionStyle.Default, alert =>
                {
                    answer(ConfirmThreeButtonsResponse.Neutral);
                }));
                confirm.AddAction(UIAlertAction.Create(negative, UIAlertActionStyle.Cancel, alert => {
                    answer(ConfirmThreeButtonsResponse.Negative);
                }));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(confirm, true, null);
            });
        }

        public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            var tcs = new TaskCompletionSource<ConfirmThreeButtonsResponse>();
			ConfirmThreeButtons(message, (r) => tcs.TrySetResult(r), title, positive, negative, neutral);
            return tcs.Task;
        }

		public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
                var alert = UIAlertController.Create(title ?? string.Empty, message, UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create(okButton, UIAlertActionStyle.Default, x => {
                    done?.Invoke();
                }));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
            });
		}

		public Task AlertAsync(string message, string title = "", string okButton = "OK")
		{
			var tcs = new TaskCompletionSource<object>();
			Alert(message, () => tcs.TrySetResult(null), title, okButton);
			return tcs.Task;
        }

		public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			Input(message, (ok, text) =>
			{
				if (ok)
					okClicked(text);
			},
				placeholder, title, okButton, cancelButton, initialText);
		}

		public void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
                var confirm = UIAlertController.Create(title ?? string.Empty, message, UIAlertControllerStyle.Alert);
                confirm.AddTextField(textField => {
                    textField.Placeholder = placeholder;
                    textField.Text = initialText;
                });
                confirm.AddAction(UIAlertAction.Create(okButton, UIAlertActionStyle.Default, alert => {
                    answer(true, confirm.TextFields[0].Text);
                }));
                confirm.AddAction(UIAlertAction.Create(cancelButton, UIAlertActionStyle.Cancel, alert => {
                    answer(false, confirm.TextFields[0].Text);
                }));
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(confirm, true, null);
			});
		}

		public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			var tcs = new TaskCompletionSource<InputResponse>();
			Input(message, (ok, text) => tcs.TrySetResult(new InputResponse {Ok = ok, Text = text}),	placeholder, title, okButton, cancelButton, initialText);
			return tcs.Task;
		}
	}
}

