using MySql.Data.MySqlClient;
using RegioVorbereitung.ChatRooms;
using RegioVorbereitung.Model;
using RegioVorbereitung.Properties;
using System.Windows;

namespace RegioVorbereitung.Login
{
	public class LoginViewModel
	{
		public LoginViewModel()
		{
			if (!string.IsNullOrEmpty(Username))
			{
				SaveLogin = true;
			}
		}

		public string Username { get; set; } = Settings.Default.SavedUsername;

		public bool SaveLogin { get; set; }

		public bool Login(string password)
		{
			var employee = new EmployeeModel();

			using (var connection = new MySqlConnection(Settings.Default.ConnectionString))
			{
				connection.Open();
				var query = "SELECT Id, Name, Department_Id FROM Employee WHERE Username = @username and Password = @password limit 1";
				MySqlCommand cmd = new MySqlCommand(query, connection);
				cmd.Parameters.Add(new MySqlParameter("@username", MySqlDbType.VarChar));
				cmd.Parameters.Add(new MySqlParameter("@password", MySqlDbType.VarChar));
				cmd.Prepare();

				cmd.Parameters["@username"].Value = Username;
				cmd.Parameters["@password"].Value = password;
				MySqlDataReader reader = cmd.ExecuteReader();

				if (reader.Read() && !reader.IsDBNull(0))
				{
					employee.Id = reader.GetInt32("Id");
					employee.Name = reader.GetString("Name");
					employee.Department = DepartmentModel.Load(reader.GetInt32("Department_Id"));
				}
			}

			if (employee.Id != 0)
			{
				EmployeeModel.CurrentEmployee = employee;
				if (SaveLogin)
				{
					Settings.Default.SavedUsername = Username;
					Settings.Default.SavedPassword = password;
				}
				else
				{
					Settings.Default.SavedUsername = null;
					Settings.Default.SavedPassword = null;
				}
				Settings.Default.Save();

				new ChatRoomsView().Show();

				return true;
			}
			else
			{
				MessageBox.Show("Username or Password is invalid!", "Error", MessageBoxButton.OK);
				return false;
			}
		}
	}
}
