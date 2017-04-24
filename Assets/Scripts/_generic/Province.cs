using UnityEngine;

[System.SerializableAttribute]
public class Province {

    [HideInInspector] public GameObject gameObject;
    public string name;
    public string capital;
    public int population;
    public float monthlyIncome;
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
                    if (action == Action.Production) {
                        monthlyIncome += (Random.Range(50, 100));
                    }
					else if (action == Action.Inquiry) {
						
					}

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
                    monthlyIncome -= (Random.Range(150, 300));

                    if (Random.Range(0, 1) == 0) {

                        SetStatus(Status.Angry);

                    }
                }
                break;
            case Status.Angry:
                {
                    monthlyIncome -= (Random.Range(450, 650));
                }
                break;

        }

    }

}