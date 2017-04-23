using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TradeRouteController : MonoBehaviour {

    [HeaderAttribute("Trade Routes")]
    public TradeRoute bocasDelToro;
    public TradeRoute NgabeBugle;
    public TradeRoute Chiriqui;
    public TradeRoute Veraguas;
    public TradeRoute LosSantos;
    public TradeRoute Herrera;
    public TradeRoute Cocle;
    public TradeRoute Panama;
    public TradeRoute Colon;
    public TradeRoute GunaYala;
    public TradeRoute Darien;
    public TradeRoute EmberaWounaan;

    [HideInInspector] public List<TradeRoute> tradeRoutes = new List<TradeRoute>();
    private List<Transform> needList = new List<Transform>();
    private List<TradeRoute> path = new List<TradeRoute>();

    // Use this for initialization
    void Start() {

        tradeRoutes.Add(bocasDelToro);
        tradeRoutes.Add(NgabeBugle);
        tradeRoutes.Add(Chiriqui);
        tradeRoutes.Add(Veraguas);
        tradeRoutes.Add(LosSantos);
        tradeRoutes.Add(Herrera);
        tradeRoutes.Add(Cocle);
        tradeRoutes.Add(Panama);
        tradeRoutes.Add(Colon);
        tradeRoutes.Add(GunaYala);
        tradeRoutes.Add(Darien);
        tradeRoutes.Add(EmberaWounaan);
		
		foreach (TradeRoute t in tradeRoutes) {
			t.transform.localScale = Vector3.zero;
		}

    }

    // Update is called once per frame
    void Update() {

    }

    public void SendTrade(TradeRoute origin) {

        Debug.Log("Looking for provinces that produce need: " + origin.province.need);

        needList.Clear();

        foreach(TradeRoute t in tradeRoutes) {
			
            if (origin.province.need == t.province.production) {

                Debug.Log("Province that produces need: " + t.province.name);
                needList.Add(t.transform);

            }

        }

        // Get the current path to be the shortest
        Transform shortestPath = needList[0];

        for (int i = 0; i < needList.Count; i++) {

            if (i + 1 >= needList.Count)
                break;

            float distance1 = Vector3.Distance(origin.transform.position, shortestPath.position);

            if (needList.Count > 1) {

                float distance2 = Vector3.Distance(origin.transform.position, needList[i + 1].position);

                if (distance1 > distance2) {
                    shortestPath = needList[i + 1];
                    Debug.Log("Found new shortest path: " + shortestPath.parent.name);
                }

            }

        }
		
		// shortestPath.DOShakeRotation(0.5f);
		// shortestPath.DORotate(shortestPath.eulerAngles, 0.1f).SetDelay(0.5f);
		Debug.Log("Shortest path: " + shortestPath.parent.name);

    }

	public void ShowTradeRoutes() {
		
		foreach (TradeRoute t in tradeRoutes) {
			t.transform.DOScale(1, 0.5f);
		}
		
	}
	
	public void HideTradeRoutes() {
		
		foreach (TradeRoute t in tradeRoutes) {
			t.transform.DOScale(0, 0.5f);
		}
		
	}

}