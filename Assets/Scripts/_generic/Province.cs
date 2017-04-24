using UnityEngine;

[System.SerializableAttribute]
public class Province {

    [HideInInspector] public GameObject gameObject;
    public string name;
    public string capital;
    public int population;
    public int stocks = 0;
    public Trade production;
    public Trade inquiry;
    public Status status = Status.Happy;
    public int happiness = 500;
    public TradeRoute tradeRoute;
    public Province closestTradeRoute = null;
    public Action action = Action.Nothing;

    public void SetStatus(Status status) {
        this.status = status;
    }

    public void UpdateStatus() {

        switch (status) {

            case Status.Happy:
                {
                    if (Random.Range(0, 5) == 0) {

                        SetStatus(Status.Normal);
                        Debug.Log(name + " is now in a normal mood");

                    }
                }
                break;
            case Status.Normal:
                {
                    if (Random.Range(0, 3) == 0) {

                        SetStatus(Status.Sad);
                        Debug.Log(name + " is now in a sad mood");

                    }
                }
                break;
            case Status.Sad:
                {

                    if (Random.Range(0, 2) == 0) {

                        SetStatus(Status.Angry);
                        SellAvailableStock();
                        Debug.Log(name + " is now in a sad mood");

                    }
                }
                break;
            case Status.Angry:
                {
                    SellAvailableStock();
                }
                break;

        }

    }

    public void IncreaseIncome() {

        Debug.Log("Producing for " + name);
        stocks++;

    }

    public void SellAvailableStock() {

        if (stocks > 0) {
            Debug.Log("Selling stock from " + name);
            stocks--;
        }

    }

    public void ResolveStatus() {

        Debug.Log("Resolving mood for " + name);

        switch (status) {

            case Status.Happy:
                {
                    Debug.Log("Province is already happy");
                }
                break;
            case Status.Normal:
                {
                    SetStatus(Status.Happy);
                }
                break;
            case Status.Sad:
                {
                    SetStatus(Status.Normal);
                }
                break;
            case Status.Angry:
                {
                    int randomChance = Random.Range(0, 2);
                    if (randomChance == 0) {

                        SetStatus(Status.Sad);

                    } else {

                        SetStatus(Status.Normal);

                    }
                }
                break;

        }

    }
    
    public void RandomizeInquiry() {
        
        do  {
            
            inquiry = (Trade) Random.Range(0, 5);
            
        } while (inquiry == production);
        
    }

}