using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonPosition : MonoBehaviour {
	public static ButtonPosition me;
	private Dictionary<GameObject, Position> buttons = new Dictionary<GameObject, Position>();
	public enum Position {
		left,
		right
	}
	
	void Awake() {
		me = this;	
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}
	
	public void AddButton(GameObject button, Position position)
	{
		buttons.Add(button, position);
	}
	
	public void Move()
	{
		foreach (KeyValuePair<GameObject, Position> k in buttons)
		{
			Vector3 pos = Vector3.zero;
			float x = 0;
			switch (k.Value)
			{
			case Position.left:
				x = Screen.width * 0.08f;
				break;
			case Position.right:
				x = Screen.width * 0.92f;
				if (Screen.width < 820)
					x = Screen.width * 0.85f;
				break;
			}
			pos = Camera.mainCamera.ScreenToWorldPoint(new Vector3(x, 10, Camera.mainCamera.transform.position.y - 5));
			k.Key.transform.position = new Vector3(pos.x, 1, pos.z);
		}
	}
}
