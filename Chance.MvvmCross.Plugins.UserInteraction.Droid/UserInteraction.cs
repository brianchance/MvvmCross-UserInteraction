using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Cirrious.CrossCore;
using Android.Widget;
using Cirrious.CrossCore.Droid.Platform;
using System.Threading.Tasks;

namespace Chance.MvvmCross.Plugins.UserInteraction.Droid
{
	public class UserInteraction : IUserInteraction
	{
		protected Activity CurrentActivity {
			get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
		}

		public void Confirm(string message, Action okClicked = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var task = this.ConfirmAsync(message, title, okButton, cancelButton, duration);

			if (okClicked != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => okClicked(), null));
		}

		public void Confirm(string message, Action<bool> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var task = this.ConfirmAsync(message, title, okButton, cancelButton, duration);

			if (answer != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => answer(completedTask.Result), null));
		}

		public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var tcs = new TaskCompletionSource<bool>();

			Application.SynchronizationContext.Post(ignored =>
			{
				if (CurrentActivity == null) return;

				var builder = new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
					.SetTitle(title)
					.SetPositiveButton(okButton, delegate
					{
						tcs.TrySetResult(true);
					})
					.SetNegativeButton(cancelButton, delegate
					{
						tcs.TrySetResult(false);
					});

				var dialog = this.CustomizeAndCreate(builder);
				dialog.Show();

				if (duration.HasValue)
					Task.Delay(duration.Value).ContinueWith((delayTask) => Application.SynchronizationContext.Post(ignored2 => dialog.SafeDismiss(), null));
			}, null);

			return tcs.Task;
		}

		public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer = null, string title = null, string positive = "Yes", string negative = "No",
			string neutral = "Maybe", TimeSpan? duration = null)
		{
			var task = this.ConfirmThreeButtonsAsync(message, title, positive, negative, neutral, duration);

			if (answer != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => answer(completedTask.Result), null));
		}

		public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No",
			string neutral = "Maybe", TimeSpan? duration = null)
		{
			var tcs = new TaskCompletionSource<ConfirmThreeButtonsResponse>();

			Application.SynchronizationContext.Post(ignored =>
			{
				if (CurrentActivity == null) return;

				var builder = new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
					.SetTitle(title)
					.SetPositiveButton(positive, delegate
					{
						tcs.TrySetResult(ConfirmThreeButtonsResponse.Positive);
					})
					.SetNegativeButton(negative, delegate
					{
						tcs.TrySetResult(ConfirmThreeButtonsResponse.Negative);
					})
					.SetNeutralButton(neutral, delegate
					{
						tcs.TrySetResult(ConfirmThreeButtonsResponse.Neutral);
					});

				var dialog = this.CustomizeAndCreate(builder);
				dialog.Show();

				if (duration.HasValue)
					Task.Delay(duration.Value).ContinueWith((delayTask) => Application.SynchronizationContext.Post(ignored2 => dialog.SafeDismiss(), null));
			}, null);

			return tcs.Task;
		}

		public void Alert(string message, Action done = null, string title = "", string okButton = "OK", TimeSpan? duration = null)
		{
			var task = this.AlertAsync(message, title, okButton, duration);

			if (done != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => done(), null));
		}

		public Task AlertAsync(string message, string title = "", string okButton = "OK", TimeSpan? duration = null)
		{
			var tcs = new TaskCompletionSource<object>();

			Application.SynchronizationContext.Post(ignored =>
			{
				if (CurrentActivity == null) return;
				var builder = new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
					.SetTitle(title)
					.SetPositiveButton(okButton, delegate
					{
						tcs.TrySetResult(null);
					});

				var dialog = this.CustomizeAndCreate(builder);
				dialog.Show();

				if (duration.HasValue)
					Task.Delay(duration.Value).ContinueWith((delayTask) => Application.SynchronizationContext.Post(ignored2 => dialog.SafeDismiss(), null));
			}, null);

			return tcs.Task;
		}

		public void InputText(string message, Action<string> okClicked = null, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var task = this.InputTextAsync(message, placeholder, title, okButton, cancelButton, duration);

			if (okClicked != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => okClicked(completedTask.Result.Value), null));
		}

		public void InputText(string message, Action<bool, string> answer = null, string hint = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var task = this.InputTextAsync(message, hint, title, okButton, cancelButton, duration);

			if (answer != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => answer(completedTask.Result.Ok, completedTask.Result.Value), null));
		}

		public Task<InputResponse<string>> InputTextAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			return this.InputAsync(message, placeholder, title, okButton, cancelButton, duration, false);
		}

		public void ChooseSingle(string message, string[] options, int? chosenItem, Action<int?> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var task = this.ChooseSingleAsync(message, options, chosenItem, title, okButton, cancelButton, duration);

			if (answer != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => answer(completedTask.Result), null));
		}

		public Task<int?> ChooseSingleAsync(string message, string[] options, int? chosenItem, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var tcs = new TaskCompletionSource<int?>();

			Application.SynchronizationContext.Post(ignored =>
			{
				if (this.CurrentActivity == null)
					return;

				var radioButtons = options
					.Select((option, i) =>
					{
						var checkBox = new RadioButton(this.CurrentActivity)
						{
							LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
							{
								Gravity = GravityFlags.CenterVertical
							},

							Id = (i + 1),
							Text = option,
							Gravity = GravityFlags.Center,
						};

						checkBox.SetTextColor(Color.White);

						return checkBox;
					})
					.ToArray();

				var radioGroup = new RadioGroup(this.CurrentActivity)
				{
					LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent),
					Orientation = Orientation.Vertical
				};

				foreach (var optionLayout in radioButtons)
				{
					radioGroup.AddView(optionLayout);
				}

				var scrollView = new ScrollView(this.CurrentActivity)
				{
					LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent),
				};

				scrollView.AddView(radioGroup);

				var builder = new AlertDialog.Builder(this.CurrentActivity)
					.SetMessage(message)
					.SetTitle(title)
					.SetView(scrollView)
					.SetPositiveButton(okButton, delegate
					{
						tcs.TrySetResult((radioGroup.CheckedRadioButtonId > 0) ? (radioGroup.CheckedRadioButtonId - 1) : ((int?)null));
					})
					.SetNegativeButton(cancelButton, delegate
					{
						tcs.TrySetResult(null);
					});

				var dialog = this.CustomizeAndCreate(builder);
				dialog.Show();

				if (duration.HasValue)
					Task.Delay(duration.Value).ContinueWith((delayTask) => Application.SynchronizationContext.Post(ignored2 => dialog.SafeDismiss(), null));
			}, null);

			return tcs.Task;
		}

		public void ChooseMultiple(string message, string[] options, int[] selectedOptions, Action<int[]> answer = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var task = this.ChooseMultipleAsync(message, options, selectedOptions, title, okButton, cancelButton, duration);
			
			if (answer != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => answer(completedTask.Result), null));
		}

		public Task<int[]> ChooseMultipleAsync(string message, string[] options, int[] selectedOptions, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var tcs = new TaskCompletionSource<int[]>();

			Application.SynchronizationContext.Post(ignored =>
			{
				if (this.CurrentActivity == null)
					return;

				var checkBoxes = options
					.Select(x =>
					{
						var checkBox = new CheckBox(this.CurrentActivity)
							{
								LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
								{
									Gravity = GravityFlags.CenterVertical
								},

								Gravity = GravityFlags.Center,
							};

						checkBox.SetTextColor(Color.White);

						return checkBox;
					})
					.ToArray();

				var optionLayouts = options
					.Select((option, i) =>
					{
						var optionLayout = new LinearLayout(this.CurrentActivity)
						{
							LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent),
							Orientation = Orientation.Horizontal
						};

						optionLayout.AddView(checkBoxes[i]);
						optionLayout.AddView(new TextView(this.CurrentActivity)
						{
							LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent)
							{
								Gravity = GravityFlags.CenterVertical,
								LeftMargin = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 8, CurrentActivity.Resources.DisplayMetrics)
							},

							Text = option
						});

						return optionLayout;
					})
					.ToArray();

				var linearLayout = new LinearLayout(this.CurrentActivity)
				{
					LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent),
					Orientation = Orientation.Vertical
				};

				foreach (var optionLayout in optionLayouts)
				{
					linearLayout.AddView(optionLayout);
				}

				var scrollView = new ScrollView(this.CurrentActivity)
				{
					LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.FillParent, ViewGroup.LayoutParams.WrapContent),
				};

				scrollView.AddView(linearLayout);

				var builder = new AlertDialog.Builder(this.CurrentActivity)
					.SetMessage(message)
					.SetTitle(title)
					.SetView(scrollView)
					.SetPositiveButton(okButton, delegate
					{
						tcs.TrySetResult(options.Select((x, i) => ((checkBoxes[i].Checked) ? (i) : (-1))).Where(x => x != -1).ToArray());
					})
					.SetNegativeButton(cancelButton, delegate
					{
						tcs.TrySetResult(new int[0]);
					});

				var dialog = this.CustomizeAndCreate(builder);
				dialog.Show();

				if (duration.HasValue)
					Task.Delay(duration.Value).ContinueWith((delayTask) => Application.SynchronizationContext.Post(ignored2 => dialog.SafeDismiss(), null));
			}, null);

			return tcs.Task;
		}

		public void InputNumeric(string message, Action<bool, decimal> answer = null, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var task = this.InputNumericAsync(message, placeholder, title, okButton, cancelButton, duration);

			if (answer != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => answer(completedTask.Result.Ok, completedTask.Result.Value), null));
		}

		public void InputNumeric(string message, Action<decimal> okClicked = null, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var task = this.InputNumericAsync(message, placeholder, title, okButton, cancelButton, duration);

			if (okClicked != null)
				task.ContinueWith((completedTask) => Application.SynchronizationContext.Post(ignored => okClicked(completedTask.Result.Value), null));
		}

		public async Task<InputResponse<decimal>> InputNumericAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", TimeSpan? duration = null)
		{
			var response = await this.InputAsync(message, placeholder, title, okButton, cancelButton, duration, true);
			return (response.Ok && !string.IsNullOrEmpty(response.Value)) ? new InputResponse<decimal> { Ok = true, Value = decimal.Parse(response.Value) } : new InputResponse<decimal> { Ok = false, Value = 0 };
		}

		private Task<InputResponse<string>> InputAsync(string message, string placeholder, string title, string okButton, string cancelButton, TimeSpan? duration, bool numeric)
		{
			var tcs = new TaskCompletionSource<InputResponse<string>>();

			Application.SynchronizationContext.Post(ignored =>
			{
				if (CurrentActivity == null) return;

				var input = new EditText(CurrentActivity) 
				{
					Hint = placeholder,
					InputType = numeric ? (Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal) : Android.Text.InputTypes.ClassText
				};

				var builder = new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
					.SetTitle(title)
					.SetView(input)
					.SetPositiveButton(okButton, delegate
					{
						tcs.TrySetResult(new InputResponse<string> { Ok = true, Value = input.Text });
					})
					.SetNegativeButton(cancelButton, delegate
					{
						tcs.TrySetResult(new InputResponse<string> { Ok = false, Value = input.Text });
					});

				var dialog = this.CustomizeAndCreate(builder);
				dialog.Show();

				if (duration.HasValue)
					Task.Delay(duration.Value).ContinueWith((delayTask) => Application.SynchronizationContext.Post(ignored2 => dialog.SafeDismiss(), null));
			}, null);

			return tcs.Task;
		}

		private AlertDialog CustomizeAndCreate(AlertDialog.Builder dialogBuilder)
		{
			IAlertDialogBuilderCustomizer customizer;
			if (Mvx.TryResolve(out customizer))
				customizer.Customize(dialogBuilder);

			return dialogBuilder.Create();
		}
	}
}

