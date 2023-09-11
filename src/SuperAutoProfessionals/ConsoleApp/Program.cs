using System;

namespace SuperAutoProfessionals.ConsoleApp;

internal class Program
{
	static void Main()
	{
		var game = new Game();

		var left = new Professional?[]
		{
			new() { Attack = 2, Health = 10 },
			new() { Attack = 5, Health = 10 },
			null,
			new() { Attack = 5, Health = 10 },
			new() { Attack = 7, Health = 10 }
		};

		var right = new Professional?[]
		{
			new() { Attack = 8, Health = 12 },
			null,
			new() { Attack = 3, Health = 6 },
			new() { Attack = 2, Health = 7 },
			null
		};

		Console.WriteLine(game.RunTurn(left, right));
	}
}
