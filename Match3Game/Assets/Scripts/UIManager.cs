using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
	public TrueLevelManager trueLevelManager;
	public TMP_Text LevelText;
	public TMP_Text PointsText;
	public RectTransform PointsRT;
	public GameObject TasksParent;
	public GameObject TaskPrefab;
	public TMP_Text MovesText;
	public GameObject WinPanel;
	public TMP_Text WinPointsText;
	public GameObject LosePanel;

	public void UpdatePoints(int points)
	{
		PointsText.text = points.ToString();
		StartCoroutine(BumpText(1f, 1.7f));
	}
	IEnumerator BumpText(float time, float scale)
	{
		PointsRT.localScale *= scale;
		float timeStart = Time.time;
		while(timeStart + time > Time.time)
		{
			PointsRT.localScale = Vector3.one + Vector3.one * scale * (((Time.time - timeStart) * -1/time) + 1);
			yield return null;
		}
	}
	public void InitTasks(Dictionary<Vegetable.VegeType, int[]> Mission)
	{
		foreach(var task in Mission)
		{
			GameObject newTask = Instantiate(TaskPrefab, TasksParent.transform);
			Image img = newTask.GetComponentInChildren<Image>();
			TMP_Text tmp = newTask.GetComponentInChildren<TMP_Text>();
			img.sprite = VegeSprites.FindVegeSprite(task.Key);
			tmp.text = task.Value[0] + " / " + task.Value[1];
		}
	}
	public void UpdateTask(Dictionary<Vegetable.VegeType, int[]> Mission)
	{
		foreach (var task in Mission)
		{
			foreach (Transform child in TasksParent.transform)
			{
				if (child.GetComponentInChildren<Image>().sprite == VegeSprites.FindVegeSprite(task.Key))
				{
					TMP_Text tmp = child.GetComponentInChildren<TMP_Text>();
					tmp.text = task.Value[0] + " / " + task.Value[1];
				}
			}
		}
	}
	public void InitMoves(int numberOfMoves)
	{
		MovesText.text = numberOfMoves.ToString();
	}
	public void UpdateMoves(int movesDone, int numberOfMoves)
	{
		MovesText.text = (numberOfMoves - movesDone).ToString();
	}
	public void EndGame()
	{
		foreach(Transform trans in TasksParent.transform)
		{
			Destroy(trans.gameObject);
		}
	}
	public void WinGame(int points)
	{
		WinPanel.SetActive(true);
		WinPointsText.text = points.ToString() + " points";
		if (Resources.Load<TextAsset>("Levels/level" + (trueLevelManager.currentLevelNumber+1).ToString()) == null)
		{
			WinPanel.transform.Find("Layout").Find("NextLevel").gameObject.SetActive(false);
		}
		else
		{
			WinPanel.transform.Find("Layout").Find("NextLevel").gameObject.SetActive(true);
		}
	}
	public void LoseGame()
	{
		LosePanel.SetActive(true);
	}
	public void SetLevelText(int level)
	{
		LevelText.text = "Level " + level.ToString();
	}
}