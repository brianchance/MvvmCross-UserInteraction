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
				this.CustomizeAndShow(new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(okButton, delegate {
							if (answer != null)
								answer(true);
						})
						.SetNegativeButton(cancelButton, delegate {	
							if (answer != null)
								answer(false);
						}));
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
				this.CustomizeAndShow(new AlertDialog.Builder(CurrentActivity)
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
						}));
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
				this.CustomizeAndShow(new AlertDialog.Builder(CurrentActivity)
					.SetMessage(message)
						.SetTitle(title)
						.SetPositiveButton(okButton, delegate {
							if (done != null)
								done();
						}));
			}, null);
		}

		public Task AlertAsync(string message, string title = "", string okButton = "OK")
		{
			var tcs = new TaskCompletionSource<object>();
			Alert(message, () => tcs.SetResult(null), title, okButton);
			return tcs.Task;
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
				this.CustomizeAndShow(new AlertDialog.Builder(CurrentActivity)
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
						}));
			}, null);
		}

		public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel")
		{
			var tcs = new TaskCompletionSource<InputResponse>();
			Input(message, (ok, text) => tcs.SetResult(new InputResponse {Ok = ok, Text = text}),	placeholder, title, okButton, cancelButton);
			return tcs.Task;
		}

        public void ChooseSingle(string message, string[] options, int? chosenItem, Action<int?> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
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

                this.CustomizeAndShow(new AlertDialog.Builder(this.CurrentActivity)
                    .SetMessage(message)
                    .SetTitle(title)
                    .SetView(scrollView)
                    .SetPositiveButton(okButton, delegate
                    {
                        if (answer != null)
                            answer((radioGroup.CheckedRadioButtonId > 0) ? (radioGroup.CheckedRadioButtonId - 1) : ((int?)null));
                    })
                    .SetNegativeButton(cancelButton, delegate
                    {
                        if (answer != null)
                            answer(null);
                    }));
            }, null);
        }

        public Task<int?> ChooseSingleAsync(string message, string[] options, int? chosenItem, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            var tcs = new TaskCompletionSource<int?>();
            this.ChooseSingle(message, options, chosenItem, tcs.SetResult, title, okButton, cancelButton);

            return tcs.Task;
        }

        public void ChooseMultiple(string message, string[] options, int[] selectedOptions, Action<int[]> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            Application.SynchronizationContext.Post(ignored =>
            {
                if (this.CurrentActivity == null)
                    return;

                CheckBox[] checkBoxes = null;

                checkBoxes = options
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

                this.CustomizeAndShow(new AlertDialog.Builder(this.CurrentActivity)
                    .SetMessage(message)
                    .SetTitle(title)
                    .SetView(scrollView)
                    .SetPositiveButton(okButton, delegate
                    {
                        if (answer != null)
                            answer(options.Select((x, i) => ((checkBoxes[i].Checked) ? (i) : (-1))).Where(x => x != -1).ToArray());
                    })
                    .SetNegativeButton(cancelButton, delegate
                    {
                        if (answer != null)
                            answer(new int[0]);
                    }));
            }, null);
        }

        public Task<int[]> ChooseMultipleAsync(string message, string[] options, int[] selectedOptions, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            var tcs = new TaskCompletionSource<int[]>();
            this.ChooseMultiple(message, options, selectedOptions, tcs.SetResult, title, okButton, cancelButton);

            return tcs.Task;
        }

		private void CustomizeAndShow(AlertDialog.Builder dialogBuilder)
		{
			IAlertDialogBuilderCustomizer customizer;
			if (Mvx.TryResolve(out customizer))
				customizer.Customize(dialogBuilder);

			dialogBuilder.Show();
		}
	}
}

