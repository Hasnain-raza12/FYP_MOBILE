using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("ControlFreak/Control Freak Controller")]
[ExecuteInEditMode]
public class TouchController : MonoBehaviour
{
	public enum RealWorldUnit
	{
		CM,
		INCH
	}

	public enum PreviewMode
	{
		RELEASED,
		PRESSED,
		DISABLED
	}

	public enum LayoutAnchor
	{
		BOTTOM_LEFT,
		BOTTOM_CENTER,
		BOTTOM_RIGHT,
		MID_LEFT,
		MID_CENTER,
		MID_RIGHT,
		TOP_LEFT,
		TOP_CENTER,
		TOP_RIGHT
	}

	public enum ControlShape
	{
		CIRCLE,
		RECT,
		SCREEN_REGION
	}

	public enum ScreenEmuMode
	{
		PIXEL_PERFECT,
		PHYSICAL,
		EXPAND
	}

	public enum EventResult
	{
		NOT_HANDLED,
		HANDLED,
		SHARED
	}

	public struct HitTestResult
	{
		public bool hit;

		public float dist;

		public int prio;

		public bool hitInside;

		public float distScale;

		public HitTestResult(bool hit)
		{
			this.hit = hit;
			dist = 1f;
			distScale = 1f;
			hitInside = false;
			prio = 0;
		}

		public bool IsCloserThan(HitTestResult r)
		{
			if (hit)
			{
				if (prio <= r.prio && (!hitInside || r.hitInside))
				{
					if (prio == r.prio)
					{
						return dist * distScale < r.dist * r.distScale;
					}
					return dist < r.dist;
				}
				return true;
			}
			return false;
		}
	}

	[Serializable]
	public class LayoutBox
	{
		public string name;

		public LayoutAnchor anchor;

		public bool allowNonuniformScale;

		public bool ignoreLeftHandedMode;

		public Rect normalizedRect;

		public float horzMarginMax;

		public float vertMarginMax;

		public bool uniformMargins;

		private TouchController joy;

		private Vector2 contentOfs;

		private Vector2 contentSize;

		private Vector2 contentPosScale;

		private float contentSizeScale;

		private Vector2 screenDstOfs;

		private Rect contentScreenBox;

		private Rect availableScreenBox;

		public Color debugColor = new Color(1f, 1f, 1f, 0.2f);

		public bool debugDraw;

		public LayoutBox(string name, float left, float top, float width, float height, LayoutAnchor anchor)
		{
			this.name = name;
			normalizedRect = new Rect(left, top, width, height);
			this.anchor = anchor;
			uniformMargins = true;
			horzMarginMax = 0.5f;
			vertMarginMax = 0.5f;
		}

		public void SetController(TouchController joy)
		{
			this.joy = joy;
		}

		public void ResetContent()
		{
			contentSize = Vector2.zero;
		}

		private void AddContentMinMax(Vector2 bbmin, Vector2 bbmax)
		{
			if (contentSize.x < 0.001f)
			{
				contentOfs = bbmin;
				contentSize = bbmax - bbmin;
				return;
			}
			Vector2 vector = contentOfs + contentSize;
			contentOfs.x = Mathf.Min(bbmin.x, contentOfs.x);
			contentOfs.y = Mathf.Min(bbmin.y, contentOfs.y);
			vector.x = Mathf.Max(bbmax.x, vector.x);
			vector.y = Mathf.Max(bbmax.y, vector.y);
			contentSize = vector - contentOfs;
		}

		public void AddContent(Vector2 cen, float size)
		{
			size *= 0.5f;
			Vector2 vector = new Vector2(size, size);
			AddContentMinMax(cen - vector, cen + vector);
		}

		public void AddContent(Vector2 cen, Vector2 size)
		{
			size *= 0.5f;
			AddContentMinMax(cen - size, cen + size);
		}

		public void ContentFinalize()
		{
			float screenX = joy.GetScreenX(normalizedRect.xMin);
			float screenX2 = joy.GetScreenX(normalizedRect.xMax);
			float screenY = joy.GetScreenY(normalizedRect.yMin);
			float screenY2 = joy.GetScreenY(normalizedRect.yMax);
			Vector2 vector = new Vector2(screenX2 - screenX, screenY2 - screenY);
			float num = joy.CmToPixels(contentSize.x);
			float num2 = joy.CmToPixels(contentSize.y);
			if (num < 0.01f || num2 < 0.01f)
			{
				contentPosScale = Vector2.one;
				contentSizeScale = 1f;
			}
			else
			{
				float num3 = Mathf.Clamp01(vector.x / num);
				float num4 = Mathf.Clamp01(vector.y / num2);
				contentSizeScale = Mathf.Clamp01(Mathf.Min(num3, num4));
				if (allowNonuniformScale)
				{
					contentPosScale = new Vector2(Mathf.Clamp01(num3), Mathf.Clamp01(num4));
				}
				else
				{
					contentPosScale = new Vector2(contentSizeScale, contentSizeScale);
				}
			}
			Vector2 vector2 = new Vector2(contentPosScale.x * num, contentPosScale.y * num2);
			contentPosScale *= joy.GetDPCM();
			contentSizeScale *= joy.GetDPCM();
			Vector2 topLeftOfs = vector - vector2;
			float maxMarginX = horzMarginMax * joy.GetDPCM();
			float maxMarginY = vertMarginMax * joy.GetDPCM();
			screenDstOfs = new Vector2(screenX, screenY) + AnchorLeftover(topLeftOfs, anchor, maxMarginX, maxMarginY, uniformMargins);
			contentScreenBox = new Rect(screenDstOfs.x, screenDstOfs.y, vector2.x, vector2.y);
			availableScreenBox = new Rect(screenX, screenY, vector.x, vector.y);
			if (!ignoreLeftHandedMode)
			{
				contentScreenBox = joy.RightHandToScreenRect(contentScreenBox);
				availableScreenBox = joy.RightHandToScreenRect(availableScreenBox);
			}
		}

