using UnityEngine;

[System.SerializableAttribute]
public class Province {

	[HideInInspector] public GameObject gameObject;
	public string name;
	public string capital;
	public int population;
	public float monthlyIncome;
	public Trade production;
	public Trade need;
	[HideInInspector] public Status status = Status.Happy;
	[HideInInspector] public int happiness = 0;
	
}
