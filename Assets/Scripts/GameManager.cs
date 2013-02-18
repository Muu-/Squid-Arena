using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager me;
	public static bool twoPlayersGame = false;
	public static bool playWithAIs = true;
	public static bool gameEnded = false;
	public static float timer = 8.2f;
	public static int timerCount = 7;
	public static float endTimer = 2.5f;
	public Transform bullet;
	public Transform bulletFolder;
	public GameObject nav;
	public static List<string> players = new List<string>();
	public static Transform staticCupPrefab;
	public Transform CupPrefab;
	public GameObject numberPrefab;
	
	// Use this for initialization
	void Awake () {
		NewGame();
		
		staticCupPrefab = CupPrefab;
		
		me = this;		
		List<Position> positions = MakePositions();
		
		GameObject tmp;
		PlayerControls pc;
		tmp = (GameObject) Instantiate(nav, positions[0]._position, Quaternion.identity);
		tmp.transform.rotation = positions[0]._rotation;
		tmp.name = "Player 1";
		players.Add(tmp.name);
		pc = tmp.AddComponent<PlayerControls>();
		pc.player = PlayerControls.Player.one;
		if (twoPlayersGame)
		{
			tmp = (GameObject) Instantiate(nav, positions[3]._position, Quaternion.identity);
			tmp.transform.rotation = positions[3]._rotation;
			tmp.name = "Player 2";
			players.Add(tmp.name);
			pc = tmp.AddComponent<PlayerControls>();
			pc.player = PlayerControls.Player.two;	
		}
		if (!twoPlayersGame || (twoPlayersGame && playWithAIs))
		{
			tmp = (GameObject) Instantiate(nav, positions[1]._position, Quaternion.identity);
			tmp.transform.rotation = positions[1]._rotation;
			tmp.name = "AI 1";
			players.Add(tmp.name);
			tmp.AddComponent<AIControls>();
			tmp = (GameObject) Instantiate(nav, positions[2]._position, Quaternion.identity);
			tmp.transform.rotation = positions[2]._rotation;
			tmp.name = "AI 2";
			players.Add(tmp.name);
			tmp.AddComponent<AIControls>();
		}
		if (!twoPlayersGame)
		{
			tmp = (GameObject) Instantiate(nav, positions[3]._position, Quaternion.identity);
			tmp.transform.rotation = positions[3]._rotation;
			tmp.name = "AI 3";
			players.Add(tmp.name);
			tmp.AddComponent<AIControls>();
		}
		
		bulletFolder = new GameObject("Bullets").transform;
		for (int i = 0; i<100; i++)
		{
			Bullet.BulletInstantiate((Vector3.forward * 2000) + Vector3.right * i * 10, Vector3.zero, 0);
		}
	}
	
	public void Start() {		
	}
	
	public void NewGame() {
		timer = 5.2f;
		timerCount = 5;
		endTimer = 2.5f;
		gameEnded = false;
		players = new List<string>();
		Bullet.Reset();
	}

	public void Update() {
		timer -= Time.deltaTime;
		int intTimer = (int) timer+1;
		if (timer <= timerCount && timer > 0)
		{
			timerCount--;
			TextManager.me.DrawText(intTimer.ToString(), -Vector2.up * 25, 0.9f, 36, "Game");
		}
		if (gameEnded == true)
		{
			endTimer -= Time.deltaTime;	
		}
	}
	
	private List<Position> MakePositions() {
		float distance = 165;
		List<Position> tmp = new List<Position>();
		
		if (Random.Range(0,100) > 50)
		{
			tmp.Add(new Position((Vector3.forward + Vector3.right) * distance, 45));
			tmp.Add(new Position((-Vector3.forward + Vector3.right) * distance, 135));
			tmp.Add(new Position((-Vector3.forward + -Vector3.right) * distance, 225));
			tmp.Add(new Position((Vector3.forward + -Vector3.right) * distance, 315));
		} else {
			tmp.Add(new Position(Vector3.forward * distance, 0));
			tmp.Add(new Position(Vector3.right * distance, 90));
			tmp.Add(new Position(-Vector3.forward * distance, 180));
			tmp.Add(new Position(-Vector3.right * distance, 270));
		}
		
		tmp = Shuffle(tmp);
		
		return tmp;
	}
	
	private List<Position> Shuffle(List<Position> list) {
		List<Position> tmp = new List<Position>();
		for (int i = 0; i < 4; i++)
		{
			int rr = Random.Range(0,3-i);
			tmp.Add(list[rr]);
			list.RemoveAt(rr);
		}
		return tmp;
	}
	
	public class Position {
		public Vector3 _position;
		public Quaternion _rotation;
		
		public Position(Vector3 position, float angle) {
			_position = position;
			_rotation = Quaternion.Euler(0,angle,0);
		}
	}
	
	public static void RemovePlayer(string objName) {
		if (players.Contains(objName))
		{
			players.Remove(objName);		
		}
		if (players.Count == 1)
		{
			Debug.Log("Game ends: " + players[0] + " wins.");
			gameEnded = true;
			TextManager.me.DrawText(players[0], new Vector2(- ((players[0].Length/2)*16), - 96), 30, 12, "Game");	
			TextManager.me.DrawText("WINS!", new Vector2(- 32, - 112), 30, 12, "Game");
			Instantiate(staticCupPrefab);
			
			GameObject tmp;
			Vector3 pos;
			pos = Camera.mainCamera.ScreenToWorldPoint(new Vector3(16, 4, Camera.mainCamera.transform.position.y - 10));
			tmp = TextManager.me.DrawText("Menu", new Vector2(pos.x, pos.z), 3600, 12, "Game");
			ButtonPosition.me.AddButton(tmp,ButtonPosition.Position.left);
			pos = Camera.mainCamera.ScreenToWorldPoint(new Vector3(Screen.width - 56, 4, Camera.mainCamera.transform.position.y - 10));
			tmp = TextManager.me.DrawText("Again", new Vector2(pos.x, pos.z), 3600, 12, "Game");
			ButtonPosition.me.AddButton(tmp, ButtonPosition.Position.right);
		}
	}
}