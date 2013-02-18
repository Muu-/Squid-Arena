using UnityEngine;
using System.Collections;

public class ArenaDraw : DrawableObject {
	public float squareSize;
	public float arenaSwitchColorTimer = 2;
	public const float arenaSwitchColorTime = 2;
	public PhysicMaterial arenaBordersMaterial;
	protected Vector3 topLeftBound;
	public static ArenaDraw me;
	
	// Use Start for initialization
	protected void Awake () {
		me = this;
		color = getRandomColor();
		squareSize /= 2;
		GameObject arenaParent = new GameObject("Arena");
		LineCreate(-squareSize, squareSize, squareSize, squareSize, arenaParent, "top");
		LineCreate(-squareSize, -squareSize, squareSize, -squareSize, arenaParent, "bottom");
		LineCreate(-squareSize, squareSize, -squareSize, -squareSize, arenaParent, "left");
		LineCreate(squareSize, squareSize, squareSize, -squareSize, arenaParent, "right");
		
		topLeftBound = new Vector3(-squareSize, 0, squareSize);
		
		foreach (GameObject g in lines)
		{
			g.GetComponent<BoxCollider>().material = arenaBordersMaterial;
			g.tag = "arenaBorder";
		}
	}
	
	protected void Update() {
		arenaSwitchColorTimer -= Time.deltaTime;
		if (arenaSwitchColorTimer <= 0)
		{
			arenaSwitchColorTimer = arenaSwitchColorTime;
			byte newColor = 0;
			while (newColor == color)
			{
				newColor = getRandomColor();	
			}
			color = newColor;
			foreach (GameObject g in lines)
			{
				LineChangeColor(g, color);	
			}
		}
		
		SetDistances();
	}
	
	// Automatically resize screen
	protected void SetDistances() {
		if (camera.WorldToScreenPoint(topLeftBound).x < 25)
		{
			Zoom(1);
		}
		if (camera.WorldToScreenPoint(topLeftBound).x > 25 && camera.WorldToScreenPoint(topLeftBound).y < (Screen.height - 25) )
		{
			Zoom(-1);
		}
	}
	
	protected void Zoom(int sign)
	{
		camera.transform.position = camera.transform.position + Vector3.up * 20 * (Mathf.Sign(sign));
	}
}
