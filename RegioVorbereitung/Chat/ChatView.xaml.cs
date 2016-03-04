using System.Windows;
using RegioVorbereitung.ChatRooms;
using RegioVorbereitung.EmployeeFinder;
using RegioVorbereitung.Model;

namespace RegioVorbereitung.Chat
{
	/// <summary>
	/// Interaction logic for ChatRoomsView.xaml
	/// </summary>
	public partial class ChatView : Window
	{
		public ChatView(EmployeeModel employee)
		{
			InitializeComponent();
			DataContext = new ChatViewModel();
		}
	}
}
