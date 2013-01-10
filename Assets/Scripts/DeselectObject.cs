using UnityEngine;
using System.Collections;

public class DeselectObject : MonoBehaviour
{

	private UnitManager unitManger;
	
	void Start ()
	{
		unitManger = GameObject.FindGameObjectWithTag ("PlayerUnitManager").GetComponent<UnitManager> ();
	}
	
	void Clicked ()
	{
		unitManger.DeselectAllUnits ();
	}
}