		public Vector2 GetScreenPos(Vector2 pos)
		{
			pos -= contentOfs;
			pos.x *= contentPosScale.x;
			pos.y *= contentPosScale.y;
			pos += screenDstOfs;
			if (ignoreLeftHandedMode)
			{
				return pos;
			}
			return joy.RightHandToScreen(pos);
		}

		public float GetScreenSize(float size)
		{
			return size * contentSizeScale;
		}

		public Vector2 GetScreenSize(Vector2 size)
		{
			return size * contentSizeScale;
		}
	}

	public struct AnimFloat
	{
		public float start;

		public float end;

		public float cur;

		public void Reset(float val)
		{
			cur = val;
			end = val;
			start = val;
		}

		public void MoveTo(float val)
		{
			start = cur;
			end = val;
		}

		public void Update(float lerpt)
		{
			cur = Mathf.Lerp(start, end, lerpt);
		}
	}

	public struct AnimColor
	{
		public Color start;

		public Color end;

		public Color cur;

		public void Reset(Color val)
		{
			cur = val;
			end = val;
			start = val;
		}

		public void MoveTo(Color val)
		{
			start = cur;
			end = val;
		}

		public void Update(float lerpt)
		{
			cur = Color.Lerp(start, end, lerpt);
		}
	}

	public const int DEFAULT_ZONE_PRIO = 0;

	public const int DEFAULT_STICK_PRIO = 0;

	private const int MAX_EVENT_SHARE_COUNT = 8;

	private const float NON_MOBILE_DIAGONAL_INCHES = 7f;

	private const float DEFAULT_MONITOR_DPI = 96f;

	public const int LayoutBoxCount = 16;

	[NonSerialized]
	private const double EDITOR_SAFETY_UPDATE_INTERVAL = 2.0;

	public bool automaticMode = true;

	public bool manualGui;

	public bool autoActivate = true;

	public bool disableWhenNoTouchScreen;

	public int guiDepth = 10;

	public int guiPressedOfs = 10;

	public float fingerBufferCm = 0.8f;

	private float fingerBufferRadPx = 10f;

	public float stickMagnetAngleMargin = 10f;

	public float stickDigitalEnterThresh = 0.5f;

	public float stickDigitalLeaveThresh = 0.4f;

	public float touchTapMaxTime = 142f / (339f * (float)Math.PI);

	public float doubleTapMaxGapTime = 0.33333334f;

	public float strictMultiFingerMaxTime = 0.2f;

	public float velPreserveTime = 0.1f;

	public float touchTapMaxDistCm = 0.5f;

	public float twistThresh = 5f;

	public float pinchMinDistCm = 0.5f;

	public float twistSafeFingerDistCm = 1f;

	public float curTime;

	public float deltaTime = 0.016666668f;

	public float invDeltaTime = 60f;

	private float lastRealTime;

	private bool initialized;

	public TouchStick[] sticks;

	public TouchZone[] touchZones;

	public LayoutBox[] layoutBoxes;

	[NonSerialized]
	private TouchStick blankStick;

	[NonSerialized]
	private TouchZone blankZone;

	[NonSerialized]
	private List<TouchableControl> touchables;

	[NonSerialized]
	private List<Rect> maskAreas;

	private bool layoutDirtyFlag;

	private bool contentDirtyFlag;

	private bool releaseTouchesFlag;

	public float pressAnimDuration = 0.1f;

	public float releaseAnimDuration = 0.3f;

	public float disableAnimDuration = 0.3f;

	public float enableAnimDuration = 0.3f;

	public float cancelAnimDuration = 0.3f;

	public float showAnimDuration = 0.3f;

	public float hideAnimDuration = 0.3f;

	public float releasedZoneScale = 1f;

	public float pressedZoneScale = 1.1f;

	public float disabledZoneScale = 1f;

	public float releasedStickHatScale = 0.75f;

	public float pressedStickHatScale = 0.9f;

	public float disabledStickHatScale = 0.75f;

	public float releasedStickBaseScale = 1f;

	public float pressedStickBaseScale = 0.9f;

	public float disabledStickBaseScale = 1f;

	public Color defaultPressedZoneColor = new Color(1f, 1f, 1f, 1f);

	public Color defaultReleasedZoneColor = new Color(1f, 1f, 1f, 0.75f);

	public Color defaultDisabledZoneColor = new Color(0.5f, 0.5f, 0.5f, 0.35f);

	public Color defaultPressedStickHatColor = new Color(1f, 1f, 1f, 1f);

	public Color defaultReleasedStickHatColor = new Color(1f, 1f, 1f, 0.75f);

	public Color defaultDisabledStickHatColor = new Color(0.5f, 0.5f, 0.5f, 0.35f);

	public Color defaultPressedStickBaseColor = new Color(1f, 1f, 1f, 1f);

	public Color defaultReleasedStickBaseColor = new Color(1f, 1f, 1f, 0.75f);

	public Color defaultDisabledStickBaseColor = new Color(0.5f, 0.5f, 0.5f, 0.35f);

	private float globalAlpha = 1f;

	private float globalAlphaStart;

	private float globalAlphaEnd;

	private AnimTimer globalAlphaTimer;

	private int screenWidth;

	private int screenHeight;

	private bool disableAll;

	private bool leftHandedMode;

	public KeyCode debugSecondTouchDragModeKey = KeyCode.LeftShift;

	public KeyCode debugSecondTouchPinchModeKey = KeyCode.LeftControl;

	public KeyCode debugSecondTouchTwistModeKey = KeyCode.LeftAlt;

	public bool debugDrawTouches = true;

	public bool debugDrawLayoutBoxes = true;

