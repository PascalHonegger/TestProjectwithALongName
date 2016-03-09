using RegioVorbereitung.EmployeeFinder;
using System.Windows;

namespace RegioVorbereitung.Chat
{
	/// <summary>
	/// Interaction logic for ChatRoomsView.xaml
	/// </summary>
	public partial class ChatView : Window
	{
		public ChatView(ChatViewModel viewModel)
		{
			InitializeComponent();
			ViewModel = (DataContext = viewModel) as ChatViewModel;
		}

		public ChatViewModel ViewModel { get; private set; }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.SendMessage();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			ViewModel.ChangeTopic();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			var viewModel = new EmployeeFinderViewModel(ViewModel.AddUser);
			new EmployeeFinderView(viewModel).ShowDialog();
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			ViewModel.LeaveChat();
			Close();
		}
	}
}
