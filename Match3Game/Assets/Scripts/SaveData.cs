using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public int levelNumber = 1;
	public SaveData(int level)
	{
		levelNumber = level;
	}
}
