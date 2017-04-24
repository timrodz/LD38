using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ProvinceController : MonoBehaviour {

    private CanvasController cc;

    public Province province;

    private SpriteRenderer sprite;

    private Sequence colorFade;

    [HideInInspector] public Text aboutText;

    [HideInInspector] public bool isExecutingAction = false;

    private bool isHighlighted = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        cc = FindObjectOfType<CanvasController>();

        sprite = GetComponent<SpriteRenderer>();

        aboutText = GetComponent<Text>();

    }

    // Use this for initialization
    void Start() {

        province.gameObject = this.gameObject;
        province.tradeRoute = GetComponentInChildren<TradeRoute>();

    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown() {

        if (!cc.canUpdate || isExecutingAction)
            return;

        if (!cc.selectedProvinceGameObject) {

            Debug.Log("Interacting with " + province.name + " - Status: " + province.status.ToString());

            cc.selectedProvinceGameObject = this.gameObject;

            cc.SetCurrentSelectedProvince(province, true);

            cc.DisplaySelectionPanelNoTextAnimation();

            sprite.DOFade(0.35f, 0.2f);

        }

    }

    /// <summary>
    /// Called every frame while the mouse is over the GUIElement or Collider.
    /// </summary>
    void OnMouseOver() {

        if (isHighlighted)
            return;

        if (cc.hasSelectedProvince || !cc.canUpdate || isExecutingAction)
            return;

        Highlight(true, 0.2f);

    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit() {

        if (cc.hasSelectedProvince || !cc.canUpdate || isExecutingAction)
            return;

        Highlight(false, 0.2f);

        

    }

    public void Highlight(bool highlight, float duration) {

        colorFade.Kill();

        if (highlight) {

            isHighlighted = true;

            cc.SetCurrentSelectedProvince(province, false);

            cc.gm.sm.Play();

            colorFade.Append(sprite.DOFade(0.5f, duration));

        } else {

            isHighlighted = false;
            
            cc.ResetSelectedProvince();
            
            colorFade.Append(sprite.DOFade(1, duration));

        }

    }

}