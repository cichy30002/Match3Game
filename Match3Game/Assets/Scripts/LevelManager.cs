using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public Board board;
	public TrueLevelManager trueLevelManager;
	public GameObject VegePrefab;
	public Transform VegetablesParent;
	public UIManager UIManager;
	public ParticleSystem ParticleMatch3;
	public ParticleSystem ParticleMatch4;
	public ParticleSystem ParticleMatch5;

	public float swapingTime = 0.2f;
	public float fallingTime = 0.5f;

	private Vegetable[,] gameGrid;
	private Transform[,] vegeTransforms;
	private bool readyToMove = true;

	private int points;
	[HideInInspector]public bool isLevelActive = true;
	private void Start()
	{
		isLevelActive = true;
		points = 0;
		UIManager.UpdatePoints(points);
	}
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
		Vegetable.VegeType [,] allToExterminate = new Vegetable.VegeType[board.xSize, board.ySize];
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if(gameGrid[j, i] != null)
				{
					allToExterminate[j, i] = gameGrid[j, i].Type;
				}
				else
				{
					allToExterminate[j, i] = Vegetable.VegeType.none;
				}
				
			}
		}
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if(gameGrid[j, i] != null)
				{
					List<Vector2Int> dynamicBoi = new List<Vector2Int>();
					if (j + 2 < board.xSize && allToExterminate[j + 1, i] == gameGrid[j, i].Type && allToExterminate[j + 2, i] == gameGrid[j, i].Type)
					{
						dynamicBoi.Add(new Vector2Int(j, i));
						dynamicBoi.Add(new Vector2Int(j + 1, i));
						dynamicBoi.Add(new Vector2Int(j + 2, i));
						if (j + 3 < board.xSize && allToExterminate[j + 3, i] == gameGrid[j, i].Type)
						{
							dynamicBoi.Add(new Vector2Int(j + 3, i));

							if (j + 4 < board.xSize && allToExterminate[j + 4, i] == gameGrid[j, i].Type)
							{
								dynamicBoi.Add(new Vector2Int(j + 4, i));
							}
						}
						int tmp = dynamicBoi.Count;
						for (int x = 0; x < tmp; x++)
						{
							if (i + 2 < board.ySize && allToExterminate[j + x, i + 1] == gameGrid[j, i].Type && allToExterminate[j + x, i + 2] == gameGrid[j, i].Type)
							{
								dynamicBoi.Add(new Vector2Int(j + x, i + 1));
								dynamicBoi.Add(new Vector2Int(j + x, i + 2));
								if (i - 1 > 0 && allToExterminate[j + x, i - 1] == gameGrid[j, i].Type)
								{
									dynamicBoi.Add(new Vector2Int(j + x, i - 1));
								}
								if (i - 2 > 0 && allToExterminate[j + x, i - 2] == gameGrid[j, i].Type)
								{
									dynamicBoi.Add(new Vector2Int(j + x, i - 2));
								}
							}
							else if (i + 1 < board.ySize && i - 1 > 0 && allToExterminate[j + x, i + 1] == gameGrid[j, i].Type && allToExterminate[j + x, i - 1] == gameGrid[j, i].Type)
							{
								dynamicBoi.Add(new Vector2Int(j + x, i + 1));
								dynamicBoi.Add(new Vector2Int(j + x, i - 1));
								if (i - 2 > 0 && allToExterminate[j + x, i - 2] == gameGrid[j, i].Type)
								{
									dynamicBoi.Add(new Vector2Int(j + x, i - 2));
								}
							}
							else if (i - 2 > 0 && allToExterminate[j + x, i - 1] == gameGrid[j, i].Type && allToExterminate[j + x, i - 2] == gameGrid[j, i].Type)
							{
								dynamicBoi.Add(new Vector2Int(j + x, i - 1));
								dynamicBoi.Add(new Vector2Int(j + x, i - 2));
							}
						}
						Vector2Int[] newRes = new Vector2Int[dynamicBoi.Count];
						for (int x = 0; x < dynamicBoi.Count; x++)
						{
							allToExterminate[dynamicBoi[x].x, dynamicBoi[x].y] = 0;
							newRes[x] = dynamicBoi[x];
						}
						result.Add(newRes);
					}
					else if (i + 2 < board.ySize && allToExterminate[j, i + 1] == gameGrid[j, i].Type && allToExterminate[j , i + 2] == gameGrid[j, i].Type)
					{
						dynamicBoi.Add(new Vector2Int(j, i));
						dynamicBoi.Add(new Vector2Int(j, i + 1));
						dynamicBoi.Add(new Vector2Int(j, i + 2));
						if (i + 3 < board.ySize && allToExterminate[j, i + 3] == gameGrid[j, i].Type)
						{
							dynamicBoi.Add(new Vector2Int(j, i + 3));

							if (i + 4 < board.ySize && allToExterminate[j, i + 4] == gameGrid[j, i].Type)
							{
								dynamicBoi.Add(new Vector2Int(j, i + 4));
							}
						}
						int tmp = dynamicBoi.Count;
						for (int x = 0; x < tmp; x++)
						{
							if (j + 2 < board.xSize && allToExterminate[j + 1, i + x] == gameGrid[j, i].Type && allToExterminate[j + 2, i + x] == gameGrid[j, i].Type)
							{
								dynamicBoi.Add(new Vector2Int(j + 1, i + x));
								dynamicBoi.Add(new Vector2Int(j + 2, i + x));
								if (j - 1 > 0 && allToExterminate[j - 1, i + x] == gameGrid[j, i].Type)
								{
									dynamicBoi.Add(new Vector2Int(j - 1, i + x));
								}
								if (j - 2 > 0 && allToExterminate[j - 2, i + x] == gameGrid[j, i].Type)
								{
									dynamicBoi.Add(new Vector2Int(j - 2, i + x));
								}
							}
							else if (j + 1 < board.xSize && j - 1 > 0 && allToExterminate[j + 1, i + x] == gameGrid[j, i].Type && allToExterminate[j - 1, i + x] == gameGrid[j, i].Type)
							{
								dynamicBoi.Add(new Vector2Int(j + 1, i + x));
								dynamicBoi.Add(new Vector2Int(j - 1, i + x));
								if (j - 2 > 0 && allToExterminate[j - 2, i + x] == gameGrid[j, i].Type)
								{
									dynamicBoi.Add(new Vector2Int(j - 2, i + x));
								}
							}
							else if (j - 2 > 0 && allToExterminate[j - 1, i + x] == gameGrid[j, i].Type && allToExterminate[j - 2, i + x] == gameGrid[j, i].Type)
							{
								dynamicBoi.Add(new Vector2Int(j - 1, i + x));
								dynamicBoi.Add(new Vector2Int(j - 2, i + x));
							}
						}
						Vector2Int[] newRes = new Vector2Int[dynamicBoi.Count];
						for (int x = 0; x < dynamicBoi.Count; x++)
						{
							allToExterminate[dynamicBoi[x].x, dynamicBoi[x].y] = 0;
							newRes[x] = dynamicBoi[x];
						}
						
						result.Add(newRes);
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
		
		if (a.x < 0 || a.x >= board.xSize || a.y < 0 || a.y >= board.ySize)
		{
			return;
		}
		if (b.x < 0 || b.x >= board.xSize || b.y < 0 || b.y >= board.ySize)
		{
			return;
		}
		if (board.LevelCellValue(a.x, a.y) == "disabled")
		{
			return;
		}
		if (board.LevelCellValue(b.x, b.y) == "disabled")
		{
			return;
		}
		readyToMove = false;
		Vegetable temp = gameGrid[a.x, a.y];
		gameGrid[a.x, a.y] = gameGrid[b.x, b.y];
		gameGrid[b.x, b.y] = temp;

		List<Vector2Int[]> triplets = Find3();
		if (triplets.Count == 0)
		{
			StartCoroutine(MoveLerp(a, b, 0));
			temp = gameGrid[a.x, a.y];
			gameGrid[a.x, a.y] = gameGrid[b.x, b.y];
			gameGrid[b.x, b.y] = temp;
		}
		else
		{
			StartCoroutine(MoveLerp(a, b, 1));
		}
	}
	IEnumerator MoveLerp(Vector2Int a, Vector2Int b, int back)
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
		if(back == 0)
		{
			yield return new WaitForSeconds(0.05f);
			StartCoroutine(MoveLerp(a, b, 2));
		}
		else if(back == 1)
		{
			board.ProgressMoves();
			PerformTriplets();
		}
		else
		{
			PerformTriplets();
		}
		
		
	}
	void DrawLine(Vector2Int[] triplet)
	{
		int minx = 10000, miny = 100000, maxx = 0, maxy = 0;

		foreach (Vector2Int vege in triplet)
		{
			if (vege.x < minx) minx = vege.x;
			if (vege.y < miny) miny = vege.y;
			if (vege.x > maxx) maxx = vege.x;
			if (vege.y > maxy) maxy = vege.y;
		}
		
	}
	void PerformTriplets()
	{
		List<Vector2Int[]> triplets = Find3();
		if (triplets.Count == 0)
		{
			if(AnyMovePossible())
			{
				readyToMove = true;
			}
			else
			{
				ClearBoard();
				Invoke("PerformTriplets",0.7f);
			}
			return;
		}
		
		foreach (Vector2Int[] triplet in triplets)
		{
			DrawLine(triplet);
			if (triplet.Length>3)
			{
				Debug.Log(debug(triplet) + " " +  triplet.Length.ToString());
			}
			AddPoints(triplet);
			foreach (Vector2Int vege in triplet)
			{
				DestroyVege(vegeTransforms[vege.x, vege.y].gameObject, vege, triplet.Length);
			}
		}
		StartCoroutine(ReFill(triplets));
		
	}
	IEnumerator ReFill(List<Vector2Int[]> triplets)
	{
		int[,] refillInstructions = new int[board.xSize, board.ySize];
		int[,] refillCheck = new int[board.xSize, board.ySize];
		foreach (Vector2Int[] triplet in triplets)
		{
			foreach (Vector2Int vege in triplet)
			{
				refillInstructions[vege.x, vege.y] = -1;
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
		PerformTriplets();
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
	private string debug(Vector2Int[] tab)
	{
		string ret = "";
		foreach(Vector2Int i in tab)
		{
			ret += " (" + i.x + "," + i.y + ") ";
		}
		return ret;
	}
	private void AddPoints(Vector2Int[] triplet)
	{
		foreach(Vector2Int vege in triplet)
		{
			if (board.Mission.ContainsKey(gameGrid[vege.x, vege.y].Type))
			{
				board.ProgressMission(gameGrid[vege.x, vege.y].Type);
				UIManager.UpdateTask(board.Mission);
			}
			if (board.MissionComplete())
			{
				StartCoroutine( WinGame());
			}
		}
		switch(triplet.Length)
		{
			case 3:
				//Debug.Log("100 points!");
				ChangePoints(100);
				break;
			case 4:
				//Debug.Log("300 points!");
				ChangePoints(300);
				break;
			case 5:
				//Debug.Log("1000 points! wow");
				ChangePoints(1000);
				break;
			default:
				//Debug.Log("a lot of points");
				ChangePoints(1);
				break;
		}
	}
	private void ChangePoints(int x)
	{
		points += x;
		UIManager.UpdatePoints(points);
	}
	private void DestroyVege(GameObject vegeGO, Vector2Int vege, int len)
	{
		switch (len)
		{
			case 3:
				Instantiate(ParticleMatch3, vegeGO.transform.position, vegeGO.transform.rotation);
				break;
			case 4:
				Instantiate(ParticleMatch4, vegeGO.transform.position, vegeGO.transform.rotation);
				break;
			case 5:
				Instantiate(ParticleMatch5, vegeGO.transform.position, vegeGO.transform.rotation);
				break;
			default:
				Instantiate(ParticleMatch3, vegeGO.transform.position, vegeGO.transform.rotation);
				break;
		}
		
		Destroy(vegeTransforms[vege.x, vege.y].gameObject);
	}
	public void EndGame()
	{
		isLevelActive = false;
		foreach(Vegetable vege in gameGrid)
		{
			if(vege != null)
			{
				Destroy(vege.gameObject);
			}
		}
		UIManager.EndGame();
		
	}
	public void StartGame()
	{
		points = 0;
		UIManager.UpdatePoints(points);
		StartCoroutine(startingWait());
	}
	IEnumerator startingWait()
	{
		yield return new WaitForSeconds(0.2f);
		isLevelActive = true;
	}
	public IEnumerator WinGame()
	{
		while(!readyToMove)
		{
			yield return null;
		}
		UIManager.WinGame(points);
		trueLevelManager.UnlockLevel();
		board.EndGame();
	}
	public IEnumerator LoseGame()
	{
		while (!readyToMove)
		{
			yield return null;
		}
		if(board.MissionComplete())
		{
			yield break;
		}
		UIManager.LoseGame();
		board.EndGame();
	}
	public void ClearBoard()
	{
		foreach(Transform vegeTransform in vegeTransforms)
		{
			if(vegeTransform != null)
			{
				Destroy(vegeTransform.gameObject);
			}
		}
		StartCoroutine(FillWholeBoard());
		Invoke("FirstVegeCheck", 0.5f);
	}
	public IEnumerator FillWholeBoard()
	{
		yield return new WaitForSeconds(0.1f);
		int[,] refillInstructions = new int[board.xSize, board.ySize];
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if (!board.isCellEnabled(j, i))
				{
					refillInstructions[j, i] = -2;
				}
				else
				{
					refillInstructions[j, i] = -1;
				}
			}
		}
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if (refillInstructions[j, i] == -1)
				{
					for (int y = i + 1; y <= board.ySize; y++)
					{
						if (y == board.ySize)
						{
							refillInstructions[j, i] = y - i;
							break;
						}
						if (refillInstructions[j, y] == 0)
						{
							refillInstructions[j, y] = -1;
							refillInstructions[j, i] = y - i;
							break;
						}
					}
					if (!board.isCellEnabled(j, i))
						refillInstructions[j, i] = 0;
				}
			}
		}
		int[] counts = new int[board.xSize];
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if (refillInstructions[j, i] > 0 && board.isCellEnabled(j, i))
				{
					if (i + refillInstructions[j, i] < board.ySize)
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
	}
	public bool AnyMovePossible()
	{
		Vegetable[,] tmpGameBoard = (Vegetable [,]) gameGrid.Clone();
		for (int i = 0; i < board.ySize; i++)
		{
			for (int j = 0; j < board.xSize; j++)
			{
				if(i>0 && board.isCellEnabled(j,i-1))
				{
					Vegetable tmp = tmpGameBoard[j, i];
					tmpGameBoard[j, i] = tmpGameBoard[j, i - 1];
					tmpGameBoard[j, i - 1] = tmp;
					if (anyMoveFromPosition(tmpGameBoard, j, i))
					{
						return true;
					}
					tmp = tmpGameBoard[j, i];
					tmpGameBoard[j, i] = tmpGameBoard[j, i - 1];
					tmpGameBoard[j, i - 1] = tmp;
				}
				if (j > 0 && board.isCellEnabled(j - 1, i))
				{
					Vegetable tmp = tmpGameBoard[j, i];
					tmpGameBoard[j, i] = tmpGameBoard[j - 1, i];
					tmpGameBoard[j - 1, i] = tmp;
					if (anyMoveFromPosition(tmpGameBoard, j, i))
					{
						return true;
					}
					tmp = tmpGameBoard[j, i];
					tmpGameBoard[j, i] = tmpGameBoard[j - 1, i];
					tmpGameBoard[j - 1, i] = tmp;
				}
				if (i + 1 < board.ySize && board.isCellEnabled(j, i + 1))
				{
					Vegetable tmp = tmpGameBoard[j, i];
					tmpGameBoard[j, i] = tmpGameBoard[j, i + 1];
					tmpGameBoard[j, i + 1] = tmp;
					if (anyMoveFromPosition(tmpGameBoard, j, i))
					{
						return true;
					}
					tmp = tmpGameBoard[j, i];
					tmpGameBoard[j, i] = tmpGameBoard[j, i + 1];
					tmpGameBoard[j, i + 1] = tmp;
				}
				if (j+1 < board.xSize && board.isCellEnabled(j + 1, i))
				{
					Vegetable tmp = tmpGameBoard[j, i];
					tmpGameBoard[j, i] = tmpGameBoard[j + 1, i];
					tmpGameBoard[j + 1, i] = tmp;
					if (anyMoveFromPosition(tmpGameBoard, j, i))
					{
						return true;
					}
					tmp = tmpGameBoard[j, i];
					tmpGameBoard[j, i] = tmpGameBoard[j + 1, i];
					tmpGameBoard[j + 1, i] = tmp;
				}
			}
		}
		return false;
	}
	private bool anyMoveFromPosition(Vegetable[,] tmpGameBoard, int x, int y)
	{
		Vegetable.VegeType nowType = tmpGameBoard[x, y].Type;
		int vert = 0;
		int hor = 0;
		if (y+1 < board.ySize && tmpGameBoard[x, y + 1]?.Type == nowType)
		{
			vert = 1;
			if (y + 2 < board.ySize && tmpGameBoard[x, y + 2]?.Type == nowType)
			{
				return true;
			}
		}
		if (y - 1 >=0 && tmpGameBoard[x, y - 1]?.Type == nowType)
		{
			if(vert == 1)
			{
				return true;
			}
			if (y - 2 >= 0 && tmpGameBoard[x, y - 2]?.Type == nowType)
			{
				return true;
			}
		}
		if (x + 1 < board.xSize && tmpGameBoard[x + 1, y]?.Type == nowType)
		{
			hor = 1;
			if (x + 2 < board.xSize && tmpGameBoard[x + 2, y]?.Type == nowType)
			{
				return true;
			}
		}
		if (x - 1 >= 0 && tmpGameBoard[x - 1, y]?.Type == nowType)
		{
			if (hor == 1)
			{
				return true;
			}
			if (x - 2 >= 0 && tmpGameBoard[x - 2, y]?.Type == nowType)
			{
				return true;
			}
		}
		return false;
	}
}		
