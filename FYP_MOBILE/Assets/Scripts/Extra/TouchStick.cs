using System;
using UnityEngine;

[Serializable]
public class TouchStick : TouchableControl
{
	public enum StickPosMode
	{
		FULL_ANALOG,
		ANALOG_8WAY,
		ANALOG_4WAY,
		DIGITAL_8WAY,
		DIGITAL_4WAY
	}

	public enum StickDir
	{
		NEUTRAL,
		U,
		UR,
		R,
		DR,
		D,
		DL,
		L,
		UL
	}

	public enum Vec3DMode
	{
		XZ,
		XY
	}

	private const StickDir StickDirFirst = StickDir.U;

	private const StickDir StickDirLast = StickDir.UL;

	private float safeAngle;

	public StickPosMode stickVis;

	public bool smoothReturn = true;

	public Vector2 posCm = new Vector2(2f, 5f);

	public float sizeCm = 2f;

	private Vector2 layoutPosPx;

	private float layoutRadPx;

	private Vector2 posPx = new Vector2(100f, 100f);

	private float radPx = 40f;

	public bool overrideAnimDuration;

	public float pressAnimDuration;

	public float releaseAnimDuration;

	public float disableAnimDuration;

	public float enableAnimDuration;

	public float hideAnimDuration;

	public float showAnimDuration;

	private AnimTimer animTimer;

	private TouchController.AnimFloat animHatScale;

	private TouchController.AnimFloat animBaseScale;

	private TouchController.AnimFloat animAlpha;

	private TouchController.AnimColor animHatColor;

	private TouchController.AnimColor animBaseColor;

	private bool dynamicFadeOutAnimPending;

	public bool keyboardEmu;

	public KeyCode keyUp = KeyCode.W;

	public KeyCode keyDown = KeyCode.S;

	public KeyCode keyLeft = KeyCode.A;

	public KeyCode keyRight = KeyCode.D;

	public bool dynamicMode;

	public int dynamicRegionPrio;

	public bool dynamicClamp;

	public float dynamicMaxRelativeSize = 0.2f;

	public float dynamicMarginCm = 0.5f;

	public float dynamicFadeOutDelay;

	public float dynamicFadeOutDuration = 2f;

	public Rect dynamicRegion = new Rect(0f, 0f, 0.5f, 1f);

	private Rect dynamicRegionPx = new Rect(0f, 0f, 1f, 1f);

	public bool dynamicAlwaysReset;

	private bool dynamicResetPos;

	public float hatMoveScale = 0.5f;

	public bool disableX;

	public bool disableY;

	private Vector2 touchStart;

	private int touchId;

	private bool touchVerified;

	private Vector2 pollPos;

	private float angle;

	private Vector2 posRaw;

	private Vector2 dirVec;

	private float tilt;

	private Vector2 displayPosStart;

	private Vector2 displayPos;

	private StickDir dir8way;

	private StickDir dir4way;

	private StickDir dir8wayPrev;

	private StickDir dir4wayPrev;

	private StickDir dir8wayLastNonNeutral;

	private StickDir dir4wayLastNonNeutral;

	private bool pressedCur;

	private bool pressedPrev;

	public Texture2D releasedHatImg;

	public Texture2D releasedBaseImg;

	public Texture2D pressedHatImg;

	public Texture2D pressedBaseImg;

	public bool overrideScale;

	public float releasedHatScale = 1f;

	public float pressedHatScale = 1f;

	public float disabledHatScale = 1f;

	public float releasedBaseScale = 1f;

	public float pressedBaseScale = 1f;

	public float disabledBaseScale = 1f;

	public bool overrideColors;

	public Color releasedHatColor;

	public Color releasedBaseColor;

	public Color pressedHatColor;

	public Color pressedBaseColor;

	public Color disabledHatColor;

	public Color disabledBaseColor;

	private bool touchCanceled;

	public bool enableGetKey;

	public KeyCode getKeyCodePress;

	public KeyCode getKeyCodePressAlt;

	public KeyCode getKeyCodeUp;

	public KeyCode getKeyCodeUpAlt;

	public KeyCode getKeyCodeDown;

	public KeyCode getKeyCodeDownAlt;

	public KeyCode getKeyCodeLeft;

	public KeyCode getKeyCodeLeftAlt;

	public KeyCode getKeyCodeRight;

	public KeyCode getKeyCodeRightAlt;

	public bool enableGetButton;

