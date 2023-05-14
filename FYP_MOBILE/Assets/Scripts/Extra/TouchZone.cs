using System;
using UnityEngine;

[Serializable]
public class TouchZone : TouchableControl
{
	public enum GestureDetectionOrder
	{
		TWIST_PINCH_DRAG,
		TWIST_DRAG_PINCH,
		PINCH_TWIST_DRAG,
		PINCH_DRAG_TWIST,
		DRAG_TWIST_PINCH,
		DRAG_PINCH_TWIST
	}

	private class Finger
	{
		private TouchZone zone;

		public int touchId;

		public bool touchVerified;

		public Vector2 touchPos;

		public Vector2 startPos;

		public Vector2 posPrev;

		public Vector2 posCur;

		public float startTime;

		public bool moved;

		public bool justMoved;

		public bool prevState;

		public bool curState;

		public Vector2 extremeDragCurVec;

		public Vector2 extremeDragPrevVec;

		public float extremeDragCurDist;

		public float extremeDragPrevDist;

		public float lastMoveTime;

		public Vector2 dragVel;

		public bool endedMoved;

		public bool endedWasTapCanceled;

		public float endedStartTime;

		public float endedEndTime;

		public Vector2 endedDragVel;

		public Vector2 endedPosStart;

		public Vector2 endedPosEnd;

		public Vector2 endedExtremeDragVec;

		public float endedExtremeDragDist;

		private bool justTapped;

		private bool justDoubleTapped;

		private bool justDelayTapped;

		private bool waitForDelyedTap;

		private float lastTapTime;

		private bool nextTapCanBeDoubleTap;

		private Vector2 lastTapPos;

		private bool tapCanceled;

		public bool midFrameReleased;

		public bool midFramePressed;

		public bool pollInitialState;

		public bool pollReleasedInitial;

		public bool pollTouched;

		public bool pollReleased;

		public Vector2 pollPosEnd;

		public Vector2 pollPosStart;

		public Vector2 pollPosCur;

		public Finger(TouchZone tzone)
		{
			zone = tzone;
			Reset();
		}

		public bool JustPressed(bool includeMidFramePress, bool includeMidFrameRelease)
		{
			if ((!curState || prevState) && (!includeMidFramePress || !midFramePressed))
			{
				if (includeMidFrameRelease)
				{
					return midFrameReleased;
				}
				return false;
			}
			return true;
		}

		public bool JustReleased(bool includeMidFramePress, bool includeMidFrameRelease)
		{
			if ((curState || !prevState) && (!includeMidFramePress || !midFramePressed))
			{
				if (includeMidFrameRelease)
				{
					return midFrameReleased;
				}
				return false;
			}
			return true;
		}

		public bool Pressed(bool includeMidFramePress, bool returnFalseOnMidFrameRelease)
		{
			if (curState || (includeMidFramePress && midFramePressed))
			{
				if (returnFalseOnMidFrameRelease)
				{
					return !midFrameReleased;
				}
				return true;
			}
			return false;
		}

		public bool JustTapped(bool onlyOnce = false)
		{
			if (onlyOnce)
			{
				return justDelayTapped;
			}
			return justTapped;
		}

		public bool JustDoubleTapped()
		{
			return justDoubleTapped;
		}

		public Vector2 GetTapPos(TouchCoordSys cs)
		{
			return zone.TransformPos(lastTapPos, cs, deltaMode: false);
		}

		public void OnTouchStart(Vector2 startPos, Vector2 curPos)
		{
			startTime = zone.joy.curTime;
			this.startPos = startPos;
			posPrev = startPos;
			posCur = curPos;
			curState = true;
			tapCanceled = false;
			moved = false;
			justMoved = false;
			lastMoveTime = 0f;
			dragVel = Vector2.zero;
			dragVel = Vector2.zero;
			extremeDragCurVec = (extremeDragPrevVec = Vector2.zero);
			extremeDragCurDist = (extremeDragPrevDist = 0f);
		}

		public void OnTouchEnd(Vector2 endPos)
		{
			posCur = endPos;
			UpdateState(lastUpdateMode: true);
			endedMoved = moved;
			endedStartTime = startTime;
			endedEndTime = zone.joy.curTime;
			endedPosStart = startPos;
			endedPosEnd = endPos;
			endedDragVel = dragVel;
			endedExtremeDragVec = extremeDragCurVec;
			endedExtremeDragDist = extremeDragCurDist;
			endedWasTapCanceled = tapCanceled;
			curState = false;
		}

		public void Reset()
		{
			touchId = -1;
			curState = false;
			prevState = false;
			moved = false;
			justMoved = false;
			touchVerified = true;
			dragVel = Vector2.zero;
			pollInitialState = false;
			pollReleasedInitial = false;
			pollTouched = false;
			pollReleased = false;
			tapCanceled = false;
			lastTapPos = Vector2.zero;
			lastTapTime = -100f;
			nextTapCanBeDoubleTap = false;
		}

		public void OnPrePoll()
		{
			touchVerified = false;
		}

		private void UpdateState(bool lastUpdateMode = false)
		{
			justMoved = false;
			Vector2 vector = posCur - startPos;
			extremeDragCurVec.x = Mathf.Max(Mathf.Abs(vector.x), extremeDragCurVec.x);
			extremeDragCurVec.y = Mathf.Max(Mathf.Abs(vector.y), extremeDragCurVec.y);
			extremeDragCurDist = Mathf.Max(vector.magnitude, extremeDragCurDist);
			if (!moved && extremeDragCurDist > zone.joy.touchTapMaxDistPx)
			{
				moved = true;
				justMoved = true;
			}
			if (lastUpdateMode)
			{
				return;
			}
			if (curState)
			{
				if (PxPosEquals(posCur, posPrev))
				{
					if (zone.joy.curTime - lastMoveTime > zone.joy.velPreserveTime)
					{
						dragVel = Vector2.zero;
					}
				}
				else
				{
					lastMoveTime = zone.joy.curTime;
					dragVel = (posCur - posPrev) * zone.joy.invDeltaTime;
				}
			}
			else
			{
				dragVel = Vector2.zero;
			}
		}

		public void PreUpdate(bool firstUpdate)
		{
			prevState = curState;
			posPrev = posCur;
			extremeDragPrevDist = extremeDragCurDist;
			extremeDragPrevVec = extremeDragCurVec;
			midFramePressed = false;
			midFrameReleased = false;
			if (curState && pollReleasedInitial)
			{
				OnTouchEnd(pollPosEnd);
			}
			if (pollTouched && (!pollInitialState || touchId >= 0))
			{
				OnTouchStart(pollPosStart, pollPosCur);
			}
			if (touchId >= 0 != curState)
			{
				if (curState)
				{
					OnTouchEnd(pollPosEnd);
				}
				else
				{
					OnTouchStart(pollPosStart, pollPosCur);
				}
			}
			if (touchId >= 0)
			{
				posCur = pollPosCur;
			}
			midFramePressed = !pollInitialState && pollTouched && !curState;
			midFrameReleased = pollInitialState && pollReleasedInitial && curState;
			UpdateState();
			justDelayTapped = false;
			justTapped = false;
			justDoubleTapped = false;
			if (JustReleased(includeMidFramePress: true, includeMidFrameRelease: true))
			{
				if (!endedMoved && !endedWasTapCanceled && zone.joy.curTime - endedStartTime <= zone.joy.touchTapMaxTime)
				{
					bool flag = nextTapCanBeDoubleTap && endedStartTime - lastTapTime <= zone.joy.doubleTapMaxGapTime;
					waitForDelyedTap = !flag;
					justDoubleTapped = flag;
					justTapped = true;
					lastTapPos = endedPosStart;
					lastTapTime = zone.joy.curTime;
					nextTapCanBeDoubleTap = !flag;
				}
				else
				{
					waitForDelyedTap = false;
					nextTapCanBeDoubleTap = false;
				}
			}
			else if (JustPressed(includeMidFramePress: false, includeMidFrameRelease: false))
			{
				waitForDelyedTap = false;
			}
			else if (waitForDelyedTap && zone.joy.curTime - lastTapTime > zone.joy.doubleTapMaxGapTime)
			{
				justDelayTapped = true;
				waitForDelyedTap = false;
				nextTapCanBeDoubleTap = false;
			}
			pollInitialState = curState;
			pollReleasedInitial = false;
			pollReleased = false;
			pollTouched = false;
			pollPosCur = posCur;
			pollPosStart = posCur;
			pollPosEnd = posCur;
		}

		public void PostUpdate(bool firstUpdate)
		{
		}

		public void CancelTap()
		{
			waitForDelyedTap = false;
			tapCanceled = true;
			nextTapCanBeDoubleTap = false;
		}
	}

	private const float MIN_PINCH_DIST_PX = 2f;

	private const float PIXEL_POS_EPSILON_SQR = 0.1f;

	private const float PIXEL_DIST_EPSILON = 0.1f;

	private const float TWIST_ANGLE_EPSILON = 0.5f;

	public const int BOX_LEFT = 1;

	public const int BOX_CEN = 2;

	public const int BOX_RIGHT = 4;

	public const int BOX_TOP = 8;

	public const int BOX_MID = 16;

	public const int BOX_BOTTOM = 32;

	public const int BOX_TOP_LEFT = 9;

	public const int BOX_TOP_CEN = 10;

	public const int BOX_TOP_RIGHT = 12;

	public const int BOX_MID_LEFT = 17;

	public const int BOX_MID_CEN = 18;

	public const int BOX_MID_RIGHT = 20;

	public const int BOX_BOTTOM_LEFT = 33;

	public const int BOX_BOTTOM_CEN = 34;

	public const int BOX_BOTTOM_RIGHT = 36;

	public const int BOX_H_MASK = 7;

	public const int BOX_V_MASK = 56;

	public TouchController.ControlShape shape;

	public Vector2 posCm;

	public Vector2 sizeCm;

	public Rect regionRect;

	private Vector2 posPx;

	private Vector2 sizePx;

	private Vector2 layoutPosPx;

	private Vector2 layoutSizePx;

	private Rect screenRectPx;

	private Rect layoutRectPx;

	public bool enableSecondFinger;

	public bool nonExclusiveTouches;

	public bool strictTwoFingerStart;

	public bool freezeTwistWhenTooClose;

	public bool noPinchAfterDrag;

	public bool noPinchAfterTwist;

	public bool noTwistAfterDrag;

	public bool noTwistAfterPinch;

	public bool noDragAfterPinch;

	public bool noDragAfterTwist;

	public bool startPinchWhenTwisting;

	public bool startPinchWhenDragging;

	public bool startDragWhenPinching;

	public bool startDragWhenTwisting;

	public bool startTwistWhenDragging;

	public bool startTwistWhenPinching;

	public GestureDetectionOrder gestureDetectionOrder;

	public KeyCode debugKey;

	public Texture2D releasedImg;

	public Texture2D pressedImg;

