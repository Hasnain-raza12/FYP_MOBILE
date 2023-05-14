using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Gunbob : MonoBehaviour
{
	public GameObject GunPos;

	private void Update()
	{
		if (CrossPlatformInputManager.GetAxis("Horizontal") != 0f && !aim.applymode)
		{
			GunPos.GetComponent<Animation>().Play("MoveOn");
		}
	}
}
