using UnityEngine;
using System.Collections;

public class NavControl : MonoBehaviour {
	public const float shotForce = 2000;
	public const float maxAllowedVelocity = 120;
	public const float maxAllowedAngularVelocity = 70;
	public const float bulletSpeed = 150;
	public const float shotCost = 0.20f;
	public Cannon[] cannon = new Cannon[2];
	private DrawableObject drawStuff;
	public GameObject explosionPrefab;
	public float health = 8;
	private float blink = 0;			// Time to blink when a nav is hit
	
	public enum Cannons {
		left,
		right
	};
	
	public void Awake() {
		drawStuff = GetComponent<DrawableObject>();	
	}
	
	public void Start() {
		cannon[0] = new Cannon(transform.Find("LeftCannon"));
		cannon[1] = new Cannon(transform.Find("RightCannon"));
	}
	
	public void Update () {
		if (blink > 0)
		{
			Blink();
		}
		
		if (health <= 0)
		{
			Boom ();
		}
	}
	
	public void OnCollisionEnter(Collision coll) {
		foreach(ContactPoint cp in coll)
		{
			Bullet b = cp.otherCollider.transform.GetComponent<Bullet>();
			if (b != null && b.owner != transform)
			{
				Bullet.BulletDestroy(b);
				if (blink < 0)
				{
					health -= 1;
				}
				blink = 2;
			}
		}
	}
	
	public void Blink() {
		if ( ((int)(blink * 10)) % 2 == 0 )
		{
			drawStuff.SwitchRendersBool(true);
		}
		else
		{
			drawStuff.SwitchRendersBool(false);	
		}
		blink -= Time.deltaTime;
	}	
	
	private void Boom() {
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		GameManager.RemovePlayer(name);
		gameObject.SetActive(false);
	}
	
	public void Shot(Cannons cannonToUse) {		
		int chargeLevel = GetCannon(cannonToUse).GetChargeLevel();
		Vector3 shotPosition = GetCannon(cannonToUse).toShotFrom.position;
		GameObject b = Bullet.BulletInstantiate(shotPosition, Vector3.up * transform.rotation.eulerAngles.y, bulletSpeed);
		b.GetComponent<Bullet>().setColor(drawStuff.getNextColor((byte) chargeLevel));
		b.GetComponent<Bullet>().owner = transform;
		Vector3 force = -transform.forward * shotForce * chargeLevel;
		rigidbody.AddForceAtPosition(force, shotPosition, ForceMode.Impulse);
	}
	
	public void FixedUpdate() {
		if (rigidbody.velocity.magnitude > maxAllowedVelocity)
		{
			rigidbody.velocity = rigidbody.velocity.normalized * maxAllowedVelocity;
		}
		if (rigidbody.angularVelocity.magnitude > maxAllowedAngularVelocity)
		{
			rigidbody.angularVelocity = rigidbody.angularVelocity.normalized * maxAllowedAngularVelocity;
		}	
	}
	
	public void Charge(Cannons cannonToCharge) {
		GetCannon(cannonToCharge).Charge();
	}
	
	public Cannon GetCannon(Cannons cannonsEnum)
	{
		switch (cannonsEnum)
		{
			case Cannons.right:
				return cannon[0];
			case Cannons.left:
				return cannon[1];
		}
		return null;
	}
	
	public float GetTotalCharge() {
		float tmp = 0;
		if (cannon[0].charge > 0)
			tmp += (cannon[0].charge - shotCost)/2;
		if (cannon[1].charge > 0)
			tmp += (cannon[1].charge - shotCost)/2;
		return tmp;
	}
	
	public class Cannon
	{
		public Transform toShotFrom;
		public float charge = 0;
		
		public Cannon(Transform cannonTip)
		{
			toShotFrom = cannonTip;	
		}
		
		public void Charge()
		{
			if (charge == 0)
				charge += shotCost;
			charge += Time.deltaTime;
			if (charge > shotCost*3)
				charge = shotCost*3;
		}
		
		public int GetChargeLevel()
		{	
			int level = (int) (charge/shotCost);
			charge = 0;
			return level;	
		}
	}
}
