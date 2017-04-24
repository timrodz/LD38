using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private CanvasController cc;
    private TradeRouteController tc;
    public SoundManager sm;

    public int turn = 0;
    public int year = 2017;

    public int provincesPerTurn = 5;
    [HideInInspector] public int provincesLeft;

    private Province province;

    private List<ProvinceController> provincesList = new List<ProvinceController> ();
    private List<ProvinceController> interactedProvinceList = new List<ProvinceController> ();

    private bool gameOver;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        cc = FindObjectOfType<CanvasController> ();
        tc = FindObjectOfType<TradeRouteController> ();

    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {

        ProvinceController[] provinceArray = FindObjectsOfType<ProvinceController> ();

        for (int i = 0; i < provinceArray.Length; i++) {

            provincesList.Add(provinceArray[i]);

            int happiness = 0;

            switch (provincesList[i].province.status) {

                case Status.Happy:
                    happiness = 500;
                    break;
                case Status.Normal:
                    happiness = 350;
                    break;
                case Status.Sad:
                    happiness = 150;
                    break;
                case Status.Angry:
                default:
                    break;

            }

            provincesList[i].province.happiness = happiness;

        }

        int randomIndex = Random.Range(0, provincesList.Count);

        provincesList[randomIndex].province.SetStatus(Status.Angry);

        DetermineProvinceFate();

        cc.summaryPanelText.text = "";

        tc.UpdateSprites();

    }

    public bool GameOver() {

        int numOfAngryProvinces = 0;
        int numOfSadProvinces = 0;

        foreach(ProvinceController p in provincesList) {

            if (p.province.status == Status.Angry) {
                numOfAngryProvinces++;
            } else if (p.province.status == Status.Sad) {
                numOfSadProvinces++;
            }

        }
        
        return (numOfAngryProvinces >= 3 && numOfSadProvinces >= 2);

    }

    public void DetermineProvinceFate() {

        provincesLeft = provincesPerTurn;

        Debug.Log("Interacted province list size: " + interactedProvinceList.Count);

        foreach(ProvinceController p in interactedProvinceList) {

            Debug.Log(">>>> Performing action for " + p.province.name);

            switch (p.province.action) {

                case Action.Production:
                    p.province.IncreaseIncome();
                    break;
                case Action.Inquiry:
                    p.province.closestTradeRoute.SellAvailableStock();
                    break;
                case Action.Diplomacy:
                    p.province.ResolveStatus();
                    cc.summaryPanelText.text += "- " + p.province.name + "'s citizens now have a " + p.province.status.ToString().ToLower() + " mood!\n\n";
                    break;

            }

            // p.province.action = Action.Nothing;

        }

        foreach(ProvinceController p in provincesList) {

            p.isExecutingAction = false;

            p.province.UpdateStatus();

            string provinceSummary = "";

            // Update the summary if the item is not in the interacted list
            if (interactedProvinceList.IndexOf(p) == -1) {

                if (p.province.action == Action.Nothing) {

                    switch (p.province.status) {

                        case Status.Happy:
                            provinceSummary = "- Happiness and joy spread in " + p.province.name;
                            break;
                        case Status.Normal:
                            provinceSummary = "- Boredom is starting to rule over " + p.province.name + "'s citizens";
                            break;
                        case Status.Sad:
                            provinceSummary = "- A wave of sadness has hit " + p.province.name + " due to recent events";
                            break;
                        case Status.Angry:
                            provinceSummary = "- Citizens of " + p.province.name + " are planning a revolt, stop them!";
                            break;

                    }

                }

                cc.summaryPanelText.text += provinceSummary + "\n\n";

            }

        }
        
        if (GameOver()) {
            Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>> Game over");
        }

    }

    public void IncrementTurn() {

        turn++;

        if (turn % 4 == 0) {
            year++;
        }

        cc.nextTurnButton.SetActive(false);

        cc.HideGameManagerPanel();

        foreach(ProvinceController p in interactedProvinceList) {

            p.Highlight(false, 0.2f);

        }

        DetermineProvinceFate();

        provincesLeft = provincesPerTurn;

        cc.gameManagerSummaryText.text = "";

        cc.DisplaySummaryPanel();

        tc.HideTradeRoutes();

    }

    public void ResumeGame() {

        interactedProvinceList.Clear();

        cc.HideSummaryPanel();

        cc.DisplayGameManagerPanel();

        tc.ResetTradeRoutes();
        tc.ShowTradeRoutes();

        cc.summaryPanelText.text = "";

        cc.EnableUpdate(0);

    }

    public void ReduceProvinceAvailable() {

        Debug.Log(">>> Province " + province.name + " called action");

        provincesLeft--;

        if (cc.firstMove) {
            cc.firstMove = false;
        }

        ProvinceController pc = province.gameObject.GetComponent<ProvinceController> ();

        pc.isExecutingAction = true;

        interactedProvinceList.Add(pc);

        tc.RemoveTradeRoute(pc.GetComponentInChildren<TradeRoute> ());

        cc.HideCitadelPanel();
        cc.ResetSelectedProvince();
        cc.canUpdate = false;

        if (provincesLeft <= 0) {

            cc.provincesLeft.text = "Your choices will have outcomes on the next turn";
            cc.selectedProvinceTextObject.text = "";
            cc.statusText.text = "";
            cc.nextTurnButton.SetActive(true);

        } else {

            cc.EnableUpdate(0.5f);

        }

    }

    public void SetProvince(Province p) {

        this.province = p;

    }

    // Buttons

    public void ProduceResources() {

        cc.ResetEventSystem();

        Debug.Log("Producing resources");

        province.action = Action.Production;

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

        province.action = Action.Inquiry;

        TradeRoute selectedObjectTradeRoute = cc.selectedProvinceGameObject.GetComponentInChildren<TradeRoute> ();

        Debug.Log("Object: " + selectedObjectTradeRoute.transform.name);

        Province closestProvince = tc.CalculateShortestTradeRoute(selectedObjectTradeRoute);

        province.closestTradeRoute = closestProvince;

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

        province.action = Action.Diplomacy;

        string text = "- " + province.name + " will " + cc.statusButtonText.text.ToLower() + "\n";

        // cc.summaryPanelText.text += text + "\n";

        cc.gameManagerSummaryText.text += text;

        ReduceProvinceAvailable();

    }

    public int NumberOfItemsProduced(Trade t) {

        int result = 0;

        foreach(ProvinceController p in provincesList) {

            if (p.province.production == t && p.province.action == Action.Production) {

                Debug.Log(p.province.name + " produced " + t.ToString());
                result += p.province.tradeProduced;
                p.province.tradeProduced = 0;
                p.province.action = Action.Nothing;

            }

        }

        Debug.Log("Produced " + t.ToString() + ": " + result);
        return result;

    }

    public int NumberOfItemsSold(Trade t) {

        int result = 0;

        foreach(ProvinceController p in provincesList) {

            if (p.province.inquiry == t && p.province.action == Action.Inquiry) {

                Debug.Log(p.province.name + " sold " + t.ToString());
                result += p.province.tradeSold;
                p.province.tradeSold = 0;
                p.province.action = Action.Nothing;

            }

        }

        Debug.Log("Sold " + t.ToString() + ": " + result);
        return result;

    }

}