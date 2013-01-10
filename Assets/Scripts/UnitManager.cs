using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{


	public List<GameObject> selectedUnits;//list of currently selected units
	public GameObject enemyUnit; //enemy unit you have selected
	
	public Vector3 averagePosition;//average position of all the units you have selected	
	
	public float orderDelay = 1;//delay in seconds for orders
	public MovePlaneOverlay movePlane; //the plane that is used to determine move position etc...
	public GameObject movePlaneObject; //the object move plane
	
	public bool _moveSequence = false; //is the move sequence running?
	
	public CameraControl cameraControl;//reference to the camera manager
	public Vector3 heightVector = Vector3.zero;
	
	void Awake()
	{
		
	}


	void Start ()
	{
		//clear selected units
		selectedUnits.Clear ();
		movePlane = gameObject.GetComponentInChildren<MovePlaneOverlay>();
		movePlane._draw = false;
		cameraControl = GameObject.Find("CameraManager").GetComponent<CameraControl>();
	}
	
	//Select Single Unit
	//##########################################################
	public void SelectSingleUnit (GameObject unit)
	{
		foreach (GameObject units in selectedUnits) {
			HealthBar theObject = units.GetComponent<HealthBar> ();
			HealthBarManager.Instance.Hide (theObject);
		}

		selectedUnits.Clear ();
		selectedUnits.Add (unit);
		CalculateAveragePosition ();

	}
	//drag select units
	//##########################################################
	public void DragSelectUnits(GameObject unit)
	{
		if (selectedUnits.Contains (unit))
		{
			
		} 
		else
		{
			selectedUnits.Add (unit);
			CalculateAveragePosition ();
		}
	}
	//Select Addational Unit
	//##########################################################
	public void SelectAdditionalUnit (GameObject unit)
	{		
		if (selectedUnits.Contains (unit)) {
			DeselectSingleUnit (unit);
		} else {
			selectedUnits.Add (unit);
			CalculateAveragePosition ();
		}

	}
	
	//DeselectSingleUnit (holding shift and clicking on a unit thats in your selection list
	//##########################################################
	public void DeselectSingleUnit (GameObject unit)
	{
		selectedUnits.Remove (unit);
		CalculateAveragePosition ();
	}
	
	//select a single enemy unit
	//##########################################################
	public void SelectEnemyUnit (GameObject unit)
	{
		//select enemy unit stuff goes here
	}
	
	//Deselect all units
	//##########################################################
	public void DeselectAllUnits ()
	{
		foreach (GameObject unit in selectedUnits) {
			HealthBar theObject = unit.GetComponent<HealthBar> ();
			HealthBarManager.Instance.Hide (theObject);
		}
		selectedUnits.Clear ();
	}
	
	//return a list of currnetly selected objects
	//##########################################################
	public List<GameObject> GetSelectedUnits ()
	{
		return selectedUnits;
	}
		
//Update Loop
//###########################################################################################
	void Update ()
	{
		CalculateAveragePosition(); //update the movePlane's position
		
	//	Display health bars
	//##########################################################
		foreach (GameObject unit in selectedUnits) 
		{
			HealthBar theObject = unit.GetComponent<HealthBar> ();
			HealthBarManager.Instance.Show (theObject);
		}		
		
		
//###########################################################################################
//###########################################################################################
//	Unit orders are below, beucase you need to select units in order to issue them orders
//###########################################################################################
//###########################################################################################
		if(selectedUnits.Count > 0) //if we have anything selected
		{
			Vector3 moveVector;
			
	//	Should we draw the move plane?
	//##########################################################
		if(_moveSequence == true)
		{		
		movePlane.renderer.enabled = false; //should we show the move plane?
		movePlane.transform.position = averagePosition; //center the move plane on the current avg posiiton
		}
		else
		{			
		movePlane.renderer.enabled = false;//dont show the move plane
		movePlane.transform.position = averagePosition; //center the move plane on the current avg position
		}
			
	
	//	Move()
	//##########################################################
			if(Input.GetKeyDown(KeyCode.M) && _moveSequence == false)
			{		
				_moveSequence=true; //are we issuing new move orders?
				movePlane.GetComponentInChildren<MovePlaneOverlay>()._draw = true; //draw move plane
				heightVector = Vector3.zero;//set the height vector back to where it should be
				
			}
			
			
	
	//	Adding height to Move()		
	//##########################################################
			if(Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)&& _moveSequence == true)
				{
					Screen.lockCursor = true;
					heightVector = movePlane.localMousePosition;
				}			
			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)&& _moveSequence == true)
				{
					heightVector.y += Input.GetAxis("Mouse Y")*8.0f;					
				}			
			if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)&& _moveSequence == true)
				{
					Screen.lockCursor = false;					
				}
			
	
	//	Sending message to object to Move
	//##########################################################				
			if(Input.GetMouseButtonDown(0)&& _moveSequence == true)		
					{				
					//left clicked, need to move those units to that new location				
					moveVector = (movePlane.GetComponentInChildren<MovePlaneOverlay>().localMousePosition-averagePosition);
					//adding the height vector
					moveVector.y += heightVector.y;
					//stop drawing
					movePlane.GetComponentInChildren<MovePlaneOverlay>()._draw = false;				
					//stop move sequence
					_moveSequence = false;
					//return vector to move
					foreach(GameObject unit in selectedUnits)
						{
							unit.GetComponent<UnitControls>().Move(moveVector);
							//Debug.Log (moveVector);
						}				
					}
	//	Canceling the Move Sequence
	//##########################################################			
			if(Input.GetKeyDown(KeyCode.Escape)&& _moveSequence ==true)
					{
					//Debug.Log("esc, exiting move sequence");
					//stop drawing
					movePlane.GetComponentInChildren<MovePlaneOverlay>()._draw = false;
					// stop sequence
					_moveSequence = false;
					//return nothing
					}
			
	//	Focus()
	//##########################################################		
			if(Input.GetKeyDown(KeyCode.F))
			{
				cameraControl.FocusCamera(averagePosition);
				
			}
			
	//	Stop()	
	//##########################################################
			if(Input.GetKeyDown(KeyCode.S))
			{
				foreach(GameObject unit in selectedUnits)
				{
					unit.GetComponent<UnitControls>().Stop(unit);
				}
			}
		}


	}
	
	//	CalculateAveragePosition		
	//##########################################################
	void CalculateAveragePosition ()
	{
		
		float avgX,avgY,avgZ;
		float totalX=0,totalY=0,totalZ = 0;
		if (selectedUnits.Count > 0) 
		{
			foreach (GameObject unit in selectedUnits)
				{				
				totalX = unit.transform.position.x + totalX;
				totalY = unit.transform.position.y + totalY;
				totalZ = unit.transform.position.z + totalZ;
				}
			
			avgX = totalX / selectedUnits.Count;
			avgY = totalY / selectedUnits.Count;
			avgZ = totalZ / selectedUnits.Count;

		} 
		else
		{
			avgX = 0;
			avgY = 0;
			avgZ = 0;
		}
		averagePosition = new Vector3(avgX,avgY,avgZ);
	}
	
	
	
	
}
