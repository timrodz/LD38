using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class TradeRoute : MonoBehaviour {

    [HideInInspector] public Province province;
    private LineRenderer path;

    private float animationTime = 2;

    [HideInInspector] public Sprite sprite;
    [HideInInspector] public Sprite inquirySprite;
    [HideInInspector] public Sprite originalSprite;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        path = GetComponent<LineRenderer>();
        sprite = GetComponent<Sprite>();
        province = GetComponentInParent<ProvinceController>().province;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {

        originalSprite = sprite;

    }

    public void Connect(TradeRoute destination) {

        path.transform.LookAt(destination.transform);

        Vector3 line = destination.transform.position;

        Debug.Log("Line located in " + line);

        path.SetPosition(1, destination.transform.localPosition);

        // StartCoroutine(AnimateLine(destination.transform.position));

    }

    /// <summary>
    /// Animate a line from the transform to the trade route
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator AnimateLine(Vector3 target) {

        for (float t = 0; t <= 1; t += (Time.deltaTime / animationTime)) {

            Vector3 temp = target;

            temp.Scale(Vector3.one * t);

            temp.z = -0.1f;

            path.SetPosition(1, temp);
            yield return null;

        }

    }

    public void SetSprite(Sprite p) {

        sprite = p;

    }
    
    public void ResetSprite() {
        
        sprite = originalSprite;
        
    }

}