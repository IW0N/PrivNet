using WebClient.LocalDb.Entities.UserEnvironment;

namespace WebClient.View.Pages;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}
	private async void SignUp(object sender,EventArgs e)
	{
		string uText=username.Text;
		await LocalUser.SignUp(uText);
		await Navigation.PopAsync();
	}
}