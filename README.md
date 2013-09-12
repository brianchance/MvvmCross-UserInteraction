MvvmCross-UserInteraction Plugin
================================

MvvmCross plugin for interacting with the user from a view model. 
##Features
1. Alert - simple alert to the user, optional callback when done
2. Confirm - dialog with ok/cancel, callback with button clicked or just when ok clicked
3. Input - asks user for input with ok/cancel, callback with button clicked and data or just data when ok clicked

##Usage
####Alert
```
public ICommand SubmitCommand
{
		get
		{
			return new MvxCommand(() =>
					                      {
					                        if (string.IsNullOrEmpty(FirstName)) 
					                        {
					                          Mvx.Resolve<IUserInteraction>().Alert("First Name is Required");
					                          return;
					                        }
					                        //do work
					                      });
		}
}
```

####Confirm/Input
```
public ICommand SubmitCommand
{
		get
		{
			return new MvxCommand(() =>
					                      {
					                        Mvx.Resolve<IUserInteraction>().Confirm("Are you sure?", async () => 
					                          {
					                            //Do work
					                          });
					                      });
		}
}
```

##Adding to your project
1. Follow stuarts directions (step 3) - http://slodge.blogspot.com/2012/10/build-new-plugin-for-mvvmcrosss.html
2. Grab the UserInteractionPluginBootstrap file from appropriate Droid/Touch folder. Drop into your project in the Bootstrap folder, change the namespace.

I will be working on a nuget package as time permits.
