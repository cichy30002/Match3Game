using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	public SpriteRenderer SpriteRenderer;
	public Sprite ActiveSprite;
	public Sprite DisabledSprite;
    // Start is called before the first frame update
    void Start()
    {
        if(SpriteRenderer == null)
		{
			SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		}
    }
	
	public void SetSprite(string name)
	{
		switch(name)
		{
			case "default":
				SpriteRenderer.sprite = ActiveSprite;
				break;
			case "disabled":
				SpriteRenderer.sprite = DisabledSprite;
				break;
		}
	}
}
