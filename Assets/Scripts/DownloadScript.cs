using UnityEngine;
using System.Collections;

// This script is not in use, looks like Android does not cache the audio file, wasting a lot of time and bandwidth.
// I shall probably try later with Unity Pro and assetBundles.
public class DownloadScript : MonoBehaviour {
	private string musicUrl = "";
	private WWW musicDownload = null;
	
	// Use this for initialization
	void Start() {
		musicUrl = "removed";
		Debug.Log("Downloading music at: " + musicUrl);	
		StartCoroutine(DownloadSong());
    }
	
	// Update is called once per frame
	void OnGUI () {
		if (musicDownload != null)
			GUILayout.Label("Downloading \n" + (musicDownload.progress * 100) + "%", GUILayout.MaxWidth(100));
	}
	
	IEnumerator DownloadSong()
	{	
        musicDownload = new WWW(musicUrl);
        yield return musicDownload;
		
		if (string.IsNullOrEmpty(musicDownload.error))
		{
			Debug.Log((musicDownload.size / 1024) + " kbytes downloaded");
			AudioClip ac = musicDownload.GetAudioClip(false, false);
			Jukebox.me.SetSong(ac);
			Jukebox.me.Play();
		} else {
			Debug.Log("Cannot download file: " + musicDownload.error);	
		}
	}
	
	void Update() {
		if (musicDownload != null && musicDownload.progress == 1)
		{
			if (Input.touchCount >= 1 || Input.anyKeyDown)
			{
				Application.LoadLevel("StartMenu");	
			}
		}
	}
}
	