//Attach this script to anythign you wish to show a health bar for

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
	//This array contains values between 0 and 1.
	//Each value will be a unique bar.
	public float[] values;
	HealthBarManager manager;
	
	void Update ()
	{
		values [0] = gameObject.GetComponent<UnitProperties> ().percentHealth;
	}
	
	
	//To change how health bars are displayed, implement your hide/show logic here.
	//The functions below simply turn a health bar on or off when the mouse moves over
	//the game object (as long as a collider is attached!)
	void OnMouseEnter ()
	{
		HealthBarManager.Instance.Show (this);
	}		
	
	void OnMouseExit ()
	{
		HealthBarManager.Instance.Hide (this);
	}

	
	
	
}
