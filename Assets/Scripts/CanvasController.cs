using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    private GameManager gm;

    private TradeRouteController tc;

    public Transform provinceHolder;

    public Text selectedProvinceTextObject;
    private string originalText;

    [HeaderAttribute("Selection Panel")]
    public GameObject selectionPanel;
    private CanvasGroup selectionCG;
    [HideInInspector] public GameObject selectedProvinceGameObject = null;
    [HideInInspector] public bool hasSelectedProvince = false;
    public Province currentProvince;
    private bool showingSelection = false;

    [HeaderAttribute("Information Panel")]
    public GameObject informationPanel;
    private CanvasGroup informationCG;
    public Text Capital;
    public Text Population;
    public Text Income;
    public Text Production;
    public Text CurrentNeed;
    public Text currentStatus;
    private bool showingInformation = false;

    [HeaderAttribute("Help Panel")]
    public GameObject helpPanel;
    private CanvasGroup helpCG;
    public GameObject helpButton;

    [HeaderAttribute("About Panel")]
    public GameObject aboutPanel;
    private CanvasGroup aboutCG;
    public TextMeshProUGUI aboutText;
    private bool showingAbout = false;

    [HeaderAttribute("Citadel Panel")]
    public GameObject citadelPanel;
    private CanvasGroup citadelCG;
    private bool showingCitadel = false;
    public Image productionImage;
    public Image needImage;
    public Image statusImage;
    public Text productionText;
    public Text inquiryText;
    public Text statusText;

    [HeaderAttribute("Game Manager Panel")]
    public GameObject gameManagerPanel;
    private CanvasGroup gameManagerCG;
    public Text provincesLeft;
    public Text summary;

    [HeaderAttribute("Next Turn Panel")]
    public GameObject nextTurnPanel;
    private CanvasGroup nextTurnCG;

    [HeaderAttribute("Production Images")]
    public Sprite cropsImg;
    public Sprite cattleImg;
    public Sprite potteryImg;
    public Sprite seafoodImg;
    public Sprite coffeeImg;

    // Others
    private float typeDelay = 0.0175f;

    [HideInInspector] public bool canUpdate = true;

    [HideInInspector] public bool firstMove = true;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        informationCG = informationPanel.GetComponent<CanvasGroup>();
        selectionCG = selectionPanel.GetComponent<CanvasGroup>();
        helpCG = helpPanel.GetComponent<CanvasGroup>();
        aboutCG = aboutPanel.GetComponent<CanvasGroup>();
        citadelCG = citadelPanel.GetComponent<CanvasGroup>();
        gameManagerCG = gameManagerPanel.GetComponent<CanvasGroup>();
        nextTurnCG = nextTurnPanel.GetComponent<CanvasGroup>();

        gm = FindObjectOfType<GameManager>();
        tc = FindObjectOfType<TradeRouteController>();

    }

    // Use this for initialization
    void Start() {

        selectedProvinceTextObject.fontSize = 60;
        originalText = "Explore Panama's provinces by clicking on them";

        selectionPanel.SetActive(true);
        informationPanel.SetActive(true);
        helpPanel.SetActive(true);
        aboutPanel.SetActive(true);
        citadelPanel.SetActive(true);
        gameManagerPanel.SetActive(true);
        nextTurnPanel.SetActive(true);

        HideSelectionPanel();
        HideInformationPanel();
        HideAboutPanel();
        HideCitadelPanel();
        HideGameManagerPanel();
        HideNextTurnPanel();

        helpButton.SetActive(false);

        DisplayHelpPanel();

    }

    public void SetCurrentSelectedProvince(Province p, bool hasSelectedProvince) {

        StopAllCoroutines();
        StartCoroutine(AnimateText(p.name));

        if (hasSelectedProvince) {

            this.hasSelectedProvince = true;
            currentProvince = p;
            gm.SetProvince(p);

            provinceHolder.DOMoveY(provinceHolder.position.y - 0.5f, 0.5f).SetEase(Ease.OutSine);

            DisplayInformationPanel();
            HideGameManagerPanel();

            tc.ShowTradeRoutes();

        } else {

            this.hasSelectedProvince = false;
            currentProvince = null;

        }

    }

    public void ResetSelectedProvince() {

        Debug.Log("Resetting selected province");

        StopAllCoroutines();

        HideSelectionPanel();
        HideInformationPanel();

        if (hasSelectedProvince) {

            if (selectedProvinceGameObject) {

                Debug.Log("De-highlighting selected province: " + selectedProvinceGameObject.name);

                if (!selectedProvinceGameObject.GetComponent<ProvinceController>().isExecutingAction) {

                    selectedProvinceGameObject.GetComponent<ProvinceController>().Highlight(false, 0.5f);

                }

                selectedProvinceGameObject = null;

                provinceHolder.DOMoveY(provinceHolder.position.y + 0.5f, 0.5f).SetEase(Ease.OutSine);
                tc.HideTradeRoutes();

                if (!firstMove) {
                    DisplayGameManagerPanel();
                }

            }

            hasSelectedProvince = false;

            currentProvince = null;

            EventSystem.current.SetSelectedGameObject(null);

        }

        selectedProvinceTextObject.fontSize = 60;
        selectedProvinceTextObject.text = originalText;

    }

    public void DisplaySelectionPanel() {

        selectionCG.DOFade(1, 0.5f);
        selectionCG.blocksRaycasts = true;

        canUpdate = true;

        StopAllCoroutines();

        StartCoroutine(AnimateText(currentProvince.name));

        ResetEventSystem();

    }

    public void DisplaySelectionPanelNoTextAnimation() {

        selectionCG.DOFade(1, 0.5f);
        selectionCG.blocksRaycasts = true;

        canUpdate = true;

        StopAllCoroutines();

        selectedProvinceTextObject.text = currentProvince.name;

        ResetEventSystem();

    }

    public void HideSelectionPanel() {

        selectionCG.DOFade(0, 0.1f);
        selectionCG.blocksRaycasts = false;

    }

    public void DisplayInformationPanel() {

        if (!selectedProvinceGameObject)
            return;

        Capital.text = "Capital: " + currentProvince.capital;

        Population.text = "Population: " + currentProvince.population.ToString() + " habitants";

        Income.text = "Income: " + currentProvince.monthlyIncome.ToString() + "$ USD";

        Production.text = "Produces: " + currentProvince.production.ToString();

        CurrentNeed.text = "Needs: " + currentProvince.need.ToString();

        informationCG.DOFade(1, 0.5f);

        ResetEventSystem();

    }

    public void HideInformationPanel() {

        informationCG.DOFade(0, 0.1f);
        informationCG.blocksRaycasts = false;

    }

    public void CalculateRoute() {

        Debug.Log("Calculating route");

        TradeRoute selectedObjectTradeRoute = selectedProvinceGameObject.GetComponentInChildren<TradeRoute>();

        Debug.Log("Object: " + selectedObjectTradeRoute.transform.name);
        tc.SendTrade(selectedObjectTradeRoute);

        ResetEventSystem();

    }

    public void DisplayHelpPanel() {

        Debug.Log("Help Panel");

        showingSelection = showingInformation = showingAbout = showingCitadel = false;

        if (selectionCG.alpha == 1 && !showingSelection) {

            Debug.Log("Selection was shown");
            showingSelection = true;
            HideSelectionPanel();

        }

        if (informationCG.alpha == 1 && !showingInformation) {

            Debug.Log("Information was shown");
            showingInformation = true;
            HideInformationPanel();

        }

        if (aboutCG.alpha == 1 && !showingAbout) {

            Debug.Log("About was shown");
            showingAbout = true;
            HideAboutPanel();

        }

        if (citadelCG.alpha == 1 && !showingCitadel) {

            Debug.Log("Citadel was shown");
            showingCitadel = true;
            HideCitadelPanel();

        }

        canUpdate = false;
        helpButton.SetActive(false);

        selectedProvinceTextObject.text = "";

        helpCG.DOFade(1, 0);
        helpCG.blocksRaycasts = true;

    }

    public void HideHelpPanel() {

        canUpdate = true;
        helpButton.SetActive(true);

        if (!selectedProvinceGameObject) {
            selectedProvinceTextObject.fontSize = 60;
            selectedProvinceTextObject.text = originalText;
        } else {

            ResetPanels();

        }

        helpCG.DOFade(0, 0);
        helpCG.blocksRaycasts = false;

    }

    public void DisplayAboutPanel() {

        canUpdate = false;

        HideSelectionPanel();
        HideInformationPanel();

        selectedProvinceTextObject.text = "";
        aboutText.text = currentProvince.gameObject.GetComponent<ProvinceController>().aboutText.text;

        aboutCG.DOFade(1, 0);
        aboutCG.blocksRaycasts = true;

    }

    public void HideAboutPanel() {

        canUpdate = true;

        selectionCG.blocksRaycasts = true;

        aboutCG.DOFade(0, 0);
        aboutCG.blocksRaycasts = false;

        if (selectedProvinceGameObject) {

            StopAllCoroutines();
            StartCoroutine(AnimateText(currentProvince.name));

        }

        ResetEventSystem();

    }

    public void DisplayCitadelPanel() {

        canUpdate = false;

        HideSelectionPanel();
        HideInformationPanel();

        productionText.text = "Produce " + currentProvince.production.ToString().ToLower() + " for the next season";
        inquiryText.text = "Make an inquiry of " + currentProvince.need.ToString().ToLower() + "";
        
        statusImage.raycastTarget = true;
        string stringStatus = "";

        switch (currentProvince.status) {

            case Status.Happy:
                statusImage.raycastTarget = false;
                stringStatus = "Happiness can be found everywhere!";
                break;
            case Status.Normal:
                stringStatus = "Minor issues could be resolved";
                break;
            case Status.Sad:
                stringStatus = "Sadness spreads due to recent events";
                break;
            case Status.Angry:
                stringStatus = "Anger curses your streets, actions required!";
                break;

        }

        statusText.text = stringStatus;

        switch (currentProvince.production) {

            case Trade.Crops:
                productionImage.sprite = cropsImg;
                break;
            case Trade.Cattle:
                productionImage.sprite = cattleImg;
                break;
            case Trade.Pottery:
                productionImage.sprite = potteryImg;
                break;
            case Trade.Seafood:
                productionImage.sprite = seafoodImg;
                break;
            case Trade.Coffee:
                productionImage.sprite = coffeeImg;
                break;

        }

        switch (currentProvince.need) {

            case Trade.Crops:
                needImage.sprite = cropsImg;
                break;
            case Trade.Cattle:
                needImage.sprite = cattleImg;
                break;
            case Trade.Pottery:
                needImage.sprite = potteryImg;
                break;
            case Trade.Seafood:
                needImage.sprite = seafoodImg;
                break;
            case Trade.Coffee:
                needImage.sprite = coffeeImg;
                break;

        }

        citadelCG.DOFade(1, 0.5f);
        citadelCG.blocksRaycasts = true;

    }

    public void HideCitadelPanel() {

        canUpdate = true;

        citadelCG.DOFade(0, 0);
        citadelCG.blocksRaycasts = false;

    }

    public void DisplayGameManagerPanel() {

        gameManagerCG.DOFade(1, 0.5f);

        provincesLeft.text = "Provinces left this turn: " + gm.provincesLeftForInteraction.ToString();

    }

    public void HideGameManagerPanel() {

        gameManagerCG.DOFade(0, 0);

    }

    public void DisplayNextTurnPanel() {

        nextTurnCG.DOFade(1, 0.5f);
        nextTurnCG.blocksRaycasts = true;

        provincesLeft.text = "Provinces left this turn: " + gm.provincesLeftForInteraction.ToString();

    }

    public void HideNextTurnPanel() {

        nextTurnCG.DOFade(0, 0);
        nextTurnCG.blocksRaycasts = false;

    }

    // ------------------------------------------------------------------------------------------

    public void ResetEventSystem() {

        EventSystem.current.SetSelectedGameObject(null);

    }

    public void Fade(CanvasGroup cg, float amount, float duration) {

        cg.DOFade(amount, duration);

    }

    private IEnumerator AnimateText(string _province) {

        selectedProvinceTextObject.fontSize = 80;
        selectedProvinceTextObject.text = "";

        for (int i = 0; i < _province.Length; i++) {

            selectedProvinceTextObject.text += _province[i];
            yield return new WaitForSeconds(typeDelay);

        }

    }

    public void ResetPanels() {

        if (selectionCG.alpha == 0 && showingSelection) {
            Debug.Log("Showing Selection");
            DisplaySelectionPanel();
        }
        showingSelection = false;

        if (informationCG.alpha == 0 && showingInformation) {
            Debug.Log("Showing Information");
            DisplayInformationPanel();
        }
        showingInformation = false;

        if (aboutCG.alpha == 0 && showingAbout) {
            Debug.Log("Showing About");
            DisplayAboutPanel();
        }
        showingAbout = false;

        if (citadelCG.alpha == 0 && showingCitadel) {
            Debug.Log("Showing Citadel");
            DisplayCitadelPanel();
            StartCoroutine(AnimateText(currentProvince.name));
        }
        showingCitadel = false;

    }

}