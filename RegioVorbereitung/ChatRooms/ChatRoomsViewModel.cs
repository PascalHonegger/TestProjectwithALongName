using RegioVorbereitung.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RegioVorbereitung.ChatRooms
{
	public class ChatRoomsViewModel
	{
		public string HelloText { get; } = $"Hello {EmployeeModel.CurrentEmployee.Name}!";

		public ObservableCollection<ChatRoomModel> ChatRooms { get; set; }
		public ChatRoomsViewModel()
		{
			ChatRooms = new ObservableCollection<ChatRoomModel>(ChatRoomModel.LoadAll());
		}
	}
}
