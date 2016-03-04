using MySql.Data.MySqlClient;
using RegioVorbereitung.Properties;
using System.Collections.Generic;

namespace RegioVorbereitung.Model
{
	public class DepartmentModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public static DepartmentModel Load(int id)
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "SELECT Id, Name FROM Department WHERE Id = @id";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int32));
				cmd.Prepare();

				cmd.Parameters["@id"].Value = id;
				MySqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read() && !reader.IsDBNull(0))
				{
					var department = new DepartmentModel
					{
						Id = reader.GetInt32("Id"),
						Name = reader.GetString("Name")
					};
					return department;
				}
				else
				{
					//throw new NotSupportedArgument("Id not found");
					return null;
				}
			}
		}

		public static IEnumerable<DepartmentModel> LoadAll()
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "SELECT Id, Name FROM Department";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read() && !reader.IsDBNull(0))
				{
					var department = new DepartmentModel
					{
						Id = reader.GetInt32("Id"),
						Name = reader.GetString("Name")
					};
					yield return department;
				}
			}
		}

		public override bool Equals(object obj)
		{
			var other = obj as DepartmentModel;
			if (other != null)
			{
				return Equals(other.Id, Id);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
