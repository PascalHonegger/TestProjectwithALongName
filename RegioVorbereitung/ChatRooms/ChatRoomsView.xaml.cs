using RegioVorbereitung.EmployeeFinder;
using System.Windows;
using RegioVorbereitung.Chat;

namespace RegioVorbereitung.ChatRooms
{
	/// <summary>
	/// Interaction logic for ChatRoomsView.xaml
	/// </summary>
	public partial class ChatRoomsView : Window
	{
		public ChatRoomsView()
		{
			InitializeComponent();
			DataContext = new ChatRoomsViewModel();
		}

		private void CloseApp_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		public void EmployeeFinder_Click(object sender, RoutedEventArgs e)
		{
			var employeeFinderViewModel = new EmployeeFinderViewModel(user => { new ChatView(new ChatViewModel(user)).Show(); });

			new EmployeeFinderView(employeeFinderViewModel).ShowDialog();
		}


		public void Chat_DoubleClick(object sender, object e)
		{
			var viewModel = (DataContext as ChatRoomsViewModel);

			if (viewModel?.SelectedChatRoom != null)
			{
				viewModel.OpenSelectedChat();
			}
		}
	}
}
