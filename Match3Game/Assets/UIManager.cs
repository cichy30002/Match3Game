using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
	public TMP_Text PointsText;
	public void UpdatePoints(int points)
	{
		PointsText.text = points.ToString();
	}
}
