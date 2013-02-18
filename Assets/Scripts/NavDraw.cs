using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavDraw : DrawableObject {
	protected NavControl controls;
	protected float charge = 0;
	protected Vector3 scale;
	
	// Use this for initialization
	protected void Awake () {
		controls = GetComponent<NavControl>();
		scale = transform.localScale;
		// Make the ship
		color = getRandomColor();
		Shape();
	}
	
	protected void Start()
	{
		TextManager.me.DrawText(name, new Vector2(transform.position.x - (((name.Length-1)/2)*16), transform.position.z - 36), 5, 12, "Game");	
	}
	
	// Update is called once per frame
	protected void Update () {
		Charge();
	}
	
	protected void Shape() {
		float 	half = transform.lossyScale.x / 2,
					one = transform.lossyScale.x,
					x = transform.position.x,
					y = transform.position.z;

		// /\
		// \/
		// /\
		LineCreate(x,		y-one,		x+half,		y,		gameObject, "Head 1");
		LineCreate(x,		y-one,		x-half,		y,		gameObject, "Head 2");
		LineCreate(x,		y+half,		x+half,		y,		gameObject, "Head 3");
		LineCreate(x,		y+half,		x-half,		y,		gameObject, "Head 4");
		LineCreate(x+half,	y+one,		x,			y+half,	gameObject, "RightCannon");
		LineCreate(x-half,	y+one,		x,			y+half,	gameObject, "LeftCannon");
		
		// Eyes
		List<GameObject> tmp = new List<GameObject>();
		tmp.Add(LineCreate(x+transform.lossyScale.x * 0.15f,	y-transform.lossyScale.x * 0.50f, x+transform.lossyScale.x * 0.15f, y-transform.lossyScale.x * 0.15f,	gameObject, "RightEye"));
		tmp.Add(LineCreate(x-transform.lossyScale.x * 0.15f,	y-transform.lossyScale.x * 0.50f, x-transform.lossyScale.x * 0.15f, y-transform.lossyScale.x * 0.15f,	gameObject, "LeftEye"));
		
		
		foreach (GameObject g in tmp)
		{	
			Destroy(g.GetComponent<Collider>());
			LineChangeColor(g, getNextColor());
		}
	}
	
	protected void Charge() {
		if (charge < controls.GetTotalCharge())
		{
			charge += Time.deltaTime;
			if (charge > controls.GetTotalCharge())
			{
				 charge = controls.GetTotalCharge();	
			}
		}
		if (charge > controls.GetTotalCharge())
		{
			charge -= Time.deltaTime*3;
			if (charge < controls.GetTotalCharge())
			{
				 charge = controls.GetTotalCharge();	
			}
		}
		
		float scaleDiff = charge * 16;
		transform.localScale = new Vector3(scale.x + scaleDiff/4, scale.y * 10, scale.x - scaleDiff);
	}
}
