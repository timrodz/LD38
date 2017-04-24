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
    public Action action = Action.Nothing;

    public void SetStatus(Status status) {
        this.status = status;
    }

    public void Update() {

        switch (status) {

            case Status.Happy:
                {
                    if (Random.Range(0, 2) == 0) {

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

                    if (Random.Range(0, 1) == 0) {

                        SetStatus(Status.Angry);
                        SellIncome();
                        Debug.Log(name + " is now in a sad mood");

                    }
                }
                break;
            case Status.Angry:
                {
                    SellIncome();
                }
                break;

        }

    }

    public void ProduceIncome() {

        stocks++;

    }

    public void SellIncome() {

        if (stocks > 0)
            stocks--;

    }

}