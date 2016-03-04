using System;
using Android.App;
using Android.Content;
using Android.Widget;
using System.Threading.Tasks;
//using AlertDialog = Android.Support.V7.App.AlertDialog;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace Chance.MvvmCross.Plugins.UserInteraction.Droid
{
	public class UserInteraction : IUserInteraction
	{
		protected Activity CurrentActivity {
			get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
		}

		public void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel")
		{
			Confirm(message, confirmed => {
				if (confirmed)
					okClicked();
			},
			title, okButton, cancelButton);
		}

		public void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel")
		{
			//Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction();
			Application.SynchronizationContext.Post(ignored => {
				if (CurrentActivity == null) return;
				try{
					new Android.Support.V7.App.AlertDialog.Builder(CurrentActivity)
						.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(okButton, delegate {
							if (answer != null)
								answer(true);
						})
						.SetNegativeButton(cancelButton, delegate {	
							if (answer != null)
								answer(false);
						})
						.Show();
				}
				catch(Exception ex)
				{
					new AlertDialog.Builder(CurrentActivity)
						.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(okButton, delegate {
							if (answer != null)
								answer(true);
						})
						.SetNegativeButton(cancelButton, delegate {	
							if (answer != null)
								answer(false);
						})
						.Show();
				}

			}, null);
		}

		public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
		{
			var tcs = new TaskCompletionSource<bool>();
			Confirm(message, tcs.SetResult, title, okButton, cancelButton);
			return tcs.Task;
		}

	    public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No",
	        string neutral = "Maybe")
	    {
	        Application.SynchronizationContext.Post(ignored =>
            {
                if (CurrentActivity == null) return;
				try{
					new Android.Support.V7.App.AlertDialog.Builder(CurrentActivity)
						.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(positive, delegate {
							if (answer != null)
								answer(ConfirmThreeButtonsResponse.Positive);
						})
						.SetNegativeButton(negative, delegate {
							if (answer != null)
								answer(ConfirmThreeButtonsResponse.Negative);
						})
						.SetNeutralButton(neutral, delegate {
							if (answer != null)
								answer(ConfirmThreeButtonsResponse.Neutral);
						})
						.Show();
				}
				catch(Exception ex){
					new AlertDialog.Builder(CurrentActivity)
						.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(positive, delegate {
							if (answer != null)
								answer(ConfirmThreeButtonsResponse.Positive);
						})
						.SetNegativeButton(negative, delegate {
							if (answer != null)
								answer(ConfirmThreeButtonsResponse.Negative);
						})
						.SetNeutralButton(neutral, delegate {
							if (answer != null)
								answer(ConfirmThreeButtonsResponse.Neutral);
						})
						.Show();
				}
            }, null);
	    }

        public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No",
            string neutral = "Maybe")
	    {
	        var tcs = new TaskCompletionSource<ConfirmThreeButtonsResponse>();
	        ConfirmThreeButtons(message, tcs.SetResult, title, positive, negative, neutral);
	        return tcs.Task;
	    }

		public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
		{
			Application.SynchronizationContext.Post(ignored => {
				if (CurrentActivity == null) return;
				try{
					new Android.Support.V7.App.AlertDialog.Builder(CurrentActivity)
						.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(okButton, delegate {
							if (done != null)
								done();
						})
						.Show();
				}
				catch(Exception ex){
					new AlertDialog.Builder(CurrentActivity)
						.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(okButton, delegate {
							if (done != null)
								done();
						})
						.Show();
				}

			}, null);
		}

		public Task AlertAsync(string message, string title = "", string okButton = "OK")
		{
			var tcs = new TaskCompletionSource<object>();
			Alert(message, () => tcs.SetResult(null), title, okButton);
			return tcs.Task;
		}

		public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			Input(message, (ok, text) => {
				if (ok)
					okClicked(text);
			},
				placeholder, title, okButton, cancelButton, initialText);
		}

		public void Input(string message, Action<bool, string> answer, string hint = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			Application.SynchronizationContext.Post(ignored => {
				if (CurrentActivity == null) return;
				var input = new EditText(CurrentActivity) { Hint = hint, Text = initialText };

				try{
					new Android.Support.V7.App.AlertDialog.Builder(CurrentActivity)
						.SetMessage(message)
						.SetTitle(title)
						.SetView(input)
						.SetPositiveButton(okButton, delegate {
							if (answer != null)
								answer(true, input.Text);
						})
						.SetNegativeButton(cancelButton, delegate {	
							if (answer != null)
								answer(false, input.Text);
						})
						.Show();
				}
				catch(Exception ex)
				{
					new AlertDialog.Builder(CurrentActivity)
						.SetMessage(message)
						.SetTitle(title)
						.SetView(input)
						.SetPositiveButton(okButton, delegate {
							if (answer != null)
								answer(true, input.Text);
						})
						.SetNegativeButton(cancelButton, delegate {	
							if (answer != null)
								answer(false, input.Text);
						})
						.Show();
				}

			}, null);
		}

		public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
		{
			var tcs = new TaskCompletionSource<InputResponse>();
			Input(message, (ok, text) => tcs.SetResult(new InputResponse {Ok = ok, Text = text}),	placeholder, title, okButton, cancelButton, initialText);
			return tcs.Task;
		}
	}
}

