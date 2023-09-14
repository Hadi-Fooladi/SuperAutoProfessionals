namespace SuperAutoProfessionals;

class Event
{
	public Event(EventCode code, Professional? p)
	{
		Code = code;
		Professional = p;
	}

	public EventCode Code { get; }
	public Professional? Professional { get; }

	public static readonly Event StartOfBattle = new(EventCode.StartOfBattle, null);
}
