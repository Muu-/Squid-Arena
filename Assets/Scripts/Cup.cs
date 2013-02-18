using UnityEngine;
using System.Collections;

public class Cup : DrawableObject {
	float timer = 5;
	// Use this for initialization
	void Start () {
		transform.position = transform.position + Vector3.up * 25;
		LineCreate(-100, 100, 100, 100, gameObject, "cup");
		LineCreate(-100, 100, -10, 0, gameObject, "cup");
		LineCreate(100, 100, 10, 0, gameObject, "cup");
		
		LineCreate(-10, -0, 10, 0, gameObject, "cup");
		
		LineCreate(-50, -50, -10, 0, gameObject, "cup");
		LineCreate(50, -50, 10, 0, gameObject, "cup");
		LineCreate(-50, -50, 50, -50, gameObject, "cup");
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		foreach (GameObject g in lines)
		{
			if ( ((int)(timer * 5)) % 2 == 0 )
			{
				color = getNextColor();
				LineChangeColor(g, color);
			}
		}
	}
}
