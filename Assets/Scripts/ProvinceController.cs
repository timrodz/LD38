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

    private bool hasHovered = false;

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

    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown() {

        if (!cc.canUpdate)
            return;

        if (!cc.selectedProvinceGameObject) {

            Debug.Log("Interacting with " + province.name + " - Status: " + province.status.ToString());
            cc.selectedProvinceGameObject = this.gameObject;
            
            cc.SetCurrentSelectedProvince(province, true);
            cc.DisplaySelectionPanelNoTextAnimation();

            // colorFade.Kill();
            colorFade.Append(sprite.DOFade(0.25f, 0.2f));

        }

    }

    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    /// </summary>
    void OnMouseEnter() {

        if (cc.hasSelectedProvince || !cc.canUpdate)
            return;

        Debug.Log(">> Hovering over " + province.name);
        Highlight(true, 0.2f);

        cc.SetCurrentSelectedProvince(province, false);

    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit() {

        if (cc.hasSelectedProvince || !cc.canUpdate)
            return;

        Debug.Log("<< Finish hovering " + province.name);
        Highlight(false, 0.2f);

        cc.ResetSelectedProvince();

    }

    public void Highlight(bool highlight, float duration) {

        // colorFade.Kill();

        if (highlight) {
            colorFade.Append(sprite.DOFade(0.5f, duration));
        } else {
            colorFade.Append(sprite.DOFade(1, duration));
        }

    }

}