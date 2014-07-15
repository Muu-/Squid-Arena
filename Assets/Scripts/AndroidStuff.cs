using UnityEngine;
using System.Collections;

public class AndroidStuff : MonoBehaviour {
	void Awake() {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Rotate screen
		if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft && Screen.orientation != ScreenOrientation.LandscapeLeft)
		{
			Camera.main.transform.position = new Vector3(0, 600, 0);
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
		if (Input.deviceOrientation == DeviceOrientation.LandscapeRight && Screen.orientation != ScreenOrientation.LandscapeRight)
		{
			Camera.main.transform.position = new Vector3(0, 600, 0);
			Screen.orientation = ScreenOrientation.LandscapeRight;
		}
		if (Input.deviceOrientation == DeviceOrientation.Portrait && Screen.orientation != ScreenOrientation.Portrait)
		{
			Camera.main.transform.position = new Vector3(0, 450, 0);
			Screen.orientation = ScreenOrientation.Portrait;
		}
		if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown && Screen.orientation != ScreenOrientation.PortraitUpsideDown)
		{
			Camera.main.transform.position = new Vector3(0, 450, 0);
			Screen.orientation = ScreenOrientation.PortraitUpsideDown;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (Application.loadedLevelName == "PlayScene")
				Application.LoadLevel("StartMenu");
			if (Application.loadedLevelName == "StartMenu")
				Application.Quit();
		}
	}
}
