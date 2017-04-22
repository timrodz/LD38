using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.SerializableAttribute]
public class Province {

	[HideInInspector] public GameObject gameObject;
	public string name;
	public string capital;
	public int population;
	public float monthlyIncome;
	public Status status;
	public Trade production;
	public Trade currentNeed;
	
}
