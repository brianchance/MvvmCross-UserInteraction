using System;
using Android.App;
using Cirrious.CrossCore;
using Android.Widget;
using Cirrious.CrossCore.Droid.Platform;

namespace Chance.MvvmCross.Plugins.UserInteraction.Droid
{
	public class UserInteraction : IUserInteraction
	{
		protected Activity CurrentActivity
		{
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
			}, null);
		}

		public void Alert(string message, Action done = null, string title = "")
		{
			Application.SynchronizationContext.Post(ignored => {
				if (CurrentActivity == null) return;
				new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton("OK", delegate {
							if (done != null)
								done();
						})
						.Show();
			}, null);
		}

		public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel")
		{
			Input(message, (ok, text) => {
				if (ok)
					okClicked(text);
			},
			placeholder, title, okButton, cancelButton);
		}

		public void Input(string message, Action<bool, string> answer, string hint = null, string title = null, string okButton = "OK", string cancelButton = "Cancel")
		{
			Application.SynchronizationContext.Post(ignored => {
				if (CurrentActivity == null) return;
				var input = new EditText(CurrentActivity) { Hint = hint };

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
			}, null);
		}
	}
}

