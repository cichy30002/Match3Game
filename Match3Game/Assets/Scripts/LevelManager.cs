using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public Board board;
	public GameObject VegePrefab;
	public Transform VegetablesParent;

	public float swapingTime = 0.2f;
	public float fallingTime = 0.5f;

	private Vegetable[,] gameGrid;
	private Transform[,] vegeTransforms;
	private bool readyToMove = true;
	public void Fill()
	{
		gameGrid = new Vegetable[board.xSize, board.ySize];
		vegeTransforms = new Transform[board.xSize, board.ySize];
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if(board.LevelCellValue(j,i) == "default")
				{
					GameObject newInstance = Instantiate(VegePrefab, board.cordGrid[j, i], Quaternion.identity, VegetablesParent);
					gameGrid[j, i] = newInstance.GetComponent<Vegetable>();
					vegeTransforms[j,i] = newInstance.transform;
				}
				
			}
		}
	}
	public List<Vector2Int[]> Find3()
	{
		List<Vector2Int[]> result = new List<Vector2Int[]>();
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
								int count = 3;
								while(j+count<board.xSize && gameGrid[j + count, i] != null && gameGrid[j, i].Type == gameGrid[j + count, i].Type)
								{
									count++;
								}
								Vector2Int[] tmp = new Vector2Int[count];
								Vector2Int[] tmp2 = new Vector2Int[0];
								int count2 = 0;
								for (int x = 0; x < count; x++)
								{
									if (i + 2 < board.ySize)
									{
										if (gameGrid[j + x, i + 1] != null && gameGrid[j + x, i + 2] != null)
										{
											if (gameGrid[j, i].Type == gameGrid[j + x, i + 1].Type && gameGrid[j, i].Type == gameGrid[j + x, i + 2].Type)
											{
												Debug.Log("found!");
												count2 = 3;
												while (i + count2 < board.ySize && gameGrid[j + x, i + count2] != null && gameGrid[j + x, i + count2].Type == gameGrid[j + x, i + count2].Type)
												{
													count2++;
												}
												tmp2 = new Vector2Int[count2];
												for (int y = 0; y < count2; y++)
												{
													tmp2[y] = new Vector2Int(j + x, i + y);
												}
											}
										}
									}
									tmp[x] = new Vector2Int(j+x, i);
								}
								Vector2Int[] res = new Vector2Int[count + count2];
								for (int x = 0; x < count+count2; x++)
								{
									if(x<count)
									{
										res[x] = tmp[x];
									}
									else
									{
										res[x] = tmp2[x - count];
									}
								}
								if(count2!=0)
								{
									Debug.Log(count + " " + count2);
								}
								
								result.Add(tmp);
							}
						}
					}
					if(i + 2 < board.ySize)
					{
						if (gameGrid[j, i + 1] != null && gameGrid[j, i + 2] != null)
						{
							if (gameGrid[j, i].Type == gameGrid[j, i + 1].Type && gameGrid[j, i].Type == gameGrid[j, i + 2].Type)
							{
								Vector2Int[] tmp = new Vector2Int[3];
								tmp[0] = new Vector2Int(j, i);
								tmp[1] = new Vector2Int(j, i + 1);
								tmp[2] = new Vector2Int(j, i + 2);
								result.Add(tmp);
							}
						}
					}
				}
				
			}
		}
		return result;
	}
	public void FirstVegeCheck()
	{
		List<Vector2Int[]> triplets = Find3();
		if(triplets.Count == 0)
		{
			return;
		}
		foreach (Vector2Int[] triplet in triplets)
		{
			
			Vegetable.VegeType newVege = gameGrid[triplet[0].x, triplet[0].y].RandomVegeType();
			while(newVege == gameGrid[triplet[0].x, triplet[0].y].Type)
			{
				newVege = gameGrid[triplet[0].x, triplet[0].y].RandomVegeType();
			}
			gameGrid[triplet[0].x, triplet[0].y].SetVegeType(newVege);
		}
		FirstVegeCheck();
	}
	public void MakeAMove(Vector2Int a, Vector2Int b)
	{
		if(!readyToMove)
		{
			return;
		}
		readyToMove = false;
		if (a.x < 0 || a.x >= board.xSize || a.y < 0 || a.y >= board.ySize)
		{
			Debug.LogError("Invalid move!");
			return;
		}
		if (b.x < 0 || b.x >= board.xSize || b.y < 0 || b.y >= board.ySize)
		{
			Debug.LogError("Invalid move!");
			return;
		}
		Vegetable temp = gameGrid[a.x, a.y];
		gameGrid[a.x, a.y] = gameGrid[b.x, b.y];
		gameGrid[b.x, b.y] = temp;

		List<Vector2Int[]> triplets = Find3();
		if (triplets.Count == 0)
		{
			StartCoroutine(MoveLerp(a, b, true));
			temp = gameGrid[a.x, a.y];
			gameGrid[a.x, a.y] = gameGrid[b.x, b.y];
			gameGrid[b.x, b.y] = temp;
		}
		else
		{
			StartCoroutine(MoveLerp(a, b, false));
		}
	}
	IEnumerator MoveLerp(Vector2Int a, Vector2Int b, bool back)
	{
		Vector2 startPosA = vegeTransforms[a.x, a.y].position;
		Vector2 startPosB = vegeTransforms[b.x, b.y].position;
		float startTime = Time.time;
		while((Time.time - startTime) / swapingTime <= 1)
		{
			vegeTransforms[a.x, a.y].position = Vector2.Lerp(startPosA, startPosB, (Time.time - startTime) / swapingTime);
			vegeTransforms[b.x, b.y].position = Vector2.Lerp(startPosB, startPosA, (Time.time - startTime) / swapingTime);
			yield return null;
		}
		Transform tmp = vegeTransforms[a.x, a.y];
		vegeTransforms[a.x, a.y] = vegeTransforms[b.x, b.y];
		vegeTransforms[b.x, b.y] = tmp;
		if(back)
		{
			yield return new WaitForSeconds(0.05f);
			StartCoroutine(MoveLerp(a, b, false));
		}
		else
		{
			readyToMove = true;
			PerformTriplets(Find3());
		}
	}
	void PerformTriplets(List<Vector2Int[]> triplets)
	{
		if (triplets.Count == 0) return;
		foreach(Vector2Int[] triplet in triplets)
		{
			foreach(Vector2Int vege in triplet)
			{
				Destroy(vegeTransforms[vege.x, vege.y].gameObject);
			}
		}
		StartCoroutine(ReFill(triplets));
		
	}
	IEnumerator ReFill(List<Vector2Int[]> triplets)
	{
		int[,] refillInstructions = new int[board.xSize, board.ySize];
		int[,] refillCheck = new int[board.xSize, board.ySize];
		int[] newVegeInstructions = new int[board.xSize];
		foreach (Vector2Int[] triplet in triplets)
		{
			foreach (Vector2Int vege in triplet)
			{
				newVegeInstructions[vege.x] += 1;
				refillInstructions[vege.x, vege.y] = -1;
				//EMPTY CELLS!!!!
			}
		}
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if (!board.isCellEnabled(j, i))
				{
					refillInstructions[j, i] = -2;
				}
			}
		}
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if(refillInstructions[j,i] == -1)
				{
					for (int y = i+1; y <= board.ySize; y++)
					{
						if(y == board.ySize)
						{
							refillInstructions[j, i] = y - i;
							break;
						}
						if(refillInstructions[j, y] == 0)
						{
							refillInstructions[j, y] = -1;
							refillInstructions[j, i] = y-i;
							break;
						}
					}
					if (!board.isCellEnabled(j, i))
						refillInstructions[j, i] = 0;
				}
			}
		}
		/*
		string tmp = "";
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				tmp += refillInstructions[j, i];
			}
			tmp += "\n";
		}
		Debug.Log(tmp);
		*/
		int[] counts = new int[board.xSize];
		for (int i = 0; i < board.ySize; i++)
		{
			
			for (int j = 0; j < board.xSize; j++)
			{
				if(refillInstructions[j,i]>0 && board.isCellEnabled(j,i))
				{
					if(i+refillInstructions[j,i]<board.ySize )
					{
						gameGrid[j, i] = gameGrid[j, i + refillInstructions[j, i]];
						vegeTransforms[j, i] = vegeTransforms[j, i + refillInstructions[j, i]];
					}
					else
					{
						Vector2 newPos = new Vector2(board.cordGrid[j, board.ySize - 1].x, board.cordGrid[j, board.ySize - 1].y + (counts[j] + 1) * board.CellSize);
						GameObject newInstance = Instantiate(VegePrefab, newPos, Quaternion.identity, VegetablesParent);
						gameGrid[j, i] = newInstance.GetComponent<Vegetable>();
						vegeTransforms[j, i] = newInstance.transform;
						counts[j]++;
					}
				}
				
					
			}
		}
		yield return StartCoroutine(FillLerp(refillInstructions));
		yield return new WaitForSeconds(0.1f);
		PerformTriplets(Find3());
	}
	IEnumerator FillLerp(int[,] refillInstr)
	{
		float startTime = Time.time;
		Vector2[,] startingPositions = new Vector2[board.xSize, board.ySize];
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if(refillInstr[j,i] > 0)
				{
					startingPositions[j, i] = vegeTransforms[j, i].position;
				}
			}
		}

		while ((Time.time - startTime) / fallingTime <= 1)
		{
			for (int i = 0; i < board.ySize; i++)
			{
				for (int j = 0; j < board.xSize; j++)
				{
					if(refillInstr[j,i] > 0)
						vegeTransforms[j,i].position = Vector2.Lerp(startingPositions[j,i], board.cordGrid[j,i], (Time.time - startTime) / fallingTime);
						
				}
			}
			yield return null;
		}

	}
}		
