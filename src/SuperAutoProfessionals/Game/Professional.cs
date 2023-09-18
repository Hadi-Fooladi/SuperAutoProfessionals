//#define LOG

using System;

namespace SuperAutoProfessionals;

public class Professional
{
	const int
		MAX_HEALTH = 50,
		MIN_ATTACK = 1,
		MAX_ATTACK = 50;

	int _health, _attack;

	public Game Game { get; set; } = null!;
	public Team Team { get; set; } = null!;
	public Team EnemyTeam { get; set; } = null!;

	public int Attack
	{
		get => _attack;
		set => _attack = Math.Clamp(value, MIN_ATTACK, MAX_ATTACK);
	}

	public int Health
	{
		get => _health;
		set => _health = Math.Min(value, MAX_HEALTH);
	}

	public bool IsDead => Health <= 0;

	public int Index => Team.GetIndex(this);

	public virtual string CodeName => "Pr";

	/// <returns>true if something has been done</returns>
	internal virtual bool On(Event e)
	{
#if LOG
		if (e.Professional == null)
			Log($"{e.Code}");
		else
			Log($"{e.Code} ({e.Professional})");
#endif

		return false;
	}

	public bool IsEnemy(Professional p) => p.Team != Team;
	public bool IsFriend(Professional p) => p.Team == Team;

	public override string ToString() => $"{Attack} {CodeName} {Health}";

	protected void Log(string text)
	{
		Game.Log($"{this} => {text}");
	}
}
