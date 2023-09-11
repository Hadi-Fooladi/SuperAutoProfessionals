namespace SuperAutoProfessionals;

public class Professional
{
	public Game Game { get; set; } = null!;

	public int Attack { get; set; }
	public int Health { get; set; }

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
