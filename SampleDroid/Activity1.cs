using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Chance.MvvmCross.Plugins.UserInteraction.Droid;

namespace SampleDroid
{
    [Activity(Label = "SampleDroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += showMessage;
        }

        private void showMessage(object sender, EventArgs e)
        {
           UserInteraction interaction = new UserInteraction();
            interaction.Alert("test");
        }
    }
}

