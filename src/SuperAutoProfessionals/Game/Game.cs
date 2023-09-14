using System;

namespace SuperAutoProfessionals;

public class Game
{
	Random _rnd = new();

	public void Log(string text) { Console.WriteLine(text); }

	public int RunTurn(Team left, Team right)
	{
		left.SetGame(this);
		right.SetGame(this);
		Log($"{left} - {right}");

		int iteration = 1;
		while (left.AnyLeft && right.AnyLeft)
		{
			Log($"\n\nIteration #{iteration++}:");

			Professional
				lp = left.First!,
				rp = right.First!;

			for (int i = 0; i < Team.MAX_PROFESSIONALS; i++)
			{
				left[i]?.OnFriendBeforeAttack(lp);
				left[i]?.OnEnemyBeforeAttack(rp);

				right[i]?.OnFriendBeforeAttack(rp);
				right[i]?.OnEnemyBeforeAttack(lp);
			}

			lp.Health -= rp.Attack;
			rp.Health -= lp.Attack;

			for (int i = 0; i < Team.MAX_PROFESSIONALS; i++)
			{
				left[i]?.OnFriendAfterAttack(lp);
				left[i]?.OnEnemyAfterAttack(rp);

				right[i]?.OnFriendAfterAttack(rp);
				right[i]?.OnEnemyAfterAttack(lp);
			}

			for (int i = 0; i < Team.MAX_PROFESSIONALS; i++)
			{
				left[i]?.OnFriendHurt(lp);
				left[i]?.OnEnemyHurt(rp);

				right[i]?.OnFriendHurt(rp);
				right[i]?.OnEnemyHurt(lp);
			}

			bool
				isLeftDead = lp.IsDead,
				isRightDead = rp.IsDead;

			for (int i = 0; i < Team.MAX_PROFESSIONALS; i++)
			{
				if (isLeftDead) left[i]?.OnFriendDie(lp);
				if (isRightDead) left[i]?.OnEnemyDie(rp);

				if (isRightDead) right[i]?.OnFriendDie(rp);
				if (isLeftDead) right[i]?.OnEnemyDie(lp);
			}

			if (isLeftDead)
				left[left.GetIndex(lp)] = null;

			if (isRightDead)
				right[right.GetIndex(rp)] = null;

			Log("\n\nResult");
			Log($"{left} - {right}");

			Console.ReadKey(true);
		}

		return left.AnyLeft
			? -1
			: right.AnyLeft ? 1 : 0;
	}
}
