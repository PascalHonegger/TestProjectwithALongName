using MySql.Data.MySqlClient;
using RegioVorbereitung.Properties;
using System.Collections.Generic;

namespace RegioVorbereitung.Model
{
	public class EmployeeModel
	{
		public static IEnumerable<EmployeeModel> LoadAll()
		{
			return new List<EmployeeModel>
			{
				new EmployeeModel
				{
					Id = 1,
					Name = "Hans Meier",
					Department = DepartmentModel.Load(1)
				},
				new EmployeeModel
				{
					Id = 2,
					Name = "Wurstus Hansus",
					Department = DepartmentModel.Load(2)
				}
			};

			/*using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "SELECT Id, Name, Department_Id FROM Employee";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				MySqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					if (!reader.IsDBNull(0))
					{
						var employee = new EmployeeModel
						{
							Id = reader.GetInt32("Id"),
							Name = reader.GetString("Name"),
							Department = DepartmentModel.Load(reader.GetInt32("Department_Id"))
						};
						yield return employee;
					}
				}
			}*/
		}

		public static EmployeeModel CurrentEmployee { get; set; }

		public int Id { get; set; }
		public string Name { get; set; }
		public DepartmentModel Department { get; set; }
	}
}
