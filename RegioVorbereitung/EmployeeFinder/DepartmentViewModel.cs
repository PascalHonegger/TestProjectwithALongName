using RegioVorbereitung.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RegioVorbereitung.EmployeeFinder
{
	public class DepartmentViewModel : INotifyPropertyChanged
	{
		private bool _isSelected = true;

		public DepartmentViewModel(DepartmentModel model)
		{
			Model = model;
		}

		public DepartmentModel Model { get; }

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				if (value != _isSelected)
				{
					_isSelected = value;

					OnPropertyChanged();
				}
			}
		}

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}