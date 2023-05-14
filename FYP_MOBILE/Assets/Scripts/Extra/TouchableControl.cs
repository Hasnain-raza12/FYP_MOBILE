using UnityEngine;

public class TouchableControl
{
	public bool initiallyDisabled;

	public bool initiallyHidden;

	protected bool enabled;

	protected bool visible;

	public int prio;

	public float hitDistScale;

	public string name;

	public bool disableGui;

	public int guiDepth;

	public int layoutBoxId;

	public bool acceptSharedTouches;

	protected TouchController joy;

	public virtual void Init(TouchController joy)
	{
		this.joy = joy;
		visible = true;
		enabled = true;
	}

	public virtual TouchController.HitTestResult HitTest(Vector2 pos, int touchId)
	{
		return new TouchController.HitTestResult(hit: false);
	}

	public virtual TouchController.EventResult OnTouchStart(int touchId, Vector2 pos)
	{
		return TouchController.EventResult.NOT_HANDLED;
	}

	public virtual TouchController.EventResult OnTouchEnd(int touchId, bool cancel = false)
	{
		return TouchController.EventResult.NOT_HANDLED;
	}

	public virtual TouchController.EventResult OnTouchMove(int touchId, Vector2 pos)
	{
		return TouchController.EventResult.NOT_HANDLED;
	}

	public virtual void OnReset()
	{
	}

	public virtual void OnPrePoll()
	{
	}

	public virtual void OnPostPoll()
	{
	}

	public virtual void OnUpdate(bool firstPostPollUpdate)
	{
	}

	public virtual void OnPostUpdate(bool firstPostPollUpdate)
	{
	}

	public virtual void OnLayoutAddContent()
	{
	}

	public virtual void OnLayout()
	{
	}

	public virtual void DrawGUI()
	{
	}

	public virtual void ReleaseTouches()
	{
	}

	public virtual void TakeoverTouches(TouchableControl controlToUntouch)
	{
	}

	public virtual void ResetRect()
	{
	}

	public void DisableGUI()
	{
		disableGui = true;
	}

	public void EnableGUI()
	{
		disableGui = false;
	}

	public bool DefaultGUIEnabled()
	{
		return !disableGui;
	}

	public bool Enabled()
	{
		return enabled;
	}

	public virtual void Enable(bool skipAnimation)
	{
		enabled = true;
	}

	public void Enable()
	{
		Enable(skipAnimation: false);
	}

	public virtual void Disable(bool skipAnimation)
	{
		enabled = false;
		ReleaseTouches();
	}

	public void Disable()
	{
		Disable(skipAnimation: false);
	}

	public virtual void Show(bool skipAnim)
	{
		visible = true;
	}

	public void Show()
	{
		Show(skipAnim: false);
	}

	public virtual void Hide(bool skipAnim)
	{
		visible = false;
		ReleaseTouches();
	}

	public void Hide()
	{
		Hide(skipAnim: false);
	}
}
