using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Texture))]
public class ForcedReset : MonoBehaviour
{
	private void Update()
	{
		if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
		{
			SceneManager.LoadScene(SceneManager.GetSceneAt(0).path);
		}
	}
}
