using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {
	public static float lineThickness;
	public float startingX, startingY, endingX, endingY;
	public Quaternion startRotation;
	
	// Use this for initialization
	void Awake () {
		startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (startingX, 1, startingY);
		Vector2 delta = new Vector2(endingX-startingX, endingY-startingY);
		float angle = (float) System.Math.Atan2(delta.x, delta.y) * 180 / ((float) System.Math.PI);
		if (angle < 0)
			angle += 360;
		transform.rotation = startRotation;
		transform.Rotate(0, angle, 0);
		float distance = Vector2.Distance(new Vector2(startingX,startingY), new Vector2(endingX,endingY));
		transform.localScale = new Vector3(5*lineThickness,5*lineThickness,distance);
	}
}
