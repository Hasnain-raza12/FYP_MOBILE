using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public void PlayeGame(string level)
	{
		SceneManager.LoadScene(level);
		Time.timeScale = 1f;
	}

	
}