	public string getButtonName;

	public bool enableGetAxis;

	public string axisHorzName;

	public string axisVertName;

	public bool axisHorzFlip;

	public bool axisVertFlip;

	public bool codeCustomGUI;

	public bool codeCustomLayout;

	[NonSerialized]
	private bool colorsDirtyFlag;

	public bool dynamicVisible => animAlpha.cur > 0.01f;

	public bool Pressed()
	{
		return pressedCur;
	}

	public bool JustPressed()
	{
		if (pressedCur)
		{
			return !pressedPrev;
		}
		return false;
	}

	public bool JustReleased()
	{
		if (!pressedCur)
		{
			return pressedPrev;
		}
		return false;
	}

	public float GetTilt()
	{
		return tilt;
	}

	public float GetAngle()
	{
		return angle;
	}

	public Vector2 GetVec()
	{
		return posRaw;
	}

	public Vector2 GetNormalizedVec()
	{
		return dirVec;
	}

	public Vector2 GetVecEx(StickPosMode vis)
	{
		float dirCodeAngle = angle;
		float num = tilt;
		switch (vis)
		{
		case StickPosMode.FULL_ANALOG:
			return posRaw;
		case StickPosMode.ANALOG_8WAY:
			dirCodeAngle = GetDirCodeAngle(dir8way);
			num = ((dir8way == StickDir.NEUTRAL) ? 0f : num);
			break;
		case StickPosMode.ANALOG_4WAY:
			dirCodeAngle = GetDirCodeAngle(dir4way);
			num = ((dir4way == StickDir.NEUTRAL) ? 0f : num);
			break;
		case StickPosMode.DIGITAL_8WAY:
			dirCodeAngle = GetDirCodeAngle(dir8way);
			num = ((dir8way != 0) ? 1 : 0);
			break;
		case StickPosMode.DIGITAL_4WAY:
			dirCodeAngle = GetDirCodeAngle(dir4way);
			num = ((dir4way != 0) ? 1 : 0);
			break;
		}
		return RotateVec2(new Vector2(0f, 1f), dirCodeAngle) * num;
	}

	public Vector3 GetVec3d(bool normalized, float orientByAngle)
	{
		Vector2 pos = ((!normalized) ? posRaw : dirVec);
		if (orientByAngle != 0f)
		{
			pos = RotateVec2(pos, orientByAngle);
		}
		return new Vector3(pos.x, 0f, pos.y);
	}

	public Vector3 GetVec3d(Vec3DMode vecMode, bool normalized, float orientByAngle)
	{
		Vector2 pos = ((!normalized) ? posRaw : dirVec);
		if (orientByAngle != 0f)
		{
			pos = RotateVec2(pos, orientByAngle);
		}
		switch (vecMode)
		{
		case Vec3DMode.XZ:
			return new Vector3(pos.x, 0f, pos.y);
		default:
			return Vector3.zero;
		case Vec3DMode.XY:
			return new Vector3(pos.x, pos.y, 0f);
		}
	}

	public Vector3 GetVec3d(Vec3DMode vecMode, bool normalized)
	{
		return GetVec3d(vecMode, normalized, 0f);
	}

	public StickDir GetDigitalDir(bool eightWayMode)
	{
		if (eightWayMode)
		{
			return dir8way;
		}
		return dir4way;
	}

	public StickDir GetDigitalDir()
	{
		return dir8way;
	}

	public StickDir GetFourWayDir()
	{
		return dir4way;
	}

	public StickDir GetPrevDigitalDir(bool eightWayMode = true)
	{
		if (eightWayMode)
		{
			return dir8wayPrev;
		}
		return dir4wayPrev;
	}

	public StickDir GetPrevDigitalDir()
	{
		return dir8wayPrev;
	}

	public StickDir GetPrevFourWayDir()
	{
		return dir4wayPrev;
	}

	public bool DigitalJustChanged(bool eightWayMode)
	{
		if (eightWayMode)
		{
			return dir8way != dir8wayPrev;
		}
		return dir4way != dir4wayPrev;
	}

	public bool DigitalJustChanged()
	{
		return dir8way != dir8wayPrev;
	}

	public bool FourWayJustChanged()
	{
		return dir4way != dir4wayPrev;
	}

