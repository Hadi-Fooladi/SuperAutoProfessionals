namespace SuperAutoProfessionals;

public class Buthcer : Professional
{
	public override string CodeName => "Bu";

	internal override bool On(Event e)
	{
		base.On(e);

		if (e.Code != EventCode.BeforeAttack) return false;

		var p = e.Professional!;
		if (p != this) return false;

		var enemey = EnemyTeam.First;
		if (enemey == null) return false;

		Log($"Dealing 5 damage to {enemey}");
		Game.Attack(enemey, 5);

		return true;
	}
}