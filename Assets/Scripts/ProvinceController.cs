using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvinceController : MonoBehaviour {

    public Province information;

    // Use this for initialization
    void Start() {

        information.gameObject = this.gameObject;

    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown() {
		
		Debug.Log("Interacting with " + information.name + " - Status: " + information.status.ToString());

    }

}