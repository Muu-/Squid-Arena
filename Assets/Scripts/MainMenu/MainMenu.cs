using UnityEngine;
using System.Collections;

public class MainMenu : PlayerControls {
	public int selection = 0;
	public GameObject cursor;
	private GameObject textPlayers,
						textAIs;

	// Use this for initialization
	void Start () {		
		TextManager.me.DrawText("Squid", new Vector2(-80, 120), 3600, 32, "Game");
		TextManager.me.DrawText("Arena", new Vector2(-80, 80), 3600, 32, "Game");
		
		DrawModifiableText();
		TextManager.me.DrawText("Play", new Vector2(-50, -100), 3600, 16, "Game");
		
		GameObject tmpObj;
		tmpObj = TextManager.me.DrawText("Move", new Vector2(-50, -100), 3600, 12, "Game");
		ButtonPosition.me.AddButton(tmpObj,ButtonPosition.Position.left);
		tmpObj = TextManager.me.DrawText("OK", new Vector2(-50, -100), 3600, 12, "Game");
		ButtonPosition.me.AddButton(tmpObj,ButtonPosition.Position.right);
		
		cursor.transform.Rotate(0,270,0);
	}
	
	// Update is called once per frame
	new void Update () {
		Control();
		MoveCursor();
		
		if (Input.GetKeyDown(KeyCode.F5))
		{
			DrawableObject.colorblindMode = !DrawableObject.colorblindMode;	
		}
	}
	
	void DrawModifiableText() {
		string tmp = "1";
		if (GameManager.twoPlayersGame == true)
			tmp = "2";
		textPlayers = TextManager.me.DrawText("Players # " + tmp, new Vector2(-50, 0), 3600, 12, "Game");
		tmp = "no";
		if (GameManager.playWithAIs == true)
			tmp = "yes";	
		textAIs = TextManager.me.DrawText("AI players " + tmp, new Vector2(-50, -50), 3600, 12, "Game");
	}
	
	void DeleteModifiableText() {
		TextManager.me.RemoveText(textPlayers);
		TextManager.me.RemoveText(textAIs);
	}
	
	void MoveCursor() {
		cursor.transform.position = new Vector3(-80, 0, 7 - (selection*50));	
	}
	
	void Control()
	{
		if (Input.GetButtonDown("P1 Left"))
		{
			P1LeftButton();
		}
		if (Input.GetButtonDown("P1 Right"))
		{
			P1RightButton();
		}
		if (Input.GetButtonDown("P2 Left"))
		{
			P2LeftButton();
		}
		if (Input.GetButtonDown("P2 Right"))
		{
			P2RightButton();
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
						P1LeftButton();
					}
					if(TouchButton(Player.one, Button.right, t.position))
					{
						P1RightButton();
					}
					if(TouchButton(Player.two, Button.left, t.position))
					{
						P2LeftButton();
					}
					if(TouchButton(Player.two, Button.right, t.position))
					{
						P2RightButton();
					}
				}
			}
		}
	}
	
	void P1LeftButton()
	{
		selection++;
		if (selection > 2)
			selection = 0;	
	}
	
	void P1RightButton()
	{
		switch (selection)
			{
			case 0:
				GameManager.twoPlayersGame = !GameManager.twoPlayersGame;
				if (!GameManager.twoPlayersGame)
					GameManager.playWithAIs = true;
				DeleteModifiableText();
				DrawModifiableText();
				break;
			case 1:
				if (GameManager.twoPlayersGame)
				{
					GameManager.playWithAIs = !GameManager.playWithAIs;
					DeleteModifiableText();
					DrawModifiableText();
				}
				break;
			case 2:
				Application.LoadLevel("PlayScene");
				break;
			}	
	}
	
	void P2LeftButton()
	{
		DrawableObject.colorblindMode = !DrawableObject.colorblindMode;
	}
	
	void P2RightButton()
	{
		Jukebox.me.MuteOnOff();
	}
}
