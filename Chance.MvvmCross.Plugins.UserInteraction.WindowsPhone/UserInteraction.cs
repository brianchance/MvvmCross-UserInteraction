using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace Chance.MvvmCross.Plugins.UserInteraction.WindowsPhone
{
    public class UserInteraction : IUserInteraction
    {
        public void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            Task<bool> result = ConfirmAsync(message, title, okButton, cancelButton);


            result.ContinueWith((button) =>
            {
                if (button.Result)
                {
                    //ok clicked
                    okClicked();
                }
            });
        }

        public void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            Task<bool> result = ConfirmAsync(message, title, okButton, cancelButton);

            result.ContinueWith((button) => answer(button.Result));
        }

        public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
        {
            var box = new Microsoft.Phone.Controls.CustomMessageBox()
            {
                Caption = title,
                Message = message,
                LeftButtonContent = okButton,
                RightButtonContent = cancelButton
            };
            var complete = new TaskCompletionSource<bool>();
            box.Dismissed += (sender, args) => complete.TrySetResult(args.Result == CustomMessageBoxResult.LeftButton);
            box.Show();
            return complete.Task;
        }






        public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
        {

            AlertAsync(message, title, okButton).ContinueWith((button) => { if (done != null) done(); });
        }

        public Task AlertAsync(string message, string title = "", string okButton = "OK")
        {
            var box = new Microsoft.Phone.Controls.CustomMessageBox()
            {
                Caption = title,
                Message = message,
                LeftButtonContent = okButton,
                IsRightButtonEnabled = false
            };
            var complete = new TaskCompletionSource<bool>();
            box.Dismissed += (sender, args) => complete.TrySetResult(true);
            box.Show();
            return complete.Task;
        }


        public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK",
           string cancelButton = "Cancel", string initialText = null)
        {
            var result = InputAsync(message, placeholder, title, okButton, cancelButton);
            result.ContinueWith((value) =>
            {
                if (value.Result.Ok)
                {
                    okClicked(value.Result.Text);
                }
            });
        }
        
        public void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK",
            string cancelButton = "Cancel", string initialText = null)
        {
            var result = InputAsync(message, placeholder, title, okButton, cancelButton);
            result.ContinueWith(value => answer(value.Result.Ok, value.Result.Text));
        }

       public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK",
            string cancelButton = "Cancel", string initialText = null)
        {
            var textBox = new PhoneTextBox { Hint = placeholder };

            var box = new Microsoft.Phone.Controls.CustomMessageBox()
            {
                Caption = title,
                Message = message,
                LeftButtonContent = okButton,
                RightButtonContent = cancelButton,
                Content = textBox
            };

            var response = new TaskCompletionSource<InputResponse>();
            box.Dismissed += (sender, args) => response.TrySetResult(new InputResponse()
            {
                Ok = args.Result == CustomMessageBoxResult.LeftButton,
                Text = textBox.Text
            });
            box.Show();
            return response.Task;
        }


        public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {

            var result = ConfirmThreeButtonsAsync(message, title, positive, negative, neutral);
            result.ContinueWith((value) => answer(value.Result));
        }

        public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            StackPanel contents = new StackPanel();
            contents.Orientation = Orientation.Vertical;
            var positiveButton = new Button() { Content = positive };
            var neutralButton = new Button() { Content = neutral };
            var negativeButton = new Button() { Content = negative };
            contents.Children.Add(positiveButton);
            contents.Children.Add(neutralButton);
            contents.Children.Add(negativeButton);

            var box = new Microsoft.Phone.Controls.CustomMessageBox()
            {
                Caption = title,
                Message = message,
                IsLeftButtonEnabled = false,
                IsRightButtonEnabled = false,
                Content = contents
            };

            var response = new TaskCompletionSource<ConfirmThreeButtonsResponse>();
            positiveButton.Click += (sender, args) =>
            {
                response.TrySetResult(ConfirmThreeButtonsResponse.Positive);
                box.Dismiss();
            };
            neutralButton.Click += (sender, args) =>
            {
                response.TrySetResult(ConfirmThreeButtonsResponse.Neutral);
                box.Dismiss();
            };
            negativeButton.Click += (sender, args) =>
            {
                response.TrySetResult(ConfirmThreeButtonsResponse.Negative);
                box.Dismiss();
            };
            box.Show();
            return response.Task;


        }
    }
}