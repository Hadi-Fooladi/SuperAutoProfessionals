using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SuperAutoProfessionals;

public class Game
{
	Random _rnd = new();
	readonly List<TurnEvent> _events = new();

	public ILogger Logger { get; set; } = ConsoleLogger.Instance;
	public Func<Task> WaitForNextIteration { get; set; } = WaitForNextIterationByConsole;

	public void Log(string text) { Logger.WriteLine(text); }

	public async Task<Team?> RunTurn(Team left, Team right)
	{
		left.SetGame(this);
		right.SetGame(this);
		Log($"{left} - {right}");
		await WaitForNextIteration();

		left.ForEach(p => _events.Add(new TurnEvent(p, Event.StartOfBattle)));
		right.ForEach(p => _events.Add(new TurnEvent(p, Event.StartOfBattle)));
		ProcessEvents();

		IEnumerable<Event> events;
		void addEvents(Professional p)
		{
			foreach (var e in events)
				_events.Add(new TurnEvent(p, e));
		}

		int iteration = 1;
		while (left.AnyLeft && right.AnyLeft)
		{
			Log($"\n\nIteration #{iteration++}:");

			Professional
				lp = left.First!,
				rp = right.First!;

			process(EventCode.BeforeAttack);

			lp.Health -= rp.Attack;
			rp.Health -= lp.Attack;

			process(EventCode.AfterAttack);
			process(EventCode.Hurt);

			events = dieEvents().ToList();
			addAndProcess();

			if (lp.IsDead)
				left[lp.Index] = null;

			if (rp.IsDead)
				right[rp.Index] = null;

			Log("\n\nResult");
			Log($"{left} - {right}");

			await WaitForNextIteration();

			void process(EventCode code)
			{
				events = new Event[] { new(code, lp), new(code, rp) };
				addAndProcess();
			}

			void addAndProcess()
			{
				left.ForEach(addEvents);
				right.ForEach(addEvents);
				ProcessEvents();
			}

			IEnumerable<Event> dieEvents()
			{
				if (lp.IsDead) yield return new Event(EventCode.Die, lp);
				if (rp.IsDead) yield return new Event(EventCode.Die, rp);
			}
		}

		return left.AnyLeft
			? left
			: right.AnyLeft ? right : null;
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
				return; // All events are processed

			next.Call();
			_events.Remove(next);
		}
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
