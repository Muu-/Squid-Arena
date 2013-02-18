using UnityEngine;
using System.Collections;

public class Explosion : DrawableObject {
	float timer = 4;
	
	// Use this for initialization
	void Awake () {
		transform.position = transform.position - Vector3.up * 20;
		float x = 0;
  		float y = 0;
		float x2 = 0;
  		float y2 = 0;
		float radius = 10;
		float radius2 = 20;
		for (int i = 1; i < 19; i++)
		{
			int j = i * 360/18;
			x = transform.position.x + radius*Mathf.Cos(2*Mathf.PI/360 * j);
  			y = transform.position.z + radius*Mathf.Sin(2*Mathf.PI/360 * j);
			x2 = transform.position.x + radius2*Mathf.Cos(2*Mathf.PI/360 * j);
  			y2 = transform.position.z + radius2*Mathf.Sin(2*Mathf.PI/360 * j);
			LineCreate(x, y, x2, y2, gameObject, "Line");
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		bool tmpBool = false;
		if ( ((int)(timer * 5)) % 2 == 0 )
		{
			tmpBool = true;	
		}
		foreach (GameObject g in lines)
		{
			if (tmpBool)
			{
				color = getNextColor();
				LineChangeColor(g, color);
			}
			g.transform.Translate( (g.transform.position - transform.position).normalized * 100 * Time.deltaTime, Space.World);	
		}
		
		if (timer < -5)
		{
			gameObject.SetActive(false);	
		}
	}
}