	public bool overrideScale;

	public float releasedScale;

	public float pressedScale;

	public float disabledScale;

	public bool overrideColors;

	public Color releasedColor;

	public Color pressedColor;

	public Color disabledColor;

	public bool overrideAnimDuration;

	public float pressAnimDuration;

	public float releaseAnimDuration;

	public float disableAnimDuration;

	public float enableAnimDuration;

	public float showAnimDuration;

	public float hideAnimDuration;

	private AnimTimer animTimer;

	private TouchController.AnimFloat animScale;

	private TouchController.AnimColor animColor;

	private Finger fingerA;

	private Finger fingerB;

	private bool multiCur;

	private bool multiPrev;

	private bool multiMoved;

	private bool multiJustMoved;

	private bool multiMidFrameReleased;

	private bool multiMidFramePressed;

	private float multiStartTime;

	private Vector2 multiPosCur;

	private Vector2 multiPosPrev;

	private Vector2 multiPosStart;

	private float multiExtremeCurDist;

	private Vector2 multiExtremeCurVec;

	private float multiLastMoveTime;

	private Vector2 multiDragVel;

	private bool justMultiTapped;

	private bool justMultiDoubleTapped;

	private bool justMultiDelayTapped;

	private bool waitForMultiDelyedTap;

	private float lastMultiTapTime;

	private bool nextTapCanBeMultiDoubleTap;

	private Vector2 lastMultiTapPos;

	[NonSerialized]
	private float twistStartAbs;

	[NonSerialized]
	private float twistCurAbs;

	[NonSerialized]
	private float twistPrevAbs;

	[NonSerialized]
	private float twistCur;

	[NonSerialized]
	private float twistPrev;

	[NonSerialized]
	private float twistCurRaw;

	[NonSerialized]
	private float twistStartRaw;

	[NonSerialized]
	private float twistExtremeCur;

	[NonSerialized]
	private float twistLastMoveTime;

	[NonSerialized]
	private float twistVel;

	[NonSerialized]
	private float pinchDistStart;

	[NonSerialized]
	private float pinchCurDist;

	[NonSerialized]
	private float pinchPrevDist;

	[NonSerialized]
	private float pinchExtremeCurDist;

	[NonSerialized]
	private float pinchLastMoveTime;

	[NonSerialized]
	private float pinchDistVel;

	private bool endedMultiMoved;

	private bool endedTwistMoved;

	private bool endedPinchMoved;

	private float endedMultiStartTime;

	private float endedMultiEndTime;

	private float endedPinchDistStart;

	private float endedPinchDistEnd;

	private float endedPinchDistVel;

	private float endedTwistAngle;

	private float endedTwistVel;

	private Vector2 endedMultiPosStart;

	private Vector2 endedMultiPosEnd;

	private Vector2 endedMultiDragVel;

	private bool pollMultiInitialState;

	private bool pollMultiReleasedInitial;

	private bool pollMultiTouched;

	private bool pollMultiReleased;

	private Vector2 pollMultiPosEnd;

	private Vector2 pollMultiPosStart;

	private Vector2 pollMultiPosCur;

	private bool twistMoved;

	private bool twistJustMoved;

	private bool pinchMoved;

	private bool pinchJustMoved;

	private bool uniMoved;

	private bool uniJustMoved;

	private bool uniCur;

	private bool uniPrev;

	private float uniStartTime;

	private Vector2 uniPosCur;

	private Vector2 uniPosStart;

	private Vector2 uniTotalDragCur;

	private Vector2 uniTotalDragPrev;

	private float uniExtremeDragCurDist;

	private Vector2 uniExtremeDragCurVec;

	private float uniLastMoveTime;

	private Vector2 uniDragVel;

	private float endedUniStartTime;

	private float endedUniEndTime;

	private Vector2 endedUniPosStart;

	private Vector2 endedUniPosEnd;

	private Vector2 endedUniTotalDrag;

	private Vector2 endedUniDragVel;

	private bool endedUniMoved;

	private bool uniMidFrameReleased;

	private bool uniMidFramePressed;

	private bool pollUniInitialState;

	private bool pollUniReleasedInitial;

	private bool pollUniTouched;

	private bool pollUniReleased;

	private bool pollUniWaitForDblStart;

	private bool pollUniWaitForDblEnd;

	private Vector2 pollUniPosEnd;

	private Vector2 pollUniPosStart;

	private Vector2 pollUniPosCur;

	private Vector2 pollUniPosPrev;

	private Vector2 pollUniDeltaAccum;

	private Vector2 pollUniDblEndPos;

	private Vector2 pollUniDeltaAccumAtEnd;

	private bool touchCanceled;

	public bool enableGetKey;

	public KeyCode getKeyCode;

	public KeyCode getKeyCodeAlt;

	public KeyCode getKeyCodeMulti;

	public KeyCode getKeyCodeMultiAlt;

	public bool enableGetButton;

	public string getButtonName;

	public string getButtonMultiName;

	public bool emulateMouse;

	public bool mousePosFromFirstFinger;

	public bool enableGetAxis;

	public string axisHorzName;

	public string axisVertName;

	public bool axisHorzFlip;

	public bool axisVertFlip;

	public float axisValScale;

	public bool codeUniJustPressed;

	public bool codeUniPressed;

	public bool codeUniJustReleased;

	public bool codeUniJustDragged;

	public bool codeUniDragged;

	public bool codeUniReleasedDrag;

	public bool codeMultiJustPressed;

	public bool codeMultiPressed;

	public bool codeMultiJustReleased;

	public bool codeMultiJustDragged;

	public bool codeMultiDragged;

	public bool codeMultiReleasedDrag;

	public bool codeJustTwisted;

	public bool codeTwisted;

	public bool codeReleasedTwist;

	public bool codeJustPinched;

	public bool codePinched;

	public bool codeReleasedPinch;

	public bool codeSimpleTap;

	public bool codeSingleTap;

	public bool codeDoubleTap;

	public bool codeSimpleMultiTap;

	public bool codeMultiSingleTap;

	public bool codeMultiDoubleTap;

	public bool codeCustomGUI;

	public bool codeCustomLayout;

	[NonSerialized]
	private bool colorsDirtyFlag;

	private Finger GetFinger(int i)
	{
		switch (i)
		{
		case 0:
			return fingerA;
		case 1:
			return fingerB;
		default:
			return null;
		}
	}

	public bool Pressed(int fingerId, bool trueOnMidFramePress, bool falseOnMidFrameRelease)
	{
		return ((fingerId != 1) ? fingerA : fingerB).Pressed(trueOnMidFramePress, falseOnMidFrameRelease);
	}

	public bool Pressed(int fingerId)
	{
		return Pressed(fingerId, trueOnMidFramePress: false, falseOnMidFrameRelease: false);
	}

	public bool UniPressed(bool trueOnMidFramePress, bool falseOnMidFrameRelease)
	{
		if (uniCur || (trueOnMidFramePress && uniMidFramePressed))
		{
			if (falseOnMidFrameRelease)
			{
				return !uniMidFrameReleased;
			}
			return true;
		}
		return false;
	}

	public bool UniPressed()
	{
		return UniPressed(trueOnMidFramePress: false, falseOnMidFrameRelease: false);
	}

	public bool MultiPressed(bool trueOnMidFramePress, bool falseOnMidFrameRelease)
	{
		if (multiCur || (trueOnMidFramePress && multiMidFramePressed))
		{
			if (falseOnMidFrameRelease)
			{
				return !multiMidFrameReleased;
			}
			return true;
		}
		return false;
	}

	public bool MultiPressed()
	{
		return MultiPressed(trueOnMidFramePress: false, falseOnMidFrameRelease: false);
	}

	public bool JustPressed(int fingerId, bool trueOnMidFramePress, bool trueOnMidFrameRelease)
	{
		return ((fingerId != 1) ? fingerA : fingerB).JustPressed(trueOnMidFramePress, trueOnMidFrameRelease);
	}

	public bool JustPressed(int fingerId)
	{
		return JustPressed(fingerId, trueOnMidFramePress: false, trueOnMidFrameRelease: false);
	}

	public bool JustUniPressed(bool trueOnMidFramePress, bool trueOnMidFrameRelease)
	{
		if ((uniPrev || !uniCur) && (!trueOnMidFramePress || !uniMidFramePressed))
		{
			if (trueOnMidFrameRelease)
			{
				return uniMidFrameReleased;
			}
			return false;
		}
		return true;
	}

	public bool JustUniPressed()
	{
		return JustUniPressed(trueOnMidFramePress: false, trueOnMidFrameRelease: false);
	}

	public bool JustMultiPressed(bool trueOnMidFramePress, bool trueOnMidFrameRelease)
	{
		if ((multiPrev || !multiCur) && (!trueOnMidFramePress || !multiMidFramePressed))
		{
			if (trueOnMidFrameRelease)
			{
				return multiMidFrameReleased;
			}
			return false;
		}
		return true;
	}

	public bool JustMultiPressed()
	{
		return JustMultiPressed(trueOnMidFramePress: false, trueOnMidFrameRelease: false);
	}

	public bool JustReleased(int fingerId, bool trueOnMidFramePress, bool trueOnMidFrameRelease)
	{
		return ((fingerId != 1) ? fingerA : fingerB).JustPressed(trueOnMidFramePress, trueOnMidFrameRelease);
	}

	public bool JustReleased(int fingerId)
	{
		return JustReleased(fingerId, trueOnMidFramePress: false, trueOnMidFrameRelease: false);
	}

	public bool JustUniReleased(bool trueOnMidFramePress, bool trueOnMidFrameRelease)
	{
		if ((!uniPrev || uniCur) && (!trueOnMidFramePress || !uniMidFramePressed))
		{
			if (trueOnMidFrameRelease)
			{
				return uniMidFrameReleased;
			}
			return false;
		}
		return true;
	}

	public bool JustUniReleased()
	{
		return JustUniReleased(trueOnMidFramePress: false, trueOnMidFrameRelease: false);
	}

	public bool JustMultiReleased(bool trueOnMidFramePress, bool trueOnMidFrameRelease)
	{
		if ((!multiPrev || multiCur) && (!trueOnMidFramePress || !multiMidFramePressed))
		{
			if (trueOnMidFrameRelease)
			{
				return multiMidFrameReleased;
			}
			return false;
		}
		return true;
	}

	public bool JustMultiReleased()
	{
		return JustMultiReleased(trueOnMidFramePress: false, trueOnMidFrameRelease: false);
	}

	public bool JustMidFramePressed(int fingerId)
	{
		return ((fingerId != 1) ? fingerA : fingerB).midFramePressed;
	}

	public bool JustMidFrameReleased(int fingerId)
	{
		return ((fingerId != 1) ? fingerA : fingerB).midFrameReleased;
	}

	public bool JustMidFrameUniPressed()
	{
		return uniMidFramePressed;
	}

	public bool JustMidFrameUniReleased()
	{
		return uniMidFrameReleased;
	}

