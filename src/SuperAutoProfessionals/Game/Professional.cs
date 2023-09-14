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

	public virtual string CodeName => "Pr";

	public virtual void OnStartOfBattle() { }

	public virtual void OnFriendBeforeAttack(Professional friend) { }
	public virtual void OnEnemyBeforeAttack(Professional enemy) { }

	public virtual void OnFriendAfterAttack(Professional friend) { }
	public virtual void OnEnemyAfterAttack(Professional enemy) { }

	public virtual void OnFriendHurt(Professional friend) { }
	public virtual void OnEnemyHurt(Professional enemy) { }

	public virtual void OnFriendDie(Professional friend) { }
	public virtual void OnEnemyDie(Professional enemy) { }

	public virtual void OnFriendSpawn(Professional friend) { }
	public virtual void OnEnemySpawn(Professional enemy) { }

	public override string ToString() => $"{Attack} {CodeName} {Health}";
}
