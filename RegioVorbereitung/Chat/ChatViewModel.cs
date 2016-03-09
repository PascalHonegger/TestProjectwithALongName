using RegioVorbereitung.Model;
using System.Collections.ObjectModel;
using System.Timers;
using System;
using MySql.Data.MySqlClient;
using RegioVorbereitung.Properties;

namespace RegioVorbereitung.Chat
{
	public class ChatViewModel
	{
		public ChatRoomModel Chat { get; }

		public ChatViewModel(EmployeeModel employee)
		{
			Chat = ChatRoomModel.CreateChat(employee);
		}

		public ChatViewModel(ChatRoomModel chat)
		{
			Chat = chat;
		}

		public ObservableCollection<EmployeeModel> Members => Chat?.Members;

		public ObservableCollection<ChatMessageModel> Messages => Chat?.Messages;

		public void SendMessage()
		{
			if (!string.IsNullOrEmpty(MessageText))
			{
				using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
				{
					connection.Open();
					var query = "INSERT INTO ChatMessage(Chatroom_Id, Employee_Id, Message) VALUES(@chatroom, @employee, @message)";
					MySqlCommand cmd = new MySqlCommand(query, connection);
					cmd.Parameters.Add(new MySqlParameter("@chatroom", MySqlDbType.Int32));
					cmd.Parameters.Add(new MySqlParameter("@employee", MySqlDbType.Int32));
					cmd.Parameters.Add(new MySqlParameter("@message", MySqlDbType.String));
					cmd.Prepare();

					cmd.Parameters["@chatroom"].Value = Chat.Id;
					cmd.Parameters["@employee"].Value = EmployeeModel.CurrentEmployee.Id;
					cmd.Parameters["@message"].Value = MessageText;
					cmd.ExecuteNonQuery();
					Chat.Load();

					MessageText = string.Empty;
				}
			}
		}

		public string MessageText { get; set; }
	}
}
