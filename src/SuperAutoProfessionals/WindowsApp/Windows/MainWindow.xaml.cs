﻿using System.Windows;
using System.Threading.Tasks;

namespace SuperAutoProfessionals.WindowsApp.Windows;

partial class MainWindow
{
	public MainWindow()
	{
		InitializeComponent();
	}

	TaskCompletionSource? _waitTask;

	Task WaitForNextIteration()
	{
		_leftTeam.Update();
		_rightTeam.Update();

		_waitTask = new TaskCompletionSource();
		return _waitTask.Task;
	}

	void Next_OnClick(object sender, RoutedEventArgs e)
	{
		_waitTask?.SetResult();
	}

	async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
	{
		var game = new Game
		{
			LogTeams = false,
			Logger = new TextBoxLogger(_log),
			WaitForNextIteration = WaitForNextIteration
		};

		var left = new Professional?[]
		{
			new() { Attack = 2, Health = 10 },
			new() { Attack = 20, Health = 10 },
			null,
			new() { Attack = 5, Health = 10 },
			new() { Attack = 7, Health = 10 }
		};

		var right = new Professional?[]
		{
			new() { Attack = 8, Health = 30 },
			new Nurse { Attack = 7, Health = 10 },
			new() { Attack = 3, Health = 6 },
			new() { Attack = 2, Health = 7 },
			null
		};

		Team
			lt = new(left, Side.Left),
			rt = new(right, Side.Right);

		_leftTeam.Team = lt;
		_rightTeam.Team = rt;

		var winner = await game.RunTurn(lt, rt);

		MessageBox.Show(winner == null ? "Draw" : $"Winner: {winner.Side}", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
	}
}
