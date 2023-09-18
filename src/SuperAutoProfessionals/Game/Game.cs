using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SuperAutoProfessionals;

public class Game
{
	public Game(Team left, Team right)
	{
		_left = left;
		_right = right;

		left.SetGame(this);
		right.SetGame(this);
	}

	Random _rnd = new();
	readonly Team _left, _right;
	readonly List<TurnEvent> _events = new();
	readonly List<Professional> _deads = new();

	public bool LogTeams { get; set; } = true;
	public ILogger Logger { get; set; } = ConsoleLogger.Instance;
	public Func<Task> WaitForNextIteration { get; set; } = WaitForNextIterationByConsole;

	public void Log(string text) { Logger.WriteLine(text); }

	public async Task<Team?> RunTurn()
	{
		logTeams();
		await WaitForNextIteration();

		NotifyAll(Event.StartOfBattle);
		ProcessEvents();

		int iteration = 1;
		while (_left.AnyLeft && _right.AnyLeft)
		{
			Log($"Iteration #{iteration++}");

			Professional
				lp = _left.First!,
				rp = _right.First!;

			process(EventCode.BeforeAttack);

			if (!lp.IsDead && !rp.IsDead)
			{
				lp.Health -= rp.Attack;
				rp.Health -= lp.Attack;

				process(EventCode.AfterAttack);
				process(EventCode.Hurt);
			}

			logTeams();
			
			await WaitForNextIteration();

			void process(EventCode code)
			{
				NotifyAll(new Event(code, lp));
				NotifyAll(new Event(code, rp));
				ProcessEvents();
			}
		}

		return _left.AnyLeft
			? _left
			: _right.AnyLeft ? _right : null;

		void logTeams()
		{
			if (LogTeams)
				Log($"{_left} - {_right}");
		}
	}

	void ProcessEvents()
	{
		for (;;)
		{
			var maxPriority = -1;
			TurnEvent? next = null;
			foreach (var e in _events)
			{
				var priority = e.Priority;
				if (priority > maxPriority)
				{
					next = e;
					maxPriority = priority;
				}
			}

			if (next == null)
			{
				CheckDeads();
				if (_events.Any()) continue;

				return; // All events are processed
			}

			next.Call();
			_events.Remove(next);
		}
	}

	void CheckDeads()
	{
		var deads = _left.Deads
			.Concat(_right.Deads)
			.Where(p => !_deads.Contains(p))
			.ToList();

		if (!deads.Any()) return;

		foreach (var dead in deads)
		{
			_deads.Add(dead);
			NotifyAll(new Event(EventCode.Die, dead));
		}

		ProcessEvents();

		foreach (var dead in deads)
		{
			_deads.Remove(dead);
			dead.Team.Remove(dead);
		}
	}

	void NotifyAll(Event e)
	{
		_left.ForEach(p => _events.Add(new TurnEvent(p, e)));
		_right.ForEach(p => _events.Add(new TurnEvent(p, e)));
	}

	static Task WaitForNextIterationByConsole()
	{
		Console.ReadKey(true);
		return Task.CompletedTask;
	}

	class TurnEvent
	{
		public TurnEvent(Professional subject, Event e)
		{
			_event = e;
			_code = e.Code;
			_subject = subject;
		}

		readonly Event _event;
		readonly EventCode _code;
		readonly Professional _subject;

		public int Priority => (100 - (int)_code) * 10000 + _subject.Attack * 100 + _subject.Health;

		public void Call()
		{
			_subject.On(_event);
		}
	}
}
