using UnityEngine;
using System.Collections;

public class Jukebox : MonoBehaviour {
	public static Jukebox me;
	public static bool muted = false;
	
	// Use this for initialization
	void Awake () {
		me = this;
		DontDestroyOnLoad (gameObject);
		Play();
	}
	
	public void Play() {
		audio.Play();	
	}
	
	public void Stop() {
		audio.Stop();
	}
	
	public void SetSong(AudioClip clip){
		audio.clip = clip;
	}
	
	public void MuteOnOff()
	{
		muted = !muted;
		switch (muted){
		case true:
			Stop();
			muted = true;
			break;
		case false:
			Play();
			muted = false;
			break;
		}
	}
}
