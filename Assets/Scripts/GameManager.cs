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
        
    }

    public void DetermineProvinceFate() {

        foreach(ProvinceController p in provincesList) {

            p.isExecutingAction = false;

            // Determine the new moods
            switch (p.province.status) {

                case Status.Happy:
                    {
                        if (Random.Range(0, 2) == 0) {

                            p.province.SetStatus(Status.Normal);
                            Debug.Log(p.province.name + " is now in a normal mood");

                        }
                    }
                    break;
                case Status.Normal:
                    if (Random.Range(0, 3) == 0) {

                        p.province.SetStatus(Status.Sad);
                        Debug.Log(p.province.name + " is now in a sad mood");

                    }
                    break;
                case Status.Sad:
                    {
                        if (Random.Range(0, 1) == 0) {

                            p.province.SetStatus(Status.Angry);

                        }

                    }
                    break;
                case Status.Angry:
                    {

                    }
                    break;

            }

        }

    }

    public void IncrementTurn() {

        cc.nextTurnButton.SetActive(false);

        foreach(ProvinceController p in interactedProvinceList) {

            p.Highlight(false, 0.2f);

        }

        DetermineProvinceFate();

        provincesLeftForInteraction = 3;

        cc.summary.text = "";
        cc.canUpdate = true;
        cc.DisplayGameManagerPanel();

        interactedProvinceList.Clear();

        tc.ResetTradeRoutes();
        tc.ShowTradeRoutes();

        turn++;

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

        tc.ShowDiscontentTradeRoutes();

        if (provincesLeftForInteraction <= 0) {

            cc.canUpdate = false;
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

        string text = "- " + province.name + " will produce " + province.production.ToString().ToLower() + "\n";

        cc.summary.text += text;

        ReduceProvinceAvailable();

    }

    public void InquireResources() {

        cc.ResetEventSystem();

        Debug.Log("Inquiring resources");

        TradeRoute selectedObjectTradeRoute = cc.selectedProvinceGameObject.GetComponentInChildren<TradeRoute>();

        Debug.Log("Object: " + selectedObjectTradeRoute.transform.name);

        Province closestProvince = tc.CalculateShortestTradeRoute(selectedObjectTradeRoute);

        string text = "- " + province.name + " will buy " + province.inquiry.ToString().ToLower() + " from " + closestProvince.name + "\n";

        cc.summary.text += text;

        ReduceProvinceAvailable();

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

        ReduceProvinceAvailable();

    }

}