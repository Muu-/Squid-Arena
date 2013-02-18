using UnityEngine;
using System.Collections;

public class Cursor : DrawableObject {
	
	// Use this for initialization
	void Awake () {
		
	}
	
	public void Make(PlayerControls.Player player) {
		switch (player)
		{
		case PlayerControls.Player.one:
			One();
			break;
		case PlayerControls.Player.two:
			Two();
			break;
		}
	}
	
	private void One() {
		float 	half = transform.lossyScale.x / 2,
			x = transform.position.x,
			y = transform.position.z;
		
		LineCreate(x-half,		y+half,		x+half,		y+half,		gameObject, "TopHor");
		LineCreate(x-half,		y-half,		x+half,		y-half,		gameObject, "BotHor");
		LineCreate(x,			y+half,		x,			y-half,		gameObject, "Vert");	
	}
	
	private void Two() {
		float 	half = transform.lossyScale.x / 2,
			x = transform.position.x,
			y = transform.position.z;
		
		LineCreate(x-half,		y+half,		x+half,		y+half,		gameObject, "TopHor");
		LineCreate(x-half,		y-half,		x+half,		y-half,		gameObject, "BotHor");
		LineCreate(x-6,			y+half,		x-6,			y-half,		gameObject, "Vert");
		LineCreate(x+6,			y+half,		x+6,			y-half,		gameObject, "Vert");
		
	}
	
	// Update is called once per frame
	void Update () {		
		GameManager.timer -= Time.deltaTime;
		if (GameManager.timer < 2)
		{
			transform.Translate(transform.position.normalized * 400 * Time.deltaTime);
		}
		if (GameManager.timer < -5)
		{
			Destroy(gameObject);
		}
		
		ColorStuff();
	}
	
	void ColorStuff() {
		byte[] b = System.BitConverter.GetBytes(GameManager.timer);
		LineAllChangeColor((byte)((b[0]) % 8));
	}
}
