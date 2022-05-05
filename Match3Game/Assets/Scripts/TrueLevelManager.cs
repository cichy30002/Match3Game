using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrueLevelManager : MonoBehaviour
{
	public Board board;
	public int currentLevelNumber = 1;
	public int unlockedLevelNumber = 1;

	private void Start()
	{
		currentLevelNumber = Save.LoadLevel().levelNumber;
		unlockedLevelNumber = Save.LoadLevel().unlockedLevel;
		LoadLevel();
	}

	public void LoadLevel()
	{
		board.LevelLayoutTxt = Resources.Load<TextAsset>("Levels/level" + currentLevelNumber.ToString());
		board.MissionTxt = Resources.Load<TextAsset>("Levels/mission" + currentLevelNumber.ToString());
		board.StartGame();
		board.levelManager.UIManager.SetLevelText(currentLevelNumber);
	}
	public void NextLevel()
	{
		currentLevelNumber++;
		unlockedLevelNumber = Mathf.Max(currentLevelNumber, unlockedLevelNumber);
		Save.SaveLevel(currentLevelNumber, unlockedLevelNumber);
		LoadLevel();
	}
	public void UnlockLevel()
	{
		unlockedLevelNumber++;
		Save.SaveLevel(currentLevelNumber, unlockedLevelNumber);
	}
	public void LoadMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
	public void LoadLevelSelection()
	{
		SceneManager.LoadScene("LevelSelection");
	}
}
