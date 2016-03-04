using System.Windows;

namespace RegioVorbereitung.EmployeeFinder
{
	/// <summary>
	/// Interaction logic for EmployeeFinderView.xaml
	/// </summary>
	public partial class EmployeeFinderView : Window
	{
		public EmployeeFinderView(EmployeeFinderViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}

		EmployeeFinderViewModel ViewModel => DataContext as EmployeeFinderViewModel;

		private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (EmployeeSelector.SelectedItem != null)
			{
				ViewModel.InvokeCallback();
				Close();
			}
			
		}
	}
}
