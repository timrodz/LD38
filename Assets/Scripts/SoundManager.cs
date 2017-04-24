using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	
	[HeaderAttribute("Audio Sources")]
	public AudioSource sfx;
	public AudioSource music;
	
	[HeaderAttribute("Sound Effects")]
	public AudioClip click;
	public AudioClip select;

	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0)) {
			
			sfx.PlayOneShot(select);
			
		}
		
	}
	
	public void Play() {
		
		sfx.PlayOneShot(click);
		
	}
	
}