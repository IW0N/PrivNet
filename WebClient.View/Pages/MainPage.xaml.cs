namespace WebClient.View;
using LocalDb;
using Microsoft.Maui.Controls;
using System.Linq;
using WebClient.LocalDb.Entities.UserEnvironment;
public partial class MainPage : ContentPage
{
    const string userIconPath = "default_user.png";
    const string transparentColor = "#00000000";
    bool visualizeUsers = false;
    
    public MainPage()
    {
        
        InitializeComponent();
    }
    private IEnumerable<string> GetIncludedUsers() =>
        localUsers.Select(view => view is Label label ? label.Text : null).
        Where(userName => userName != null);
    private void OnAvatarClicked(object sender, EventArgs eargs)
    {
        var users = LocalUser.GetLocalUsers();
        VisualizeUsersChosen(users);
    }
    private void AddLocalUserToMenu(string user)
    {
        ImageButton imageButton = new()
        {
            Source = userIconPath,
            BackgroundColor = Color.FromArgb(transparentColor),
            HeightRequest = 50
        };
        Label userName = new() { Text = user, HorizontalOptions = LayoutOptions.Center };
        localUsers.Add(imageButton);
        localUsers.Add(userName);
    }
    private void VisualizeUsersChosen(IEnumerable<string> users)
    {
        visualizeUsers = !visualizeUsers;
        menuUsersView.IsVisible = visualizeUsers; 
        if (visualizeUsers)
        {
            var userNames = GetIncludedUsers();
            foreach (var user in users)
                if (!userNames.Contains(user))
                    AddLocalUserToMenu(user);
        }
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Pages.SignUpPage());
    }
}