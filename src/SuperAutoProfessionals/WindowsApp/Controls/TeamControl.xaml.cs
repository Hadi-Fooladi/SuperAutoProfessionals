using System.Linq;
using System.Diagnostics;

namespace SuperAutoProfessionals.WindowsApp.Controls;

partial class TeamControl
{
	public TeamControl()
	{
		InitializeComponent();

		_controls = _grid.Children.Cast<ProfessionalControl>().ToArray();

		Debug.Assert(_controls.Length == Team.MAX_PROFESSIONALS);
	}

	Team? _team;
	readonly ProfessionalControl[]? _controls;

	public Team Team
	{
		set
		{
			_team = value;
			Update();
		}
	}

	ProfessionalControl this[int ndx]
		=> _team!.Side == Side.Right
			? _controls![ndx]
			: _controls![Team.MAX_PROFESSIONALS - 1 - ndx];

	public void Update()
	{
		if (_team == null) return;

		for (int i = 0; i < Team.MAX_PROFESSIONALS; i++)
			this[i].Professional = _team[i];
	}
}
