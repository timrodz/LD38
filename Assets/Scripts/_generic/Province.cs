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
    public int happiness = 0;
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
                }
                break;
            case Status.Normal:
                {
					
                }
                break;
            case Status.Sad:
                {
					monthlyIncome -= (Random.Range(150, 300));
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