MvvmCross-UserInteraction Plugin
================================

MvvmCross plugin for interacting with the user from a view model. Currently supports
1. Alert - shows a message with an OK button, optional callback when button is clicked
2. Confirm - confirmation dialog with ok and cancel, callbacks for which button was clicked OR just if ok was clicked
3. Input - requests input from the user with ok and cancel, callback for which button was clicked and text or just if ok was clicked with text

##Usage from a view model:
Alert:

```
public ICommand SubmitCommand
{
	get 
	{
		return new MvxCommand(async () => {
			if (string.IsNullOrEmpty(FirstName))
			{
				Mvx.Resolve<IUserInteraction>().Alert("First Name is required");
				return;
			}
			//do work
		}
	}
}
```

Confirm:
```
public ICommand SubmitCommand
{
	get 
	{
		return new MvxCommand(async () => {
			Mvx.Resolve<IUserInteraction>().Confirm("Are you sure?",
			async () => {
				//do work			 
			});
		}
	}
}
```

Adding to your project.
1. Follow Stuart's guide here, start at step 3: http://slodge.blogspot.com/2012/10/build-new-plugin-for-mvvmcrosss.html
2. Or, grab the UserInteractionPluginBootstrap.cs file from the appropriate project and drop into your Bootstrap folder, then change the namespace.

I will be working on a nuget package as time permits.