using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIManager : MonoBehaviour {
	public static AIManager me;
	List<AIControls> AIs;
	Dictionary<Bullet, Dictionary<float, Vector2>> bulletMap = new Dictionary<Bullet, Dictionary<float, Vector2>>();
	
	// Use this for initialization
	void Start () {
		me = this;
		AIs = new List<AIControls>();
		if (GameManager.playWithAIs)
		{
			if (GameObject.Find("AI 1").GetComponent<AIControls>() != null)
				AIs.Add(GameObject.Find("AI 1").GetComponent<AIControls>());
			if (GameObject.Find("AI 2").GetComponent<AIControls>() != null)
				AIs.Add(GameObject.Find("AI 2").GetComponent<AIControls>());
			if (!GameManager.twoPlayersGame)
				if (GameObject.Find("AI 3").GetComponent<AIControls>() != null)
					AIs.Add(GameObject.Find("AI 3").GetComponent<AIControls>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.playWithAIs)
		{
			MakeBulletMap();
			CheckAIAgainstBullets();
			ShowDebugs();
		}
	}
	
	void MakeBulletMap() {
		bulletMap = new Dictionary<Bullet, Dictionary<float, Vector2>>();
		
		foreach (Bullet b in Bullet.activeBullets)
		{
			Dictionary<float, Vector2> tmp = new Dictionary<float, Vector2>();
			Vector3 start = b.rigidbody.position;
			Vector3 speed = b.rigidbody.velocity;
			float time = 0.05f;
			for (int i = 1; i<30; i++)
			{
				tmp.Add(time * i, new Vector2(start.x + (speed.x * time * i), start.z + (speed.z * time * i)));
			}
			bulletMap.Add(b, tmp);
		}	
	}
	
	void CheckAIAgainstBullets()
	{
		if (AIs.Count > 0)
		{
			for (int i = 0; i<AIs.Count; i++)
			{
				foreach (KeyValuePair<Bullet, Dictionary<float, Vector2>> k in bulletMap)
				{
					if (k.Key.owner != null && k.Key.owner != AIs[i].transform)
					{
						foreach (KeyValuePair<float, Vector2> kk in k.Value)
						{
							if (CheckPosition(i, kk.Key, kk.Value))
							{
								AIs[i].AddThreat(k.Key, kk.Value, kk.Key);
							}
						}
					}
				}
			}
		}
	}
	
	private bool CheckPosition(int AIIndex, float time, Vector2 position)
	{
		Vector2 tmp = new Vector2(AIs[AIIndex].rigidbody.position.x + AIs[AIIndex].rigidbody.velocity.x * time, AIs[AIIndex].rigidbody.position.z + AIs[AIIndex].rigidbody.velocity.z * time);
		if (Vector2.Distance(tmp, position) < 35)
			return true;
		return false;
	}
	
	private void ShowDebugs() {
		foreach(KeyValuePair<Bullet, Dictionary<float, Vector2>> k in bulletMap)
		{
			foreach(KeyValuePair<float, Vector2> kk in k.Value)
			{
				Debug.DrawRay(new Vector3(kk.Value.x, -50, kk.Value.y), Vector3.up*100);
			}
		}
	}
	
	public void RemoveBullet(Bullet toRemove)
	{
		if (GameManager.playWithAIs)
			foreach (AIControls ai in AIs)
				ai.RemoveThreat(toRemove);
		bulletMap.Remove(toRemove);	
	}
}
