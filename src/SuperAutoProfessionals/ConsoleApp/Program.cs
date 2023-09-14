﻿using System;

namespace SuperAutoProfessionals.ConsoleApp;

internal class Program
{
	static void Main()
	{
		var game = new Game();

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
			new() { Attack = 8, Health = 50 },
			new Nurse { Attack = 7, Health = 10 },
			new() { Attack = 3, Health = 6 },
			new() { Attack = 2, Health = 7 },
			null
		};

		var winner = game.RunTurn(new Team(left, Side.Left), new Team(right, Side.Right));

		Console.WriteLine();
		Console.WriteLine();
		Console.WriteLine(winner == null ? "Draw" : $"Winner: {winner.Side}");
	}
}
