using RegioVorbereitung.Chat;
using RegioVorbereitung.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System;
using System.Linq;
using System.Windows;

namespace RegioVorbereitung.ChatRooms
{
	public class ChatRoomsViewModel
	{
		private Timer _updateTimer = new Timer(5000);

		public string HelloText { get; } = $"Hello {EmployeeModel.CurrentEmployee.Name}!";

		public ObservableCollection<ChatRoomModel> ChatRooms { get; set; }
		public ChatRoomsViewModel()
		{
			ChatRooms = new ObservableCollection<ChatRoomModel>(ChatRoomModel.LoadAll());

			_updateTimer.Start();

			_updateTimer.Elapsed += (sender, e) => Application.Current.Dispatcher.Invoke(Update);
		}

		private void Update()
		{
			var allChats = ChatRoomModel.LoadAll();

			ChatRooms.Clear();

			foreach (var chat in allChats)
			{
				ChatRooms.Add(chat);
			}

			foreach (var chat in ChatRooms)
			{
				chat.Load();
			}
		}

		public ChatRoomModel SelectedChatRoom { get; set; }

		public void OpenSelectedChat()
		{
			new ChatView(new ChatViewModel(SelectedChatRoom)).Show();
		}
	}
}
