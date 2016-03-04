using RegioVorbereitung.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Threading.Tasks;

namespace RegioVorbereitung.EmployeeFinder
{
	public class EmployeeFinderViewModel : INotifyPropertyChanged
	{
		private Action<EmployeeModel> _callback;

		public ObservableCollection<DepartmentViewModel> Departments { get; } = new ObservableCollection<DepartmentViewModel>(DepartmentModel.LoadAll().ToList().Select(m => new DepartmentViewModel(m)));
		private List<DepartmentViewModel> _selectedDepartments => Departments.Where(d => d.IsSelected).ToList();
		public EmployeeModel SelectedEmployee { get; set; }
		public ObservableCollection<EmployeeModel> AvailableEmployees { get; } = new ObservableCollection<EmployeeModel>();
		private IEnumerable<EmployeeModel> _allEmployees;
		private string _searchText;

		public string SearchText
		{
			get
			{
				return _searchText;
			}
			set
			{
				if (!Equals(value, _searchText))
				{
					_searchText = value;
					OnPropertyChanged();

					ApplyFilter();
				}
			}
		}
		public EmployeeFinderViewModel(Action<EmployeeModel> callback)
		{
			_callback = callback;

			foreach (var department in Departments)
			{
				department.PropertyChanged += Department_PropertyChanged;
			}

			LoadEmployees();
		}

		private void LoadEmployees()
		{
			_allEmployees = EmployeeModel.LoadAll();
			ApplyFilter();
		}

		private void Department_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (Equals(e.PropertyName, nameof(DepartmentViewModel.IsSelected)))
			{
				//Application.Current.Dispatcher.Invoke(ApplyFilter);

				ApplyFilter();
			}
		}

		private void ApplyFilter()
		{
			AvailableEmployees.Clear();
			foreach (var employee in _allEmployees.Where(e => _selectedDepartments.Select(d => d.Model).Contains(e.Department) && (string.IsNullOrEmpty(SearchText) || e.Name.ToLower().Contains(SearchText.ToLower()))))
			{
				AvailableEmployees.Add(employee);
			}
		}

		public void InvokeCallback()
		{
			_callback(SelectedEmployee);
		}


		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
