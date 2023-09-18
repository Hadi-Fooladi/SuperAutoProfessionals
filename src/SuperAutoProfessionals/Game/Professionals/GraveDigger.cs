namespace SuperAutoProfessionals;

/// <summary>
/// Gets +1/+1 when anyone dies (same team or the other team)
/// </summary>
public class GraveDigger : Professional
{
	public override string CodeName => "GD";

	internal override bool On(Event e)
	{
		base.On(e);

		if (e.Code != EventCode.Die) return false;

		var dead = e.Professional!;
		if (dead == this) return false;

		var before = $"Buffing {this}";
		Attack += 1;
		Health += 1;
		Log($"{before} to {this}");
		return true;
	}
}