	private static bool KeyCodeInDir(KeyCode keyCode, StickDir dir)
	{
		if (dir == StickDir.NEUTRAL)
		{
			return false;
		}
		switch (keyCode)
		{
		default:
			return false;
		case KeyCode.W:
		case KeyCode.UpArrow:
			if (dir != StickDir.U && dir != StickDir.UL)
			{
				return dir == StickDir.UR;
			}
			return true;
		case KeyCode.S:
		case KeyCode.DownArrow:
			if (dir != StickDir.D && dir != StickDir.DL)
			{
				return dir == StickDir.DR;
			}
			return true;
		case KeyCode.A:
		case KeyCode.LeftArrow:
			if (dir != StickDir.L && dir != StickDir.DL)
			{
				return dir == StickDir.UL;
			}
			return true;
		case KeyCode.D:
		case KeyCode.RightArrow:
			if (dir != StickDir.R && dir != StickDir.DR)
			{
				return dir == StickDir.UR;
			}
			return true;
		}
	}

	public float GetAxis(string name)
	{
		bool supported = false;
		return GetAxisEx(name, out supported);
	}

	public float GetAxisEx(string name, out bool supported)
	{
		if (!enableGetAxis)
		{
			supported = false;
			return 0f;
		}
		if (name == axisHorzName)
		{
			supported = true;
			if (axisHorzFlip)
			{
				return 0f - posRaw.x;
			}
			return posRaw.x;
		}
		if (name == axisVertName)
		{
			supported = true;
			if (axisVertFlip)
			{
				return 0f - posRaw.y;
			}
			return posRaw.y;
		}
		supported = false;
		return 0f;
	}

	public bool GetButton(string buttonName)
	{
		bool buttonSupported = false;
		return GetButtonEx(buttonName, out buttonSupported);
	}

	public bool GetButtonDown(string buttonName)
	{
		bool buttonSupported = false;
		return GetButtonDownEx(buttonName, out buttonSupported);
	}

	public bool GetButtonUp(string buttonName)
	{
		bool buttonSupported = false;
		return GetButtonUpEx(buttonName, out buttonSupported);
	}

	public bool GetButtonEx(string buttonName, out bool buttonSupported)
	{
		if (buttonSupported = enableGetButton && buttonName == getButtonName)
		{
			return Pressed();
		}
		return false;
	}

	public bool GetButtonDownEx(string buttonName, out bool buttonSupported)
	{
		if (buttonSupported = enableGetButton && buttonName == getButtonName)
		{
			return JustPressed();
		}
		return false;
	}

	public bool GetButtonUpEx(string buttonName, out bool buttonSupported)
	{
		if (buttonSupported = enableGetButton && buttonName == getButtonName)
		{
			return JustReleased();
		}
		return false;
	}

