using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelsGenerator : MonoBehaviour
{
	[SerializeField] private int levels = 1;
	public GameObject levelButtonPrefab;
	public MenuManager menuManager;
    void Start()
    {
		int unlocked = Save.LoadLevel().unlockedLevel;
		for (int i = 1; i <= levels; i++)
		{
			int x = i;
			GameObject newButton = Instantiate(levelButtonPrefab, transform);
			Button button = newButton.GetComponent<Button>();
			button.onClick.AddListener(new UnityEngine.Events.UnityAction(() => menuManager.LoadLevel(x, unlocked) ));
			TMP_Text text = newButton.GetComponentInChildren<TMP_Text>();
			text.text = i.ToString();
			if(i > unlocked)
			{
				button.interactable = false;
			}
		}
    }

    
}
