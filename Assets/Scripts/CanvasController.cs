using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    private TradeRouteController tc;

    public Transform provinceHolder;

    public Text selectedProvinceTextObject;
    private string originalText;

    [HeaderAttribute("Selection Panel")]
    public GameObject selectionPanel;
    private CanvasGroup selectionCG;
    [HideInInspector] public GameObject selectedProvinceGameObject = null;
    [HideInInspector] public bool hasSelectedProvince = false;
    private Province selectedProvince;
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

    // Others
    private float typeDelay = 0.0175f;

    [HideInInspector] public bool canUpdate = true;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        informationCG = informationPanel.GetComponent<CanvasGroup>();
        selectionCG = selectionPanel.GetComponent<CanvasGroup>();
        helpCG = helpPanel.GetComponent<CanvasGroup>();
        aboutCG = aboutPanel.GetComponent<CanvasGroup>();
        citadelCG = citadelPanel.GetComponent<CanvasGroup>();

        tc = FindObjectOfType<TradeRouteController>();

    }

    // Use this for initialization
    void Start() {

        originalText = "Explore Panama's provinces by clicking on them";

        selectionPanel.SetActive(true);
        informationPanel.SetActive(true);
        helpPanel.SetActive(true);
        aboutPanel.SetActive(true);
        citadelPanel.SetActive(true);

        HideSelectionPanel();
        HideInformationPanel();
        HideAboutPanel();
        HideCitadelPanel();

        helpButton.SetActive(false);

        DisplayHelpPanel();

    }

    public void SetCurrentSelectedProvince(Province p, bool hasSelectedProvince) {

        StopAllCoroutines();
        StartCoroutine(AnimateText(p.name));

        if (hasSelectedProvince) {

            this.hasSelectedProvince = true;
            selectedProvince = p;

            provinceHolder.DOMoveY(provinceHolder.position.y - 0.5f, 0.5f).SetEase(Ease.OutSine);

            DisplayInformationPanel();

            tc.ShowTradeRoutes();

        } else {

            this.hasSelectedProvince = false;
            selectedProvince = null;

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
                selectedProvinceGameObject.GetComponent<ProvinceController>().Highlight(false, 0.5f);
                selectedProvinceGameObject = null;

                provinceHolder.DOMoveY(provinceHolder.position.y + 0.5f, 0.5f).SetEase(Ease.OutSine);
                tc.HideTradeRoutes();

            }

            hasSelectedProvince = false;

            selectedProvince = null;

            EventSystem.current.SetSelectedGameObject(null);

        }

        selectedProvinceTextObject.text = originalText;

    }

    public void DisplaySelectionPanel() {

        selectionCG.DOFade(1, 0.5f);
        selectionCG.blocksRaycasts = true;

        canUpdate = true;

        ResetEventSystem();

    }

    public void HideSelectionPanel() {

        selectionCG.DOFade(0, 0.1f);
        selectionCG.blocksRaycasts = false;

    }

    public void DisplayInformationPanel() {

        if (!selectedProvinceGameObject)
            return;

        Capital.text = "Capital: " + selectedProvince.capital;

        Population.text = "Population: " + selectedProvince.population.ToString() + " habitants";

        Income.text = "Income: " + selectedProvince.monthlyIncome.ToString() + "$ USD";

        Production.text = "Produces: " + selectedProvince.production.ToString();

        CurrentNeed.text = "Needs: " + selectedProvince.currentNeed.ToString();

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
            selectedProvinceTextObject.text = originalText;
        } else {
            selectedProvinceTextObject.text = selectedProvince.name;

            ResetPanels();

        }

        helpCG.DOFade(0, 0);
        helpCG.blocksRaycasts = false;

    }

    public void DisplayAboutPanel() {

        canUpdate = false;

        selectionCG.blocksRaycasts = false;
        HideInformationPanel();

        selectedProvinceTextObject.text = "";
        aboutText.text = selectedProvince.gameObject.GetComponent<ProvinceController>().aboutText.text;

        aboutCG.DOFade(1, 0);
        aboutCG.blocksRaycasts = true;

    }

    public void HideAboutPanel() {
        
        selectionCG.blocksRaycasts = true;

        aboutCG.DOFade(0, 0);
        aboutCG.blocksRaycasts = false;
        
        ResetEventSystem();

    }

    public void DisplayCitadelPanel() {

        citadelCG.DOFade(1, 0.5f);

    }

    public void HideCitadelPanel() {

        citadelCG.DOFade(0, 0);
        citadelCG.blocksRaycasts = false;

    }

    private void ResetEventSystem() {

        EventSystem.current.SetSelectedGameObject(null);

    }

    public void Fade(CanvasGroup cg, float amount, float duration) {

        cg.DOFade(amount, duration);

    }

    private IEnumerator AnimateText(string _province) {

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
        }
        showingCitadel = false;

    }

}