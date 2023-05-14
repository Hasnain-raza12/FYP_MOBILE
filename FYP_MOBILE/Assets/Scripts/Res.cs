using UnityEngine;
using UnityEngine.SceneManagement;

public class Res : MonoBehaviour
{
	private void Start()
	{
		if (PlayerPrefs.GetString("Resu") == "400")
		{
			Screen.SetResolution(800, 450, fullscreen: false, 60);
			GetComponent<Camera>().aspect = 1.7777778f;
		}
		if (PlayerPrefs.GetString("Resu") == "500")
		{
			Screen.SetResolution(900, 506, fullscreen: false, 60);
			GetComponent<Camera>().aspect = 1.7777778f;
		}
		if (PlayerPrefs.GetString("Resu") == "700")
		{
			Screen.SetResolution(1280, 720, fullscreen: false, 60);
			GetComponent<Camera>().aspect = 1.7777778f;
		}
		if (PlayerPrefs.GetInt("Loaded") != 3)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			PlayerPrefs.SetInt("Loaded", 3);
		}
		else
		{
			PlayerPrefs.SetInt("Loaded", 7);
		}
	}
}
