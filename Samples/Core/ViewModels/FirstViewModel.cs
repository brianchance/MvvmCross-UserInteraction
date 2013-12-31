using System.Windows.Input;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace Core.ViewModels
{
    public class FirstViewModel 
		: MvxViewModel
    {
        private readonly IUserInteraction _userInteraction;

        public FirstViewModel(IUserInteraction userInteraction)
        {
            _userInteraction = userInteraction;
        }

        public ICommand ConfirmCommand
        {
            get
            {
                return
                    new MvxCommand(
                        () => _userInteraction.Confirm("The message", () => Mvx.Trace("Ok pressed"), "The title",
                            "OK button", "Cancel Button"));
            }
        }
        public ICommand AlertCommand
        {
            get
            {
                return
                    new MvxCommand(
                        () => _userInteraction.Alert("The message", () => Mvx.Trace("Done"), "The title",
                            "OK button"));
            }
        }

        public ICommand InputCommand
        {
            get
            {
                return
                    new MvxCommand(
                        () => _userInteraction.Input("The message", (result) => Mvx.Trace(result + " returned"), "place holder text","The title",
                            "OK button","Cancel button"));
            }
        }

        public ICommand ConfirmThreeButtonsCommand
        {
            get
            {
                return
                    new MvxCommand(
                        () => _userInteraction.ConfirmThreeButtons("The message", (result) => Mvx.Trace(result + " returned"), "place holder text", "positive",
                            "negative", "neutral"));
            }
        }

    }
}
