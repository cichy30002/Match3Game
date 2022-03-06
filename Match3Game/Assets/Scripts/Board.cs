using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Board : MonoBehaviour
{
	public LevelManager levelManager;
	public TextAsset LevelLayoutTxt;
	public TextAsset MissionTxt;
	public GameObject CellPrefab;
	public float CellSize = 1f;
	[HideInInspector]
	public int xSize;
	[HideInInspector]
	public int ySize;
	[HideInInspector]
	public Vector2[,] cordGrid;
	public Dictionary<char, int> TxtNotationDict = new Dictionary<char, int>() {
		{ 'x', 1 },
		{ 'o', 0 }
	};
	public Dictionary<int, string> GridNotationDict = new Dictionary<int, string>() {
		{ 1, "default" },
		{ 0, "disabled" },
		{ 2, "selected" }
	};
	public Dictionary<Vegetable.VegeType, int[]> Mission;
	public int[] Moves;

	private int[,] levelGrid;
	private Cell[,] cellArr;

	// Start is called before the first frame update
	public void StartGame()
	{
		levelManager.StartGame();
		ProcessTextLayout(LevelLayoutTxt.text);
		ProcessTextMission(MissionTxt.text);
		levelManager.UIManager.InitTasks(Mission);
		cordGrid = new Vector2[xSize, ySize];
		CalculateCordGrid();
		DrawLevelLayout();
		levelManager.Fill();
		Invoke("FirstVegeCheck", 0.1f);
		
	}
	void FirstVegeCheck()
	{
		levelManager.FirstVegeCheck();
		Invoke("AnyMoveCheck", 0.7f);
	}
	void AnyMoveCheck()
	{
		if (!levelManager.AnyMovePossible())
		{
			levelManager.ClearBoard();
			Invoke("AnyMoveCheck", 0.7f);
		}
	}
	void ProcessTextLayout(string levelLayout)
	{

		for (int i = 0; i < levelLayout.Length; i++)
		{
			if (levelLayout[i] == '\n')
			{

				xSize = i - 1;
				break;
			}
		}
		for (int i = 0; i < levelLayout.Length; i++)
		{
			if (levelLayout[i] == '\n')
			{
				if (i % (xSize + 2) != xSize + 1)
				{
					Debug.LogError("not equal lengths of level layout");
				}
			}
		}
		ySize = 0;
		for (int i = 0; i < levelLayout.Length; i += xSize + 2)
		{
			ySize++;
		}
		levelGrid = new int[xSize, ySize];
		for (int i = 0; i < ySize; i++)
		{
			for (int j = 0; j < xSize; j++)
			{
				levelGrid[j, i] = TxtNotationDict[levelLayout[i * (xSize + 2) + j]];

			}
		}
		

	}
	void CalculateCordGrid()
	{
		float x, y;
		x = (xSize - 1) / 2 * (-1) * CellSize;
		y = (ySize - 1) / 2 * (-1) * CellSize;
		for (int i = 0; i < ySize; i++)
		{
			x = (xSize - 1) / 2 * (-1) * CellSize;
			for (int j = 0; j < xSize; j++)
			{
				cordGrid[j, i] = new Vector2(x, y);
				x += CellSize;
			}
			y += CellSize;
		}
		
	}
	void DrawLevelLayout()
	{
		cellArr = new Cell[xSize, ySize];
		for (int i = 0; i < ySize; i++)
		{
			for (int j = 0; j < xSize; j++)
			{
				GameObject tmp = Instantiate(CellPrefab, cordGrid[j, i], Quaternion.identity, transform);
				cellArr[j, i] = tmp.GetComponent<Cell>();
				cellArr[j, i].SetSprite(GridNotationDict[levelGrid[j, i]]);
			}
		}
	}
	public string LevelCellValue(int x, int y)
	{
		if (x >= xSize || y >= ySize)
		{
			Debug.LogError("Asked for value outside of array!");
			return "error!";
		}
		return GridNotationDict[levelGrid[x, y]];
	}
	public Vector2Int FindNearestCell(Vector2 point)
	{
		Vector2Int result = new Vector2Int(-1,-1);
		float minDist = CellSize;
		for (int i = 0; i < ySize; i++)
		{
			for (int j = 0; j < xSize; j++)
			{
				if(Vector2.Distance(point, cordGrid[j,i]) < minDist)
				{
					result = new Vector2Int(j, i);
					minDist = Vector2.Distance(point, cordGrid[j, i]);
				}
			}
		}
		return result;
	}
	public void SelectCell(int x, int y,bool value = true)
	{
		if(x<0 || x>=xSize || y<0 || y>=ySize)
		{
			//Debug.LogError("You want to select cell that does not exist!");
			return;
		}
		if(levelGrid[x,y]==0)
		{
			return;
		}
		if(value)
		{
			levelGrid[x, y] = 2;
			cellArr[x, y].SetSprite("selected");
		}
		else
		{
			levelGrid[x, y] = 1;
			cellArr[x, y].SetSprite("default");
		}
		
	}
	public bool isCellEnabled(int x,int y)
	{
		if (levelGrid[x, y] != 0) return true;
		return false;
	}
	private void ProcessTextMission (string text)
	{
		Mission = new Dictionary<Vegetable.VegeType, int[]>();
		string [] pairs = text.Split('\n');
		string moves = pairs[0];
		pairs = pairs.Skip(1).ToArray();
		string[] movesTab = moves.Split(' ');
		if(movesTab[0] != "Moves")
		{
			Debug.LogError("wrong Mission format - moves");
		}
		else
		{
			Moves = new int[2];
			Moves[0] = 0;
			Moves[1] = Int32.Parse( movesTab[1]);
		}
		levelManager.UIManager.InitMoves(Moves[1]);
		foreach (string pair in pairs)
		{
			string[] tmp = pair.Split(' ');
			Mission[(Vegetable.VegeType) Enum.Parse(typeof( Vegetable.VegeType), tmp[0])] = new int[] { 0, Int32.Parse(tmp[1])};
		}
	}
	public void ProgressMission(Vegetable.VegeType type)
	{
		if(Mission[type][1] > Mission[type][0])
		{
			Mission[type][0]++;
		}
		
	}
	public bool MissionComplete()
	{
		foreach(KeyValuePair<Vegetable.VegeType, int[]> task in Mission)
		{
			if(task.Value[0]<task.Value[1])
			{
				return false;
			}
		}
		return true;
	}
	public void ProgressMoves()
	{
		Moves[0]++;
		if(Moves[0] == Moves[1] && !MissionComplete())
		{
			StartCoroutine( levelManager.LoseGame());
		}
		levelManager.UIManager.UpdateMoves(Moves[0], Moves[1]);
	}
	public void EndGame()
	{
		foreach(Cell cell in cellArr)
		{
			Destroy(cell.gameObject);
		}
		levelManager.EndGame();
	}
}
