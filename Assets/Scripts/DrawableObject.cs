using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawableObject : MonoBehaviour {
	public GameObject linePrefab;			// Line prefab, used to instantiate new lines
	public static float lineSize = 0.75f;	// Line thickness
	public byte color;						// Line color, must be in 0-15 range
	public List<GameObject> lines = new List<GameObject>();	// List to store an object's instantiated lines
	public static bool colorblindMode = false;
	
	public GameObject LineCreate(float startingX, float startingY, float endingX, float endingY, GameObject lineParent, string lineName) {
		GameObject tmp_line = (GameObject) Instantiate(linePrefab, new Vector3(startingX, lineParent.transform.position.y + 0.05f, startingY), linePrefab.transform.rotation);
		tmp_line.name = lineName;
		UVFunctions.SetUVByPixelSizes(getUVColorPosition(color), Vector2.one*8, false, tmp_line.GetComponent<MeshFilter>().mesh, tmp_line.GetComponent<MeshRenderer>().sharedMaterial.mainTexture);
		Vector2 delta = new Vector2(endingX-startingX, endingY-startingY);
		float angle = (float) System.Math.Atan2(delta.x, delta.y) * 180 / ((float) System.Math.PI);
		if (angle < 0)
			angle += 360;
		tmp_line.transform.Rotate(0, angle, 0);
		float distance = Vector2.Distance(new Vector2(startingX,startingY), new Vector2(endingX,endingY));
		tmp_line.transform.localScale = new Vector3(5*lineSize,5*lineSize,distance + lineSize);
		tmp_line.transform.parent = lineParent.transform;
		lines.Add(tmp_line);
		return tmp_line;
	}
	
	public void LineEdit(GameObject line, float startingX, float startingY, float endingX, float endingY)
	{
		Vector2 delta = new Vector2(endingX-startingX, endingY-startingY);
		float angle = (float) System.Math.Atan2(delta.x, delta.y) * 180 / ((float) System.Math.PI);
		if (angle < 0)
			angle += 360;
		line.transform.rotation = Quaternion.identity;
		line.transform.Rotate(0, angle, 0);
		float distance = Vector2.Distance(new Vector2(startingX,startingY), new Vector2(endingX,endingY));
		line.transform.localScale = new Vector3(5*lineSize/line.transform.parent.lossyScale.x,5*lineSize/line.transform.parent.lossyScale.z,distance);
	}
	
	public void LineEdit(string name, float startingX, float startingY, float endingX, float endingY)
	{
		Transform tmp = transform.FindChild(name);
		LineEdit(tmp.gameObject, startingX, startingY, endingX, endingY);
	}
	
	public void LineChangeColor(GameObject lineToColor, byte newColor) {
		UVFunctions.SetUVByPixelSizes(getUVColorPosition(newColor), Vector2.one*8, false, lineToColor.GetComponent<MeshFilter>().mesh, lineToColor.GetComponent<MeshRenderer>().sharedMaterial.mainTexture);
	}
	
	public void LineAllChangeColor(byte newColor)
	{
		foreach (GameObject g in lines)
		{
			LineChangeColor(g, newColor);	
		}
	}
	
	public static Vector2 getUVColorPosition(byte colorToReturn) {
		if (colorblindMode) {
			colorToReturn += 8;	
		}
		return new Vector2((colorToReturn % 4), (colorToReturn / 4));
	}
	
	public static byte getRandomColor() {
		return (byte) (Mathf.Floor(Random.Range(0,799)/100));
	}
	
	public byte getNextColor(byte next) {
		byte tmp = (byte) (color + next);
		if (tmp > 7) tmp -= 8;
		return tmp;
	}
	
	public byte getNextColor() {
		return getNextColor((byte) 1);
	}
	
	public void Clear() {
		foreach(GameObject g in lines)
		{
			Destroy(g);
		}
		lines = new List<GameObject>();
	}
	
	public void SwitchRendersBool(bool on) {
		foreach(GameObject g in lines)
		{
			g.renderer.enabled = on;	
		}
	}
}
