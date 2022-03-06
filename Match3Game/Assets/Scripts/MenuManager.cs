using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void LoadLevel(int level, int unlocked)
	{
		Save.SaveLevel(level, unlocked);
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
