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
	public Sprite Broccoli;
	public Sprite Carrot;
	public Sprite Potato;
	public Sprite Chili;
	public Sprite Eggplant;
	public Sprite Garlic;
	private void Start()
	{
		Type = RandomVegeType();
		switch(Type)
		{
			case VegeType.Broccoli:
				spriteRenderer.sprite = Broccoli;
				break;
			case VegeType.Carrot:
				spriteRenderer.sprite = Carrot;
				break;
			case VegeType.Chili:
				spriteRenderer.sprite = Chili;
				break;
			case VegeType.Eggplant:
				spriteRenderer.sprite = Eggplant;
				break;
			case VegeType.Garlic:
				spriteRenderer.sprite = Garlic;
				break;
			case VegeType.Potato:
				spriteRenderer.sprite = Potato;
				break;
		}
	}
	public enum VegeType
	{
		Broccoli,
		Carrot,
		Potato,
		Chili,
		Eggplant,
		Garlic
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
}
