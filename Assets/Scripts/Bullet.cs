using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : DrawableObject {
	public static List<Bullet> activeBullets = new List<Bullet>();
	private static List<Bullet> unusedBullets = new List<Bullet>();
	public Vector3 storedVelocity;
	public Transform owner;
	
	// Use this for initialization
	void Start () {
		rigidbody.MovePosition(rigidbody.position + transform.forward.normalized * 5);
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(transform.position.y) > 20)
		{
			BulletDestroy(this);	
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		foreach(ContactPoint contact in collision)
		{
			if (contact.otherCollider.tag == "arenaBorder")
			{
				BulletDestroy(this);	
				break;
			}
		}
	}
	
	void FixedUpdate()
	{
		rigidbody.velocity = storedVelocity;	
	}
	
	public static void BulletDestroy(Bullet bulletToDestroy) {
		activeBullets.Remove(bulletToDestroy);
		unusedBullets.Add(bulletToDestroy);
		AIManager.me.RemoveBullet(bulletToDestroy);
		bulletToDestroy.owner = null;
		bulletToDestroy.storedVelocity = Vector3.zero;
		bulletToDestroy.transform.position = (Vector3.forward * 2000) + Vector3.right * unusedBullets.Count * 10;
		bulletToDestroy.rigidbody.velocity = Vector3.zero;
		bulletToDestroy.rigidbody.angularVelocity = Vector3.zero;
	}
	
	public static GameObject BulletInstantiate(Vector3 newPosition, Vector3 newRotation, float forceMagnitude) {
		Transform b;
		if (unusedBullets.Count > 0)
		{
			b = unusedBullets[unusedBullets.Count-1].transform;
			unusedBullets.RemoveAt(unusedBullets.Count-1);
		} 
		else
		{
			b = (Transform) Instantiate(GameManager.me.bullet, Vector3.back * 2000, Quaternion.identity);
			b.parent = GameManager.me.bulletFolder;
		}
		b.transform.position = newPosition;
		b.transform.rotation = Quaternion.identity;
		b.transform.Rotate(newRotation);
		
		b.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
		b.rigidbody.AddForce(b.transform.forward * forceMagnitude, ForceMode.VelocityChange);
		b.GetComponent<Bullet>().storedVelocity = b.transform.forward * forceMagnitude;
		
		if (!activeBullets.Contains(b.GetComponent<Bullet>()))
			activeBullets.Add(b.GetComponent<Bullet>());
		return b.gameObject;
	}
	
	public static void Reset() {
		activeBullets = new List<Bullet>();
		unusedBullets = new List<Bullet>();	
	}
	
	public void setColor(byte newColor) {
		if (newColor > 7)
		{
			newColor -= 8;	
		}
		LineChangeColor(gameObject, newColor);
	}
}
