using MySql.Data.MySqlClient;
using MySql.Data.Types;
using RegioVorbereitung.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RegioVorbereitung.Model
{
	public class ChatRoomModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string property = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}

		public static ChatRoomModel CreateChat(EmployeeModel with)
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				//New Chatroom
				var query = "INSERT INTO ChatRoom VALUES(null, @topic)";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.Parameters.Add(new MySqlParameter("@topic", MySqlDbType.String));
				cmd.Prepare();

				cmd.Parameters["@topic"].Value = $"Chat between {EmployeeModel.CurrentEmployee.Name} and {with.Name}";

				cmd.ExecuteNonQuery();

				//Get Chatroom-ID
				query = "SELECT LAST_INSERT_ID() as Id";
				cmd = new MySqlCommand(query, connection);

				var reader = cmd.ExecuteReader();
				int chatId = 0;

				if (reader.Read())
				{
					chatId = reader.GetInt32("Id");
				}

				reader.Close();

				//New ChatMember
				cmd.CommandText = "INSERT INTO ChatMember VALUES(null, @member, @chatroom)";
				cmd.Parameters.Add(new MySqlParameter("@member", MySqlDbType.Int32));
				cmd.Parameters.Add(new MySqlParameter("@chatroom", MySqlDbType.Int32));
				cmd.Prepare();

				cmd.Parameters["@chatroom"].Value = chatId;

				//Add me
				cmd.Parameters["@member"].Value = EmployeeModel.CurrentEmployee.Id;
				cmd.ExecuteNonQuery();

				//Add with
				cmd.Parameters["@member"].Value = with.Id;
				cmd.ExecuteNonQuery();


				var newChatModel = new ChatRoomModel
				{
					Id = chatId
				};
				newChatModel.Load();

				return newChatModel;
			}
		}

		public static void InviteToChat(EmployeeModel employee, ChatRoomModel chatRoom)
		{
			throw new NotImplementedException();
		}

		public static IEnumerable<ChatRoomModel> LoadAll()
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "SELECT ChatRoom.Id as Id "
					+ "FROM ChatRoom "
					+ "INNER JOIN ChatMember ON Chatroom.Id = ChatMember.Chatroom_Id "
					+ "WHERE ChatMember.Employee_Id = @id";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int32));
				cmd.Prepare();

				cmd.Parameters["@id"].Value = EmployeeModel.CurrentEmployee.Id;
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					if (!reader.IsDBNull(0))
					{
						var chatRoom = new ChatRoomModel
						{
							Id = reader.GetInt32("Id")
						};
						chatRoom.Load();
						yield return chatRoom;
					}
				}
			}
		}

		public void Load()
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();

				//Load Messages
				var query = "SELECT Id, Chatroom_id, Employee_Id, Date, Message "
					+ "FROM ChatMessage "
					+ "WHERE Chatroom_Id = @chatroom";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.Parameters.Add(new MySqlParameter("@chatroom", MySqlDbType.Int32));
				cmd.Prepare();

				cmd.Parameters["@chatroom"].Value = Id;
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					if (!reader.IsDBNull(0))
					{
						var message= new ChatMessageModel(reader.GetInt32("Id"), EmployeeModel.Load(reader.GetInt32("Employee_Id")), reader.GetString("Message"), reader.GetDateTime("Date"));

						if (!Messages.Contains(message))
						{
							Messages.Add(message);
						}
					}
				}
				reader.Close();

				//Load Members
				cmd.CommandText = "SELECT Employee.Id as Id "
					+ "FROM ChatMember "
					+ "INNER JOIN Employee "
					+ "ON Employee.Id = ChatMember.Employee_Id "
					+ "WHERE Chatroom_id = @chatroom";
				cmd.Prepare();
				
				reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					if (!reader.IsDBNull(0))
					{
						var employee = EmployeeModel.Load(reader.GetInt32("Id"));
						if (!Members.Contains(employee))
						{
							Members.Add(employee);
						}
					}
				}
				reader.Close();

				//Load Topic
				cmd.CommandText = "SELECT Topic "
					+ "FROM ChatRoom "
					+ "WHERE Id = @chatroom";
				cmd.Prepare();

				reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					Topic = reader.GetString("Topic");
				}
			}
		}

		public int Id { get; set; }
		public string Topic { get; set; }
		public DateTime LastMessageDate => Messages.Any() ? Messages.Max(m => m.Created) : DateTime.MinValue;
		public ObservableCollection<EmployeeModel> Members { get; } = new ObservableCollection<EmployeeModel>();
		public ObservableCollection<ChatMessageModel> Messages { get; } = new ObservableCollection<ChatMessageModel>();

		public ChatRoomModel()
		{
			Messages.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(LastMessageDate));
		}
	}
}
