using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{
	public GameObject[] MainMenu;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	private void Update()
	{
	}

	public void MenuCall(int Menu)
	{
		for (int i = 0; i < MainMenu.Length; i++)
		{
			MainMenu[i].SetActive(value: false);
		}
		MainMenu[Menu].SetActive(value: true);
	}

	public void Leaving()
	{
		Application.Quit();
	}

	public void LoadLevel(string Level)
	{
		MenuCall(2);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		SceneManager.LoadScene(Level);
	}

	public void SetSettings(int SetQua)
	{
		QualitySettings.SetQualityLevel(SetQua, applyExpensiveChanges: true);
	}
}
