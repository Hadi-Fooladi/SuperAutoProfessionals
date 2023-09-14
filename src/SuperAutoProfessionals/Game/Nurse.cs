namespace SuperAutoProfessionals;

public class Nurse : Professional
{
	public override string CodeName => "Nu";

	internal override bool On(Event e)
	{
		base.On(e);

		if (e.Code != EventCode.Hurt) return false;

		var p = e.Professional!;
		if (p == this || p.IsDead || IsEnemy(p)) return false;

		p.Health += 3;

		Log($"Healing {p}");
		return true;
	}
}
