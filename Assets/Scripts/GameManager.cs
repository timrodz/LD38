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

        provincesList[randomIndex].province.SetStatus(Status.Angry);

        DetermineProvinceFate();
        
        cc.summaryPanelText.text = "";

    }

    public void DetermineProvinceFate() {

        foreach(ProvinceController p in provincesList) {

            p.isExecutingAction = false;

            p.province.Update();

            string provinceSummary = "- " + p.province.name;
            
            // Update the summary if the item is not in the interacted list
            if (interactedProvinceList.IndexOf(p) == -1) {

                switch (p.province.action) {

                    case Action.Nothing:
                        provinceSummary += " did not arrange any plans for next season";
                        break;
                    case Action.Production:
                        provinceSummary += " will produce " + p.province.production.ToString().ToLower() + " for the upcoming season";
                        break;
                    case Action.Inquiry:
                        provinceSummary += " will inquire " + p.province.production.ToString().ToLower() + " for the upcoming season";
                        break;
                    case Action.Diplomacy:
                        provinceSummary += " will produce " + p.province.production.ToString().ToLower() + " for the upcoming season";
                        break;

                }

                cc.summaryPanelText.text += provinceSummary + "\n\n";

            }

        }
        
        tc.UpdateSprites();

    }

    public void IncrementTurn() {

        cc.nextTurnButton.SetActive(false);

        foreach(ProvinceController p in interactedProvinceList) {

            p.Highlight(false, 0.2f);

        }

        DetermineProvinceFate();

        provincesLeftForInteraction = 3;

        cc.gameManagerSummaryText.text = "";
        
        cc.DisplaySummaryPanel();

        interactedProvinceList.Clear();

        turn++;

    }
    
    public void ResumeGame() {
        
        cc.DisplayGameManagerPanel();
        
        cc.canUpdate = true;
        
        tc.ResetTradeRoutes();
        tc.ShowTradeRoutes();
        
        cc.summaryPanelText.text = "";
        
    }

    public void ReduceProvinceAvailable() {

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
        cc.EnableUpdate(0.5f);

        if (provincesLeftForInteraction <= 0) {
            
            cc.provincesLeft.text = "Your choices will have outcomes on the next turn";
            cc.selectedProvinceTextObject.text = "";
            cc.statusText.text = "";
            cc.nextTurnButton.SetActive(true);

        }

    }

    public void SetProvince(Province p) {

        this.province = p;

    }

    // Buttons

    public void ProduceResources() {

        cc.ResetEventSystem();

        Debug.Log("Producing resources");

        string text = "- " + province.name;
        
        switch (province.production) {  
                
            case Trade.Crops:
            text += " will cultivate and harvest ";
            break;
            case Trade.Cattle:
            text += " will raise and develop ";
            break;
            case Trade.Pottery:
            text += " will craft fine ";
            break;
            case Trade.Seafood:
            text += " will fish for available ";
            break;
            case Trade.Coffee:
            text += " will produce ";
            break;
            
        }
        
        text += province.production.ToString().ToLower() + "\n";

        cc.summaryPanelText.text += text + "\n";

        cc.gameManagerSummaryText.text += text;

        ReduceProvinceAvailable();

    }

    public void InquireResources() {

        cc.ResetEventSystem();

        Debug.Log("Inquiring resources");

        TradeRoute selectedObjectTradeRoute = cc.selectedProvinceGameObject.GetComponentInChildren<TradeRoute>();

        Debug.Log("Object: " + selectedObjectTradeRoute.transform.name);

        Province closestProvince = tc.CalculateShortestTradeRoute(selectedObjectTradeRoute);

        string text = "- " + province.name + " will acquire " + province.inquiry.ToString().ToLower() + " from " + closestProvince.name + "\n";

        cc.summaryPanelText.text += text + "\n";

        cc.gameManagerSummaryText.text += text;

        ReduceProvinceAvailable();

    }

    public void ResolveMood() {

        cc.ResetEventSystem();

        if (province.status == Status.Happy) {
            Debug.Log("Province is already happy");
            return;
        }

        Debug.Log("Resolving mood");

        string text = "- " + province.name + " will " + cc.statusButtonText.text.ToLower() + "\n";

        cc.summaryPanelText.text += text + "\n";

        cc.gameManagerSummaryText.text += text;

        ReduceProvinceAvailable();

    }

}