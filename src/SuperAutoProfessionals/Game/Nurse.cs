namespace SuperAutoProfessionals;

public class Nurse : Professional
{
	public override string CodeName => "Nu";

	public override void OnFriendHurt(Professional friend)
	{
		base.OnFriendHurt(friend);

		if (friend == this || friend.IsDead) return;

		Game.Log($"Healing {friend.CodeName}");

		friend.Health += 3;
	}
}
