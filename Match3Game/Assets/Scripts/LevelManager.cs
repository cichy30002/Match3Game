using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public Board board;
	public GameObject VegePrefab;
	public Transform VegetablesParent;
	private Vegetable[,] gameGrid;

	public class Triplet
	{
		public Triplet(Direct dir, int x, int y)
		{
			Dir = dir;
			X = x;
			Y = y;
		}
		public enum Direct
		{
			up,
			right
		}
		public Direct Dir;
		public int X;
		public int Y;
	}
	public void Fill()
	{
		gameGrid = new Vegetable[board.xSize, board.ySize];
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if(board.LevelCellValue(j,i) == "default")
				{
					gameGrid[j, i] = Instantiate(VegePrefab, board.cordGrid[j, i], Quaternion.identity, VegetablesParent).GetComponent<Vegetable>();
				}
				
			}
		}
	}
	public List<Triplet> Find3()
	{
		List<Triplet> result = new List<Triplet>();
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if(gameGrid[j, i] != null)
				{
					if(j + 2 < board.xSize)
					{
						if (gameGrid[j + 1, i] != null && gameGrid[j + 2, i] != null)
						{
							if (gameGrid[j, i].Type == gameGrid[j + 1, i].Type && gameGrid[j, i].Type == gameGrid[j + 2, i].Type)
							{
								//Debug.Log("tripe: " + j + " " + i + " " + gameGrid[j, i].Type);
								result.Add(new Triplet(Triplet.Direct.right, j, i));
							}
						}
					}
					if(i + 2 < board.ySize)
					{
						if (gameGrid[j, i + 1] != null && gameGrid[j, i + 2] != null)
						{
							if (gameGrid[j, i].Type == gameGrid[j, i + 1].Type && gameGrid[j, i].Type == gameGrid[j, i + 2].Type)
							{
								//Debug.Log("tripe: " + j + " " + i + " " + gameGrid[j, i].Type);
								result.Add(new Triplet(Triplet.Direct.up, j, i));
							}
						}
					}
				}
				
			}
		}
		return result;
	}

}
