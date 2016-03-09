using MySql.Data.MySqlClient;
using RegioVorbereitung.Properties;
using System.Collections.Generic;
using System;

namespace RegioVorbereitung.Model
{
	public class EmployeeModel
	{
		public static IEnumerable<EmployeeModel> LoadAll()
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "SELECT Id, Name, Department_Id FROM Employee";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					if (!reader.IsDBNull(0))
					{
						var employeeId = reader.GetInt32("Id");
						if (!Equals(employeeId, EmployeeModel.CurrentEmployee.Id))
						{
							var employee = new EmployeeModel
							{
								Id = employeeId,
								Name = reader.GetString("Name"),
								Department = DepartmentModel.Load(reader.GetInt32("Department_Id"))
							};
							yield return employee;
						}
					}
				}
			}
		}

		public static EmployeeModel CurrentEmployee { get; set; }

		public int Id { get; set; }
		public string Name { get; set; }
		public DepartmentModel Department { get; set; }

		public static EmployeeModel Load(int id)
		{
			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "SELECT Name, Department_Id FROM Employee WHERE Id = @id";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.Parameters.Add("@id", MySqlDbType.Int32);
				cmd.Prepare();

				cmd.Parameters["@id"].Value = id;

				MySqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					var employee = new EmployeeModel
					{
						Id = id,
						Name = reader.GetString("Name"),
						Department = DepartmentModel.Load(reader.GetInt32("Department_Id"))
					};
					return employee;
				}
				else
				{
					return null;
				}
			}
		}

		public override bool Equals(object obj)
		{
			var other = obj as EmployeeModel;

			return other != null && Equals(other.Id, Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
