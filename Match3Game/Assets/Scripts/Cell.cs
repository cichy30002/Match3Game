using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	public Sprite ActiveSprite;
	public Sprite DisabledSprite;
	public Sprite SelectedSprite;
    // Start is called before the first frame update
    void Start()
    {
        if(spriteRenderer == null)
		{
			spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		}
    }
	
	public void SetSprite(string name)
	{
		switch(name)
		{
			case "default":
				spriteRenderer.sprite = ActiveSprite;
				break;
			case "disabled":
				spriteRenderer.sprite = DisabledSprite;
				break;
			case "selected":
				spriteRenderer.sprite = SelectedSprite;
				break;
		}
	}
}
