using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Popups;
using WinRTXamlToolkit.Controls;

namespace Chance.MvvmCross.Plugins.UserInteraction.WindowsPhoneStore
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

        public async Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
        {
            var result = false;
            var box = new MessageDialog(message, title);

            box.Commands.Add(new UICommand(okButton, delegate { result = true; }));
            box.Commands.Add(new UICommand(cancelButton, delegate { result = false; }));

            await box.ShowAsync();

            return result;
        }


        public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
        {

            AlertAsync(message, title, okButton).ContinueWith((button) => { if (done != null) done(); });
        }

        public async Task AlertAsync(string message, string title = "", string okButton = "OK")
        {
            var box = new MessageDialog(message, title);
            
            box.Commands.Add(new UICommand(okButton));

            await box.ShowAsync();
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

       public async Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK",
            string cancelButton = "Cancel", string initialText = null)
       {
           var box = new InputDialog { InputText = initialText ?? string.Empty };
           var result = await box.ShowAsync(title ?? string.Empty, message, okButton, cancelButton);
           return new InputResponse() {Text = box.InputText, Ok = result == okButton};
       }


        public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {

            var result = ConfirmThreeButtonsAsync(message, title, positive, negative, neutral);
            result.ContinueWith((value) => answer(value.Result));
        }

        public async Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {

            ConfirmThreeButtonsResponse result = ConfirmThreeButtonsResponse.Neutral;
            var box = new MessageDialog(message, title ?? string.Empty);

            box.Commands.Add(new UICommand(positive, delegate { result = ConfirmThreeButtonsResponse.Positive; }));
            box.Commands.Add(new UICommand(neutral, delegate { result = ConfirmThreeButtonsResponse.Neutral; }));
            box.Commands.Add(new UICommand(negative, delegate { result = ConfirmThreeButtonsResponse.Negative; }));

            await box.ShowAsync();

            return result;
        }
    }
}