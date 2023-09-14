using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace SuperAutoProfessionals;

public class Team
{
	public const int MAX_PROFESSIONALS = 5;

	readonly Professional?[] _professionals;

	public Team(IEnumerable<Professional?> professionals, Side side)
	{
		Side = side;
		_professionals = professionals.ToArray();

		Debug.Assert(_professionals.Length == MAX_PROFESSIONALS);

		ForEach(p => p.Team = this);
	}
	
	public Side Side { get; }

	public Professional? this[int ndx]
	{
		get => _professionals[ndx];
		set => _professionals[ndx] = value;
	}

	public bool AnyLeft => _professionals.Any(p => p != null);
	public Professional? First => _professionals.FirstOrDefault(p => p != null);

	public int GetIndex(Professional? professional) => Array.IndexOf(_professionals, professional);

	public void SetGame(Game game)
	{
		ForEach(p => p.Game = game);
	}

	public override string ToString()
	{
		var p = Side == Side.Left ? _professionals.Reverse() : _professionals;
		return string.Join(" , ", p.Select(x => x == null ? "*" : x.ToString()));
	}

	public void ForEach(Action<Professional> action)
	{
		foreach (var p in _professionals)
			if (p != null)
				action(p);
	}
}
