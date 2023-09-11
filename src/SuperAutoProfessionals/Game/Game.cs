using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace SuperAutoProfessionals;

public class Game
{
	const int MAX_PROFESSIONALS = 5;
	
	Random _rnd = new();

	public int RunTurn(Professional?[] left, Professional?[] right)
	{
		Debug.Assert(left.Length == MAX_PROFESSIONALS);
		Debug.Assert(right.Length == MAX_PROFESSIONALS);

		SetGame(left);
		SetGame(right);

		Console.WriteLine($"{ToString(left.Reverse())} - {ToString(right)}");

		int iteration = 1;
		while (AnyLeft(left) && AnyLeft(right))
		{
			Console.WriteLine($"\n\nIteration #{iteration++}:");

			Professional
				lp = GetFirst(left)!,
				rp = GetFirst(right)!;

			for (int i = 0; i < MAX_PROFESSIONALS; i++)
			{
				left[i]?.OnFriendBeforeAttack(lp);
				left[i]?.OnEnemyBeforeAttack(rp);

				right[i]?.OnFriendBeforeAttack(rp);
				right[i]?.OnEnemyBeforeAttack(lp);
			}

			lp.Health -= rp.Attack;
			rp.Health -= lp.Attack;

			for (int i = 0; i < MAX_PROFESSIONALS; i++)
			{
				left[i]?.OnFriendAfterAttack(lp);
				left[i]?.OnEnemyAfterAttack(rp);

				right[i]?.OnFriendAfterAttack(rp);
				right[i]?.OnEnemyAfterAttack(lp);
			}

			for (int i = 0; i < MAX_PROFESSIONALS; i++)
			{
				left[i]?.OnFriendHurt(lp);
				left[i]?.OnEnemyHurt(rp);

				right[i]?.OnFriendHurt(rp);
				right[i]?.OnEnemyHurt(lp);
			}

			bool
				isLeftDead = lp.IsDead,
				isRightDead = rp.IsDead;

			for (int i = 0; i < MAX_PROFESSIONALS; i++)
			{
				if (isLeftDead) left[i]?.OnFriendDie(lp);
				if (isRightDead) left[i]?.OnEnemyDie(rp);

				if (isRightDead) right[i]?.OnFriendDie(rp);
				if (isLeftDead) right[i]?.OnEnemyDie(lp);
			}

			if (isLeftDead)
				left[Array.IndexOf(left, lp)] = null;

			if (isRightDead)
				right[Array.IndexOf(right, rp)] = null;

			Console.WriteLine("\n\nResult");
			Console.WriteLine($"{ToString(left.Reverse())} - {ToString(right)}");

			Console.ReadKey(true);
		}

		return AnyLeft(left)
			? -1
			: AnyLeft(right) ? 1 : 0;
	}

	static bool AnyLeft(IEnumerable<Professional?> p) => p.Any(x => x != null);

	static Professional? GetFirst(IEnumerable<Professional?> p) => p.FirstOrDefault(x => x != null);

	void SetGame(IEnumerable<Professional?> list)
	{
		foreach (var p in list)
			if (p != null)
				p.Game = this;
	}

	static string ToString(IEnumerable<Professional?> list) => string.Join(", ", list);
}
