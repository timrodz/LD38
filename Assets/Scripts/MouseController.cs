using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hit;
		
		Debug.DrawLine(Vector3.zero, Camera.main.ScreenPointToRay(Input.mousePosition).direction);
		
		if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
			
			Debug.Log("Hovering over " + hit.transform.name);
			
		}
		
	}
	
	
	
}
