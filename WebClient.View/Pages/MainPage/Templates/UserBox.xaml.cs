using Common.Responses;

namespace WebClient.View.Pages.MainPage.Templates;

public partial class UserBox : ContentView
{
	public readonly static BindableProperty UsernameProperty = GetProp<string>(nameof(Username));
	public readonly static BindableProperty AvatarSourceProperty = GetProp<string>(nameof(AvatarSource),"default_user.png");
	public readonly static BindableProperty UserIdProperty = GetProp<long>(nameof(UserId));
	public event EventHandler Clicked { add => btn.Clicked += value; remove => btn.Clicked -= value; }
	public string Username
	{ 
		get => (string)GetValue(UsernameProperty);
		set => SetValue(UsernameProperty, value);
	}
	public long UserId 
	{
        get => (long)GetValue(UserIdProperty);
        set => SetValue(UserIdProperty, value);
    }
	public string AvatarSource
	{ 
		get=>(string)GetValue(AvatarSourceProperty); 
		set=>SetValue(AvatarSourceProperty,value); 
	}
	public string RowOfGrid { get; set; }
	public UserBox()
	{
		InitializeComponent();
	}
	static BindableProperty GetProp<T>(string propertyName,T defaultValue=default)
	{
		
		return BindableProperty.Create
        (
            propertyName: propertyName,
            returnType: typeof(T),
            declaringType: typeof(UserBox),
			defaultValue:defaultValue
        );
    }
	
}