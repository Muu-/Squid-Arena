using UnityEngine;
using System.Collections;

public class MenuControls : PlayerControls {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	new void Update () {
		if (Application.loadedLevelName == "PlayScene" && GameManager.gameEnded && GameManager.endTimer < 0)
		{
			MenuController();
		}
	}
	
	
	private void MenuController() {
		if (Input.GetButtonDown("P1 Left"))
		{
			Application.LoadLevel("StartMenu");
		}
		if (Input.GetButtonDown("P1 Right"))
		{
			Application.LoadLevel("PlayScene");
		}
		
		// Android touch controls
		if (Input.touches.Length >= 1)
		{
			for (int i = 0; i < Input.touches.Length; i++)
			{
				Touch t = Input.touches[i];
				
				if(t.phase == TouchPhase.Began)
				{
					if(TouchButton(Player.one, Button.left, t.position))
					{
						Application.LoadLevel("StartMenu");
					}
					if(TouchButton(Player.one, Button.right, t.position))
					{
						Application.LoadLevel("PlayScene");
					}
				}
			}
		}
	}
}
