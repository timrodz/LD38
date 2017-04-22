using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProvinceController : MonoBehaviour {

    private CanvasController cc;

    public Province province;

    private SpriteRenderer sprite;

    private Sequence colorFade;

    private bool hasHovered = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        cc = FindObjectOfType<CanvasController> ();

        sprite = GetComponent<SpriteRenderer> ();

    }

    // Use this for initialization
    void Start() {

        province.gameObject = this.gameObject;

    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown() {

        if (!cc.selectedProvinceGameObject) {
            
            Debug.Log("Interacting with " + province.name + " - Status: " + province.status.ToString());
            cc.selectedProvinceGameObject = this.gameObject;
            cc.SetCurrentSelectedProvince(province, true);
            cc.DisplaySelectionPanel();
            
            colorFade.Kill();
            colorFade.Append(sprite.DOFade(0.35f, 0.2f));

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

        colorFade.Kill();

        if (highlight) {
            colorFade.Append(sprite.DOFade(0.5f, duration));
        } else {
            colorFade.Append(sprite.DOFade(1, duration));
        }

    }

}