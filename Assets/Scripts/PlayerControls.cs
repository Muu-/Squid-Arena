using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {
	NavControl controls;
	protected enum Button { left, right };
	public enum Player { one, two };
	public Player player = Player.one;
	
	// Use this for initialization
	void Awake () {
		controls = GetComponent<NavControl>();
	}
	
	void Start() {

	}
	
	// Update is called once per frame
	public void Update () {
		if(GameManager.timer <  0 && !GameManager.gameEnded)
		{
			GameControls();
		}
	}
	
	private void GameControls() {
		if (Input.GetButton(GetButton(Button.left)))
		{
			controls.Charge(NavControl.Cannons.left);
		}
		if (Input.GetButton(GetButton(Button.right)))
		{
			controls.Charge(NavControl.Cannons.right);
		}
		if (Input.GetButtonUp(GetButton(Button.left)))
		{
			controls.Shot(NavControl.Cannons.left);
		}
		if (Input.GetButtonUp(GetButton(Button.right)))
		{
			controls.Shot(NavControl.Cannons.right);
		}
		
		// Androd touch controls
		if (Input.touches.Length >= 1)
		{
			for (int i = 0; i < Input.touches.Length; i++)
			{
				Touch t = Input.touches[i];
				
				if(t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
				{
					if(TouchButton(player, Button.left, t.position))
					{
						controls.Charge(NavControl.Cannons.left);
					}
					if(TouchButton(player, Button.right, t.position))
					{
						controls.Charge(NavControl.Cannons.right);
					}
				}
				if(t.phase == TouchPhase.Ended)
				{
					if(TouchButton(player, Button.left, t.position))
					{
						controls.Shot(NavControl.Cannons.left);
					}
					if(TouchButton(player, Button.right, t.position))
					{
						controls.Shot(NavControl.Cannons.right);
					}
				}
			}
		}
	}
	
	protected bool TouchButton(Player player, Button button, Vector2 position) {
		int buttonWidth = Screen.width / 3;
		int buttonHeight = Screen.height / 3;
		switch (player)
		{
		case Player.one:
			switch (button)
			{
			case Button.left:
				if (position.x < buttonWidth && position.y < buttonHeight)
					return true;
				break;
			case Button.right:
				if(position.x > Screen.width - buttonWidth && position.y < buttonHeight)
					return true;
				break;
			}	
			break;
		case Player.two:
			switch (button)
			{
			case Button.left:
				if (position.x > Screen.width - buttonWidth && position.y > Screen.height - buttonHeight)
					return true;
				break;
			case Button.right:
				if(position.x < buttonWidth && position.y > Screen.height - buttonHeight)
					return true;
				break;
			}	
			break;	
		}
		
		
		return false;
	}
	
	protected string GetButton(Button button)
	{
		switch (player)
		{
		case Player.one:
			switch (button)
			{
			case Button.left:
				return "P1 Left";
			case Button.right:
				return "P1 Right";
			}	
			break;
		case Player.two:
			switch (button)
			{
			case Button.left:
				return "P2 Left";
			case Button.right:
				return "P2 Right";	
			}	
			break;	
		}
		
		return string.Empty;
	}
}
