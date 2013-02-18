using UnityEngine;
using System.Collections;

public class MainMenuZooming : ArenaDraw {
	
	new void Awake()
	{
		
	}	
	
	// Use this for initialization
	void Start () {
		topLeftBound = new Vector3(-150, 0, 150);
	}
	
	// Update is called once per frame
	new void Update () {
		SetDistances();
	}
}