	public bool debugDrawAreas = true;

	public Texture2D debugTouchSprite;

	public Texture2D debugSecondTouchSprite;

	public Color debugFirstTouchNormalColor = new Color(1f, 1f, 0.6f, 0.3f);

	public Color debugFirstTouchActiveColor = new Color(1f, 1f, 0f, 0.7f);

	public Color debugSecondTouchNormalColor = new Color(1f, 1f, 1f, 0.3f);

	public Color debugSecondTouchActiveColor = new Color(1f, 0f, 0f, 0.6f);

	public Texture2D defaultZoneImg;

	public Texture2D defaultStickHatImg;

	public Texture2D defaultStickBaseImg;

	public Texture2D debugCircleImg;

	public Texture2D debugRectImg;

	public bool screenEmuOn;

	public bool screenEmuPortrait;

	public bool screenEmuShrink = true;

	public Vector2 screenEmuPan = new Vector2(0.5f, 0.5f);

	public int screenEmuHwDpi = 250;

	public ScreenEmuMode screenEmuMode = ScreenEmuMode.EXPAND;

	public int screenEmuHwHorzRes = 1024;

	public int screenEmuHwVertRes = 640;

	public float monitorDiagonal = 15f;

	public Color screenEmuBorderColor = new Color(0f, 0f, 0f, 0.75f);

	public Color screenEmuBorderBadColor = new Color(0.5f, 0f, 0f, 0.75f);

	[NonSerialized]
	private float monitorDpi = 96f;

	[NonSerialized]
	private float screenEmuCurWidth;

	[NonSerialized]
	private float screenEmuCurHeight;

	[NonSerialized]
	private float screenEmuCurDPI;

	[NonSerialized]
	private float screenEmuCurAspectRatio;

	[NonSerialized]
	private Vector2 screenEmuCurOfs;

	[NonSerialized]
	private bool screenEmuBorderShrunk;

	public RealWorldUnit rwUnit;

	public PreviewMode previewMode;

	private bool firstPostPollUpdate;

	private bool customLayoutNeedsRebuild;

	private Vector2 emuMousePos;

	public int version;

	[NonSerialized]
	private Vector2 debugPrevFrameMousePos;

	[NonSerialized]
	private Vector2 debugFirstTouchPos;

	[NonSerialized]
	private Vector2 debugSecondTouchPos;

	[NonSerialized]
	private bool debugPrevFrameMouseButton0;

	[NonSerialized]
	private bool debugPrevFrameMouseButton1;

	public float twistSafeFingerDistPx = 10f;

	public float pinchMinDistPx = 10f;

	public float touchTapMaxDistPx = 10f;

	[NonSerialized]
	private bool autoPollErrReported;

	[NonSerialized]
	private bool autoUpdateErrReported;

	[NonSerialized]
	private bool autoGuiErrReported;

	[NonSerialized]
	private double editorLastSafetyUpdateTime;

	public int StickCount => sticks.Length;

	public int ZoneCount => touchZones.Length;

	public int ControlCount
	{
		get
		{
			if (touchables == null)
			{
				return 0;
			}
			return touchables.Count;
		}
	}

	public void InitController()
	{
		contentDirtyFlag = false;
		firstPostPollUpdate = true;
		curTime = 0f;
		deltaTime = 0.016666668f;
		invDeltaTime = 1f / deltaTime;
		lastRealTime = Time.realtimeSinceStartup;
		emuMousePos = new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
		if (sticks == null)
		{
			sticks = new TouchStick[0];
		}
		if (touchZones == null)
		{
			touchZones = new TouchZone[0];
		}
		if (touchables == null)
		{
			touchables = new List<TouchableControl>(16);
		}
		touchables.Clear();
		if (sticks != null)
		{
			TouchStick[] array = sticks;
			foreach (TouchStick touchStick in array)
			{
				if (touchStick != null)
				{
					touchables.Add(touchStick);
				}
			}
		}
		if (touchZones != null)
		{
			TouchZone[] array2 = touchZones;
			foreach (TouchZone touchZone in array2)
			{
				if (touchZone != null)
				{
					touchables.Add(touchZone);
				}
			}
		}
		foreach (TouchableControl touchable in touchables)
		{
			touchable.Init(this);
		}
		if (!initialized)
		{
			StartAlphaAnim(1f, 0f);
		}
		Layout();
		initialized = true;
	}

	public void PollController()
	{
		if (!automaticMode)
		{
			PollControllerInternal();
		}
	}

	public void UpdateController()
	{
		if (!automaticMode)
		{
			UpdateControllerInternal();
		}
	}

	public void DrawControllerGUI()
	{
		if (!automaticMode || manualGui)
		{
			DrawControllerGUIInternal();
		}
	}

	public void ResetController()
	{
		if (touchables != null)
		{
			for (int i = 0; i < touchables.Count; i++)
			{
				touchables[i].OnReset();
			}
		}
	}

	public void ReleaseTouches()
	{
		foreach (TouchableControl touchable in touchables)
		{
			touchable.ReleaseTouches();
		}
	}

	public void ShowController(float animDuration)
	{
		StartAlphaAnim(1f, animDuration);
	}

	public void HideController(float animDuration)
	{
		StartAlphaAnim(0f, animDuration);
	}

	public float GetAlpha()
	{
		return globalAlpha;
	}

	public void DisableController()
	{
		disableAll = true;
		ReleaseTouches();
	}

	public void EnableController()
	{
		disableAll = false;
	}

	public bool ControllerEnabled()
	{
		return !disableAll;
	}

	public bool LayoutChanged()
	{
		return customLayoutNeedsRebuild;
	}

	public void LayoutChangeHandled()
	{
		customLayoutNeedsRebuild = false;
	}

