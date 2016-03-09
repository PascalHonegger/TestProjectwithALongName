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
	}
}
