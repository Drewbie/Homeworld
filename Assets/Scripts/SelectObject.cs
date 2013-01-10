using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectObject : MonoBehaviour
{
	
	private UnitManager unitManger;
	
	void Start ()
	{
		unitManger = GameObject.FindGameObjectWithTag ("PlayerUnitManager").GetComponent<UnitManager> ();
	}
	
	void Clicked ()
	{
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) {
			unitManger.SelectAdditionalUnit (gameObject);
		} else {
			unitManger.SelectSingleUnit (gameObject);
		}				
	}
	
	
}
