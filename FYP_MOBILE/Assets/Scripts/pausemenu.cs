using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
	public GameObject pausepanel;

	private bool IsPause;

	public void pause()
	{
		pausepanel.SetActive(value: true);
		Time.timeScale = 0f;
		IsPause = true;
	}

	public void resumeG()
	{
		pausepanel.SetActive(value: false);
		Time.timeScale = 1f;
		IsPause = false;
	}

	public void tryS()
	{
		pausepanel.SetActive(value: false);
		Camps.Alert = false;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1f;
	}

	public void level()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		SceneManager.LoadScene("MainMenu");
	}

	public void exit()
	{
		pausepanel.SetActive(value: false);
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1f;
	}

	private void Start()
	{
		
		IsPause = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (IsPause)
			{
				resumeG();
				IsPause = false;
			}
			else
			{
				pause();
				IsPause = true;
			}
		}
	}
}
