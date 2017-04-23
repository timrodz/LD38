using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private CanvasController cc;
	private ProvinceController pc;

    private int turn = 0;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        cc = FindObjectOfType<CanvasController>();
		pc = FindObjectOfType<ProvinceController>();
	}

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void IncrementTurn() {

        turn++;

    }

}