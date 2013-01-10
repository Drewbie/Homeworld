using UnityEngine;
using System.Collections;

public class UnitProperties : MonoBehaviour {

	public int maxHealth;
	public int curHealth;
	public float percentHealth;
	
	void Update()
	{
		percentHealth = curHealth/maxHealth;
	}
	
	float GetPercentHealth()
	{
		return percentHealth;
	}
}
