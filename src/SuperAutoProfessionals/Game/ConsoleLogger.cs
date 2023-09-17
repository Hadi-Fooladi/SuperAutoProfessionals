using System;

namespace SuperAutoProfessionals;

class ConsoleLogger : ILogger
{
	ConsoleLogger() { }

	public static ConsoleLogger Instance { get; } = new();

	public void Write(string log) { Console.Write(log); }
	public void WriteLine(string log) { Console.WriteLine(log); }
}