	public bool JustMidFrameMultiPressed()
	{
		return multiMidFramePressed;
	}

	public bool JustMidFrameMultiReleased()
	{
		return multiMidFrameReleased;
	}

	public float GetTouchDuration(int fingerId)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		if (finger.curState)
		{
			return joy.curTime - finger.startTime;
		}
		return 0f;
	}

	public float GetUniTouchDuration()
	{
		if (uniCur)
		{
			return joy.curTime - uniStartTime;
		}
		return 0f;
	}

	public float GetMultiTouchDuration()
	{
		if (multiCur)
		{
			return joy.curTime - multiStartTime;
		}
		return 0f;
	}

	public Vector2 GetPos(int fingerId, TouchCoordSys cs)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		return TransformPos(finger.posCur, cs, deltaMode: false);
	}

	public Vector2 GetPos(int fingerId)
	{
		return GetPos(fingerId, TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetUniPos(TouchCoordSys cs)
	{
		return TransformPos(uniPosCur, cs, deltaMode: false);
	}

	public Vector2 GetUniPos()
	{
		return GetUniPos(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetMultiPos(TouchCoordSys cs)
	{
		return TransformPos(multiPosCur, cs, deltaMode: false);
	}

	public Vector2 GetMultiPos()
	{
		return GetMultiPos(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetStartPos(int fingerId, TouchCoordSys cs)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		return TransformPos(finger.startPos, cs, deltaMode: false);
	}

	public Vector2 GetStartPos(int fingerId)
	{
		return GetStartPos(fingerId, TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetUniStartPos(TouchCoordSys cs)
	{
		return TransformPos(uniPosStart, cs, deltaMode: false);
	}

	public Vector2 GetUniStartPos()
	{
		return GetUniStartPos(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetMultiStartPos(TouchCoordSys cs)
	{
		return TransformPos(multiPosStart, cs, deltaMode: false);
	}

	public Vector2 GetMultiStartPos()
	{
		return GetMultiStartPos(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetDragVec(int fingerId, TouchCoordSys cs, bool raw)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		if (!raw && !finger.moved)
		{
			return Vector2.zero;
		}
		return TransformPos(finger.posCur - finger.startPos, cs, deltaMode: true);
	}

	public Vector2 GetDragVec(int fingerId)
	{
		return GetDragVec(fingerId, TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetDragVec(int fingerId, TouchCoordSys cs)
	{
		return GetDragVec(fingerId, cs, raw: false);
	}

	public Vector2 GetDragVec(int fingerId, bool raw)
	{
		return GetDragVec(fingerId, TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetDragVecRaw(int fingerId)
	{
		return GetDragVec(fingerId, TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetDragVecRaw(int fingerId, TouchCoordSys cs)
	{
		return GetDragVec(fingerId, cs, raw: true);
	}

	public Vector2 GetUniDragVec(TouchCoordSys cs, bool raw)
	{
		if (!raw && !uniMoved)
		{
			return Vector2.zero;
		}
		return TransformPos(uniTotalDragCur, cs, deltaMode: true);
	}

	public Vector2 GetUniDragVec()
	{
		return GetUniDragVec(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetUniDragVec(TouchCoordSys cs)
	{
		return GetUniDragVec(cs, raw: false);
	}

	public Vector2 GetUniDragVec(bool raw)
	{
		return GetUniDragVec(TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetUniDragVecRaw()
	{
		return GetUniDragVec(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetUniDragVecRaw(TouchCoordSys cs)
	{
		return GetUniDragVec(cs, raw: true);
	}

	public Vector2 GetMultiDragVec(TouchCoordSys cs, bool raw)
	{
		if (!raw && !uniMoved)
		{
			return Vector2.zero;
		}
		return TransformPos(multiPosCur - multiPosStart, cs, deltaMode: true);
	}

	public Vector2 GetMultiDragVec()
	{
		return GetMultiDragVec(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetMultiDragVec(TouchCoordSys cs)
	{
		return GetMultiDragVec(cs, raw: false);
	}

	public Vector2 GetMultiDragVec(bool raw)
	{
		return GetMultiDragVec(TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetMultiDragVecRaw()
	{
		return GetMultiDragVec(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetMultiDragVecRaw(TouchCoordSys cs)
	{
		return GetMultiDragVec(cs, raw: true);
	}

	public Vector2 GetDragDelta(int fingerId, TouchCoordSys cs, bool raw)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		if (!raw && !finger.moved)
		{
			return Vector2.zero;
		}
		return TransformPos((raw || !finger.justMoved) ? (finger.posCur - finger.posPrev) : (finger.posCur - finger.startPos), cs, deltaMode: true);
	}

	public Vector2 GetDragDelta(int fingerId)
	{
		return GetDragDelta(fingerId, TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetDragDelta(int fingerId, TouchCoordSys cs)
	{
		return GetDragDelta(fingerId, cs, raw: false);
	}

	public Vector2 GetDragDelta(int fingerId, bool raw)
	{
		return GetDragDelta(fingerId, TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetDragDeltaRaw(int fingerId)
	{
		return GetDragDelta(fingerId, TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetDragDeltaRaw(int fingerId, TouchCoordSys cs)
	{
		return GetDragDelta(fingerId, cs, raw: true);
	}

	public Vector2 GetUniDragDelta(TouchCoordSys cs, bool raw)
	{
		if (!raw && !uniMoved)
		{
			return Vector2.zero;
		}
		return TransformPos((raw || !uniJustMoved) ? (uniTotalDragCur - uniTotalDragPrev) : uniTotalDragCur, cs, deltaMode: true);
	}

	public Vector2 GetUniDragDelta()
	{
		return GetUniDragDelta(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetUniDragDelta(TouchCoordSys cs)
	{
		return GetUniDragDelta(cs, raw: false);
	}

	public Vector2 GetUniDragDelta(bool raw)
	{
		return GetUniDragDelta(TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetUniDragDeltaRaw()
	{
		return GetUniDragDelta(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetUniDragDeltaRaw(TouchCoordSys cs)
	{
		return GetUniDragDelta(cs, raw: true);
	}

	public Vector2 GetMultiDragDelta(TouchCoordSys cs, bool raw)
	{
		if (!raw && !multiMoved)
		{
			return Vector2.zero;
		}
		return TransformPos((raw || !multiJustMoved) ? (multiPosCur - multiPosPrev) : (multiPosCur - multiPosStart), cs, deltaMode: true);
	}

	public Vector2 GetMultiDragDelta()
	{
		return GetMultiDragDelta(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetMultiDragDelta(TouchCoordSys cs)
	{
		return GetMultiDragDelta(cs, raw: false);
	}

	public Vector2 GetMultiDragDelta(bool raw)
	{
		return GetMultiDragDelta(TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetMultiDragDeltaRaw()
	{
		return GetMultiDragDelta(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetMultiDragDeltaRaw(TouchCoordSys cs)
	{
		return GetMultiDragDelta(cs, raw: true);
	}

	public bool Dragged(int fingerId)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		if (finger.curState)
		{
			return finger.moved;
		}
		return false;
	}

	public bool UniDragged()
	{
		if (uniCur)
		{
			return uniMoved;
		}
		return false;
	}

	public bool MultiDragged()
	{
		if (multiCur)
		{
			return multiMoved;
		}
		return false;
	}

	public bool JustDragged(int fingerId)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		if (finger.curState)
		{
			return finger.justMoved;
		}
		return false;
	}

	public bool JustUniDragged()
	{
		if (uniCur)
		{
			return uniJustMoved;
		}
		return false;
	}

	public bool JustMultiDragged()
	{
		if (multiCur)
		{
			return multiJustMoved;
		}
		return false;
	}

	public Vector2 GetDragVel(int fingerId, TouchCoordSys cs)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		return TransformPos(finger.dragVel, cs, deltaMode: true);
	}

	public Vector2 GetDragVel(int fingerId)
	{
		return GetDragVel(fingerId, TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetUniDragVel(TouchCoordSys cs)
	{
		return TransformPos(uniDragVel, cs, deltaMode: true);
	}

	public Vector2 GetUniDragVel()
	{
		return GetUniDragVel(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetMultiDragVel(TouchCoordSys cs)
	{
		return TransformPos(multiDragVel, cs, deltaMode: true);
	}

	public Vector2 GetMultiDragVel()
	{
		return GetMultiDragVel(TouchCoordSys.SCREEN_PX);
	}

	public bool Twisted()
	{
		return twistMoved;
	}

	public bool JustTwisted()
	{
		return twistJustMoved;
	}

	public float GetTwistVel()
	{
		return twistVel;
	}

	public float GetTotalTwist(bool raw)
	{
		if (!raw && !twistMoved)
		{
			return 0f;
		}
		return twistCur;
	}

	public float GetTotalTwist()
	{
		return GetTotalTwist(raw: false);
	}

	public float GetTotalTwistRaw()
	{
		return GetTotalTwist(raw: true);
	}

	public float GetTwistDelta(bool raw)
	{
		if (!raw && twistJustMoved)
		{
			return twistCur;
		}
		return Mathf.DeltaAngle(twistPrev, twistCur);
	}

	public float GetTwistDelta()
	{
		return GetTwistDelta(raw: false);
	}

	public float GetTwistDeltaRaw()
	{
		return GetTwistDelta(raw: true);
	}

	public bool Pinched()
	{
		return pinchMoved;
	}

	public bool JustPinched()
	{
		return pinchJustMoved;
	}

	public float GetPinchDist(TouchCoordSys cs, bool raw)
	{
		if (!multiCur || (!raw && !pinchMoved))
		{
			return 0f;
		}
		return TransformPos(pinchCurDist, cs);
	}

	public float GetPinchDist()
	{
		return GetPinchDist(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public float GetPinchDist(TouchCoordSys cs)
	{
		return GetPinchDist(cs, raw: false);
	}

	public float GetPinchDist(bool raw)
	{
		return GetPinchDist(TouchCoordSys.SCREEN_PX, raw);
	}

	public float GetPinchDistRaw()
	{
		return GetPinchDist(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public float GetPinchDistRaw(TouchCoordSys cs)
	{
		return GetPinchDist(cs, raw: true);
	}

	public float GetPinchDistDelta(TouchCoordSys cs, bool raw)
	{
		if (!multiCur || (!raw && !pinchMoved))
		{
			return 0f;
		}
		return TransformPos(pinchCurDist - ((raw || !pinchJustMoved) ? pinchPrevDist : pinchDistStart), cs);
	}

	public float GetPinchDistDelta()
	{
		return GetPinchDistDelta(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public float GetPinchDistDelta(TouchCoordSys cs)
	{
		return GetPinchDistDelta(cs, raw: false);
	}

	public float GetPinchDistDelta(bool raw)
	{
		return GetPinchDistDelta(TouchCoordSys.SCREEN_PX, raw);
	}

	public float GetPinchDistDeltaRaw()
	{
		return GetPinchDistDelta(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public float GetPinchDistDeltaRaw(TouchCoordSys cs)
	{
		return GetPinchDistDelta(cs, raw: true);
	}

	public float GetPinchScale(bool raw)
	{
		if (!multiCur || (!raw && !pinchMoved))
		{
			return 1f;
		}
		return pinchCurDist / pinchDistStart;
	}

	public float GetPinchScale()
	{
		return GetPinchScale(raw: false);
	}

	public float GetPinchScaleRaw()
	{
		return GetPinchScale(raw: true);
	}

	public float GetPinchRelativeScale(bool raw)
	{
		if (!multiCur || (!raw && !pinchMoved))
		{
			return 1f;
		}
		return pinchCurDist / ((raw || !pinchJustMoved) ? pinchPrevDist : pinchDistStart);
	}

	public float GetPinchRelativeScale()
	{
		return GetPinchRelativeScale(raw: false);
	}

	public float GetPinchRelativeScaleRaw()
	{
		return GetPinchRelativeScale(raw: true);
	}

	public float GetPinchDistVel()
	{
		return pinchDistVel;
	}

	public bool JustTapped()
	{
		return fingerA.JustTapped();
	}

	public bool JustMultiTapped()
	{
		return justMultiTapped;
	}

	public bool JustSingleTapped()
	{
		return fingerA.JustTapped(onlyOnce: true);
	}

	public bool JustMultiSingleTapped()
	{
		return justMultiDelayTapped;
	}

	public bool JustDoubleTapped()
	{
		return fingerA.JustDoubleTapped();
	}

	public bool JustMultiDoubleTapped()
	{
		return justMultiDoubleTapped;
	}

	public Vector2 GetTapPos(TouchCoordSys cs)
	{
		return fingerA.GetTapPos(cs);
	}

	public Vector2 GetTapPos()
	{
		return GetTapPos(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetMultiTapPos(TouchCoordSys cs)
	{
		return TransformPos(lastMultiTapPos, cs, deltaMode: false);
	}

	public Vector2 GetMultiTapPos()
	{
		return GetMultiTapPos(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetReleasedStartPos(int fingerId, TouchCoordSys cs)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		return TransformPos(finger.endedPosStart, cs, deltaMode: false);
	}

	public Vector2 GetReleasedStartPos(int fingerId)
	{
		return GetReleasedStartPos(fingerId, TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetReleasedUniStartPos(TouchCoordSys cs)
	{
		return TransformPos(endedUniPosStart, cs, deltaMode: false);
	}

	public Vector2 GetReleasedUniStartPos()
	{
		return GetReleasedUniStartPos(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetReleasedMultiStartPos(TouchCoordSys cs)
	{
		return TransformPos(endedMultiPosStart, cs, deltaMode: false);
	}

	public Vector2 GetReleasedMultiStartPos()
	{
		return GetReleasedMultiStartPos(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetReleasedEndPos(int fingerId, TouchCoordSys cs)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		return TransformPos(finger.endedPosEnd, cs, deltaMode: false);
	}

	public Vector2 GetReleasedEndPos(int fingerId)
	{
		return GetReleasedEndPos(fingerId, TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetReleasedUniEndPos(TouchCoordSys cs)
	{
		return TransformPos(endedUniPosEnd, cs, deltaMode: false);
	}

	public Vector2 GetReleasedUniEndPos()
	{
		return GetReleasedUniEndPos(TouchCoordSys.SCREEN_PX);
	}

	public Vector2 GetReleasedMultiEndPos(TouchCoordSys cs)
	{
		return TransformPos(endedMultiPosEnd, cs, deltaMode: false);
	}

	public Vector2 GetReleasedMultiEndPos()
	{
		return GetReleasedMultiEndPos(TouchCoordSys.SCREEN_PX);
	}

	public bool ReleasedDragged(int fingerId)
	{
		return ((fingerId != 1) ? fingerA : fingerB).endedMoved;
	}

	public bool ReleasedUniDragged()
	{
		return endedUniMoved;
	}

	public bool ReleasedMultiDragged()
	{
		return endedMultiMoved;
	}

	public bool ReleasedMoved(int fingerId)
	{
		return ReleasedDragged(fingerId);
	}

	public bool ReleasedUniMoved()
	{
		return ReleasedUniDragged();
	}

	public bool ReleasedMultiMoved()
	{
		return ReleasedMultiDragged();
	}

	public Vector2 GetReleasedDragVec(int fingerId, TouchCoordSys cs, bool raw)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		if (!raw && !finger.endedMoved)
		{
			return Vector2.zero;
		}
		return TransformPos(finger.endedPosEnd - finger.endedPosStart, cs, deltaMode: true);
	}

	public Vector2 GetReleasedDragVec(int fingerId)
	{
		return GetReleasedDragVec(fingerId, TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetReleasedDragVec(int fingerId, TouchCoordSys cs)
	{
		return GetReleasedDragVec(fingerId, cs, raw: false);
	}

	public Vector2 GetReleasedDragVec(int fingerId, bool raw)
	{
		return GetReleasedDragVec(fingerId, TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetReleasedDragVecRaw(int fingerId)
	{
		return GetReleasedDragVec(fingerId, TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetReleasedDragVecRaw(int fingerId, TouchCoordSys cs)
	{
		return GetReleasedDragVec(fingerId, cs, raw: true);
	}

	public Vector2 GetReleasedUniDragVec(TouchCoordSys cs, bool raw)
	{
		if (!raw && !endedUniMoved)
		{
			return Vector2.zero;
		}
		return TransformPos(endedUniTotalDrag, cs, deltaMode: true);
	}

	public Vector2 GetReleasedUniDragVec()
	{
		return GetReleasedUniDragVec(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetReleasedUniDragVec(TouchCoordSys cs)
	{
		return GetReleasedUniDragVec(cs, raw: false);
	}

	public Vector2 GetReleasedUniDragVec(bool raw)
	{
		return GetReleasedUniDragVec(TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetReleasedUniDragVecRaw()
	{
		return GetReleasedUniDragVec(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetReleasedUniDragVecRaw(TouchCoordSys cs)
	{
		return GetReleasedUniDragVec(cs, raw: true);
	}

	public Vector2 GetReleasedMultiDragVec(TouchCoordSys cs, bool raw)
	{
		if (!raw && !endedMultiMoved)
		{
			return Vector2.zero;
		}
		return TransformPos(endedMultiPosEnd - endedMultiPosStart, cs, deltaMode: true);
	}

	public Vector2 GetReleasedMultiDragVec()
	{
		return GetReleasedMultiDragVec(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetReleasedMultiDragVec(TouchCoordSys cs)
	{
		return GetReleasedMultiDragVec(cs, raw: false);
	}

	public Vector2 GetReleasedMultiDragVec(bool raw)
	{
		return GetReleasedMultiDragVec(TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetReleasedMultiDragVecRaw()
	{
		return GetReleasedMultiDragVec(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetReleasedMultiDragVecRaw(TouchCoordSys cs)
	{
		return GetReleasedMultiDragVec(cs, raw: true);
	}

	public float GetReleasedDuration(int fingerId)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		return finger.endedEndTime - finger.endedStartTime;
	}

	public float GetReleasedUniDuration()
	{
		return endedUniEndTime - endedUniStartTime;
	}

	public float GetReleasedMultiDuration()
	{
		return endedMultiEndTime - endedMultiStartTime;
	}

	public Vector2 GetReleasedDragVel(int fingerId, TouchCoordSys cs, bool raw)
	{
		Finger finger = ((fingerId != 1) ? fingerA : fingerB);
		if (!raw && !finger.endedMoved)
		{
			return Vector2.zero;
		}
		return TransformPos(finger.endedDragVel, cs, deltaMode: true);
	}

	public Vector2 GetReleasedDragVel(int fingerId)
	{
		return GetReleasedDragVel(fingerId, TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetReleasedDragVel(int fingerId, TouchCoordSys cs)
	{
		return GetReleasedDragVel(fingerId, cs, raw: false);
	}

	public Vector2 GetReleasedDragVel(int fingerId, bool raw)
	{
		return GetReleasedDragVel(fingerId, TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetReleasedDragVelRaw(int fingerId)
	{
		return GetReleasedDragVel(fingerId, TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetReleasedDragVelRaw(int fingerId, TouchCoordSys cs)
	{
		return GetReleasedDragVel(fingerId, cs, raw: true);
	}

	public Vector2 GetReleasedUniDragVel(TouchCoordSys cs, bool raw)
	{
		if (!raw && !endedUniMoved)
		{
			return Vector2.zero;
		}
		return TransformPos(endedUniDragVel, cs, deltaMode: true);
	}

	public Vector2 GetReleasedUniDragVel()
	{
		return GetReleasedUniDragVel(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetReleasedUniDragVel(TouchCoordSys cs)
	{
		return GetReleasedUniDragVel(cs, raw: false);
	}

	public Vector2 GetReleasedUniDragVel(bool raw)
	{
		return GetReleasedUniDragVel(TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetReleasedUniDragVelRaw()
	{
		return GetReleasedUniDragVel(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetReleasedUniDragVelRaw(TouchCoordSys cs)
	{
		return GetReleasedUniDragVel(cs, raw: true);
	}

	public Vector2 GetReleasedMultiDragVel(TouchCoordSys cs, bool raw)
	{
		if (!raw && !endedMultiMoved)
		{
			return Vector2.zero;
		}
		return TransformPos(endedMultiDragVel, cs, deltaMode: true);
	}

	public Vector2 GetReleasedMultiDragVel()
	{
		return GetReleasedMultiDragVel(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public Vector2 GetReleasedMultiDragVel(TouchCoordSys cs)
	{
		return GetReleasedMultiDragVel(cs, raw: false);
	}

	public Vector2 GetReleasedMultiDragVel(bool raw)
	{
		return GetReleasedMultiDragVel(TouchCoordSys.SCREEN_PX, raw);
	}

	public Vector2 GetReleasedMultiDragVelRaw()
	{
		return GetReleasedMultiDragVel(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public Vector2 GetReleasedMultiDragVelRaw(TouchCoordSys cs)
	{
		return GetReleasedMultiDragVel(cs, raw: true);
	}

	public bool ReleasedTwisted()
	{
		return endedTwistMoved;
	}

	public bool ReleasedTwistMoved()
	{
		return ReleasedTwisted();
	}

	public float GetReleasedTwistAngle(bool raw)
	{
		if (!raw && !endedTwistMoved)
		{
			return 0f;
		}
		return endedTwistAngle;
	}

	public float GetReleasedTwistAngle()
	{
		return GetReleasedTwistAngle(raw: false);
	}

	public float GetReleasedTwistAngleRaw()
	{
		return GetReleasedTwistAngle(raw: true);
	}

	public float GetReleasedTwistVel(bool raw)
	{
		if (!raw && !endedTwistMoved)
		{
			return 0f;
		}
		return endedTwistVel;
	}

	public float GetReleasedTwistVel()
	{
		return GetReleasedTwistVel(raw: false);
	}

	public float GetReleasedTwistVelRaw()
	{
		return GetReleasedTwistVel(raw: true);
	}

	public bool ReleasedPinched()
	{
		return endedPinchMoved;
	}

	public bool ReleasedPinchMoved()
	{
		return ReleasedPinched();
	}

	public float GetReleasedPinchScale(bool raw)
	{
		if (!raw && !endedPinchMoved)
		{
			return 1f;
		}
		return endedPinchDistEnd / endedPinchDistStart;
	}

	public float GetReleasedPinchScale()
	{
		return GetReleasedPinchScale(raw: false);
	}

	public float GetReleasedPinchScaleRaw()
	{
		return GetReleasedPinchScale(raw: true);
	}

	public float GetReleasedPinchStartDist(TouchCoordSys cs)
	{
		return TransformPos(endedPinchDistStart, cs);
	}

	public float GetReleasedPinchStartDist()
	{
		return GetReleasedPinchStartDist(TouchCoordSys.SCREEN_PX);
	}

	public float GetReleasedPinchEndDist(TouchCoordSys cs)
	{
		return TransformPos(endedPinchDistEnd, cs);
	}

	public float GetReleasedPinchEndDist()
	{
		return GetReleasedPinchEndDist(TouchCoordSys.SCREEN_PX);
	}

	public float GetReleasedPinchDistVel(TouchCoordSys cs, bool raw)
	{
		if (!raw && !endedPinchMoved)
		{
			return 0f;
		}
		return TransformPos(endedPinchDistVel, cs);
	}

	public float GetReleasedPinchDistVel()
	{
		return GetReleasedPinchDistVel(TouchCoordSys.SCREEN_PX, raw: false);
	}

	public float GetReleasedPinchDistVel(TouchCoordSys cs)
	{
		return GetReleasedPinchDistVel(cs, raw: false);
	}

	public float GetReleasedPinchDistVel(bool raw)
	{
		return GetReleasedPinchDistVel(TouchCoordSys.SCREEN_PX, raw);
	}

	public float GetReleasedPinchDistVelRaw()
	{
		return GetReleasedPinchDistVel(TouchCoordSys.SCREEN_PX, raw: true);
	}

	public float GetReleasedPinchDistVelRaw(TouchCoordSys cs)
	{
		return GetReleasedPinchDistVel(cs, raw: true);
	}

	public void TotalTakeover()
	{
		joy.EndTouch(fingerA.touchId, this);
		joy.EndTouch(fingerB.touchId, this);
	}

	public override void Enable(bool skipAnimation)
	{
		enabled = true;
		AnimateParams((!overrideScale) ? joy.releasedZoneScale : releasedScale, TouchController.ScaleAlpha((!overrideColors) ? joy.defaultReleasedZoneColor : releasedColor, visible ? 1 : 0), skipAnimation ? 0f : ((!overrideAnimDuration) ? joy.enableAnimDuration : enableAnimDuration));
	}

	public override void Disable(bool skipAnim)
	{
		enabled = false;
		ReleaseTouches();
		AnimateParams((!overrideScale) ? joy.disabledZoneScale : disabledScale, TouchController.ScaleAlpha((!overrideColors) ? joy.defaultDisabledZoneColor : disabledColor, visible ? 1 : 0), skipAnim ? 0f : ((!overrideAnimDuration) ? joy.disableAnimDuration : disableAnimDuration));
	}

	public override void Show(bool skipAnim)
	{
		visible = true;
		AnimateParams(overrideScale ? ((!enabled) ? disabledScale : releasedScale) : ((!enabled) ? joy.disabledZoneScale : joy.releasedZoneScale), overrideColors ? ((!enabled) ? disabledColor : releasedColor) : ((!enabled) ? joy.defaultDisabledZoneColor : joy.defaultReleasedZoneColor), skipAnim ? 0f : ((!overrideAnimDuration) ? joy.showAnimDuration : showAnimDuration));
	}

	public override void Hide(bool skipAnim)
	{
		visible = false;
		ReleaseTouches();
		Color end = animColor.end;
		end.a = 0f;
		AnimateParams(animScale.end, end, skipAnim ? 0f : ((!overrideAnimDuration) ? joy.hideAnimDuration : hideAnimDuration));
	}

	public void SetRect(Rect r)
	{
		if (screenRectPx != r)
		{
			screenRectPx = r;
			posPx = r.center;
			if (shape == TouchController.ControlShape.CIRCLE)
			{
				sizePx.x = (sizePx.y = Mathf.Min(r.width, r.height));
			}
			else
			{
				sizePx.x = r.width;
				sizePx.y = r.height;
			}
			OnReset();
		}
	}

	public override void ResetRect()
	{
		SetRect(layoutRectPx);
	}

	public Rect GetRect(bool getAutoRect)
	{
		if (getAutoRect)
		{
			return layoutRectPx;
		}
		return screenRectPx;
	}

	public Rect GetRect()
	{
		return GetRect(getAutoRect: false);
	}

	public Rect GetDisplayRect(bool applyScale)
	{
		Rect cenRect = screenRectPx;
		if (shape == TouchController.ControlShape.CIRCLE || shape == TouchController.ControlShape.RECT)
		{
			cenRect = TouchController.GetCenRect(posPx, sizePx * ((!applyScale) ? 1f : animScale.cur));
		}
		return cenRect;
	}

	public Rect GetDisplayRect()
	{
		return GetDisplayRect(applyScale: true);
	}

	public Color GetColor()
	{
		return animColor.cur;
	}

	public int GetGUIDepth()
	{
		return joy.guiDepth + guiDepth + (UniPressed() ? joy.guiPressedOfs : 0);
	}

	public Texture2D GetDisplayTex()
	{
		if (enabled && UniPressed())
		{
			return pressedImg;
		}
		return releasedImg;
	}

	public bool GetKey(KeyCode key)
	{
		bool keySupported = false;
		return GetKeyEx(key, out keySupported);
	}

	public bool GetKeyDown(KeyCode key)
	{
		bool keySupported = false;
		return GetKeyDownEx(key, out keySupported);
	}

	public bool GetKeyUp(KeyCode key)
	{
		bool keySupported = false;
		return GetKeyUpEx(key, out keySupported);
	}

	public bool GetKeyEx(KeyCode key, out bool keySupported)
	{
		keySupported = false;
		if (!enableGetKey || key == KeyCode.None)
		{
			return false;
		}
		if (key == getKeyCode || key == getKeyCodeAlt)
		{
			keySupported = true;
			if (UniPressed())
			{
				return true;
			}
		}
		if (key == getKeyCodeMulti || key == getKeyCodeMultiAlt)
		{
			keySupported = true;
			if (MultiPressed())
			{
				return true;
			}
		}
		return false;
	}

	public bool GetKeyDownEx(KeyCode key, out bool keySupported)
	{
		keySupported = false;
		if (!enableGetKey || key == KeyCode.None)
		{
			return false;
		}
		if (key == getKeyCode || key == getKeyCodeAlt)
		{
			keySupported = true;
			if (JustUniPressed())
			{
				return true;
			}
		}
		if (key == getKeyCodeMulti || key == getKeyCodeMultiAlt)
		{
			keySupported = true;
			if (JustMultiPressed())
			{
				return true;
			}
		}
		return false;
	}

	public bool GetKeyUpEx(KeyCode key, out bool keySupported)
	{
		keySupported = false;
		if (!enableGetKey || key == KeyCode.None)
		{
			return false;
		}
		if (key == getKeyCode || key == getKeyCodeAlt)
		{
			keySupported = true;
			if (JustUniReleased())
			{
				return true;
			}
		}
		if (key == getKeyCodeMulti || key == getKeyCodeMultiAlt)
		{
			keySupported = true;
			if (JustMultiReleased())
			{
				return true;
			}
		}
		return false;
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
		if (!enableGetButton)
		{
			return false;
		}
		if (buttonName == getButtonName)
		{
			buttonSupported = true;
			if (UniPressed())
			{
				return true;
			}
		}
		if (buttonName == getButtonMultiName)
		{
			buttonSupported = true;
			if (MultiPressed())
			{
				return true;
			}
		}
		return false;
	}

	public bool GetButtonDownEx(string buttonName, out bool buttonSupported)
	{
		buttonSupported = false;
		if (!enableGetButton)
		{
			return false;
		}
		if (buttonName == getButtonName)
		{
			buttonSupported = true;
			if (JustUniPressed())
			{
				return true;
			}
		}
		if (buttonName == getButtonMultiName)
		{
			buttonSupported = true;
			if (JustMultiPressed())
			{
				return true;
			}
		}
		return false;
	}

	public bool GetButtonUpEx(string buttonName, out bool buttonSupported)
	{
		buttonSupported = false;
		if (!enableGetButton)
		{
			return false;
		}
		if (buttonName == getButtonName)
		{
			buttonSupported = true;
			if (JustUniReleased())
			{
				return true;
			}
		}
		if (buttonName == getButtonMultiName)
		{
			buttonSupported = true;
			if (JustMultiReleased())
			{
				return true;
			}
		}
		return false;
	}

	public float GetAxis(string axisName)
	{
		bool supported = false;
		return GetAxisEx(axisName, out supported);
	}

	public float GetAxisEx(string axisName, out bool supported)
	{
		if (enableGetAxis)
		{
			if (axisHorzName == axisName)
			{
				supported = true;
				return GetUniDragDelta(raw: true).x * axisValScale * ((!axisHorzFlip) ? 1f : (-1f));
			}
			if (axisVertName == axisName)
			{
				supported = true;
				return GetUniDragDelta(raw: true).y * (0f - axisValScale) * ((!axisVertFlip) ? 1f : (-1f));
			}
		}
		supported = false;
		return 0f;
	}

	public static int GetBoxPortion(int horzSections, int vertSections, Vector2 normalizedPos)
	{
		int num = 0;
		int num2 = 0;
		if (horzSections == 2)
		{
			num = ((!(normalizedPos.x >= 0.5f)) ? 1 : 4);
		}
		else if (horzSections >= 3)
		{
			num = ((!(normalizedPos.x >= 0.333f)) ? 1 : ((normalizedPos.x <= 0.666f) ? 2 : 4));
		}
		if (vertSections == 2)
		{
			num2 = ((normalizedPos.y >= 0.5f) ? 32 : 8);
		}
		else if (vertSections >= 3)
		{
			num2 = ((!(normalizedPos.y >= 0.333f)) ? 8 : ((normalizedPos.y <= 0.666f) ? 16 : 32));
		}
		return num | num2;
	}

	public override void Init(TouchController joy)
	{
		base.Init(joy);
		base.joy = joy;
		fingerA = new Finger(this);
		fingerB = new Finger(this);
		AnimateParams((!overrideScale) ? base.joy.releasedZoneScale : releasedScale, (!overrideColors) ? base.joy.defaultReleasedZoneColor : releasedColor, 0f);
		OnReset();
		if (initiallyDisabled)
		{
			Disable(skipAnim: true);
		}
		if (initiallyHidden)
		{
			Hide(skipAnim: true);
		}
	}

	public int GetFingerNum()
	{
		return (fingerA.curState ? 1 : 0) + (fingerB.curState ? 1 : 0);
	}

	private void AnimateParams(float scale, Color color, float duration)
	{
		if (duration <= 0f)
		{
			animTimer.Reset();
			animTimer.Disable();
			animColor.Reset(color);
			animScale.Reset(scale);
		}
		else
		{
			animTimer.Start(duration);
			animScale.MoveTo(scale);
			animColor.MoveTo(color);
		}
	}

	public override void OnReset()
	{
		fingerA.Reset();
		fingerB.Reset();
		multiCur = (multiPrev = (justMultiTapped = (justMultiDelayTapped = (justMultiDoubleTapped = (nextTapCanBeMultiDoubleTap = false)))));
		twistMoved = (twistJustMoved = (pinchMoved = (pinchJustMoved = (uniMoved = (uniJustMoved = (uniCur = (uniPrev = false)))))));
		multiStartTime = (lastMultiTapTime = (uniStartTime = -100f));
		multiPosCur = (multiPosPrev = (multiPosStart = (lastMultiTapPos = Vector2.zero)));
		multiDragVel = Vector2.zero;
		uniDragVel = Vector2.zero;
		twistVel = 0f;
		pinchDistVel = 0f;
		twistStartAbs = (twistCurAbs = (twistCur = (twistCurRaw = (twistPrevAbs = (twistPrev = 0f)))));
		twistVel = 0f;
		pinchDistStart = (pinchCurDist = (pinchPrevDist = (pinchDistVel = 0f)));
		uniPosCur = (uniPosStart = (uniTotalDragCur = (uniTotalDragPrev = Vector2.zero)));
		touchCanceled = false;
		AnimateParams((!overrideScale) ? joy.releasedZoneScale : releasedScale, (!overrideColors) ? joy.defaultReleasedZoneColor : releasedColor, 0f);
		if (!enabled)
		{
			Disable(skipAnim: true);
		}
		if (!visible)
		{
			Hide(skipAnim: true);
		}
	}

	public override void OnPrePoll()
	{
		fingerA.OnPrePoll();
		fingerB.OnPrePoll();
	}

	public override void OnPostPoll()
	{
		if (fingerA.touchId >= 0 && !fingerA.touchVerified)
		{
			OnTouchEnd(fingerA.touchId);
		}
		if (fingerB.touchId >= 0 && !fingerB.touchVerified)
		{
			OnTouchEnd(fingerB.touchId);
		}
	}

	public override void ReleaseTouches()
	{
		if (fingerA.touchId >= 0)
		{
			OnTouchEnd(fingerA.touchId, canceled: true);
		}
		if (fingerB.touchId >= 0)
		{
			OnTouchEnd(fingerB.touchId, canceled: true);
		}
	}

	private void OnMultiStart(Vector2 startPos, Vector2 curPos)
	{
		multiCur = true;
		multiStartTime = joy.curTime;
		multiPosStart = startPos;
		multiPosCur = curPos;
		multiPosPrev = curPos;
		multiMoved = false;
		multiLastMoveTime = 0f;
		multiDragVel = Vector2.zero;
		multiExtremeCurVec = Vector2.zero;
		multiExtremeCurDist = 0f;
		pinchCurDist = (pinchPrevDist = (pinchDistStart = GetFingerDist()));
		pinchMoved = false;
		pinchJustMoved = false;
		pinchLastMoveTime = 0f;
		pinchDistVel = 0f;
		pinchExtremeCurDist = 0f;
		twistCurAbs = (twistPrevAbs = (twistStartAbs = GetFingerAbsAngle()));
		twistCur = (twistPrev = 0f);
		twistMoved = false;
		twistJustMoved = false;
		twistLastMoveTime = 0f;
		twistVel = 0f;
		twistExtremeCur = 0f;
	}

	private void OnMultiEnd(Vector2 endPos)
	{
		multiPosCur = endPos;
		UpdateMultiTouchState(lastUpdateMode: true);
		multiCur = false;
		endedMultiStartTime = multiStartTime;
		endedMultiEndTime = joy.curTime;
		endedMultiPosEnd = endPos;
		endedMultiPosStart = multiPosStart;
		endedMultiDragVel = multiDragVel;
		endedTwistAngle = twistCur;
		endedTwistVel = twistVel;
		endedPinchDistStart = pinchDistStart;
		endedPinchDistEnd = pinchCurDist;
		endedPinchDistVel = pinchDistVel;
		endedMultiMoved = multiMoved;
		endedTwistMoved = twistMoved;
		endedPinchMoved = pinchMoved;
	}

	private void OnUniStart(Vector2 startPos, Vector2 curPos)
	{
		uniCur = true;
		uniStartTime = joy.curTime;
		uniPosStart = startPos;
		uniPosCur = curPos;
		uniMoved = false;
		uniJustMoved = false;
		uniExtremeDragCurVec = Vector2.zero;
		uniExtremeDragCurDist = 0f;
		uniDragVel = Vector2.zero;
		uniTotalDragPrev = Vector2.zero;
		uniTotalDragCur = Vector2.zero;
	}

	private void OnUniEnd(Vector2 endPos, Vector2 endDeltaAccum)
	{
		uniTotalDragCur += endDeltaAccum;
		uniPosCur = endPos;
		UpdateUniTouchState(lastUpdateMode: true);
		uniCur = false;
		endedUniPosEnd = endPos;
		endedUniStartTime = uniStartTime;
		endedUniEndTime = joy.curTime;
		endedUniDragVel = uniDragVel;
		endedUniPosStart = uniPosStart;
		endedUniTotalDrag = uniTotalDragCur;
		endedUniMoved = uniMoved;
	}

	private void OnPinchStart()
	{
		if (!pinchMoved)
		{
			pinchMoved = true;
			pinchJustMoved = true;
			if (startDragWhenPinching)
			{
				OnMultiDragStart();
			}
			if (startTwistWhenPinching)
			{
				OnTwistStart();
			}
		}
	}

	private void OnTwistStart()
	{
		if (!twistMoved)
		{
			twistMoved = true;
			twistJustMoved = true;
			twistStartRaw = twistCurRaw;
			twistCur = 0f;
			if (startDragWhenTwisting)
			{
				OnMultiDragStart();
			}
			if (startPinchWhenTwisting)
			{
				OnPinchStart();
			}
		}
	}

	private void OnMultiDragStart()
	{
		if (!multiMoved)
		{
			multiMoved = true;
			multiJustMoved = true;
			if (startTwistWhenDragging)
			{
				OnTwistStart();
			}
			if (startPinchWhenDragging)
			{
				OnPinchStart();
			}
		}
	}

	private void UpdateUniTouchState(bool lastUpdateMode = false)
	{
		if (lastUpdateMode)
		{
			return;
		}
		uniExtremeDragCurVec.x = Mathf.Max(Mathf.Abs(uniTotalDragCur.x), uniExtremeDragCurVec.x);
		uniExtremeDragCurVec.y = Mathf.Max(Mathf.Abs(uniTotalDragCur.y), uniExtremeDragCurVec.y);
		uniExtremeDragCurDist = Mathf.Max(uniTotalDragCur.magnitude, uniExtremeDragCurDist);
		uniJustMoved = false;
		if (!uniMoved && uniExtremeDragCurDist > joy.touchTapMaxDistPx)
		{
			uniMoved = true;
			uniJustMoved = true;
		}
		if (!uniCur)
		{
			return;
		}
		if (PxPosEquals(uniTotalDragCur, uniTotalDragPrev))
		{
			if (joy.curTime - uniLastMoveTime > joy.velPreserveTime)
			{
				uniDragVel = Vector2.zero;
			}
		}
		else
		{
			uniLastMoveTime = joy.curTime;
			uniDragVel = (uniTotalDragCur - uniTotalDragPrev) * joy.invDeltaTime;
		}
	}

	private void UpdateMultiTouchState(bool lastUpdateMode = false)
	{
		if (lastUpdateMode)
		{
			return;
		}
		multiJustMoved = false;
		pinchJustMoved = false;
		twistJustMoved = false;
		if (multiCur)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			Vector2 vector = multiPosCur - multiPosStart;
			multiExtremeCurVec.x = Mathf.Max(Mathf.Abs(vector.x), multiExtremeCurVec.x);
			multiExtremeCurVec.y = Mathf.Max(Mathf.Abs(vector.y), multiExtremeCurVec.y);
			multiExtremeCurDist = Mathf.Max(vector.magnitude, multiExtremeCurDist);
			if (!multiMoved && multiExtremeCurDist > joy.touchTapMaxDistPx)
			{
				flag = true;
			}
			pinchJustMoved = false;
			pinchCurDist = GetFingerDist();
			pinchExtremeCurDist = Mathf.Max(Mathf.Abs(pinchCurDist - pinchDistStart), pinchExtremeCurDist);
			if (!pinchMoved && pinchExtremeCurDist > joy.pinchMinDistPx)
			{
				flag2 = true;
			}
			twistJustMoved = false;
			twistCurAbs = GetFingerAbsAngle(twistPrevAbs);
			twistCurRaw = Mathf.DeltaAngle(twistCurAbs, twistStartAbs);
			bool flag4 = pinchCurDist > joy.twistSafeFingerDistPx;
			if (!twistMoved && flag4 && Mathf.Abs(twistCurRaw) * ((float)Math.PI / 180f) * 2f * pinchCurDist > joy.pinchMinDistPx)
			{
				flag3 = true;
			}
			if (twistMoved && (flag4 || !freezeTwistWhenTooClose))
			{
				twistCur = twistCurRaw - twistStartRaw;
				twistExtremeCur = Mathf.Max(Mathf.Abs(twistCur), twistExtremeCur);
			}
			int num = 0;
			switch (gestureDetectionOrder)
			{
			case GestureDetectionOrder.TWIST_PINCH_DRAG:
				num = 136;
				break;
			case GestureDetectionOrder.TWIST_DRAG_PINCH:
				num = 80;
				break;
			case GestureDetectionOrder.PINCH_TWIST_DRAG:
				num = 129;
				break;
			case GestureDetectionOrder.PINCH_DRAG_TWIST:
				num = 17;
				break;
			case GestureDetectionOrder.DRAG_TWIST_PINCH:
				num = 66;
				break;
			case GestureDetectionOrder.DRAG_PINCH_TWIST:
				num = 10;
				break;
			}
			for (int i = 0; i < 3; i++)
			{
				switch ((num >> i * 3) & 7)
				{
				case 0:
					if (twistMoved || flag3)
					{
						if (noDragAfterTwist)
						{
							flag = false;
						}
						if (noPinchAfterTwist)
						{
							flag2 = false;
						}
					}
					break;
				case 1:
					if (pinchMoved || flag2)
					{
						if (noDragAfterPinch)
						{
							flag = false;
						}
						if (noTwistAfterPinch)
						{
							flag3 = false;
						}
					}
					break;
				case 2:
					if (multiMoved || flag)
					{
						if (noTwistAfterDrag)
						{
							flag3 = false;
						}
						if (noPinchAfterDrag)
						{
							flag2 = false;
						}
					}
					break;
				}
			}
			if (flag)
			{
				OnMultiDragStart();
			}
			if (flag2)
			{
				OnPinchStart();
			}
			if (flag3)
			{
				OnTwistStart();
			}
		}
		if (!multiCur)
		{
			return;
		}
		if (PxPosEquals(multiPosCur, multiPosPrev))
		{
			if (joy.curTime - multiLastMoveTime > joy.velPreserveTime)
			{
				multiDragVel = Vector2.zero;
			}
		}
		else
		{
			multiLastMoveTime = joy.curTime;
			multiDragVel = (multiPosCur - multiPosPrev) * joy.invDeltaTime;
		}
		if (PxDistEquals(pinchCurDist, pinchPrevDist))
		{
			if (joy.curTime - pinchLastMoveTime > joy.velPreserveTime)
			{
				pinchDistVel = 0f;
			}
		}
		else
		{
			pinchLastMoveTime = joy.curTime;
			pinchDistVel = (pinchCurDist - pinchPrevDist) * joy.invDeltaTime;
		}
		if (TwistAngleEquals(twistCur, twistPrev))
		{
			if (joy.curTime - twistLastMoveTime > joy.velPreserveTime)
			{
				twistVel = 0f;
			}
		}
		else
		{
			twistLastMoveTime = joy.curTime;
			twistVel = (twistCur - twistPrev) * joy.invDeltaTime;
		}
	}

	public override void OnUpdate(bool firstUpdate)
	{
		fingerA.PreUpdate(firstUpdate);
		fingerB.PreUpdate(firstUpdate);
		uniPrev = uniCur;
		uniTotalDragPrev = uniTotalDragCur;
		uniMidFramePressed = false;
		uniMidFrameReleased = false;
		if (uniCur && pollUniReleasedInitial)
		{
			OnUniEnd(pollUniPosEnd, pollUniDeltaAccumAtEnd);
		}
		if (pollUniTouched)
		{
			OnUniStart(pollUniPosStart, pollUniPosCur);
		}
		if ((fingerA.touchId >= 0 || fingerB.touchId >= 0) != uniCur)
		{
			if (uniCur)
			{
				OnUniEnd(pollUniPosEnd, pollUniDeltaAccumAtEnd);
			}
			else
			{
				OnUniStart(pollUniPosStart, pollUniPosCur);
			}
		}
		uniMidFramePressed = !pollUniInitialState && pollUniTouched && !uniCur;
		uniMidFrameReleased = pollUniInitialState && pollUniReleasedInitial && uniCur;
		if (uniCur)
		{
			uniPosCur = pollUniPosCur;
		}
		uniTotalDragCur += pollUniDeltaAccum;
		UpdateUniTouchState();
		pollUniReleasedInitial = false;
		pollUniReleased = false;
		pollUniTouched = false;
		pollUniInitialState = uniCur;
		pollUniPosCur = (pollUniPosPrev = (pollUniPosStart = (pollUniPosEnd = uniPosCur)));
		pollUniWaitForDblStart = false;
		pollUniWaitForDblEnd = false;
		pollUniDeltaAccum = (pollUniDblEndPos = (pollUniDeltaAccumAtEnd = Vector2.zero));
		multiPrev = multiCur;
		multiPosPrev = multiPosCur;
		pinchPrevDist = pinchCurDist;
		twistPrevAbs = twistCurAbs;
		twistPrev = twistCur;
		multiMidFramePressed = false;
		multiMidFrameReleased = false;
		if (multiCur && pollMultiReleasedInitial)
		{
			OnMultiEnd(pollMultiPosEnd);
		}
		if (pollMultiTouched)
		{
			OnMultiStart(pollMultiPosStart, pollMultiPosCur);
		}
		if ((fingerA.touchId >= 0 && fingerB.touchId >= 0) != multiCur)
		{
			if (multiCur)
			{
				OnMultiEnd(pollMultiPosEnd);
			}
			else
			{
				OnMultiStart(pollMultiPosStart, pollMultiPosCur);
			}
		}
		multiMidFramePressed = !pollMultiInitialState && pollMultiTouched && !multiCur;
		multiMidFrameReleased = pollMultiInitialState && pollMultiReleasedInitial && multiCur;
		if (multiCur)
		{
			multiPosCur = pollMultiPosCur;
		}
		UpdateMultiTouchState();
		pollMultiReleasedInitial = false;
		pollMultiReleased = false;
		pollMultiTouched = false;
		pollMultiInitialState = multiCur;
		pollMultiPosCur = (pollMultiPosStart = (pollMultiPosEnd = multiPosCur));
		justMultiDoubleTapped = false;
		justMultiTapped = false;
		justMultiDelayTapped = false;
		if (JustMultiReleased(trueOnMidFramePress: true, trueOnMidFrameRelease: true))
		{
			if (!endedMultiMoved && endedMultiEndTime - endedMultiStartTime <= joy.touchTapMaxTime)
			{
				bool flag = nextTapCanBeMultiDoubleTap && endedMultiStartTime - lastMultiTapTime <= joy.doubleTapMaxGapTime;
				waitForMultiDelyedTap = !flag;
				justMultiDoubleTapped = flag;
				justMultiTapped = true;
				lastMultiTapPos = endedMultiPosStart;
				lastMultiTapTime = joy.curTime;
				nextTapCanBeMultiDoubleTap = !flag;
				fingerA.CancelTap();
				fingerB.CancelTap();
			}
			else
			{
				waitForMultiDelyedTap = false;
				nextTapCanBeMultiDoubleTap = false;
			}
		}
		else if (JustMultiPressed(trueOnMidFramePress: true, trueOnMidFrameRelease: true))
		{
			waitForMultiDelyedTap = false;
		}
		else if (waitForMultiDelyedTap && joy.curTime - lastMultiTapTime > joy.doubleTapMaxGapTime)
		{
			justMultiDelayTapped = true;
			waitForMultiDelyedTap = false;
			nextTapCanBeMultiDoubleTap = true;
		}
		if (emulateMouse)
		{
			joy.SetInternalMousePos((!mousePosFromFirstFinger) ? GetUniPos(TouchCoordSys.SCREEN_PX) : GetPos(0, TouchCoordSys.SCREEN_PX));
		}
		if (uniCur != uniPrev && enabled)
		{
			if (uniCur)
			{
				AnimateParams((!overrideScale) ? joy.pressedZoneScale : pressedScale, (!overrideColors) ? joy.defaultPressedZoneColor : pressedColor, (!overrideAnimDuration) ? joy.pressAnimDuration : pressAnimDuration);
			}
			else
			{
				AnimateParams((!overrideScale) ? joy.releasedZoneScale : releasedScale, (!overrideColors) ? joy.defaultReleasedZoneColor : releasedColor, touchCanceled ? joy.cancelAnimDuration : ((!overrideAnimDuration) ? joy.releaseAnimDuration : releaseAnimDuration));
			}
		}
		if (animTimer.Enabled)
		{
			animTimer.Update(joy.deltaTime);
			float lerpt = TouchController.SlowDownEase(animTimer.Nt);
			animColor.Update(lerpt);
			animScale.Update(lerpt);
			if (animTimer.Completed)
			{
				animTimer.Disable();
			}
		}
	}

	public override void OnPostUpdate(bool firstUpdate)
	{
		fingerA.PostUpdate(firstUpdate);
		fingerB.PostUpdate(firstUpdate);
	}

	public override void OnLayoutAddContent()
	{
		if (shape != TouchController.ControlShape.SCREEN_REGION)
		{
			TouchController.LayoutBox layoutBox = joy.layoutBoxes[layoutBoxId];
			switch (shape)
			{
			case TouchController.ControlShape.RECT:
				layoutBox.AddContent(posCm, sizeCm);
				break;
			case TouchController.ControlShape.CIRCLE:
				layoutBox.AddContent(posCm, sizeCm.x);
				break;
			}
		}
	}

	public override void OnLayout()
	{
		switch (shape)
		{
		case TouchController.ControlShape.CIRCLE:
		case TouchController.ControlShape.RECT:
		{
			TouchController.LayoutBox layoutBox = joy.layoutBoxes[layoutBoxId];
			layoutPosPx = layoutBox.GetScreenPos(posCm);
			layoutSizePx = layoutBox.GetScreenSize(sizeCm);
			layoutRectPx = new Rect(layoutPosPx.x - 0.5f * layoutSizePx.x, layoutPosPx.y - 0.5f * layoutSizePx.y, layoutSizePx.x, layoutSizePx.y);
			break;
		}
		case TouchController.ControlShape.SCREEN_REGION:
			layoutRectPx = joy.NormalizedRectToPx(regionRect);
			layoutPosPx = layoutRectPx.center;
			layoutSizePx.x = layoutRectPx.width;
			layoutSizePx.y = layoutRectPx.height;
			screenRectPx = layoutRectPx;
			break;
		}
		posPx = layoutPosPx;
		sizePx = layoutSizePx;
		screenRectPx = layoutRectPx;
		OnReset();
	}

	public override void DrawGUI()
	{
		if (!disableGui)
		{
			bool flag = UniPressed(trueOnMidFramePress: true, falseOnMidFrameRelease: false);
			Texture2D texture2D = ((!flag) ? releasedImg : pressedImg);
			if (texture2D != null)
			{
				GUI.depth = joy.guiDepth + guiDepth + (flag ? joy.guiPressedOfs : 0);
				Rect displayRect = GetDisplayRect(applyScale: true);
				GUI.color = TouchController.ScaleAlpha(animColor.cur, joy.GetAlpha());
				GUI.DrawTexture(displayRect, texture2D);
			}
		}
	}

	public override void TakeoverTouches(TouchableControl controlToUntouch)
	{
		if (controlToUntouch != null)
		{
			if (fingerA.touchId >= 0)
			{
				controlToUntouch.OnTouchEnd(fingerA.touchId, cancel: true);
			}
			if (fingerB.touchId >= 0)
			{
				controlToUntouch.OnTouchEnd(fingerB.touchId, cancel: true);
			}
		}
	}

	public bool MultiTouchPossible()
	{
		if (enableSecondFinger && fingerA.touchId >= 0 && fingerB.touchId < 0)
		{
			if (strictTwoFingerStart)
			{
				return joy.curTime - fingerA.startTime < joy.strictMultiFingerMaxTime;
			}
			return true;
		}
		return false;
	}

	public override TouchController.HitTestResult HitTest(Vector2 touchPos, int touchId)
	{
		if (!enabled || !visible || (fingerA.touchId >= 0 && (!enableSecondFinger || fingerB.touchId >= 0 || (strictTwoFingerStart && !fingerA.pollTouched && joy.curTime - fingerA.startTime > joy.strictMultiFingerMaxTime))) || touchId == fingerA.touchId || touchId == fingerB.touchId)
		{
			return new TouchController.HitTestResult(hit: false);
		}
		TouchController.HitTestResult result;
		switch (shape)
		{
		case TouchController.ControlShape.CIRCLE:
			result = joy.HitTestCircle(posPx, 0.5f * sizePx.x, touchPos);
			break;
		case TouchController.ControlShape.RECT:
			result = joy.HitTestBox(posPx, sizePx, touchPos);
			break;
		case TouchController.ControlShape.SCREEN_REGION:
			result = joy.HitTestRect(screenRectPx, touchPos);
			break;
		default:
			result = new TouchController.HitTestResult(hit: false);
			break;
		}
		result.prio = prio;
		result.distScale = hitDistScale;
		return result;
	}

	public override TouchController.EventResult OnTouchStart(int touchId, Vector2 pos)
	{
		Finger finger = ((fingerA.touchId < 0) ? fingerA : ((fingerB.touchId >= 0) ? null : fingerB));
		if (finger == null)
		{
			return TouchController.EventResult.NOT_HANDLED;
		}
		touchCanceled = false;
		Finger finger2 = ((finger != fingerA) ? fingerA : fingerB);
		finger.touchId = touchId;
		finger.touchVerified = true;
		finger.touchPos = pos;
		finger.pollTouched = true;
		finger.pollPosStart = pos;
		finger.pollPosCur = pos;
		if (finger2.touchId < 0)
		{
			pollUniTouched = true;
			pollUniPosStart = pos;
			pollUniPosCur = pos;
			pollUniWaitForDblStart = true;
			pollUniWaitForDblEnd = false;
			pollUniDeltaAccum = Vector2.zero;
		}
		else
		{
			finger2.CancelTap();
			pollMultiTouched = true;
			pollMultiPosStart = (pollMultiPosCur = (fingerA.pollPosCur + fingerB.pollPosCur) / 2f);
			pollUniPosCur = pollMultiPosCur;
			if (pollUniWaitForDblStart)
			{
				pollUniPosStart = pollUniPosCur;
				pollUniWaitForDblStart = false;
				pollUniWaitForDblEnd = true;
			}
		}
		pollUniPosPrev = pollUniPosCur;
		if (nonExclusiveTouches)
		{
			return TouchController.EventResult.SHARED;
		}
		return TouchController.EventResult.HANDLED;
	}

	public override TouchController.EventResult OnTouchEnd(int touchId, bool canceled = false)
	{
		Finger finger = ((fingerA.touchId == touchId) ? fingerA : ((fingerB.touchId != touchId) ? null : fingerB));
		if (finger == null)
		{
			return TouchController.EventResult.NOT_HANDLED;
		}
		Finger finger2 = ((finger != fingerA) ? fingerA : fingerB);
		finger.touchId = -1;
		finger.touchVerified = true;
		if (!finger.pollReleased)
		{
			finger.pollReleased = true;
			finger.pollPosEnd = finger.pollPosCur;
			if (finger.pollInitialState)
			{
				finger.pollReleasedInitial = true;
			}
		}
		finger.pollTouched = false;
		if (finger2.touchId >= 0)
		{
			pollUniPosCur = finger2.pollPosCur;
			pollUniWaitForDblEnd = true;
			pollUniDblEndPos = (fingerA.pollPosCur + fingerB.pollPosCur) / 2f;
		}
		else
		{
			if (!pollUniReleased)
			{
				pollUniReleased = true;
				if (pollUniWaitForDblEnd)
				{
					pollUniPosEnd = pollUniDblEndPos;
					pollUniWaitForDblEnd = false;
				}
				else
				{
					pollUniPosEnd = pollUniPosCur;
				}
				pollUniDeltaAccumAtEnd = pollUniDeltaAccum;
				pollUniDeltaAccum = Vector2.zero;
				if (pollUniInitialState)
				{
					pollUniReleasedInitial = true;
				}
			}
			pollUniTouched = false;
		}
		pollUniPosPrev = pollUniPosCur;
		if (finger2.touchId >= 0)
		{
			if (!pollMultiReleased)
			{
				pollMultiReleased = true;
				pollMultiPosEnd = pollMultiPosCur;
				if (pollMultiInitialState)
				{
					pollMultiReleasedInitial = true;
				}
			}
			pollMultiTouched = false;
		}
		if (nonExclusiveTouches)
		{
			return TouchController.EventResult.SHARED;
		}
		return TouchController.EventResult.HANDLED;
	}

	public override TouchController.EventResult OnTouchMove(int touchId, Vector2 pos)
	{
		Finger finger = ((fingerA.touchId == touchId) ? fingerA : ((fingerB.touchId != touchId) ? null : fingerB));
		if (finger == null)
		{
			return TouchController.EventResult.NOT_HANDLED;
		}
		Finger finger2 = ((finger != fingerA) ? fingerA : fingerB);
		finger.touchVerified = true;
		finger.pollPosCur = pos;
		if (finger2.touchId >= 0)
		{
			pollMultiPosCur = (fingerA.pollPosCur + fingerB.pollPosCur) / 2f;
			pollUniPosCur = pollMultiPosCur;
		}
		else
		{
			pollUniPosCur = pos;
		}
		if (pollUniPosCur != pollUniPosPrev)
		{
			pollUniWaitForDblEnd = false;
			pollUniWaitForDblStart = false;
			pollUniDeltaAccum += pollUniPosCur - pollUniPosPrev;
			pollUniPosPrev = pollUniPosCur;
		}
		if (nonExclusiveTouches)
		{
			return TouchController.EventResult.SHARED;
		}
		return TouchController.EventResult.HANDLED;
	}

	private Vector2 GetCenterPos()
	{
		return (fingerA.posCur + fingerB.posCur) * 0.5f;
	}

	private float GetFingerDist()
	{
		return Mathf.Max(2f, Vector2.Distance(fingerA.posCur, fingerB.posCur));
	}

	private float GetFingerAbsAngle(float lastAngle = 0f)
	{
		Vector2 vector = fingerB.posCur - fingerA.posCur;
		if (vector.sqrMagnitude < 1E-05f)
		{
			return lastAngle;
		}
		vector.Normalize();
		return Mathf.Atan2(vector.y, vector.x) * 57.29578f;
	}

	private Vector2 TransformPos(Vector2 screenPosPx, TouchCoordSys posType, bool deltaMode)
	{
		Vector2 vector = screenPosPx;
		if (!deltaMode && (posType == TouchCoordSys.LOCAL_CM || posType == TouchCoordSys.LOCAL_INCH || posType == TouchCoordSys.LOCAL_NORMALIZED || posType == TouchCoordSys.LOCAL_PX))
		{
			vector.x -= screenRectPx.xMin;
			vector.y -= screenRectPx.yMin;
		}
		switch (posType)
		{
		case TouchCoordSys.SCREEN_PX:
		case TouchCoordSys.LOCAL_PX:
			return vector;
		case TouchCoordSys.SCREEN_NORMALIZED:
			vector.x /= joy.GetScreenWidth();
			vector.y /= joy.GetScreenHeight();
			return vector;
		case TouchCoordSys.SCREEN_CM:
		case TouchCoordSys.LOCAL_CM:
			return vector / joy.GetDPCM();
		case TouchCoordSys.SCREEN_INCH:
		case TouchCoordSys.LOCAL_INCH:
			return vector / joy.GetDPI();
		case TouchCoordSys.LOCAL_NORMALIZED:
			vector.x /= screenRectPx.width;
			vector.y /= screenRectPx.height;
			return vector;
		default:
			return vector;
		}
	}

	private float TransformPos(float screenPosPx, TouchCoordSys posType)
	{
		switch (posType)
		{
		case TouchCoordSys.SCREEN_PX:
		case TouchCoordSys.LOCAL_PX:
			return screenPosPx;
		case TouchCoordSys.SCREEN_NORMALIZED:
			return screenPosPx / Mathf.Max(joy.GetScreenWidth(), joy.GetScreenHeight());
		case TouchCoordSys.SCREEN_CM:
		case TouchCoordSys.LOCAL_CM:
			return screenPosPx / joy.GetDPCM();
		case TouchCoordSys.SCREEN_INCH:
		case TouchCoordSys.LOCAL_INCH:
			return screenPosPx / joy.GetDPI();
		case TouchCoordSys.LOCAL_NORMALIZED:
			return screenPosPx / screenRectPx.width;
		default:
			return screenPosPx;
		}
	}

	private float TransformPosX(float screenPosPx, TouchCoordSys posType)
	{
		switch (posType)
		{
		case TouchCoordSys.SCREEN_PX:
		case TouchCoordSys.LOCAL_PX:
			return screenPosPx;
		case TouchCoordSys.SCREEN_NORMALIZED:
			return screenPosPx / joy.GetScreenWidth();
		case TouchCoordSys.SCREEN_CM:
		case TouchCoordSys.LOCAL_CM:
			return screenPosPx / joy.GetDPCM();
		case TouchCoordSys.SCREEN_INCH:
		case TouchCoordSys.LOCAL_INCH:
			return screenPosPx / joy.GetDPI();
		case TouchCoordSys.LOCAL_NORMALIZED:
			return screenPosPx / screenRectPx.width;
		default:
			return screenPosPx;
		}
	}

	private float TransformPosY(float screenPosPx, TouchCoordSys posType)
	{
		switch (posType)
		{
		case TouchCoordSys.SCREEN_PX:
		case TouchCoordSys.LOCAL_PX:
			return screenPosPx;
		case TouchCoordSys.SCREEN_NORMALIZED:
			return screenPosPx / joy.GetScreenHeight();
		case TouchCoordSys.SCREEN_CM:
		case TouchCoordSys.LOCAL_CM:
			return screenPosPx / joy.GetDPCM();
		case TouchCoordSys.SCREEN_INCH:
		case TouchCoordSys.LOCAL_INCH:
			return screenPosPx / joy.GetDPI();
		case TouchCoordSys.LOCAL_NORMALIZED:
			return screenPosPx / screenRectPx.height;
		default:
			return screenPosPx;
		}
	}

	public static bool PxPosEquals(Vector2 p0, Vector2 p1)
	{
		return (p0 - p1).sqrMagnitude < 0.1f;
	}

	public static bool PxDistEquals(float d0, float d1)
	{
		return Mathf.Abs(d0 - d1) < 0.1f;
	}

	public static bool TwistAngleEquals(float a0, float a1)
	{
		return Mathf.Abs(Mathf.DeltaAngle(a0, a1)) < 0.5f;
	}
}
