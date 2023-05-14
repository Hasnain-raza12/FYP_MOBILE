public class NullCheckable
{
	public static implicit operator bool(NullCheckable o)
	{
		return o != null;
	}
}
