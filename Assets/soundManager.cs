using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour {
	
	[HeaderAttribute("Audio Sources")]
	public AudioSource sfx;
	public AudioSource music;
	
	[HeaderAttribute("Sound Effects")]
	public AudioClip click;
	public AudioClip select;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0)) {
			
			sfx.PlayOneShot(click);
			
		}
		
	}
	
}