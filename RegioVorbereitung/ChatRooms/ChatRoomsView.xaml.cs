using RegioVorbereitung.EmployeeFinder;
using System.Windows;

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

		private void EmployeeFinder_Click(object sender, RoutedEventArgs e)
		{
			var employeeFinderViewModel = new EmployeeFinderViewModel((user) => { throw new ResourceReferenceKeyNotFoundException(); });

			new EmployeeFinderView(employeeFinderViewModel).ShowDialog();
		}
	}
}
