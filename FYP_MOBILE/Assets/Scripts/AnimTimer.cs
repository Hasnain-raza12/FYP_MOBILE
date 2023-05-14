public struct AnimTimer
{
	private bool enabled;

	private bool running;

	private float elapsed;

	private float duration;

	private float nt;

	private float ntPrev;

	public bool Enabled => enabled;

	public bool Running => running;

	public bool Completed => !running;

	public float Elapsed => elapsed;

	public float Duration => duration;

	public float Nt => nt;

	public float NtPrev => ntPrev;

	public void Reset(float t = 0f)
	{
		enabled = false;
		running = false;
		elapsed = 0f;
		nt = t;
		ntPrev = t;
	}

	public void Start(float duration)
	{
		enabled = true;
		running = true;
		nt = 0f;
		ntPrev = 0f;
		elapsed = 0f;
		this.duration = ((duration <= 0f) ? 0f : duration);
	}

	public void Update(float dt)
	{
		if (!enabled)
		{
			return;
		}
		ntPrev = nt;
		if (running)
		{
			elapsed += dt;
			if (elapsed > duration)
			{
				nt = 1f;
				running = false;
			}
			else if (duration > 0.0001f)
			{
				nt = elapsed / duration;
			}
			else
			{
				nt = 0f;
			}
		}
	}

	public void Disable()
	{
		enabled = false;
		running = false;
	}
}
