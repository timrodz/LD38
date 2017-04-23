using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private CanvasController cc;
    private ProvinceController pc;

    public int turn = 0;

    public int provincesLeftForInteraction = 3;

    private Province province;

    private List<ProvinceController> provincesList = new List<ProvinceController>();
    private List<ProvinceController> interactedProvinceList = new List<ProvinceController>();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {
        
        cc = FindObjectOfType<CanvasController>();
        pc = FindObjectOfType<ProvinceController>();
        
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        
        ProvinceController[] provinceArray = FindObjectsOfType<ProvinceController>();
        
        for (int i = 0; i < provinceArray.Length; i++) {
            
            provincesList.Add(provinceArray[i]);
            
        }

    }

    public void ProduceResources() {

        cc.ResetEventSystem();

        Debug.Log("Producing resources");

        string text = province.name + " will produce " + province.production.ToString().ToLower() + "\n";

        cc.summary.text += text;

        ReduceProvinces();

    }

    public void InquireResources() {

        cc.ResetEventSystem();

        Debug.Log("Inquiring resources");

        string text = province.name + " will inquire " + province.need.ToString().ToLower() + "\n";

        cc.summary.text += text;

        ReduceProvinces();

    }

    public void ResolveMood() {

        cc.ResetEventSystem();

        if (province.status == Status.Happy) {
            Debug.Log("Province is already happy");
            return;
        }

        Debug.Log("Resolving mood");

        string text = province.name + " will call for diplomatic actions\n";

        cc.summary.text += text;

        ReduceProvinces();

    }

    public void IncrementTurn() {

        foreach(ProvinceController p in interactedProvinceList) {

            p.Highlight(false, 0.2f);

        }

        cc.summary.text = "";

        interactedProvinceList.Clear();

        turn++;

    }

    public void ReduceProvinces() {

        if (cc.firstMove) {
            cc.firstMove = false;
        }

        ProvinceController pc = province.gameObject.GetComponent<ProvinceController>();

        pc.isExecutingAction = true;
        
        interactedProvinceList.Add(pc);
        
        provincesLeftForInteraction--;
        
        cc.HideCitadelPanel();
        cc.ResetSelectedProvince();

        if (provincesLeftForInteraction <= 0) {

            cc.provincesLeft.text = "Wait for next turn";

        }

    }

    public void SetProvince(Province p) {

        this.province = p;

    }

}