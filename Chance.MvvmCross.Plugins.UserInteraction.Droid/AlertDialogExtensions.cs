using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;

namespace Android.App
{
    internal static class AlertDialogExtensions
    {
        private sealed class DialogInterfaceOnDismissListenerImpl : Java.Lang.Object, IDialogInterfaceOnDismissListener
        {
            private readonly Action<IDialogInterface> _dismissAction;

            public DialogInterfaceOnDismissListenerImpl(Action<IDialogInterface> dismissAction)
            {
                this._dismissAction = dismissAction;
            }

            public void OnDismiss(IDialogInterface dialog)
            {
                this._dismissAction(dialog);
            }
        }

        public static void SetOnDismissListener(this Dialog dialog, Action<IDialogInterface> dismissAction)
        {
            dialog.SetOnDismissListener(new DialogInterfaceOnDismissListenerImpl(dismissAction));
        }

        public static void SafeDismiss(this Dialog dialog)
        {
            try
            {
                dialog.Dismiss();
            }
            catch   //catch 'em all. No good style but inevitable I think ?
            {
            }
        }
    }
}