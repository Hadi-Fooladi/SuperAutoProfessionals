namespace SuperAutoProfessionals;

public class Trainer : Professional
{
	public override string CodeName => "Tr";

	internal override bool On(Event e)
	{
		base.On(e);

		if (e.Code != EventCode.Die) return false;

		var p = e.Professional!;
		if (p != this) return false;

		var pos = p.Index;
		var friend = new Professional
		{
			Attack = 6,
			Health = 6
		};

		Log($"Trying to spawn {friend} at {pos}");
		Game.Spwan(p.Team, friend, pos);
		return true;
	}
}
