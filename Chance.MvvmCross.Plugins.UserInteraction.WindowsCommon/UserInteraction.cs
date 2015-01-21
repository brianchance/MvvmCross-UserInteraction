using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Popups;

namespace Chance.MvvmCross.Plugins.UserInteraction.WindowsCommon
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
            var result = ConfirmAsync(message, title, okButton, cancelButton);
            result.ContinueWith((button) => answer(button.Result));
        }

        public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
        {
            var complete = new TaskCompletionSource<bool>();
            var dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand(okButton, command => complete.TrySetResult(true)));
            dialog.Commands.Add(new UICommand(cancelButton, command => complete.TrySetResult(false)));
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            dialog.ShowAsync();
            return complete.Task;
        }

        public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
        {

            AlertAsync(message, title, okButton).ContinueWith((button) => { if (done != null) done(); });
        }

        public Task AlertAsync(string message, string title = "", string okButton = "OK")
        {
            var dialog = new MessageDialog(message, title);
            dialog.Commands.Add(new UICommand(okButton));
            dialog.ShowAsync();
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
           
            //var textBox = new PhoneTextBox { Hint = placeholder };

            //var box = new Microsoft.Phone.Controls.CustomMessageBox()
            //{
            //    Caption = title,
            //    Message = message,
            //    LeftButtonContent = okButton,
            //    RightButtonContent = cancelButton,
            //    Content = textBox
            //};

            var response = new TaskCompletionSource<InputResponse>();
            //box.Dismissed += (sender, args) => response.TrySetResult(new InputResponse()
            //{
            //    Ok = args.Result == CustomMessageBoxResult.LeftButton,
            //    Text = textBox.Text
            //});
            //box.Show();
            return response.Task;
        }


        public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {

            var result = ConfirmThreeButtonsAsync(message, title, positive, negative, neutral);
            result.ContinueWith((value) => answer(value.Result));
        }

        public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            var complete = new TaskCompletionSource<ConfirmThreeButtonsResponse>();
            var dialog = new MessageDialog(message, title ?? "");
            dialog.Commands.Add(new UICommand(positive, command => complete.TrySetResult(ConfirmThreeButtonsResponse.Positive)));
            dialog.Commands.Add(new UICommand(neutral, command => complete.TrySetResult(ConfirmThreeButtonsResponse.Neutral)));
            dialog.Commands.Add(new UICommand(negative, command => complete.TrySetResult(ConfirmThreeButtonsResponse.Negative)));
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 2;
            dialog.ShowAsync();
            return complete.Task;
        }
    }
}