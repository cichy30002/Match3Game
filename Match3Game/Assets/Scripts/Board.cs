using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	public LevelManager levelManager;
	public TextAsset LevelLayoutTxt;
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
		{ 0, "disabled" }
	};

	private int[,] levelGrid;
	private Cell[,] cellArr;

	// Start is called before the first frame update
	void Start()
	{
		ProcessTextLayout(LevelLayoutTxt.text);
		cordGrid = new Vector2[xSize, ySize];
		CalculateCordGrid();
		DrawLevelLayout();
		levelManager.Fill();
		Invoke("FirstVegeCheck", 0.1f);
		
	}
	void FirstVegeCheck()
	{
		List<LevelManager.Triplet> triplets =  levelManager.Find3();

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
		Debug.Log("Level size: x-" + xSize + " y-" + ySize);
		for (int i = 0; i < ySize; i++)
		{
			for (int j = 0; j < xSize; j++)
			{
				levelGrid[j, i] = TxtNotationDict[levelLayout[i * (xSize + 2) + j]];

			}
		}
		/*
		for (int i = 0; i < ySize; i++)
		{
			string tmp = "";
			for (int j = 0; j < xSize; j++)
			{
				tmp += levelGrid[j, i];
			}
			Debug.Log(tmp);
		}
		*/

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
		/*
		for (int i = 0; i < ySize; i++)
		{
			string tmp = "";
			for (int j = 0; j < xSize; j++)
			{
				tmp += cordGrid[j, i];
			}
			Debug.Log(tmp);
		}
		*/
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
}
