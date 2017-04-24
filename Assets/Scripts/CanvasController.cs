using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    [HideInInspector] public GameManager gm;

    private TradeRouteController tc;

    public Transform provinceHolder;

    public Text selectedProvinceTextObject;
    public Text statusText;
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
    public Image productionImage;
    public Image inquiryImage;
    private bool showingInformation = false;

    [HeaderAttribute("Help Panel")]
    public GameObject helpPanel;
    private CanvasGroup helpCG;
    public GameObject helpButton;

    [HeaderAttribute("About Panel")]
    public GameObject aboutPanel;
    private CanvasGroup aboutCG;
    public Text aboutText;
    private bool showingAbout = false;

    [HeaderAttribute("Citadel Panel")]
    public GameObject citadelPanel;
    private CanvasGroup citadelCG;
    public Image productionButton;
    public Image inquiryButton;
    public Image statusButton;
    public Text productionButtonText;
    public Text inquiryButtonText;
    public Text statusButtonText;
    private bool showingCitadel = false;

    [HeaderAttribute("Game Manager Panel")]
    public GameObject gameManagerPanel;
    private CanvasGroup gameManagerCG;
    public Text provincesLeft;
    public Text gameManagerSummaryText;
    public GameObject nextTurnButton;
    private bool showingGameManager = false;

    [HeaderAttribute("Summary Panel")]
    public GameObject summaryPanel;
    private CanvasGroup summaryCG;
    public Text summaryPanelText;
    public Text informationText;
    public Text cropsText;
    public Text cattleText;
    public Text potteryText;
    public Text seafoodText;
    public Text coffeeText;
    

    [HeaderAttribute("Production Images")]
    public Sprite cropsImg;
    public Sprite cattleImg;
    public Sprite potteryImg;
    public Sprite seafoodImg;
    public Sprite coffeeImg;

    // Others
    private float typeDelay = 0.0175f;

    public bool canUpdate = true;

    public bool firstMove = true;

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
        summaryCG = summaryPanel.GetComponent<CanvasGroup>();

        gm = FindObjectOfType<GameManager>();
        tc = FindObjectOfType<TradeRouteController>();

    }

    // Use this for initialization
    void Start() {

        selectedProvinceTextObject.gameObject.SetActive(true);
        statusText.gameObject.SetActive(true);

        selectionPanel.SetActive(true);
        informationPanel.SetActive(true);
        helpPanel.SetActive(true);
        aboutPanel.SetActive(true);
        citadelPanel.SetActive(true);
        gameManagerPanel.SetActive(true);
        nextTurnButton.SetActive(false);
        summaryPanel.SetActive(true);

        HideSelectionPanel();
        HideInformationPanel();
        HideAboutPanel();
        HideCitadelPanel();
        HideGameManagerPanel();
        HideSummaryPanel();

        helpButton.SetActive(false);

        DisplayHelpPanel();

        selectedProvinceTextObject.fontSize = 60;
        originalText = "Explore Panama's provinces by clicking on them";

    }

    public void SetCurrentSelectedProvince(Province p, bool hasSelectedProvince) {

        currentProvince = p;
        ShowProvinceInformation();

        if (hasSelectedProvince) {

            this.hasSelectedProvince = true;
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

        StopAllCoroutines();

        if (hasSelectedProvince) {

            HideSelectionPanel();
            HideInformationPanel();

            if (selectedProvinceGameObject) {

                Debug.Log("De-highlighting selected province: " + selectedProvinceGameObject.name);

                if (!selectedProvinceGameObject.GetComponent<ProvinceController>().isExecutingAction) {

                    selectedProvinceGameObject.GetComponent<ProvinceController>().Highlight(false, 0.5f);

                }

                selectedProvinceGameObject = null;

                provinceHolder.DOMoveY(provinceHolder.position.y + 0.5f, 0.5f);

                if (!firstMove) {
                    DisplayGameManagerPanel();
                    tc.ShowTradeRoutes();
                }

            }

            hasSelectedProvince = false;

            currentProvince = null;

            EventSystem.current.SetSelectedGameObject(null);

        }

        selectedProvinceTextObject.fontSize = 60;
        selectedProvinceTextObject.text = originalText;

        statusText.text = "";

    }

    public void DisplaySelectionPanel() {

        selectionCG.DOFade(1, 0.5f);
        selectionCG.blocksRaycasts = true;

        canUpdate = true;

        ShowProvinceInformation();

        tc.ShowTradeRoutes();

        ResetEventSystem();

    }

    public void DisplaySelectionPanelNoTextAnimation() {

        selectionCG.DOFade(1, 0.5f);
        selectionCG.blocksRaycasts = true;

        canUpdate = true;

        StopAllCoroutines();

        selectedProvinceTextObject.text = currentProvince.name;

        switch (currentProvince.status) {
            case Status.Happy:
                statusText.text = "citizens are happy";
                break;
            case Status.Normal:
                statusText.text = "citizens are bored";
                break;
            case Status.Sad:
                statusText.text = "citizens are sad";
                break;
            case Status.Angry:
                statusText.text = "citizens are angry";
                break;
        }

        ResetEventSystem();

    }

    public void HideSelectionPanel() {

        if (!showingCitadel && canUpdate) {

            if (firstMove) {

                tc.ShowDiscontentTradeRoutes();

            } else {

                tc.ShowTradeRoutes();

            }

        } else {

            showingCitadel = false;

        }

        selectionCG.DOFade(0, 0.1f);
        selectionCG.blocksRaycasts = false;

    }

    public void DisplayInformationPanel() {

        if (!selectedProvinceGameObject)
            return;

        Capital.text = "Province capital: " + currentProvince.capital;

        // Population.text = "Population: " + currentProvince.population.ToString() + " habitants";

        Income.text = "Resources for sale: " + currentProvince.stocks.ToString();

        switch (currentProvince.production) {

            case Trade.Crops:
                productionImage.sprite = cropsImg;
                productionButton.sprite = cropsImg;
                break;
            case Trade.Cattle:
                productionImage.sprite = cattleImg;
                productionButton.sprite = cattleImg;
                break;
            case Trade.Pottery:
                productionImage.sprite = potteryImg;
                productionButton.sprite = potteryImg;
                break;
            case Trade.Seafood:
                productionImage.sprite = seafoodImg;
                productionButton.sprite = seafoodImg;
                break;
            case Trade.Coffee:
                productionImage.sprite = coffeeImg;
                productionButton.sprite = coffeeImg;
                break;

        }

        switch (currentProvince.inquiry) {

            case Trade.Crops:
                inquiryImage.sprite = cropsImg;
                inquiryButton.sprite = cropsImg;
                break;
            case Trade.Cattle:
                inquiryImage.sprite = cattleImg;
                inquiryButton.sprite = cattleImg;
                break;
            case Trade.Pottery:
                inquiryImage.sprite = potteryImg;
                inquiryButton.sprite = potteryImg;
                break;
            case Trade.Seafood:
                inquiryImage.sprite = seafoodImg;
                inquiryButton.sprite = seafoodImg;
                break;
            case Trade.Coffee:
                inquiryImage.sprite = coffeeImg;
                inquiryButton.sprite = coffeeImg;
                break;

        }

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
        tc.CalculateShortestTradeRoute(selectedObjectTradeRoute);

        ResetEventSystem();

    }

    public void DisplayHelpPanel() {

        Debug.Log("Help Panel");
        
        canUpdate = false;

        showingSelection = showingInformation = showingAbout = showingCitadel = showingGameManager = false;

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

        if (gameManagerCG.alpha == 1 && !showingGameManager) {

            Debug.Log("Game Manager was shown");
            showingGameManager = true;
            HideGameManagerPanel();

        }

        helpButton.SetActive(false);

        selectedProvinceTextObject.text = "";
        statusText.text = "";

        helpCG.DOFade(1, 0);
        helpCG.blocksRaycasts = true;
        
        canUpdate = false;
        
        tc.HideTradeRoutes();

    }

    public void HideHelpPanel() {

        canUpdate = true;
        helpButton.SetActive(true);

        if (!selectedProvinceGameObject) {

            selectedProvinceTextObject.fontSize = 60;
            selectedProvinceTextObject.text = originalText;

            if (firstMove) {

                tc.ShowDiscontentTradeRoutes();

            } else {

                tc.ShowTradeRoutes();

            }

        } else {
            
            if (!showingAbout) {
                tc.ShowTradeRoutes();
            }
            
            ResetPanels();

        }

        helpCG.DOFade(0, 0);
        helpCG.blocksRaycasts = false;

    }

    public void DisplayAboutPanel() {

        StopAllCoroutines();

        canUpdate = false;

        HideSelectionPanel();
        HideInformationPanel();

        tc.HideTradeRoutes();

        selectedProvinceTextObject.text = "";
        statusText.text = "";
        aboutText.text = currentProvince.gameObject.GetComponent<ProvinceController>().aboutText.text;

        aboutCG.DOFade(1, 0);
        aboutCG.blocksRaycasts = true;

    }

    public void HideAboutPanel() {

        // canUpdate = true;

        aboutCG.DOFade(0, 0);
        aboutCG.blocksRaycasts = false;

        ResetEventSystem();

    }

    public void DisplayCitadelPanel() {

        canUpdate = false;

        showingCitadel = true;

        EnableCitadelOption(productionButton);
        EnableCitadelOption(inquiryButton);
        EnableCitadelOption(statusButton);

        productionButtonText.text = "Produce " + currentProvince.production.ToString().ToLower() + " for the next season";

        inquiryButton.raycastTarget = true;

        int provincesThatProduceNeed = 0;
        bool foundProvincesThatProduceNeed = tc.FoundProvincesThatProduceNeed(currentProvince.inquiry, out provincesThatProduceNeed);

        if (!foundProvincesThatProduceNeed) {

            DisableCitadelOption(inquiryButton);

            if (provincesThatProduceNeed > 0) {
                inquiryButtonText.text = "Provinces that produce " + currentProvince.inquiry.ToString().ToLower() + " are unavailable";
            } else {
                inquiryButtonText.text = "There are no provinces with stocks of " + currentProvince.inquiry.ToString().ToLower();
            }

        } else {

            inquiryButtonText.text = "Make an inquiry of " + currentProvince.inquiry.ToString().ToLower();

        }

        statusButton.raycastTarget = true;

        string stringStatus = "";

        switch (currentProvince.status) {

            case Status.Happy:
                {
                    DisableCitadelOption(statusButton);
                    stringStatus = "The town is already celebrating on their own";
                }
                break;
            case Status.Normal:
                {
                    stringStatus = "Attend to minor issues";
                }
                break;
            case Status.Sad:
                {
                    DisableCitadelOption(productionButton);
                    productionButtonText.text = currentProvince.production.ToString() + " producers are not motivated to work";
                    stringStatus = "Organize a festival to cheer up the vibes";
                }
                break;
            case Status.Angry:
                {
                    DisableCitadelOption(productionButton);
                    DisableCitadelOption(inquiryButton);

                    stringStatus = "Scheme a subtle solution to avoid a coup";
                    productionButtonText.text = currentProvince.production.ToString() + " producers are striking";
                    inquiryButtonText.text = "The mayor is not in a position to arrange trades";
                }
                break;

        }

        statusButtonText.text = stringStatus;

        citadelCG.DOFade(1, 0.5f);
        citadelCG.blocksRaycasts = true;

    }

    public void HideCitadelPanel() {

        canUpdate = true;

        tc.ShowTradeRoutes();

        citadelCG.DOFade(0, 0);
        citadelCG.blocksRaycasts = false;

    }

    public void DisplayGameManagerPanel() {

        gameManagerCG.DOFade(1, 0.5f);

        provincesLeft.text = "Provinces left this turn: " + gm.provincesLeft.ToString();

    }

    public void HideGameManagerPanel() {

        gameManagerCG.DOFade(0, 0);

    }

    public void DisplaySummaryPanel() {

        summaryCG.DOFade(1, 0.5f);
        summaryCG.blocksRaycasts = true;
        
        cropsText.text = "Produced: " + gm.NumberOfItemsProduced(Trade.Crops).ToString() + "\nSold: " + gm.NumberOfItemsSold(Trade.Crops).ToString();
        cattleText.text = "Produced: " + gm.NumberOfItemsProduced(Trade.Cattle).ToString() + "\nSold: " + gm.NumberOfItemsSold(Trade.Cattle).ToString();
        potteryText.text = "Produced: " + gm.NumberOfItemsProduced(Trade.Pottery).ToString() + "\nSold: " + gm.NumberOfItemsSold(Trade.Pottery).ToString();
        seafoodText.text = "Produced: " + gm.NumberOfItemsProduced(Trade.Seafood).ToString() + "\nSold: " + gm.NumberOfItemsSold(Trade.Seafood).ToString();
        coffeeText.text = "Produced: " + gm.NumberOfItemsProduced(Trade.Coffee).ToString() + "\nSold: " + gm.NumberOfItemsSold(Trade.Coffee).ToString();
        
        informationText.text = "";
        informationText.text += "Year: " + gm.year + "\nTurn: " + gm.turn;

    }

    public void HideSummaryPanel() {

        summaryCG.DOFade(0, 0);
        summaryCG.blocksRaycasts = false;

    }

    // ------------------------------------------------------------------------------------------

    public void EnableUpdate(float delay) {

        StopAllCoroutines();
        StartCoroutine(ResetUpdateDelay(delay));

    }

    private IEnumerator ResetUpdateDelay(float delay) {

        canUpdate = false;
        yield return new WaitForSeconds(delay);
        Debug.Log("-----    Enabling update after " + delay + " seconds");
        canUpdate = true;

    }

    public void ResetEventSystem() {

        EventSystem.current.SetSelectedGameObject(null);

    }

    public void Fade(CanvasGroup cg, float amount, float duration) {

        cg.DOFade(amount, duration);

    }

    private IEnumerator AnimateText(string source, Text destination, int size) {

        destination.fontSize = size;
        destination.text = "";

        if (destination == statusText) {

            yield return new WaitForSeconds(0.2f);
            destination.DOFade(0, 0.3f).From();
            destination.text = source;

        } else {

            for (int i = 0; i < source.Length; i++) {

                destination.text += source[i];
                yield return new WaitForSeconds(typeDelay);

            }
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
            ShowProvinceInformation();
        }
        showingCitadel = false;

        if (gameManagerCG.alpha == 0 && showingGameManager) {
            Debug.Log("Showing game manager");
            DisplayGameManagerPanel();
        }
        showingGameManager = false;

    }

    public void EnableCitadelOption(Image button) {

        button.raycastTarget = true;
        button.DOFade(1, 0);

    }

    public void DisableCitadelOption(Image button) {

        button.raycastTarget = false;
        button.DOFade(0.5f, 0);

    }

    public void ShowProvinceInformation() {

        if (currentProvince == null)
            return;

        StopAllCoroutines();

        Province p = currentProvince;

        StartCoroutine(AnimateText(p.name, selectedProvinceTextObject, 80));

        switch (p.status) {
            case Status.Happy:
                StartCoroutine(AnimateText("citizens are happy", statusText, 40));
                break;
            case Status.Normal:
                StartCoroutine(AnimateText("citizens are bored", statusText, 40));
                break;
            case Status.Sad:
                StartCoroutine(AnimateText("citizens are sad", statusText, 40));
                break;
            case Status.Angry:
                StartCoroutine(AnimateText("citizens are angry", statusText, 40));
                break;
        }

    }

}