using Android.App;

namespace Chance.MvvmCross.Plugins.UserInteraction.Droid
{
    public interface IAlertDialogBuilderCustomizer
    {
        void Customize(AlertDialog.Builder dialogBuilder);
    }
}