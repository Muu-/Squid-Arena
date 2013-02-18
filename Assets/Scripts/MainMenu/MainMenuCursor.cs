using UnityEngine;
using System.Collections;

public class MainMenuCursor : NavDraw {
	bool buttonDown = true;
	
	new void Awake()
	{
		base.Awake();
		
	}
	
	new void Start() {
		
	}
	
	new void Update() {
		CursorCharge();
	}
	
	void CursorCharge()
	{
		if (buttonDown)
		{
			if (charge > 0.50f)
			{
				buttonDown = false;
			}
			charge += Time.deltaTime;
		}
		else
		{
			if (charge < 0.0f)
			{
				buttonDown = true;
			}
			charge -= Time.deltaTime * 3;
		}
		
		float scaleDiff = charge * 16;
		transform.localScale = new Vector3(scale.x + scaleDiff/4, scale.y * 10, scale.x - scaleDiff);
	}
	
}
