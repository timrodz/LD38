using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TradeRouteController : MonoBehaviour {

    [HeaderAttribute("Status images")]
    public Sprite sadSprite;
    public Sprite angrySprite;

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
    private List<TradeRoute> needList = new List<TradeRoute>();
    private List<TradeRoute> productionList = new List<TradeRoute>();

    // Use this for initialization
    void Start() {

        ResetTradeRoutes();

    }

    public Province CalculateShortestTradeRoute(TradeRoute origin) {

        Debug.Log("Looking for provinces that produce need: " + origin.province.need);

        needList.Clear();

        foreach(TradeRoute t in tradeRoutes) {

            if (origin.province.need == t.province.production) {

                Debug.Log("Province that produces need: " + t.province.name);
                needList.Add(t);

            }

        }

        // Get the current path to be the shortest
        TradeRoute shortestPath = needList[0];

        for (int i = 0; i < needList.Count; i++) {

            if (i + 1 >= needList.Count)
                break;

            float distance1 = Vector3.Distance(origin.transform.localPosition, shortestPath.transform.localPosition);

            if (needList.Count > 1) {

                float distance2 = Vector3.Distance(origin.transform.position, needList[i + 1].transform.localPosition);

                if (distance1 > distance2) {
                    shortestPath = needList[i + 1];
                    Debug.Log("Found new shortest path: " + shortestPath.transform.parent.name);
                }

            }

        }

        Debug.Log("Shortest path: " + shortestPath.transform.parent.name);

        // origin.Connect(shortestPath);

        tradeRoutes.Remove(shortestPath);

        return (shortestPath.GetComponentInParent<ProvinceController>().province);

    }

    public void ShowTradeRoutes() {

        foreach(TradeRoute t in tradeRoutes) {
            switch (t.province.status) {

                case Status.Happy:
                    {
                        t.ResetSprite();
                    }
                    break;
                case Status.Normal:
                    {
                        t.ResetSprite();
                    }
                    break;
                case Status.Sad:
                    {
                        t.SetSprite(sadSprite);
                    }
                    break;
                case Status.Angry:
                    {
                        t.SetSprite(angrySprite);
                    }
                    break;

            }
            t.transform.DOScale(1, 0.5f);
        }

    }

    public void ShowDiscontentTradeRoutes() {

        foreach(TradeRoute t in tradeRoutes) {

            if (t.province.status != Status.Happy && t.province.status != Status.Normal) {

                switch (t.province.status) {

                    case Status.Sad:
                        t.SetSprite(sadSprite);
                        break;
                    case Status.Angry:
                        t.SetSprite(angrySprite);
                        break;

                }

                t.transform.DOScale(1, 0.5f);

            }

        }

    }

    public void HideTradeRoutes() {

        foreach(TradeRoute t in tradeRoutes) {
            t.transform.DOScale(0, 0.5f);
        }

    }

    public void RemoveTradeRoute(TradeRoute tr) {

        tradeRoutes.Remove(tr);

    }

    public void ResetTradeRoutes() {

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

        foreach(TradeRoute t in tradeRoutes) {
            t.transform.localScale = Vector3.zero;
        }

    }

    public bool FoundProvincesThatProduceNeed(Trade trade) {

        productionList.Clear();

        foreach(TradeRoute t in tradeRoutes) {

            if (t.province.production == trade) {

                productionList.Add(t);

            }

        }

        return (productionList.Count > 0);

    }

}