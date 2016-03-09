using RegioVorbereitung.Model;
using System.Collections.ObjectModel;
using System.Timers;
using System;
using MySql.Data.MySqlClient;
using RegioVorbereitung.Properties;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RegioVorbereitung.Chat
{
	public class ChatViewModel : INotifyPropertyChanged
	{
		private string _messageText;

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string property = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}

		public ChatRoomModel Chat { get; }

		public ChatViewModel(EmployeeModel employee)
		{
			Chat = ChatRoomModel.CreateChat(employee);
		}

		public void AddUser(EmployeeModel user)
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "INSERT INTO ChatMember VALUES(null, @member, @chatroom)";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.Parameters.Add(new MySqlParameter("@member", MySqlDbType.Int32));
				cmd.Parameters.Add(new MySqlParameter("@chatroom", MySqlDbType.Int32));
				cmd.Prepare();

				cmd.Parameters["@chatroom"].Value = Chat.Id;
				cmd.Parameters["@member"].Value = user.Id;
				cmd.ExecuteNonQuery();
				Chat.Load();
			}
		}

		public void LeaveChat()
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "DELETE FROM ChatMember WHERE Employee_Id = @member and Chatroom_Id = @chatroom";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.Parameters.Add(new MySqlParameter("@member", MySqlDbType.Int32));
				cmd.Parameters.Add(new MySqlParameter("@chatroom", MySqlDbType.Int32));
				cmd.Prepare();

				cmd.Parameters["@chatroom"].Value = Chat.Id;
				cmd.Parameters["@member"].Value = EmployeeModel.CurrentEmployee.Id;
				cmd.ExecuteNonQuery();
				Chat.Load();
			}
		}

		public ChatViewModel(ChatRoomModel chat)
		{
			Chat = chat;
		}

		public ObservableCollection<EmployeeModel> Members => Chat?.Members;

		public ObservableCollection<ChatMessageModel> Messages => Chat?.Messages;

		internal void ChangeTopic()
		{
			if (!string.IsNullOrEmpty(NewTopic))
			{
				using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
				{
					connection.Open();
					var query = "UPDATE Chatroom SET Topic=@topic WHERE Id=@id";
					MySqlCommand cmd = new MySqlCommand(query, connection);
					cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int32));
					cmd.Parameters.Add(new MySqlParameter("@topic", MySqlDbType.String));
					cmd.Prepare();

					cmd.Parameters["@id"].Value = Chat.Id;
					cmd.Parameters["@topic"].Value = NewTopic;
					cmd.ExecuteNonQuery();
					Chat.Load();
				}
			}
		}

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

		public string MessageText
		{
			get { return _messageText; }
			set
			{
				_messageText = value;
				OnPropertyChanged();
			}
		}

		public string NewTopic { get; set; }
	}
}
