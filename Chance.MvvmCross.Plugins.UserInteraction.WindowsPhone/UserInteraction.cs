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




        public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null,
            string okButton = "OK", string cancelButton = "Cancel")
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

        public void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            var result = InputAsync(message, placeholder, title, okButton, cancelButton);
            result.ContinueWith(value => answer(value.Result.Ok, value.Result.Text));
        }

        public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            var textBox = new PhoneTextBox {Hint = placeholder};

            var box = new Microsoft.Phone.Controls.CustomMessageBox()
            {
                Caption = title,
                Message = message,
                LeftButtonContent = okButton,
                RightButtonContent = cancelButton,
                Content = textBox
            };

            var response = new TaskCompletionSource<InputResponse>();
            box.Dismissed+= (sender, args) => response.TrySetResult(new InputResponse()
            {
                Ok = args.Result == CustomMessageBoxResult.LeftButton,
                Text = textBox.Text
            });
            box.Show();
            return response.Task;
        }



        public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            throw new NotImplementedException();

            var result =ConfirmThreeButtonsAsync(message, title, positive, negative, neutral);
            result.ContinueWith((value)=> answer(value.Result));
        }

        public async Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            throw new NotImplementedException();

            //This crashes as CustomMessageBox won't accept more than 2 buttons :-(
            //This was uising the coding 4 fun toolkit
            //int result = await CustomMessageBox.ShowAsync(title, message, 0, CustomMessageBoxIcon.None, positive, neutral,negative);
            //return (ConfirmThreeButtonsResponse) result;

        }
    }
}
