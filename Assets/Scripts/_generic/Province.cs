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

    public int tradeProduced = 0;
    public int tradeSold = 0;

    public void SetStatus(Status status) {
        this.status = status;
    }

    public void UpdateStatus() {

        // Very happy
        if (happiness >= 450) {

            switch (status) {
                case Status.Normal:
                    {
                        SetStatus(Status.Happy);
                    }
                    break;
                case Status.Sad:
                    {
                        SetStatus(Status.Happy);
                    }
                    break;
                case Status.Angry:
                    {
                        SetStatus(Status.Normal);
                    }
                    break;

            }

        }
        // Content
        else if (happiness >= 300 && happiness < 450) {

            switch (status) {

                case Status.Happy:
                    if (Random.Range(0, 2) == 0) {
                        SetStatus(Status.Normal);
                    }
                    break;
                case Status.Normal:
                    {
                        if (Random.Range(0, 3) == 0) {

                            SetStatus(Status.Happy);

                        }
                    }
                    break;
                case Status.Sad:
                    {
                        SetStatus(Status.Normal);
                    }
                    break;
                case Status.Angry:
                    {
                        SetStatus(Status.Sad);
                    }
                    break;

            }

        } else if (happiness >= 150 && happiness < 300) {

            switch (status) {

                case Status.Happy:
                    {
                        SetStatus(Status.Normal);
                    }
                    break;
                case Status.Normal:
                    {
                        SetStatus(Status.Sad);
                    }
                    break;
                case Status.Sad:
                    {
                        if (Random.Range(0, 3) == 0) {
                            SetStatus(Status.Normal);
                        }
                    }
                    break;
                case Status.Angry:
                    {
                        if (Random.Range(0, 5) == 0) {

                            SetStatus(Status.Sad);

                        }
                    }
                    break;

            }

        } else if (happiness < 150) {
            
            switch (status) {

            case Status.Happy:
                {
                    SetStatus(Status.Sad);
                }
                break;
            case Status.Normal:
                {
                    SetStatus(Status.Sad);
                }
                break;
            case Status.Sad:
                {
                    SetStatus(Status.Angry);
                }
                break;
            case Status.Angry:
                {

                }
                break;

        }

        }

        // Final happiness subtraction
        switch (status) {

            case Status.Happy:
                {
                    happiness += 75;
                }
                break;
            case Status.Normal:
                {
                    
                }
                break;
            case Status.Sad:
                {
                    happiness -= 50;
                }
                break;
            case Status.Angry:
                {
                    happiness -= 100;
                }
                break;

        }

    }

    public void IncreaseIncome() {

        Debug.Log("Producing for " + name);

        if (status == Status.Happy) {
            stocks += 2;
            tradeProduced += 2;
            happiness -= 25;
        } else {
            stocks++;
            tradeProduced++;
            happiness -= 50;
        }

    }

    public void SellAvailableStock(Province p) {

        if (p.stocks > 0) {
            Debug.Log("Selling stock from " + p.name);
            p.stocks--;
            p.tradeSold++;
        }

        happiness += 75;

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
                    Debug.Log("Happy");
                    SetStatus(Status.Happy);
                    happiness += 25;
                }
                break;
            case Status.Sad:
                {
                    Debug.Log("Normal");
                    SetStatus(Status.Normal);
                    happiness += 75;
                }
                break;
            case Status.Angry:
                {
                    int randomChance = Random.Range(0, 2);
                    if (randomChance == 0) {

                        Debug.Log("Sad");
                        SetStatus(Status.Sad);
                        happiness += 50;

                    } else {

                        Debug.Log("Normal");
                        SetStatus(Status.Normal);
                        happiness += 50;

                    }
                }
                break;

        }

    }

    public void RandomizeInquiry() {

        do {

            inquiry = (Trade) Random.Range(0, 5);

        } while (inquiry == production);

    }

}