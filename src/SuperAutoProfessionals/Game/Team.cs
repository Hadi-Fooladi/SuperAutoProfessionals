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

	public IEnumerable<Professional> Deads => Members.Where(p => p.IsDead);
	public IEnumerable<Professional> Members => _professionals.Where(p => p != null)!;

	public int GetIndex(Professional? professional) => Array.IndexOf(_professionals, professional);

	public void SetGame(Game game)
	{
		ForEach(p => p.Game = game);
	}

	public void Remove(Professional professional)
	{
		_professionals[professional.Index] = null;
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

	public bool Spawn(Professional pro, int pos)
	{
		if (pos is < 0 or >= MAX_PROFESSIONALS) return false;

		if (spawn(pos)) return true;

		for (int i = MAX_PROFESSIONALS - 1; i >= 0; i--)
			if (spawn(i))
				return true;

		return false;

		bool spawn(int ndx)
		{
			if (this[ndx] != null) return false;

			this[ndx] = pro;
			return true;
		}
	}
}