	public bool GetKey(KeyCode key)
	{
		if (enableGetKey && key != 0)
		{
			if ((key != getKeyCodePress && key != getKeyCodePressAlt) || !Pressed())
			{
				if (dir8way != 0)
				{
					return CheckKeyCode(key, dir8way);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public bool GetKeyDown(KeyCode key)
	{
		if (enableGetKey && key != 0)
		{
			if ((key != getKeyCodePress && key != getKeyCodePressAlt) || !JustPressed())
			{
				if (dir8way != dir8wayPrev && CheckKeyCode(key, dir8way))
				{
					return !CheckKeyCode(key, dir8wayPrev);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public bool GetKeyUp(KeyCode key)
	{
		if (enableGetKey && key != 0)
		{
			if ((key != getKeyCodePress && key != getKeyCodePressAlt) || !JustReleased())
			{
				if (dir8way != dir8wayPrev && !CheckKeyCode(key, dir8way))
				{
					return CheckKeyCode(key, dir8wayPrev);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public bool GetKeyEx(KeyCode key, out bool keySupported)
	{
		keySupported = IsKeySupported(key);
		return GetKey(key);
	}

	public bool GetKeyDownEx(KeyCode key, out bool keySupported)
	{
		keySupported = IsKeySupported(key);
		return GetKeyDown(key);
	}

	public bool GetKeyUpEx(KeyCode key, out bool keySupported)
	{
		keySupported = IsKeySupported(key);
		return GetKeyUp(key);
	}

	public bool IsKeySupported(KeyCode key)
	{
		if (enableGetKey && key != getKeyCodePress && key != getKeyCodePressAlt && key != getKeyCodeUp && key != getKeyCodeUpAlt && key != getKeyCodeDown && key != getKeyCodeDownAlt && key != getKeyCodeLeft && key != getKeyCodeLeftAlt && key != getKeyCodeRight)
		{
			return key == getKeyCodeRightAlt;
		}
		return true;
	}

	private bool CheckKeyCode(KeyCode key, StickDir dir)
	{
		if (dir == StickDir.NEUTRAL)
		{
			return false;
		}
		if (key == getKeyCodeUp || key == getKeyCodeUpAlt)
		{
			if (dir != StickDir.U && dir != StickDir.UL)
			{
				return dir == StickDir.UR;
			}
			return true;
		}
		if (key == getKeyCodeDown || key == getKeyCodeDownAlt)
		{
			if (dir != StickDir.D && dir != StickDir.DL)
			{
				return dir == StickDir.DR;
			}
			return true;
		}
		if (key == getKeyCodeLeft || key == getKeyCodeLeftAlt)
		{
			if (dir != StickDir.L && dir != StickDir.DL)
			{
				return dir == StickDir.UL;
			}
			return true;
		}
		if (key == getKeyCodeRight || key == getKeyCodeRightAlt)
		{
			if (dir != StickDir.R && dir != StickDir.DR)
			{
				return dir == StickDir.UR;
			}
			return true;
		}
		return false;
	}

	public void SetDynamicMode(bool dynamicMode)
	{
		if (this.dynamicMode != dynamicMode)
		{
			this.dynamicMode = dynamicMode;
			joy.SetLayoutDirtyFlag();
		}
	}

	public override void Enable(bool skipAnimation)
	{
		enabled = true;
		AnimateParams((!overrideScale) ? joy.releasedStickHatScale : releasedHatScale, (!overrideScale) ? joy.releasedStickBaseScale : releasedBaseScale, (!overrideColors) ? joy.defaultReleasedStickHatColor : releasedHatColor, (!overrideColors) ? joy.defaultReleasedStickBaseColor : releasedBaseColor, (!dynamicMode) ? 1 : 0, skipAnimation ? 0f : ((!overrideAnimDuration) ? joy.enableAnimDuration : enableAnimDuration));
	}

	public override void Disable(bool skipAnimation)
	{
		enabled = false;
		ReleaseTouches();
		AnimateParams((!overrideScale) ? joy.disabledStickHatScale : disabledHatScale, (!overrideScale) ? joy.disabledStickBaseScale : disabledBaseScale, (!overrideColors) ? joy.defaultDisabledStickHatColor : disabledHatColor, (!overrideColors) ? joy.defaultDisabledStickBaseColor : disabledBaseColor, (!dynamicMode) ? 1 : 0, skipAnimation ? 0f : ((!overrideAnimDuration) ? joy.disableAnimDuration : disableAnimDuration));
	}

	public override void Show(bool skipAnim)
	{
		visible = true;
		AnimateParams(overrideScale ? ((!enabled) ? disabledHatScale : releasedHatScale) : ((!enabled) ? joy.disabledStickHatScale : joy.releasedStickHatScale), overrideScale ? ((!enabled) ? disabledBaseScale : releasedBaseScale) : ((!enabled) ? joy.disabledStickBaseScale : joy.releasedStickBaseScale), overrideColors ? ((!enabled) ? disabledHatColor : releasedHatColor) : ((!enabled) ? joy.defaultDisabledStickHatColor : joy.defaultReleasedStickHatColor), overrideColors ? ((!enabled) ? disabledBaseColor : releasedBaseColor) : ((!enabled) ? joy.defaultDisabledStickBaseColor : joy.defaultReleasedStickBaseColor), (!dynamicMode) ? 1 : (Pressed() ? 1 : 0), skipAnim ? 0f : ((!overrideAnimDuration) ? joy.showAnimDuration : showAnimDuration));
	}

	public override void Hide(bool skipAnim)
	{
		visible = false;
		ReleaseTouches();
		Color end = animHatColor.end;
		Color end2 = animBaseColor.end;
		AnimateParams(animHatScale.end, animBaseScale.end, end, end2, 0f, skipAnim ? 0f : ((!overrideAnimDuration) ? joy.hideAnimDuration : hideAnimDuration));
	}

	public void SetRect(Rect r)
	{
		Vector2 center = r.center;
		float num = Mathf.Min(r.width, r.height) / 2f;
		if (!dynamicMode && (posPx != center || radPx != num))
		{
			posPx = center;
			radPx = num;
			OnReset();
		}
	}

	public override void ResetRect()
	{
		if (!dynamicMode)
		{
			posPx = layoutPosPx;
			radPx = layoutRadPx;
		}
	}

	public Rect GetRect(bool getAutoRect)
	{
		return TouchController.GetCenRect((!getAutoRect) ? posPx : layoutPosPx, ((!getAutoRect) ? radPx : layoutRadPx) * 2f);
	}

	public Rect GetRect()
	{
		return GetRect(getAutoRect: false);
	}

	public Vector2 GetScreenPos()
	{
		return posPx;
	}

	public float GetScreenRad()
	{
		return radPx;
	}

	public Rect GetHatDisplayRect(bool applyScale)
	{
		return TouchController.GetCenRect(posPx + InternalToScreenPos(displayPos) * radPx * hatMoveScale, 2f * radPx * ((!applyScale) ? 1f : animHatScale.cur));
	}

	public Rect GetHatDisplayRect()
	{
		return GetHatDisplayRect(applyScale: true);
	}

	public Rect GetBaseDisplayRect(bool applyScale = true)
	{
		return TouchController.GetCenRect(posPx, radPx * 2f * ((!applyScale) ? 1f : animBaseScale.cur));
	}

	public Rect GetBaseDisplayRect()
	{
		return GetBaseDisplayRect(applyScale: true);
	}

	public Color GetHatColor()
	{
		return animHatColor.cur;
	}

	public Color GetBaseColor()
	{
		return animBaseColor.cur;
	}

	public int GetGUIDepth()
	{
		return joy.guiDepth + guiDepth + (Pressed() ? joy.guiPressedOfs : 0);
	}

	public Texture2D GetBaseDisplayTex()
	{
		if (enabled && Pressed())
		{
			return pressedBaseImg;
		}
		return releasedBaseImg;
	}

	public Texture2D GetHatDisplayTex()
	{
		if (enabled && Pressed())
		{
			return pressedHatImg;
		}
		return releasedHatImg;
	}

	public static bool IsDiagonalAxis(StickDir dir)
	{
		return ((dir - 1) & StickDir.U) == StickDir.U;
	}

	public static float GetDirCodeAngle(StickDir d)
	{
		if (d < StickDir.U || d > StickDir.UL)
		{
			return 0f;
		}
		return (float)(d - 1) * 45f;
	}

	public static StickDir GetDirCodeFromAngle(float ang, bool as8way)
	{
		ang += ((!as8way) ? 45f : 22.5f);
		ang = NormalizeAnglePositive(ang);
		if (as8way)
		{
			if (ang < 45f)
			{
				return StickDir.U;
			}
			if (ang < 90f)
			{
				return StickDir.UR;
			}
			if (ang < 135f)
			{
				return StickDir.R;
			}
			if (ang < 180f)
			{
				return StickDir.DR;
			}
			if (ang < 225f)
			{
				return StickDir.D;
			}
			if (ang < 270f)
			{
				return StickDir.DL;
			}
			if (ang < 315f)
			{
				return StickDir.L;
			}
			return StickDir.UL;
		}
		if (ang < 90f)
		{
			return StickDir.U;
		}
		if (ang < 180f)
		{
			return StickDir.R;
		}
		if (ang < 270f)
		{
			return StickDir.D;
		}
		return StickDir.L;
	}

	private void AnimateParams(float hatScale, float baseScale, Color hatColor, Color baseColor, float alpha, float duration)
	{
		if (duration <= 0f)
		{
			animTimer.Reset();
			animTimer.Disable();
			animHatColor.Reset(hatColor);
			animHatScale.Reset(hatScale);
			animBaseColor.Reset(baseColor);
			animBaseScale.Reset(baseScale);
			animAlpha.Reset(alpha);
			displayPosStart = (displayPos = ((!Pressed()) ? Vector2.zero : GetVecEx(stickVis)));
		}
		else
		{
			animTimer.Start(duration);
			animHatScale.MoveTo(hatScale);
			animHatColor.MoveTo(hatColor);
			animBaseScale.MoveTo(baseScale);
			animBaseColor.MoveTo(baseColor);
			animAlpha.MoveTo(alpha);
		}
	}

	public override void Init(TouchController joy)
	{
		base.Init(joy);
		OnReset();
		if (initiallyDisabled)
		{
			Disable(skipAnimation: true);
		}
		if (initiallyHidden)
		{
			Hide(skipAnim: true);
		}
	}

	public override void OnReset()
	{
		pressedCur = false;
		pressedPrev = false;
		touchId = -1;
		dir4way = StickDir.NEUTRAL;
		dir8way = StickDir.NEUTRAL;
		dir4wayLastNonNeutral = StickDir.NEUTRAL;
		dir8wayLastNonNeutral = StickDir.NEUTRAL;
		dir4wayPrev = StickDir.NEUTRAL;
		dir8wayPrev = StickDir.NEUTRAL;
		touchCanceled = false;
		SetInternalPos(Vector2.zero);
		tilt = 0f;
		dirVec = Vector2.zero;
		posRaw = Vector2.zero;
		displayPos = Vector2.zero;
		displayPosStart = Vector2.zero;
		AnimateParams((!overrideScale) ? joy.releasedStickHatScale : releasedHatScale, (!overrideScale) ? joy.releasedStickBaseScale : releasedBaseScale, (!overrideColors) ? joy.defaultReleasedStickHatColor : releasedHatColor, (!overrideColors) ? joy.defaultReleasedStickBaseColor : releasedBaseColor, (!dynamicMode) ? 1 : 0, 0f);
		if (!enabled)
		{
			Disable(skipAnimation: true);
		}
		if (!visible)
		{
			Hide(skipAnim: true);
		}
	}

	public static Vector2 InternalToScreenPos(Vector2 internalStickPos)
	{
		internalStickPos.y = 0f - internalStickPos.y;
		return internalStickPos;
	}

	private void SetPollPos(Vector2 pos, bool screenPos)
	{
		if (screenPos)
		{
			pos = (pos - posPx) / radPx;
			pos.y = 0f - pos.y;
		}
		pollPos = pos;
	}

	private void SetInternalPos(Vector2 pos)
	{
		pollPos = pos;
		if (disableX)
		{
			pos.x = 0f;
		}
		if (disableY)
		{
			pos.y = 0f;
		}
		float num = Mathf.Clamp01(pos.magnitude);
		Vector2 normalized = dirVec;
		float num2 = safeAngle;
		if (num > 0.01f)
		{
			normalized = pos.normalized;
			num2 = Mathf.Atan2(normalized.x, normalized.y) * 57.29578f;
		}
		if (num > ((dir8way != 0) ? joy.stickDigitalLeaveThresh : joy.stickDigitalEnterThresh))
		{
			if (dir8wayLastNonNeutral == StickDir.NEUTRAL)
			{
				dir4way = GetDirCodeFromAngle(num2, as8way: false);
				dir8way = GetDirCodeFromAngle(num2, as8way: true);
			}
			else if (num > joy.stickDigitalEnterThresh)
			{
				if (Mathf.Abs(Mathf.DeltaAngle(GetDirCodeAngle(dir8wayLastNonNeutral), num2)) > 22.5f + joy.stickMagnetAngleMargin)
				{
					dir8way = GetDirCodeFromAngle(num2, as8way: true);
				}
				else
				{
					dir8way = dir8wayLastNonNeutral;
				}
				if (Mathf.Abs(Mathf.DeltaAngle(GetDirCodeAngle(dir4wayLastNonNeutral), num2)) > 45f + joy.stickMagnetAngleMargin)
				{
					dir4way = GetDirCodeFromAngle(num2, as8way: false);
				}
				else
				{
					dir4way = dir4wayLastNonNeutral;
				}
			}
		}
		else
		{
			dir4way = StickDir.NEUTRAL;
			dir8way = StickDir.NEUTRAL;
		}
		if (dir4way != 0)
		{
			dir4wayLastNonNeutral = dir4way;
		}
		if (dir8way != 0)
		{
			dir8wayLastNonNeutral = dir8way;
		}
		tilt = num;
		angle = num2;
		safeAngle = num2;
		posRaw = normalized * num;
		dirVec = normalized;
	}

	public override void OnPrePoll()
	{
		touchVerified = false;
	}

	public override void OnPostPoll()
	{
		if (!touchVerified && touchId >= 0)
		{
			OnTouchEnd(touchId);
		}
	}

	public override void ReleaseTouches()
	{
		if (touchId >= 0)
		{
			OnTouchEnd(touchId, cancelMode: true);
		}
	}

	public override void TakeoverTouches(TouchableControl controlToUntouch)
	{
		if (controlToUntouch != null && touchId >= 0)
		{
			controlToUntouch.OnTouchEnd(touchId, cancel: true);
		}
	}

	public override void OnUpdate(bool firstUpdate)
	{
		dir8wayPrev = dir8way;
		dir4wayPrev = dir4way;
		pressedPrev = pressedCur;
		pressedCur = touchId >= 0;
		SetInternalPos(pollPos);
		if (pressedCur)
		{
			displayPos = (displayPosStart = GetVecEx(stickVis));
		}
		else if (!smoothReturn)
		{
			displayPos = (displayPosStart = Vector2.zero);
		}
		if (pressedCur != pressedPrev && enabled)
		{
			if (pressedCur)
			{
				dynamicFadeOutAnimPending = false;
				AnimateParams((!overrideScale) ? joy.pressedStickHatScale : pressedHatScale, (!overrideScale) ? joy.pressedStickBaseScale : pressedBaseScale, (!overrideColors) ? joy.defaultPressedStickHatColor : pressedHatColor, (!overrideColors) ? joy.defaultPressedStickBaseColor : pressedBaseColor, 1f, (!overrideAnimDuration) ? joy.pressAnimDuration : pressAnimDuration);
			}
			else
			{
				dynamicFadeOutAnimPending = dynamicMode && !touchCanceled;
				AnimateParams((!overrideScale) ? joy.releasedStickHatScale : releasedHatScale, (!overrideScale) ? joy.releasedStickBaseScale : releasedBaseScale, (!overrideColors) ? joy.defaultReleasedStickHatColor : releasedHatColor, (!overrideColors) ? joy.defaultReleasedStickBaseColor : releasedBaseColor, (!dynamicMode) ? 1f : ((!touchCanceled) ? animAlpha.cur : 0f), touchCanceled ? joy.cancelAnimDuration : ((!overrideAnimDuration) ? joy.releaseAnimDuration : releaseAnimDuration));
			}
		}
		if (!animTimer.Enabled)
		{
			return;
		}
		animTimer.Update(joy.deltaTime);
		float num = TouchController.SlowDownEase(animTimer.Nt);
		animAlpha.Update(num);
		animHatColor.Update(num);
		animHatScale.Update(num);
		animBaseColor.Update(num);
		animBaseScale.Update(num);
		if (smoothReturn && !Pressed())
		{
			displayPos = Vector2.Lerp(displayPosStart, Vector2.zero, num);
		}
		if (animTimer.Completed)
		{
			displayPosStart = displayPos;
			if (dynamicMode && dynamicFadeOutAnimPending)
			{
				dynamicFadeOutAnimPending = false;
				AnimateParams((!overrideScale) ? joy.releasedStickHatScale : releasedHatScale, (!overrideScale) ? joy.releasedStickBaseScale : releasedBaseScale, (!overrideColors) ? joy.defaultReleasedStickHatColor : releasedHatColor, (!overrideColors) ? joy.defaultReleasedStickBaseColor : releasedBaseColor, 0f, dynamicFadeOutDuration);
			}
			else
			{
				animTimer.Disable();
			}
		}
	}

	public override TouchController.HitTestResult HitTest(Vector2 pos, int touchId)
	{
		if (this.touchId >= 0 || !enabled || !visible)
		{
			return new TouchController.HitTestResult(hit: false);
		}
		TouchController.HitTestResult result;
		if (dynamicMode)
		{
			if (!dynamicAlwaysReset && dynamicVisible)
			{
				TouchController.HitTestResult hitTestResult = (result = joy.HitTestCircle(posPx, radPx, pos));
				if (hitTestResult.hit)
				{
					result.prio = prio;
					dynamicResetPos = false;
					return result;
				}
			}
			dynamicResetPos = true;
			result = joy.HitTestRect(dynamicRegionPx, pos);
			result.prio = dynamicRegionPrio;
			result.distScale = hitDistScale;
			return result;
		}
		result = joy.HitTestCircle(posPx, radPx, pos);
		result.prio = prio;
		result.distScale = hitDistScale;
		return result;
	}

	public override TouchController.EventResult OnTouchStart(int touchId, Vector2 touchPos)
	{
		if (dynamicMode && dynamicResetPos)
		{
			float num = Mathf.Min(joy.GetScreenHeight(), joy.GetScreenWidth());
			if (dynamicClamp)
			{
				float value = radPx + dynamicMarginCm * joy.GetDPCM();
				float num2 = (num - radPx * 2f) / 2f;
				value = ((num2 > 0f) ? Mathf.Clamp(value, 0f, num2) : 0f);
				posPx.x = Mathf.Clamp(touchPos.x, joy.GetScreenX(0f) + value, joy.GetScreenX(1f) - value);
				posPx.y = Mathf.Clamp(touchPos.y, joy.GetScreenY(0f) + value, joy.GetScreenY(1f) - value);
			}
			else
			{
				posPx = touchPos;
			}
		}
		touchCanceled = false;
		this.touchId = touchId;
		touchVerified = true;
		SetPollPos(touchPos, screenPos: true);
		return TouchController.EventResult.HANDLED;
	}

	public override TouchController.EventResult OnTouchEnd(int touchId, bool cancelMode = false)
	{
		if (this.touchId != touchId)
		{
			return TouchController.EventResult.NOT_HANDLED;
		}
		_ = dynamicMode;
		this.touchId = -1;
		touchVerified = true;
		touchCanceled = cancelMode;
		SetPollPos(Vector2.zero, screenPos: false);
		return TouchController.EventResult.HANDLED;
	}

	public override TouchController.EventResult OnTouchMove(int touchId, Vector2 touchPos)
	{
		if (this.touchId != touchId)
		{
			return TouchController.EventResult.NOT_HANDLED;
		}
		touchVerified = true;
		SetPollPos(touchPos, screenPos: true);
		return TouchController.EventResult.HANDLED;
	}

	public override void OnLayoutAddContent()
	{
		if (!dynamicMode)
		{
			joy.layoutBoxes[layoutBoxId].AddContent(posCm, sizeCm);
		}
	}

	public override void OnLayout()
	{
		dynamicRegionPx = joy.NormalizedRectToPx(dynamicRegion);
		if (dynamicMode)
		{
			radPx = CalculateDynamicRad();
		}
		else
		{
			layoutPosPx = joy.layoutBoxes[layoutBoxId].GetScreenPos(posCm);
			layoutRadPx = joy.layoutBoxes[layoutBoxId].GetScreenSize(sizeCm / 2f);
			posPx = layoutPosPx;
			radPx = layoutRadPx;
		}
		OnReset();
	}

	public override void DrawGUI()
	{
		if (!disableGui && !(joy.GetAlpha() * animAlpha.cur < 0.001f))
		{
			GUI.color = Color.white;
			bool num = Pressed();
			Color cur = animHatColor.cur;
			Color cur2 = animBaseColor.cur;
			Texture2D texture2D = ((!num) ? releasedHatImg : pressedHatImg);
			Texture2D texture2D2 = ((!num) ? releasedBaseImg : pressedBaseImg);
			GUI.depth = joy.guiDepth + guiDepth + (Pressed() ? joy.guiPressedOfs : 0);
			if (texture2D2 != null)
			{
				GUI.color = TouchController.ScaleAlpha(cur2, joy.GetAlpha() * animAlpha.cur);
				GUI.DrawTexture(GetBaseDisplayRect(applyScale: true), texture2D2);
			}
			if (texture2D != null)
			{
				GUI.color = TouchController.ScaleAlpha(cur, joy.GetAlpha() * animAlpha.cur);
				GUI.DrawTexture(GetHatDisplayRect(applyScale: true), texture2D);
			}
		}
	}

	private float CalculateDynamicRad()
	{
		float num = Mathf.Min(joy.GetScreenHeight(), joy.GetScreenWidth());
		return Mathf.Max(4f, 0.5f * Mathf.Min(sizeCm * joy.GetDPCM(), num * Mathf.Clamp(dynamicMaxRelativeSize, 0.01f, 1f)));
	}

	private static Vector2 RotateVec2(Vector2 pos, float ang)
	{
		float num = Mathf.Sin((0f - ang) * ((float)Math.PI / 180f));
		float num2 = Mathf.Cos((0f - ang) * ((float)Math.PI / 180f));
		return new Vector2(pos.x * num2 - pos.y * num, pos.x * num + pos.y * num2);
	}

	private static float NormalizeAnglePositive(float a)
	{
		if (a >= 360f)
		{
			return Mathf.Repeat(a, 360f);
		}
		if (a >= 0f)
		{
			return a;
		}
		if (a <= -360f)
		{
			a = Mathf.Repeat(a, 360f);
		}
		return 360f + a;
	}
}
