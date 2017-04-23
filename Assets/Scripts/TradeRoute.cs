using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (LineRenderer))]
public class TradeRoute : MonoBehaviour {

    [HideInInspector] public Province province;
    private LineRenderer path;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        province = GetComponentInParent<ProvinceController>().province;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {

    }

    public void Connect(TradeRoute destination) {

    }

}