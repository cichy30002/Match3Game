using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VegeSprites
{
	public static Sprite Broccoli = Resources.Load<Sprite>("Broccoli");
	public static Sprite Carrot = Resources.Load<Sprite>("Carrot");
	public static Sprite Potato = Resources.Load<Sprite>("Potato");
	public static Sprite Chili = Resources.Load<Sprite>("Chili");
	public static Sprite Eggplant = Resources.Load<Sprite>("Eggplant");
	public static Sprite Garlic = Resources.Load<Sprite>("Garlic");
	public static Sprite FindVegeSprite(Vegetable.VegeType type)
	{
		switch (type)
		{
			case Vegetable.VegeType.Broccoli:
				return Broccoli;
			case Vegetable.VegeType.Carrot:
				return Carrot;
			case Vegetable.VegeType.Chili:
				return Chili;
			case Vegetable.VegeType.Eggplant:
				return Eggplant;
			case Vegetable.VegeType.Garlic:
				return Garlic;
			case Vegetable.VegeType.Potato:
				return Potato;
		}
		return null;
	}
}
