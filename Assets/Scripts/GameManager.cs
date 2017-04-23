using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private CanvasController cc;
    private TradeRouteController tc;
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
        tc = FindObjectOfType<TradeRouteController>();
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
        
        int randomIndex = Random.Range(0, provincesList.Count);
        
        provincesList[randomIndex].province.status = Status.Sad;
        provincesList.Remove(provincesList[randomIndex]);
        
        // randomIndex = Random.Range(0, provincesList.Count);
        
        // provincesList[randomIndex].province.status = Status.Sad;
        // provincesList.Remove(provincesList[randomIndex]);
        
        randomIndex = Random.Range(0, provincesList.Count);
        
        provincesList[Random.Range(0, provincesList.Count)].province.status = Status.Angry;
        provincesList.Remove(provincesList[randomIndex]);

    }

    public void ProduceResources() {

        cc.ResetEventSystem();

        Debug.Log("Producing resources");

        string text = "- " + province.name + " will produce " + province.production.ToString().ToLower() + "\n";

        cc.summary.text += text;

        ReduceProvinces();

    }

    public void InquireResources() {

        cc.ResetEventSystem();

        Debug.Log("Inquiring resources");
        
        TradeRoute selectedObjectTradeRoute = cc.selectedProvinceGameObject.GetComponentInChildren<TradeRoute>();

        Debug.Log("Object: " + selectedObjectTradeRoute.transform.name);
        
        Province closestProvince = tc.CalculateShortestTradeRoute(selectedObjectTradeRoute);

        string text = "- " + province.name + " will buy " + province.need.ToString().ToLower() + " from " + closestProvince.name + "\n";

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

        string text = "- " + province.name + " will call for diplomatic actions\n";

        cc.summary.text += text;

        ReduceProvinces();

    }

    public void IncrementTurn() {

        foreach(ProvinceController p in interactedProvinceList) {

            p.Highlight(false, 0.2f);

        }
        
        cc.summary.text = "";
        cc.canUpdate = true;

        interactedProvinceList.Clear();

        turn++;

    }

    public void ReduceProvinces() {
        
        Debug.Log(">>> Province " + province.name + " called action");

        if (cc.firstMove) {
            cc.firstMove = false;
        }

        ProvinceController pc = province.gameObject.GetComponent<ProvinceController>();

        pc.isExecutingAction = true;
        
        interactedProvinceList.Add(pc);
        
        tc.RemoveTradeRoute(pc.GetComponentInChildren<TradeRoute>());
        
        provincesLeftForInteraction--;
        
        cc.HideCitadelPanel();
        cc.ResetSelectedProvince();
        
        tc.ShowDiscontentTradeRoutes();

        if (provincesLeftForInteraction <= 0) {
            
            cc.canUpdate = false;
            cc.provincesLeft.text = "You've made your decisions, now click on the turn button";
            cc.selectedProvinceTextObject.text = "";
            cc.statusText.text = "";

        }

    }

    public void SetProvince(Province p) {

        this.province = p;

    }
    
    public void ExitTutorialScreen() {
        
        tc.ShowDiscontentTradeRoutes();
        
    }

}