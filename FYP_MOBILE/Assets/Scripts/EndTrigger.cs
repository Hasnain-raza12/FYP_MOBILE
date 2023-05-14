
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    public GameObject completeLevelUI;
    
    public void OnTriggerEnter()
    {
        Debug.Log("LEVEL COMPLETED");
        completeLevelUI.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
