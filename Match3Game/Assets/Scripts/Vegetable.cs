using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	public int Points = 1;
	public VegeType Type;
	public VegeState State = VegeState.Default;
	
	private void Start()
	{
		SetVegeType( RandomVegeType());
		
	}
	public enum VegeType
	{
		Broccoli,
		Carrot,
		Potato,
		Chili,
		Eggplant,
		Garlic,
		none
	}
	public enum VegeState
	{
		Default
	}
	public VegeType RandomVegeType()
	{
		Array values = Enum.GetValues(typeof(VegeType));
		int winner = UnityEngine.Random.Range(0, values.Length - 1);
		return (VegeType)values.GetValue(winner);

	}
	public void SetVegeType(VegeType type)
	{
		Type = type;
		spriteRenderer.sprite = VegeSprites.FindVegeSprite(type);
	}
	
}
