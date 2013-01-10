 /* Camera Control
 * Last Modified: Dec 2nd 2012
 * Written By: Drew Crandall
 * 
 * This script attaches to the main camera object
 * if controls any mouse click as well as a few other circumstances
 * look at interface document flow chart for more info
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
	
public class CameraControl : MonoBehaviour
{
		
		
	public Transform target; //target that we are rotating around
	private float v; //vertical change in mouse position
	private float h; //horizontal change in mouse position
	private float w; //change in scroll wheel position
	private double mousePositionX; //mouse position X
	private double mousePositionY; //mouse position Y
	private float scrollSpeed = 500; //defalut scroll speed
	private float scrollArea = 3; //how close you need to be to the edge of the screen to initiate scrolling
	private float minZoomDistance = 200; //minimum zoom distance
	private float maxZoomDistance = 900; //maximum zoom distance
	public float distanceToTarget; //distance camera is from the avgselectedunitposition
	private Vector3 rightAxis;//right axis we rotate about when holding right mouse button
	public float hoSensitivity = 5; //rotation sensitivity
	public float voSensitivity = 5; //rotation sensitivity
	public float zoSensitivity = 10; //defalut zoom senstivity
	private Camera _camera; //our camera
	public bool debug = false; //is dubug on?
	public Camera detectionCamera; //optional, specific camera
	public bool allowPan;//allow pan for the camera
	private UnitManager unitManger;//unitManagerObject
	public int startPositionX;//starting point for click and drag
	public int startPositionY;//starting point for click and drag
	public int endPositionX;//ending point for click and drag
	public int endPositionY;//ending point for click and drag
	public GameObject clickanddragplane;//plane that is resized when someone clicks and drags.
		
	// Use this for initialization
	void Start ()
	{	
		//get unit manager for selection and orders
		unitManger = GameObject.FindGameObjectWithTag("PlayerUnitManager").GetComponent<UnitManager> ();
		//set focus point at the game orgin (later will be player start position)
		transform.position = new Vector3 (0, 0, 0);
		
		
		//if we dont specifically setup a camera for this script it assigns it to the main camera
		if (detectionCamera != null) 
		{
			_camera = detectionCamera;
		} 
		else 
		{
			_camera = Camera.main;
		}
		
	}//end start
	
	//allows the unit manager to focus the camera on an object
	public void FocusCamera (Vector3 unit)
	{		
		//transform.rotation = unit.transform.localRotation;
		transform.position = unit;
		
	}//end FocusCamera
	
	// Update is called once per frame
	void Update ()
	{
		
		
		
		//ray cast variables
		RaycastHit hit; //collider we hit
		Ray ray; //ray from mouse to game
		int layerMask = 1 << 8; // layer 8 should be the movement plane that is handled by the unitmanager object
		layerMask = ~layerMask; // everything but layer 8
		
		//mouse values
		h = Input.GetAxis ("Mouse X"); //horizontal change
		v = Input.GetAxis ("Mouse Y"); //vertical change
		w = Input.GetAxis ("Mouse ScrollWheel"); //zoom change (middle mouse button)				
		mousePositionX = Input.mousePosition.x; // position for camera pan
		mousePositionY = Input.mousePosition.y; //position for camera pan
		
		//distance the cmaera is from focused object
		distanceToTarget = Vector3.Distance (transform.position, Camera.main.transform.position);
		 Mathf.Clamp(distanceToTarget,minZoomDistance,maxZoomDistance);
		
		//slower zoom when near maxx or min distance to prevent erraneous values for zoom distance
		if (distanceToTarget < (maxZoomDistance / 2))
		{
			//zoom is less than halfway
			zoSensitivity = Mathf.Abs (distanceToTarget / 8);
		}
		else
		{
			//zoom is more than halfway
			zoSensitivity = Mathf.Abs ((maxZoomDistance - distanceToTarget+500) / 8);	
		}
		
		
	
	//Left Click (not hold)	
		if (Input.GetMouseButtonDown (0) && Input.GetKey(KeyCode.LeftAlt)) //0 is left, 1 is right, 2 is middle mouse button
		{  
			//Set up our ray from screen to scene
			ray = _camera.ScreenPointToRay (Input.mousePosition);
			
			//see if our raycast hit an object that is not layer 8
			if(Physics.Raycast(ray,out hit, Mathf.Infinity,layerMask))
			{
				FocusCamera(hit.transform.gameObject.transform.position);
			}
			
		}
		else if(Input.GetMouseButtonDown(0)&& unitManger._moveSequence == false) //mouse button down but no alt button
		{
			//Debug.Log("CameraControls line125");
			//Set up our ray from screen to scene
			ray = _camera.ScreenPointToRay (Input.mousePosition); 
			//If something was clicked
			startPositionX = (int)Input.mousePosition.x;
			startPositionY = (int)Input.mousePosition.y;
			
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) 
			{	
				//if it is your unit
				if(hit.transform.gameObject.tag == "selectable")
				{
					if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) //if you held down shift
					{						
						unitManger.SelectAdditionalUnit (hit.transform.gameObject);//add additional unit
					} 
					else //if you did not hold shift
					{						
						unitManger.SelectSingleUnit (hit.transform.gameObject);//select single unit
					}
				}
				else //if the object clicked isnt your unit
				{
					
					if((unitManger.selectedUnits.Count >0))//if you have anythign selected
					{
						if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))//if you are holding shift
						{
							//do nothing, you have a unit selected and clicking on an enemy while holding shift shouldnt do anything
						}
						else //you are not holding shift, but have something seleted
						{
							unitManger.SelectEnemyUnit(hit.transform.gameObject); //select enemy unit
						}
					}
					else //you have nothing selected, but not holding shift
					{
						unitManger.SelectEnemyUnit(hit.transform.gameObject);	//select enemy unit
					}
				}
			} 
			else //nothing was clicked
			{
				unitManger.DeselectAllUnits ();
			}
			
			
		}//end else
		
		
	//Right Click	(not hold)
		if (Input.GetMouseButtonDown (1)) //0 is left, 1 is right, 2 is middle mouse button
		{  
			//Set up our ray from screen to scene
			ray = _camera.ScreenPointToRay (Input.mousePosition); 
				
			//If we hit...
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask))
			{
				//hit.transform.gameObject.SendMessage ("RightClicked", hit.point, SendMessageOptions.DontRequireReceiver);
			}			
		}
		
		
	//Middle Mouse Button Click
		if(Input.GetMouseButtonDown(2)) //0 is left, 1 is right, 2 is middle mouse button
		{
			//Set up our ray from screen to scene
			ray = _camera.ScreenPointToRay (Input.mousePosition); 
				
			//If we hit...
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) 
			{	
				//hit.transform.gameObject.SendMessage ("Middle Clicked", hit.point, SendMessageOptions.DontRequireReceiver);
				if(unitManger.selectedUnits.Count > 0)
				{
					//focus on average positions of the selected units list
					FocusCamera(unitManger.movePlaneObject.transform.position);
				}
				else
				{
					//do nothing here
				}
			} 
		}
	//Left Click Hold
		if (Input.GetMouseButton (0))
		{
			//Debug.Log ("clickanddrag");
			//clickanddragplane.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(startPositionX,-50,startPositionY));
			//clickanddragplane.transform.localScale.Set(endPositionX-startPositionX,endPositionY-startPositionY,0);
			
		}
		
	//left mouse button up	
		if(Input.GetMouseButtonUp(0))
		{
			int pixelsToSkip=3;
			endPositionX = (int)Input.mousePosition.x;
			endPositionY = (int)Input.mousePosition.y;
			if(startPositionX < endPositionX)
			{
			//dragging left to right
			while(startPositionX < endPositionX)
					{
					//Debug.Log(startPositionX);
					//drag top to bottom
					if(startPositionY > endPositionY)
					{
						int count = 0;
						//send out rays from bottom to top cordinate
						while(endPositionY+count < startPositionY)
						{
							
								//Debug.Log(endPositionY);
							ray = _camera.ScreenPointToRay(new Vector3(startPositionX,endPositionY+count));
							//did the ray hit anything?
							if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) 
								{	
									unitManger.DragSelectUnits(hit.transform.gameObject);
								}
							count +=pixelsToSkip;
							
						}//done sending out rays in the vertical direction
							
					}//end top to bottom drag configuration
					else if(startPositionY < endPositionY)
					{
						int count = 0;
						while(startPositionY+count<endPositionY)
						{
								//Debug.Log(endPositionY);
							ray = _camera.ScreenPointToRay(new Vector3(startPositionX,startPositionY+count));
							//did the ray hit anything?
							if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) 
								{	
									unitManger.DragSelectUnits(hit.transform.gameObject);
								}
								count+=pixelsToSkip;
						}//end rays
					}//end bottom to top drag configuration
					
					startPositionX = startPositionX + pixelsToSkip;
				}//end while
			}
			else if (endPositionX < startPositionX)
			{
			
			//dragging right to left
			while(endPositionX < startPositionX)
			{
				//	Debug.Log (endPositionX);
				//drag top to bottom
				if(startPositionY > endPositionY)
					{
						int count = 0;
						//send out rays from bottom to top cordinate
						while(endPositionY+count < startPositionY)
						{
							
								//Debug.Log(endPositionY);
							ray = _camera.ScreenPointToRay(new Vector3(endPositionX,endPositionY+count));
							//did the ray hit anything?
							if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) 
								{	
									unitManger.DragSelectUnits(hit.transform.gameObject);
								}
							count +=pixelsToSkip;
							
						}//done sending out rays in the vertical direction
							
					}//end top to bottom drag configuration
					else if(startPositionY < endPositionY)
					{
						int count = 0;
						while(startPositionY+count<endPositionY)
						{
								//Debug.Log(endPositionY);
							ray = _camera.ScreenPointToRay(new Vector3(endPositionX,startPositionY+count));
							//did the ray hit anything?
							if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) 
								{	
									unitManger.DragSelectUnits(hit.transform.gameObject);
								}
								count+=pixelsToSkip;
						}//end rays
					}//end bottom to top drag configuration   
				
				endPositionX = endPositionX + pixelsToSkip;
			}//end while
			}
		}
		
	//Right CLick Hold
		if (Input.GetMouseButton (1)) //0 is left, 1 is right, 2 is middle mouse button
		{ 
			transform.RotateAround (transform.position, Vector3.up, h * hoSensitivity); //orbit left/right
			rightAxis = transform.right;
			transform.RotateAround (transform.position, rightAxis, -v * voSensitivity); //orbit up/down
		}//end RightClickAndHold
	
		
	//Edge of Screen pan	
		if(allowPan)
		{
				// pan around if mouse is near the edge of the screen
			if (mousePositionX < scrollArea) {transform.Translate(transform.right * -scrollSpeed * Time.deltaTime,Space.World);}
			if (mousePositionX >= Screen.width-scrollArea) {transform.Translate(transform.right * scrollSpeed * Time.deltaTime,Space.World);}
			if (mousePositionY < scrollArea) {transform.Translate(transform.forward * -scrollSpeed * Time.deltaTime,Space.World);}
			if (mousePositionY >= Screen.height-scrollArea) {transform.Translate(transform.forward * scrollSpeed * Time.deltaTime,Space.World);}
		}
				
		
	//Middle Mouse Scrolling	
		if (distanceToTarget >= minZoomDistance && distanceToTarget <= maxZoomDistance)//zoom in and out if within acceptable rance
		{
			//zoom in and out
			Vector3 forward = Vector3.Normalize (transform.position - Camera.main.transform.position);
			Camera.main.transform.Translate (forward * w * zoSensitivity, Space.World);
		} 
		else if (distanceToTarget < minZoomDistance && distanceToTarget < maxZoomDistance)//if too close, only let us zoom out from target
		{
			
			if (w < 0) //ignore zooming in
			{
				//zoom out only
				Vector3 forward = Vector3.Normalize (transform.position - Camera.main.transform.position);
				Camera.main.transform.Translate (-forward * zoSensitivity, Space.World);
			}
						
		} 
		else if (distanceToTarget > minZoomDistance && distanceToTarget > maxZoomDistance) //if too far away only let us zoom in
		{
			
			if (w > 0) //ignore zooming out
			{
				//zoom in only
				Vector3 forward = Vector3.Normalize (transform.position - Camera.main.transform.position);
				Camera.main.transform.Translate (forward * zoSensitivity, Space.World);
			}
			
		}//	end
		
		/*if(Input.GetKey(KeyCode.M))
		{
			ray = Camera.main.ScreenPointToRay(new Vector3(200, 200, 0));
        	Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
		}
		*/
		
	}//end update
		
}//end script
	
	
