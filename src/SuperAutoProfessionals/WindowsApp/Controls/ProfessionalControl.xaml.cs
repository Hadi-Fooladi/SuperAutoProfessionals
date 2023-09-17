using System.Windows;

namespace SuperAutoProfessionals.WindowsApp.Controls;

partial class ProfessionalControl
{
	public ProfessionalControl()
	{
		InitializeComponent();
	}

	public Professional? Professional
	{
		set
		{
			if (value == null)
			{
				_panel.Visibility = Visibility.Collapsed;
				return;
			}

			_name.Text = value.CodeName;
			_attack.Text = $"{value.Attack}";
			_health.Text = $"{value.Health}";
			_panel.Visibility = Visibility.Visible;
		}
	}
}
