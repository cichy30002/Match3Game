using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void LoadLevel(int level)
	{
		Save.SaveLevel(level);
		SceneManager.LoadScene("Game");
	}
	public void LoadMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
	public void LoadLevelSelection()
	{
		SceneManager.LoadScene("LevelSelection");
	}
	public void LoadCredits()
	{
		Debug.Log("loadig credits");
	}
	public void Exit()
	{
		Application.Quit();
	}
}
