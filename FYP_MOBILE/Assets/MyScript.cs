using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
public class MyScript : MonoBehaviour
{
    public FixedJoystick MoveJoystick;
    public FixedButton JumpButton;
    public FixedButton MiraButton;
    public FixedTouchField TouchField;
    public FixedButton TrocarGunsButton;

    void Update()
    {
        var fps = GetComponent<RigidbodyFirstPersonController>();
        fps.RunAxis = MoveJoystick.Direction;
        fps.JumpAxis = JumpButton.Pressed;
        fps.MiraAxis = MiraButton.Pressed;
        fps.TrocarAxis = TrocarGunsButton.Pressed;
        fps.mouseLook.LookAxis = TouchField.TouchDist;
    }
}
