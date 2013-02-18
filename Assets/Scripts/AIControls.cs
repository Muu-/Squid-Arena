using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIControls : MonoBehaviour {
	private NavControl controls;
	private List<Threat> threats = new List<Threat>();
	private List<Collider> childColliders = new List<Collider>();
	private enum Button { left, right };
	private Dictionary<string, List<float>> Buttons = new Dictionary<string, List<float>>();

	
	// Use this for initialization
	void Awake () {
		controls = GetComponent<NavControl>();
		SetChildColliders();
		Buttons.Add("Right", new List<float>());
		Buttons.Add("Left", new List<float>());
	}
	
	// Update is called once per frame
	public void Update () {
		if (GameManager.timer < 0)
		{
			AIStuff();
			ButtonManager();
			Debug.DrawRay(transform.position, -transform.forward.normalized * 60);
		}
	}
	
	private void AIStuff(){
		Threat nearest = GetNearestThreat();
		if (Buttons["Right"].Count == 0 && Buttons["Left"].Count == 0)
			if (nearest == null) 			//	Aim	
			{
				ButtonPush(RandomButton(), RandomPower());
				ButtonPush(RandomButton(), RandomPower());
			}
			else
			{
				float otherAngle = Vector2.Angle(Vector2.up, new Vector2(nearest.threatTransform.rigidbody.velocity.x, nearest.threatTransform.rigidbody.velocity.z));
				float myAngle = transform.rotation.eulerAngles.y;
				float delta = Mathf.DeltaAngle(myAngle, otherAngle);
				if (IsGoingOutside() || Mathf.Abs(delta) < 30)			// Escape by rotating
				{
					if (delta > 0)
					{
						ButtonPush(Button.right, 0.075f);
						ButtonPush(Button.right, 0.075f);
						ButtonPush(Button.right, 0.25f);
						ButtonPush(Button.left, 0.40f);
					}
					else
					{
						ButtonPush(Button.right, 0.40f);
						ButtonPush(Button.left, 0.075f);
						ButtonPush(Button.left, 0.075f);
						ButtonPush(Button.left, 0.25f);
					}
				}
				else 													// Escape by pushing only
				{
					float power = 0.05f;
					if (nearest.timeUntilCollision > 0.32f)
						power = 0.26f;
					
					ButtonPush(Button.right, power);
					ButtonPush(Button.left, power);
					ButtonPush(Button.right, power+0.2f);
					ButtonPush(Button.left, power+0.2f);
				}
				RemoveThreat(nearest.threatTransform);	
			}
	}
	
	private void ButtonManager() {
		if (Buttons["Right"].Count > 0 && Buttons["Right"][0] > 0)
		{
			ButtonDown(Button.right);
		}
		if (Buttons["Left"].Count > 0 && Buttons["Left"][0] > 0)
		{
			ButtonDown(Button.left);
		}
		if (Buttons["Right"].Count > 0 && Buttons["Right"][0] < 0)
		{
			ButtonPull(Button.right);
		}
		if (Buttons["Left"].Count > 0 && Buttons["Left"][0] < 0)
		{
			ButtonPull(Button.left);
		}
	}
	
	private void ButtonPush(Button bt, float time) {
		switch (bt)
		{
		case Button.right:
			if (Buttons["Right"].Count < 5)
				Buttons["Right"].Add(time);
			break;
		case Button.left:
			if (Buttons["Left"].Count < 5)
				Buttons["Left"].Add(time);
			break;
		}
	}
	
	private float RandomPower()
	{
		return Random.Range(0.1f, 0.4f);	
	}
	
	private Button RandomButton()
	{
		Button b = Button.left;
		if (Random.Range(0,100) > 50)
			b = Button.right;
		return b;
	}
	
	private void ButtonDown(Button bt) {
	switch (bt)
		{
		case Button.right:
			controls.Charge(NavControl.Cannons.right);
			Buttons["Right"][0] -= Time.deltaTime;
			break;
		case Button.left:
			controls.Charge(NavControl.Cannons.left);
			Buttons["Left"][0] -= Time.deltaTime;
			break;
		}
	}
	
	
	private void ButtonPull(Button bt) {
		switch (bt)	
		{
		case Button.right:
			controls.Shot(NavControl.Cannons.right);
			Buttons["Right"].RemoveAt(0);
			break;
		case Button.left:
			controls.Shot(NavControl.Cannons.left);
			Buttons["Left"].RemoveAt(0);
			break;
		}
	}
	
	private void SetChildColliders()
	{
		for (int i = 0; i<transform.childCount; i++)
		{
			if (transform.GetChild(i).collider != null)
			{
				childColliders.Add(transform.GetChild(i).collider);
			}
		}
	}
	
	public Threat GetNearestThreat() {
		Threat tmp = null;
		foreach (Threat t in threats)
		{
			if (tmp == null || tmp.timeUntilCollision > t.timeUntilCollision)
			{
				tmp = t;
			}
		}
		return tmp;
	}
	
	public void AddThreat(Bullet threat, Vector2 collisionPosition, float timeToCollision)
	{
		if (!AlreadyHasThisBullet(threat.transform))
		{
			threats.Add(new Threat(threat.transform, collisionPosition, timeToCollision));
		}
	}
	
	private bool AlreadyHasThisBullet(Transform threat) {
		foreach (Threat t in threats)
		{
			if (t.CheckBullet(threat))
			{
				return true;	
			}
		}
		return false;
	}
	
	private bool IsGoingOutside() {
		Vector3 futurePos = transform.position + (-transform.forward.normalized * 60);
		float border = ArenaDraw.me.squareSize;
		if(futurePos.x < -border || futurePos.x > border || futurePos.z < -border || futurePos.z > border)
			return true;
		return false;
	}
	
	public class Threat {
		public Transform threatTransform;
		public Vector2 collisionPosition;
		public float timeUntilCollision;
		
		public Threat(Transform newThreatTransform, Vector2 newCollisionPosition, float timeToCollision) {
			threatTransform = newThreatTransform;
			collisionPosition = newCollisionPosition;
			timeUntilCollision = timeToCollision;
		}
		
		public bool CheckBullet(Transform toCheck) {
			if (toCheck == threatTransform)
			{
				return true;	
			}
			return false;
		}
	}

	public void RemoveThreat (Bullet toRemove)
	{
		RemoveThreat(toRemove.transform);
	}
	
	public void RemoveThreat (Transform toRemove)
	{
		Threat tmp = null;
		foreach (Threat t in threats)
		{
			if (toRemove == t.threatTransform)
			{
				tmp = t;
			}
		}
		
		if (tmp != null)
		{
			threats.Remove(tmp);	
		}
	}
}
