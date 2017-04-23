using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SkyboxController : MonoBehaviour {

    public Material skybox;

    public bool lerp = false;
    private bool hasChanged = false;

    public Color topColor1, bottomColor1;
    public Color topColor2, bottomColor2;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (lerp) {
            ColorLerp();
        } else {
            if (!hasChanged) {
                skybox.SetColor("_Color2", topColor1);
                skybox.SetColor("_Color1", bottomColor1);
                hasChanged = true;
            }
        }

    }

    public void ColorLerp() {
		
		hasChanged = false;

        Color lerpedTopColor = Color.Lerp(topColor1, topColor2, Mathf.PingPong(Time.time * 0.1f, 1));
        Color lerpedBottomColor = Color.Lerp(bottomColor1, bottomColor2, Mathf.PingPong(Time.time * 0.1f, 1));

        skybox.SetColor("_Color1", lerpedBottomColor);
        skybox.SetColor("_Color2", lerpedTopColor);

    }

}