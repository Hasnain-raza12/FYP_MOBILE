using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private void Start()
	{
		//PlayerPrefs.SetInt("cinema", 0);
		//Cursor.visible = false;
	}
    //public void CompleteLevel()
    //{
    //    Debug.Log("LEVEL COMPLETE");
    //}
    /*private void OnTriggerEnter()
	{
		if (SceneManager.GetActiveScene().name == "Level1")
		{
			PlayerPrefs.SetInt("cinema", 1);
			PlayerPrefs.SetInt("Levels", 1);
		}
		if (SceneManager.GetActiveScene().name == "Level2" && PlayerPrefs.GetInt("c4") >= 2)
		{
			PlayerPrefs.SetInt("cinema", 2);
			PlayerPrefs.SetInt("Levels", 2);
		}
		if (SceneManager.GetActiveScene().name == "Level3")
		{
			PlayerPrefs.SetInt("Levels", 3);
			PlayerPrefs.SetInt("cinema", 3);
		}
		if (SceneManager.GetActiveScene().name == "Level4" && PlayerPrefs.GetInt("PcWork") == 1)
		{
			PlayerPrefs.SetInt("cinema", 2);
			PlayerPrefs.SetInt("Levels", 3);
		}
	
	}*/

}
