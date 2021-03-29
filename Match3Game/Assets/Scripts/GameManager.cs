using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Board board;
	public LevelManager levelManager;
	public float ClickTimeThreshold = 0.1f;
	public float ClickDistTreshold = 0.1f;

	private Vector2 lastMouseDown;
	private float lastMouseDownTime;
	private Vector2Int clickedCell;
	private Vector2 clickPoint;
	private Vector2Int[] moveFlags = new Vector2Int[4];
	private Vector2Int lastClickedCell;
	private void Start()
	{
		moveFlags[0] = Vector2Int.up;
		moveFlags[1] = Vector2Int.down;
		moveFlags[2] = Vector2Int.left;
		moveFlags[3] = Vector2Int.right;
		lastClickedCell = new Vector2Int(-1, -1);
	}
	void Update()
    {
		ProcessMouseInput();
    }
	void ProcessMouseInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			clickedCell = board.FindNearestCell(clickPoint);
			if (clickedCell == new Vector2Int(-1, -1))
			{
				return;
			}
			
			
			lastMouseDown = clickPoint;
			lastMouseDownTime = Time.time;
		}
		if (Input.GetMouseButtonUp(0))
		{
			Vector2 dragPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (Time.time - lastMouseDownTime < ClickTimeThreshold || Vector2.Distance(dragPoint, clickPoint)<ClickDistTreshold)
			{
				if (lastClickedCell == new Vector2Int(-1, -1))
				{
					board.SelectCell(clickedCell.x, clickedCell.y);
					lastClickedCell = clickedCell;
				}
				else if (Vector2Int.Distance(lastClickedCell, clickedCell) == 1)
				{
					//Debug.Log("Swap " + lastClickedCell + " " + clickedCell);
					levelManager.MakeAMove(clickedCell, lastClickedCell);
					board.SelectCell(clickedCell.x, clickedCell.y, false);
					board.SelectCell(lastClickedCell.x, lastClickedCell.y, false);
					lastClickedCell = new Vector2Int(-1, -1);
				}
				else
				{
					board.SelectCell(clickedCell.x, clickedCell.y);
					board.SelectCell(lastClickedCell.x, lastClickedCell.y, false);
					lastClickedCell = clickedCell;
				}
				
			}
			else
			{
				if(lastClickedCell != new Vector2Int(-1, -1))
				{
					board.SelectCell(lastClickedCell.x, lastClickedCell.y, false);
				}
				dragPoint.x -= clickPoint.x;
				dragPoint.y -= clickPoint.y;
				float flagdist = 10f;
				Vector2Int result = new Vector2Int(0, 0);
				foreach(Vector2Int flag in moveFlags)
				{
					if(Vector2.Distance(dragPoint,flag)<flagdist)
					{
						result = flag;
						flagdist = Vector2.Distance(dragPoint, flag);
					}
				}
				levelManager.MakeAMove(clickedCell, clickedCell + result);
				//Debug.Log(result);
			}
		}
	}
}
