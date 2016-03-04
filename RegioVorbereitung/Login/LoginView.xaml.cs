using RegioVorbereitung.Properties;
using System.Windows;

namespace RegioVorbereitung.Login
{
	/// <summary>
	/// Interaction logic for LoginView.xaml
	/// </summary>
	public partial class LoginView : Window
	{
		public LoginView()
		{
			InitializeComponent();
			DataContext = new LoginViewModel();
			Password.Password = Settings.Default.SavedPassword;
			Username.Focus();
		}

		private void Ok_Click(object sender, RoutedEventArgs e)
		{
			if(ViewModel.Login(Password.Password))
			{
				Close();
			}
		}

		LoginViewModel ViewModel => DataContext as LoginViewModel;

		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
