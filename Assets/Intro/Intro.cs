using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {
	private float timer = 3f;
	
	// Use this for initialization
	void Start () {
		TextManager.me.DrawText("A.", new Vector2(-150, -50), 3600, 128, "Intro");
		TextManager.me.DrawText("C.", new Vector2(-45, -50), 3600, 128, "Intro");
		TextManager.me.DrawText("id", new Vector2(60, -50), 3600, 128, "Intro");
		TextManager.me.DrawText("videogames", new Vector2(-180, -80), 3600, 64, "Intro");
		if (Application.platform == RuntimePlatform.Android)
			timer = 6f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount >= 1 || Input.anyKeyDown || timer < 0)
		{
			Application.LoadLevel("StartMenu");
		}
		timer -= Time.deltaTime;
	}	
}