	public void ResetAllRects()
	{
		foreach (TouchableControl touchable in touchables)
		{
			touchable.ResetRect();
		}
	}

	public float GetDPI()
	{
		return GetActualDPI();
	}

	public float GetDPCM()
	{
		return GetDPI() / 2.54f;
	}

	public float GetActualDPI()
	{
		if (Screen.dpi != 0f)
		{
			return Screen.dpi;
		}
		return 96f;
	}

	public Rect GetScreenEmuRect(bool viewportRect = false)
	{
		return new Rect(0f, 0f, Screen.width, Screen.height);
	}

	public bool GetLeftHandedMode()
	{
		return leftHandedMode;
	}

	public void SetLeftHandedMode(bool enableLeftedHandMode)
	{
		if (leftHandedMode != enableLeftedHandMode)
		{
			leftHandedMode = enableLeftedHandMode;
			SetLayoutDirtyFlag();
		}
	}

	public void ResetMaskAreas()
	{
		if (maskAreas == null)
		{
			maskAreas = new List<Rect>(8);
		}
		else
		{
			maskAreas.Clear();
		}
	}

	public void AddMaskArea(Rect r)
	{
		if (maskAreas == null)
		{
			maskAreas = new List<Rect>(8);
		}
		maskAreas.Add(r);
	}

	public int GetStickCount()
	{
		return sticks.Length;
	}

	public int GetStickId(string name)
	{
		TouchableControl[] carr = sticks;
		return GetTouchableArrayElemId(carr, name);
	}

	public TouchStick GetStick(int id)
	{
		if (id < 0 || sticks == null || id >= sticks.Length)
		{
			return GetBlankStick();
		}
		return sticks[id];
	}

	public TouchStick GetStick(string name)
	{
		return GetStick(GetStickId(name));
	}

	public TouchStick GetStickOrNull(int id)
	{
		if (id < 0 || sticks == null || id >= sticks.Length)
		{
			return null;
		}
		return sticks[id];
	}

	public TouchStick GetStickOrNull(string name)
	{
		return GetStickOrNull(GetStickId(name));
	}

	private TouchStick GetBlankStick()
	{
		if (blankStick != null)
		{
			return blankStick;
		}
		blankStick = new TouchStick();
		blankStick.Init(this);
		blankStick.OnReset();
		blankStick.name = "BLANK-STICK";
		return blankStick;
	}

	public int GetZoneCount()
	{
		return touchZones.Length;
	}

	public int GetZoneId(string name)
	{
		TouchableControl[] carr = touchZones;
		return GetTouchableArrayElemId(carr, name);
	}

	public TouchZone GetZone(int id)
	{
		if (id < 0 || touchZones == null || id >= touchZones.Length)
		{
			return GetBlankZone();
		}
		return touchZones[id];
	}

	public TouchZone GetZone(string name)
	{
		return GetZone(GetZoneId(name));
	}

	public TouchZone GetZoneOrNull(int id)
	{
		if (id < 0 || touchZones == null || id >= touchZones.Length)
		{
			return null;
		}
		return touchZones[id];
	}

	public TouchZone GetZoneOrNull(string name)
	{
		return GetZoneOrNull(GetZoneId(name));
	}

	private TouchZone GetBlankZone()
	{
		if (blankZone != null)
		{
			return blankZone;
		}
		blankZone = new TouchZone();
		blankZone.Init(this);
		blankZone.OnReset();
		blankZone.name = "NULL";
		return blankZone;
	}

	public int GetControlCount()
	{
		if (touchables == null)
		{
			return 0;
		}
		return touchables.Count;
	}

	public TouchableControl GetControl(int id)
	{
		if (id < 0 || touchables == null || id >= touchables.Count)
		{
			return null;
		}
		return touchables[id];
	}

	public float GetAxisEx(string name, out bool axisSupported)
	{
		axisSupported = false;
		float num = 0f;
		for (int i = 0; i < sticks.Length; i++)
		{
			bool supported = false;
			float axisEx = sticks[i].GetAxisEx(name, out supported);
			if (supported)
			{
				axisSupported = true;
				num += axisEx;
			}
		}
		for (int j = 0; j < touchZones.Length; j++)
		{
			bool supported2 = false;
			float axisEx2 = touchZones[j].GetAxisEx(name, out supported2);
			if (supported2)
			{
				axisSupported = true;
				num += axisEx2;
			}
		}
		return num;
	}

	public float GetAxis(string name)
	{
		bool axisSupported = false;
		return GetAxisEx(name, out axisSupported);
	}

