using MySql.Data.MySqlClient;
using MySql.Data.Types;
using RegioVorbereitung.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RegioVorbereitung.Model
{
	public class ChatRoomModel
	{
		public static IEnumerable<ChatRoomModel> LoadAll()
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "SELECT ChatRoom.Id as Id, ChatRoom.Topic as Topic, MAX(ChatMessage.Date) as LastMessageDate "
					+ "FROM ChatRoom "
					+ "INNER JOIN ChatroomMember ON Chatroom.Id = ChatroomMember.Chatroom_Id "
					+ "INNER JOIN ChatMessage ON Chatroom.Id = ChatMessage.chatroom_Id "
					+ "WHERE ChatroomMember.Employee_Id = @id";
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
							Id = reader.GetInt32("Id"),
							Topic = reader.GetString("Topic"),
							LastMessageDate = reader.GetDateTime("LastMessageDate")
						};
						yield return chatRoom;
					}
				}
			}
		}

		private void Load()
		{
			throw new NotImplementedException();
		}

		public int Id { get; set; }
		public string Topic { get; set; }
		public DateTime LastMessageDate { get; set; }
		public ObservableCollection<EmployeeModel> Members { get; } = new ObservableCollection<EmployeeModel>();
	}
}
