using UnityEngine;

public class CFInput
{
	public static TouchController ctrl;

	public static Vector3 mousePosition
	{
		get
		{
			if (ControllerActive())
			{
				return ctrl.GetMousePos();
			}
			return Input.mousePosition;
		}
	}

	public static bool ControllerActive()
	{
		if (ctrl != null)
		{
			return ctrl.enabled;
		}
		return false;
	}

	public static bool GetKey(KeyCode key)
	{
		if (ControllerActive())
		{
			bool keySupported = false;
			bool keyEx = ctrl.GetKeyEx(key, out keySupported);
			if (keySupported)
			{
				return keyEx;
			}
		}
		return Input.GetKey(key);
	}

	public static bool GetKeyDown(KeyCode key)
	{
		if (ControllerActive())
		{
			bool keySupported = false;
			bool keyDownEx = ctrl.GetKeyDownEx(key, out keySupported);
			if (keySupported)
			{
				return keyDownEx;
			}
		}
		return Input.GetKeyDown(key);
	}

	public static bool GetKeyUp(KeyCode key)
	{
		if (ControllerActive())
		{
			bool keySupported = false;
			bool keyUpEx = ctrl.GetKeyUpEx(key, out keySupported);
			if (keySupported)
			{
				return keyUpEx;
			}
		}
		return Input.GetKeyUp(key);
	}

	public static bool GetButton(string axisName)
	{
		if (ControllerActive())
		{
			bool buttonSupported = false;
			bool buttonEx = ctrl.GetButtonEx(axisName, out buttonSupported);
			if (buttonSupported)
			{
				return buttonEx;
			}
		}
		try
		{
			return Input.GetButton(axisName);
		}
		catch (UnityException)
		{
		}
		return false;
	}

	public static bool GetButtonDown(string axisName)
	{
		if (ControllerActive())
		{
			bool buttonSupported = false;
			bool buttonDownEx = ctrl.GetButtonDownEx(axisName, out buttonSupported);
			if (buttonSupported)
			{
				return buttonDownEx;
			}
		}
		try
		{
			return Input.GetButtonDown(axisName);
		}
		catch (UnityException)
		{
		}
		return false;
	}

	public static bool GetButtonUp(string axisName)
	{
		if (ControllerActive())
		{
			bool buttonSupported = false;
			bool buttonUpEx = ctrl.GetButtonUpEx(axisName, out buttonSupported);
			if (buttonSupported)
			{
				return buttonUpEx;
			}
		}
		try
		{
			return Input.GetButtonUp(axisName);
		}
		catch (UnityException)
		{
		}
		return false;
	}

	public static float GetAxis(string axisName)
	{
		if (ControllerActive())
		{
			bool axisSupported = false;
			float axisEx = ctrl.GetAxisEx(axisName, out axisSupported);
			if (axisSupported)
			{
				return axisEx;
			}
		}
		try
		{
			return Input.GetAxis(axisName);
		}
		catch (UnityException)
		{
		}
		return 0f;
	}

	public static float GetAxisRaw(string axisName)
	{
		if (ControllerActive())
		{
			bool axisSupported = false;
			float axisEx = ctrl.GetAxisEx(axisName, out axisSupported);
			if (axisSupported)
			{
				return axisEx;
			}
		}
		try
		{
			return Input.GetAxisRaw(axisName);
		}
		catch (UnityException)
		{
		}
		return 0f;
	}

	public static bool GetMouseButton(int i)
	{
		if (ControllerActive())
		{
			return ctrl.GetMouseButton(i);
		}
		return Input.GetMouseButton(i);
	}

	public static bool GetMouseButtonDown(int i)
	{
		if (ControllerActive())
		{
			return ctrl.GetMouseButtonDown(i);
		}
		return Input.GetMouseButtonDown(i);
	}

	public static bool GetMouseButtonUp(int i)
	{
		if (ControllerActive())
		{
			return ctrl.GetMouseButtonUp(i);
		}
		return Input.GetMouseButtonUp(i);
	}

	public static void ResetInputAxes()
	{
		Input.ResetInputAxes();
		if (ControllerActive())
		{
			ctrl.ReleaseTouches();
		}
	}
}
