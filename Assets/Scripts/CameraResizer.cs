using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResizer : MonoBehaviour {

	public float fWidth = 39.0f;

    public void LateUpdate() {
        // Orthographic size is 1/2 of the vertical size seen by the camera. 
        Camera.main.orthographicSize = (int) (fWidth * Screen.height / Screen.width * 0.5f);
    }
	
}
