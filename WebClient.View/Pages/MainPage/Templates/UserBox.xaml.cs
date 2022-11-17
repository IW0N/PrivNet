using Common.Responses;
namespace WebClient.View.Pages.MainPage.Templates;
using static TemplateContext<UserBox>;
public partial class UserBox : ContentView
{
	public readonly static BindableProperty UsernameProperty = GetProp<string>(nameof(Username));
	public readonly static BindableProperty AvatarSourceProperty = GetProp<string>(nameof(AvatarSource),"default_user.png");
	public readonly static BindableProperty UserIdProperty = GetProp<long>(nameof(UserId));
	public event EventHandler Clicked { add => button.Clicked += value; remove => button.Clicked -= value; }
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
    public static void BuildBoxesByResponses(IEnumerable<GetUserResponse> users, VerticalStackLayout interactiveStack)
    {
		interactiveStack.Clear();
        foreach (var user in users)
        {
           
            var userBox = new UserBox
            {
                UserId = user.Id,
                Username = user.Nickname, 
            };
            interactiveStack.Add(userBox);
        }
    }
}