	public float GetAxisRaw(string name)
	{
		return GetAxis(name);
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
		buttonSupported = false;
		for (int i = 0; i < sticks.Length; i++)
		{
			bool buttonSupported2 = false;
			bool buttonEx = sticks[i].GetButtonEx(buttonName, out buttonSupported2);
			if (buttonSupported2)
			{
				buttonSupported = true;
			}
			if (buttonEx)
			{
				return true;
			}
		}
		for (int j = 0; j < touchZones.Length; j++)
		{
			bool buttonSupported3 = false;
			bool buttonEx2 = touchZones[j].GetButtonEx(buttonName, out buttonSupported3);
			if (buttonSupported3)
			{
				buttonSupported = true;
			}
			if (buttonEx2)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetButtonDownEx(string buttonName, out bool buttonSupported)
	{
		buttonSupported = false;
		for (int i = 0; i < sticks.Length; i++)
		{
			bool buttonSupported2 = false;
			bool buttonDownEx = sticks[i].GetButtonDownEx(buttonName, out buttonSupported2);
			if (buttonSupported2)
			{
				buttonSupported = true;
			}
			if (buttonDownEx)
			{
				return true;
			}
		}
		for (int j = 0; j < touchZones.Length; j++)
		{
			bool buttonSupported3 = false;
			bool buttonDownEx2 = touchZones[j].GetButtonDownEx(buttonName, out buttonSupported3);
			if (buttonSupported3)
			{
				buttonSupported = true;
			}
			if (buttonDownEx2)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetButtonUpEx(string buttonName, out bool buttonSupported)
	{
		buttonSupported = false;
		for (int i = 0; i < sticks.Length; i++)
		{
			bool buttonSupported2 = false;
			bool buttonUpEx = sticks[i].GetButtonUpEx(buttonName, out buttonSupported2);
			if (buttonSupported2)
			{
				buttonSupported = true;
			}
			if (buttonUpEx)
			{
				return true;
			}
		}
		for (int j = 0; j < touchZones.Length; j++)
		{
			bool buttonSupported3 = false;
			bool buttonUpEx2 = touchZones[j].GetButtonUpEx(buttonName, out buttonSupported3);
			if (buttonSupported3)
			{
				buttonSupported = true;
			}
			if (buttonUpEx2)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetKey(KeyCode keyCode)
	{
		bool keySupported = false;
		return GetKeyEx(keyCode, out keySupported);
	}

	public bool GetKeyDown(KeyCode keyCode)
	{
		bool keySupported = false;
		return GetKeyDownEx(keyCode, out keySupported);
	}

	public bool GetKeyUp(KeyCode keyCode)
	{
		bool keySupported = false;
		return GetKeyUpEx(keyCode, out keySupported);
	}

	public bool GetKeyEx(KeyCode keyCode, out bool keySupported)
	{
		keySupported = false;
		for (int i = 0; i < sticks.Length; i++)
		{
			bool keySupported2 = false;
			bool keyEx = sticks[i].GetKeyEx(keyCode, out keySupported2);
			if (keySupported2)
			{
				keySupported = true;
			}
			if (keyEx)
			{
				return true;
			}
		}
		for (int j = 0; j < touchZones.Length; j++)
		{
			bool keySupported3 = false;
			bool keyEx2 = touchZones[j].GetKeyEx(keyCode, out keySupported3);
			if (keySupported3)
			{
				keySupported = true;
			}
			if (keyEx2)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetKeyDownEx(KeyCode keyCode, out bool keySupported)
	{
		keySupported = false;
		for (int i = 0; i < sticks.Length; i++)
		{
			bool keySupported2 = false;
			bool keyDownEx = sticks[i].GetKeyDownEx(keyCode, out keySupported2);
			if (keySupported2)
			{
				keySupported = true;
			}
			if (keyDownEx)
			{
				return true;
			}
		}
		for (int j = 0; j < touchZones.Length; j++)
		{
			bool keySupported3 = false;
			bool keyDownEx2 = touchZones[j].GetKeyDownEx(keyCode, out keySupported3);
			if (keySupported3)
			{
				keySupported = true;
			}
			if (keyDownEx2)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetKeyUpEx(KeyCode keyCode, out bool keySupported)
	{
		keySupported = false;
		for (int i = 0; i < sticks.Length; i++)
		{
			bool keySupported2 = false;
			bool keyUpEx = sticks[i].GetKeyUpEx(keyCode, out keySupported2);
			if (keySupported2)
			{
				keySupported = true;
			}
			if (keyUpEx)
			{
				return true;
			}
		}
		for (int j = 0; j < touchZones.Length; j++)
		{
			bool keySupported3 = false;
			bool keyUpEx2 = touchZones[j].GetKeyUpEx(keyCode, out keySupported3);
			if (keySupported3)
			{
				keySupported = true;
			}
			if (keyUpEx2)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetMouseButton(int i)
	{
		int keyCode;
		switch (i)
		{
		case 0:
			keyCode = 323;
			break;
		case 1:
			keyCode = 324;
			break;
		case 2:
			keyCode = 325;
			break;
		default:
			keyCode = 0;
			break;
		}
		return GetKey((KeyCode)keyCode);
	}

	public bool GetMouseButtonDown(int i)
	{
		int keyCode;
		switch (i)
		{
		case 0:
			keyCode = 323;
			break;
		case 1:
			keyCode = 324;
			break;
		case 2:
			keyCode = 325;
			break;
		default:
			keyCode = 0;
			break;
		}
		return GetKeyDown((KeyCode)keyCode);
	}

	public bool GetMouseButtonUp(int i)
	{
		int keyCode;
		switch (i)
		{
		case 0:
			keyCode = 323;
			break;
		case 1:
			keyCode = 324;
			break;
		case 2:
			keyCode = 325;
			break;
		default:
			keyCode = 0;
			break;
		}
		return GetKeyUp((KeyCode)keyCode);
	}

	public Vector2 GetMousePos()
	{
		return emuMousePos;
	}

	public static Collider PickCollider(Vector2 screenPos, Camera cam, LayerMask layerMask)
	{
		Ray ray = cam.ScreenPointToRay(new Vector3(screenPos.x, (float)Screen.height - screenPos.y, 0f));
		float radius = 0.1f;
		if (!Physics.SphereCast(ray, radius, out var hitInfo, float.PositiveInfinity, layerMask))
		{
			return null;
		}
		return hitInfo.collider;
	}

	private void InitIfNeeded()
	{
		if (!initialized)
		{
			InitController();
		}
	}

	private void OnEnable()
	{
		InitIfNeeded();
		ResetLayoutBoxes();
	}

	public static bool IsSupported()
	{
		if (SystemInfo.deviceType == DeviceType.Handheld)
		{
			return Input.multiTouchEnabled;
		}
		return false;
	}

	private void Awake()
	{
		InitIfNeeded();
		if (disableWhenNoTouchScreen && !IsSupported())
		{
			base.enabled = false;
		}
	}

	private void OnDestroy()
	{
		if (CFInput.ctrl == this)
		{
			CFInput.ctrl = null;
		}
	}

	private void Start()
	{
		if (automaticMode && !initialized)
		{
			InitController();
		}
		if (autoActivate)
		{
			CFInput.ctrl = this;
		}
	}

	private void Update()
	{
		InitIfNeeded();
		if (automaticMode)
		{
			PollControllerInternal();
			UpdateControllerInternal();
		}
	}

	private void OnGUI()
	{
		if (automaticMode && !manualGui)
		{
			DrawControllerGUIInternal();
		}
	}

	private void OnApplicationPause(bool pause)
	{
		releaseTouchesFlag = true;
	}

	public void SetLayoutDirtyFlag()
	{
		layoutDirtyFlag = true;
	}

	public void SetContentDirtyFlag()
	{
		contentDirtyFlag = true;
	}

	private void LayoutIfDirty()
	{
		if (layoutDirtyFlag || Screen.width != screenWidth || Screen.height != screenHeight)
		{
			Layout();
		}
	}

	private int GetTouchableArrayElemId(TouchableControl[] carr, string name)
	{
		if (carr == null)
		{
			return -1;
		}
		for (int i = 0; i < carr.Length; i++)
		{
			if (name.Equals(carr[i].name, StringComparison.OrdinalIgnoreCase))
			{
				return i;
			}
		}
		return -1;
	}

	private void Layout()
	{
		customLayoutNeedsRebuild = true;
		releaseTouchesFlag = true;
		layoutDirtyFlag = false;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		fingerBufferRadPx = Mathf.Max(1f, 0.5f * (fingerBufferCm * GetDPCM()));
		touchTapMaxDistPx = Mathf.Clamp(touchTapMaxDistCm * GetDPCM(), 1f, Mathf.Min(GetScreenWidth(), GetScreenHeight()) * 0.3f);
		pinchMinDistPx = Mathf.Clamp(pinchMinDistCm * GetDPCM(), 1f, Mathf.Min(GetScreenWidth(), GetScreenHeight()) * 0.3f);
		twistSafeFingerDistPx = Mathf.Clamp(twistSafeFingerDistCm * GetDPCM(), 1f, Mathf.Min(GetScreenWidth(), GetScreenHeight()) * 0.3f);
		ResetLayoutBoxes();
		foreach (TouchableControl touchable in touchables)
		{
			touchable.OnLayoutAddContent();
		}
		LayoutBox[] array = layoutBoxes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ContentFinalize();
		}
		if (touchables == null)
		{
			return;
		}
		foreach (TouchableControl touchable2 in touchables)
		{
			touchable2.OnLayout();
		}
	}

	private void ResetLayoutBoxes()
	{
		if (layoutBoxes == null || layoutBoxes.Length != 16 || layoutBoxes[0] == null)
		{
			layoutBoxes = new LayoutBox[16];
			for (int i = 0; i < layoutBoxes.Length; i++)
			{
				switch (i)
				{
				case 0:
					layoutBoxes[i] = new LayoutBox("Full-Screen", 0f, 0f, 1f, 1f, LayoutAnchor.TOP_LEFT);
					break;
				case 1:
					layoutBoxes[i] = new LayoutBox("Right-Half", 0.5f, 0f, 0.5f, 1f, LayoutAnchor.MID_RIGHT);
					break;
				case 2:
					layoutBoxes[i] = new LayoutBox("Left-Half", 0f, 0f, 0.5f, 1f, LayoutAnchor.MID_LEFT);
					break;
				case 3:
					layoutBoxes[i] = new LayoutBox("Top-Half", 0f, 0f, 1f, 0.5f, LayoutAnchor.TOP_CENTER);
					break;
				case 4:
					layoutBoxes[i] = new LayoutBox("Bottom-Half", 0f, 0.5f, 1f, 0.5f, LayoutAnchor.BOTTOM_CENTER);
					break;
				case 5:
					layoutBoxes[i] = new LayoutBox("Bottom-Right-Qrtr", 0.5f, 0.5f, 0.5f, 0.5f, LayoutAnchor.BOTTOM_RIGHT);
					break;
				case 6:
					layoutBoxes[i] = new LayoutBox("Bottom-Left-Qrtr", 0f, 0.5f, 0.5f, 0.5f, LayoutAnchor.BOTTOM_LEFT);
					break;
				case 7:
					layoutBoxes[i] = new LayoutBox("Top-Right-Qrtr", 0.5f, 0f, 0.5f, 0.5f, LayoutAnchor.TOP_RIGHT);
					break;
				case 8:
					layoutBoxes[i] = new LayoutBox("Top-Left-Qrtr", 0f, 0f, 0.5f, 0.5f, LayoutAnchor.TOP_LEFT);
					break;
				default:
					layoutBoxes[i] = new LayoutBox("User" + i.ToString("00"), 0f, 0f, 1f, 1f, LayoutAnchor.TOP_LEFT);
					break;
				}
			}
		}
		LayoutBox[] array = layoutBoxes;
		foreach (LayoutBox obj in array)
		{
			obj.SetController(this);
			obj.ResetContent();
		}
	}

	private void PollControllerInternal()
	{
		firstPostPollUpdate = true;
		LayoutIfDirty();
		foreach (TouchableControl touchable in touchables)
		{
			touchable.OnPrePoll();
		}
		if (releaseTouchesFlag)
		{
			foreach (TouchableControl touchable2 in touchables)
			{
				touchable2.ReleaseTouches();
			}
			releaseTouchesFlag = false;
		}
		int touchCount = Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Vector2 zero = Vector2.zero;
			Touch touch = Input.GetTouch(i);
			TouchPhase phase = touch.phase;
			int fingerId = touch.fingerId;
			zero = new Vector2(touch.position.x, (float)Screen.height - touch.position.y);
			if (phase == TouchPhase.Began && maskAreas != null)
			{
				bool flag = false;
				for (int j = 0; j < maskAreas.Count; j++)
				{
					if (maskAreas[j].Contains(zero))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					continue;
				}
			}
			switch (phase)
			{
			case TouchPhase.Began:
			{
				if (disableAll)
				{
					break;
				}
				for (int l = 0; l < 8; l++)
				{
					TouchableControl touchableControl = null;
					HitTestResult r = new HitTestResult(hit: false);
					for (int m = 0; m < touchables.Count; m++)
					{
						if ((l <= 0 || touchables[m].acceptSharedTouches) && (touchableControl == null || r.prio <= touchables[m].prio))
						{
							HitTestResult hitTestResult = touchables[m].HitTest(zero, fingerId);
							if (hitTestResult.hit && (touchableControl == null || hitTestResult.IsCloserThan(r)))
							{
								touchableControl = touchables[m];
								r = hitTestResult;
							}
						}
					}
					if (touchableControl != null && touchableControl.OnTouchStart(fingerId, zero) != EventResult.SHARED)
					{
						break;
					}
				}
				break;
			}
			case TouchPhase.Moved:
			case TouchPhase.Stationary:
			{
				for (int n = 0; n < touchables.Count; n++)
				{
					touchables[n].OnTouchMove(fingerId, zero);
				}
				break;
			}
			case TouchPhase.Ended:
			case TouchPhase.Canceled:
			{
				for (int k = 0; k < touchables.Count; k++)
				{
					touchables[k].OnTouchEnd(fingerId);
				}
				break;
			}
			}
		}
		foreach (TouchableControl touchable3 in touchables)
		{
			touchable3.OnPostPoll();
		}
	}

	private void UpdateControllerInternal()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		deltaTime = realtimeSinceStartup - lastRealTime;
		if (deltaTime <= 0.0001f)
		{
			deltaTime = 0.016666668f;
		}
		invDeltaTime = 1f / deltaTime;
		curTime += deltaTime;
		lastRealTime = realtimeSinceStartup;
		if (globalAlphaTimer.Enabled)
		{
			globalAlphaTimer.Update(deltaTime);
			globalAlpha = Mathf.Lerp(globalAlphaStart, globalAlphaEnd, globalAlphaTimer.Nt);
			if (globalAlphaTimer.Completed)
			{
				globalAlphaTimer.Disable();
			}
		}
		if (touchables != null)
		{
			foreach (TouchableControl touchable in touchables)
			{
				touchable.OnUpdate(firstPostPollUpdate);
			}
			foreach (TouchableControl touchable2 in touchables)
			{
				touchable2.OnPostUpdate(firstPostPollUpdate);
			}
		}
		firstPostPollUpdate = false;
	}

	private void DrawControllerGUIInternal()
	{
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		bool flag = GUI.enabled;
		int depth = GUI.depth;
		GUI.depth = guiDepth;
		GUI.enabled = true;
		if (touchables != null)
		{
			for (int i = 0; i < touchables.Count; i++)
			{
				touchables[i].DrawGUI();
			}
		}
		GUI.depth = depth;
		if (GUI.enabled != flag)
		{
			GUI.enabled = flag;
		}
	}

	public void SetInternalMousePos(Vector2 pos, bool inGuiSpace = true)
	{
		if (inGuiSpace)
		{
			pos.y = (float)Screen.height - pos.y;
		}
		emuMousePos = pos;
	}

	private void StartAlphaAnim(float targetAlpha, float time)
	{
		if (time <= 0f)
		{
			globalAlphaTimer.Reset();
			globalAlpha = targetAlpha;
			globalAlphaEnd = targetAlpha;
			globalAlphaStart = targetAlpha;
		}
		else
		{
			globalAlphaStart = globalAlpha;
			globalAlphaEnd = targetAlpha;
			globalAlphaTimer.Start(time);
		}
	}

	public HitTestResult HitTestCircle(Vector2 cen, float rad, Vector2 touchPos, bool useFingerBuffer = true)
	{
		HitTestResult result = new HitTestResult(hit: false);
		result.dist = (touchPos - cen).magnitude;
		if (result.dist > rad + ((!useFingerBuffer) ? 0f : fingerBufferRadPx))
		{
			result.hit = false;
			return result;
		}
		result.hit = true;
		result.hitInside = result.dist <= rad;
		result.distScale = 1f;
		return result;
	}

	public HitTestResult HitTestBox(Vector2 cen, Vector2 size, Vector2 touchPos, bool useFingerBuffer = true)
	{
		HitTestResult result = new HitTestResult(hit: false);
		float num = ((!useFingerBuffer) ? 0f : fingerBufferRadPx);
		Vector2 vector = new Vector2(Mathf.Abs(touchPos.x - cen.x), Mathf.Abs(touchPos.y - cen.y));
		size *= 0.5f;
		if (vector.x > size.x + num || vector.y > size.y + num)
		{
			result.hit = false;
			return result;
		}
		result.hit = true;
		result.hitInside = vector.x <= size.x && vector.y <= size.y;
		result.dist = vector.magnitude;
		result.distScale = 1f;
		return result;
	}

	public HitTestResult HitTestRect(Rect rect, Vector2 touchPos, bool useFingerBuffer = true)
	{
		HitTestResult result = new HitTestResult(hit: false);
		float num = ((!useFingerBuffer) ? 0f : fingerBufferRadPx);
		Vector2 vector = touchPos - rect.center;
		vector.x = Mathf.Abs(vector.x);
		vector.y = Mathf.Abs(vector.y);
		Vector2 vector2 = new Vector2(rect.width * 0.5f, rect.height * 0.5f);
		if (vector.x > vector2.x + num || vector.y > vector2.y + num)
		{
			result.hit = false;
			return result;
		}
		result.hit = true;
		result.hitInside = vector.x <= vector2.x && vector.y <= vector2.y;
		result.dist = vector.magnitude;
		result.distScale = 1f;
		return result;
	}

	public void EndTouch(int touchId, TouchableControl ctrlToIgnore)
	{
		if (touchId < 0)
		{
			return;
		}
		foreach (TouchableControl touchable in touchables)
		{
			if (touchable != ctrlToIgnore)
			{
				touchable.OnTouchEnd(touchId);
			}
		}
	}

	public float GetScreenWidth()
	{
		return Screen.width;
	}

	public float GetScreenHeight()
	{
		return Screen.height;
	}

	public float GetScreenX(float xFactor)
	{
		return xFactor * (float)Screen.width;
	}

	public float GetScreenY(float yFactor)
	{
		return yFactor * (float)Screen.height;
	}

	public float CmToPixels(float cmVal)
	{
		return GetDPCM() * cmVal;
	}

	public float PixelsToWorld(float pxVal)
	{
		float num = ((rwUnit != 0) ? GetDPI() : GetDPCM());
		if (num <= 1E-05f)
		{
			return 0f;
		}
		return pxVal / num;
	}

	public Rect NormalizedRectToPx(Rect nrect, bool respectLeftHandMode = true)
	{
		Rect rect = Rect.MinMaxRect(GetScreenX(nrect.xMin), GetScreenY(nrect.yMin), GetScreenX(nrect.xMax), GetScreenY(nrect.yMax));
		if (respectLeftHandMode)
		{
			return RightHandToScreenRect(rect);
		}
		return rect;
	}

	public float GetPreviewScale()
	{
		return 1f;
	}

	public Vector2 RightHandToScreen(Vector2 pos)
	{
		if (!leftHandedMode)
		{
			return pos;
		}
		pos.x = (float)Screen.width - pos.x;
		return pos;
	}

	public Rect RightHandToScreenRect(Rect rect)
	{
		if (!leftHandedMode)
		{
			return rect;
		}
		Vector2 vector = RightHandToScreen(new Vector2(rect.xMin, rect.yMin));
		Vector2 vector2 = RightHandToScreen(new Vector2(rect.xMax, rect.yMax));
		return Rect.MinMaxRect(Mathf.Min(vector.x, vector2.x), Mathf.Min(vector.y, vector2.y), Mathf.Max(vector.x, vector2.x), Mathf.Max(vector.y, vector2.y));
	}

	private static Vector2 AnchorLeftover(Vector2 topLeftOfs, LayoutAnchor anchor, float maxMarginX = 0f, float maxMarginY = 0f, bool uniformMargins = false)
	{
		float num = 0f;
		float num2 = 0f;
		if (topLeftOfs.x > 0f)
		{
			num = Mathf.Min(topLeftOfs.x, maxMarginX);
		}
		if (topLeftOfs.y > 0f)
		{
			num2 = Mathf.Min(topLeftOfs.y, maxMarginY);
		}
		if (uniformMargins && maxMarginX > 0.001f && maxMarginY > 0.001f)
		{
			float num3 = Mathf.Min(num / maxMarginX, num2 / maxMarginY);
			num = num3 * maxMarginX;
			num2 = num3 * maxMarginY;
		}
		switch (anchor)
		{
		case LayoutAnchor.BOTTOM_LEFT:
			topLeftOfs.y -= num2;
			topLeftOfs.x = 0f + num;
			break;
		case LayoutAnchor.BOTTOM_CENTER:
			topLeftOfs.y -= num2;
			topLeftOfs.x *= 0.5f;
			break;
		case LayoutAnchor.BOTTOM_RIGHT:
			topLeftOfs.y -= num2;
			topLeftOfs.x -= num;
			break;
		case LayoutAnchor.MID_LEFT:
			topLeftOfs.y *= 0.5f;
			topLeftOfs.x = 0f + num;
			break;
		case LayoutAnchor.MID_CENTER:
			topLeftOfs.y *= 0.5f;
			topLeftOfs.x *= 0.5f;
			break;
		case LayoutAnchor.MID_RIGHT:
			topLeftOfs.y *= 0.5f;
			topLeftOfs.x -= num;
			break;
		case LayoutAnchor.TOP_LEFT:
			topLeftOfs.y = 0f + num2;
			topLeftOfs.x = 0f + num;
			break;
		case LayoutAnchor.TOP_CENTER:
			topLeftOfs.y = 0f + num2;
			topLeftOfs.x *= 0.5f;
			break;
		case LayoutAnchor.TOP_RIGHT:
			topLeftOfs.y = 0f + num2;
			topLeftOfs.x -= num;
			break;
		}
		return topLeftOfs;
	}

	public static float SlowDownEase(float t)
	{
		t = 1f - t;
		return 1f - t * t;
	}

	public static float SpeedUpEase(float t)
	{
		return t * t;
	}

	public static Color ScaleAlpha(Color c, float alphaScale)
	{
		c.a *= alphaScale;
		return c;
	}

	public static Rect GetCenImgRectAtPos(Vector2 pos, Texture2D img, float scale = 1f)
	{
		if (img == null)
		{
			return new Rect(pos.x, pos.y, 1f, 1f);
		}
		pos.x -= (float)img.width * 0.5f * scale;
		pos.y -= (float)img.height * 0.5f * scale;
		return new Rect(pos.x, pos.y, (float)img.width * scale, (float)img.height * scale);
	}

	public static Rect GetCenRect(Vector2 pos, Vector2 size)
	{
		pos.x -= size.x * 0.5f;
		pos.y -= size.y * 0.5f;
		return new Rect(pos.x, pos.y, size.x, size.y);
	}

	public static Rect GetCenRect(Vector2 pos, float size)
	{
		pos.x -= size * 0.5f;
		pos.y -= size * 0.5f;
		return new Rect(pos.x, pos.y, size, size);
	}
}
