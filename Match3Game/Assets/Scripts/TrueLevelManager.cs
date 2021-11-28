using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrueLevelManager : MonoBehaviour
{
	public Board board;
	public int levelNumber = 1;

	private void Start()
	{
		levelNumber = Save.LoadLevel().levelNumber;
		LoadLevel();
	}

	public void LoadLevel()
	{
		board.LevelLayoutTxt = Resources.Load<TextAsset>("Levels/level" + levelNumber.ToString());
		board.MissionTxt = Resources.Load<TextAsset>("Levels/mission" + levelNumber.ToString());
		board.StartGame();
		board.levelManager.UIManager.SetLevelText(levelNumber);
	}
	public void NextLevel()
	{
		levelNumber++;
		Save.SaveLevel(levelNumber);
		LoadLevel();
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
