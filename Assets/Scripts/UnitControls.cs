using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitControls : MonoBehaviour
{
	
	
	public Vector3 goal;//goal that the unit should move towards
	
	public float maxAngularVelocity; //max angular velocity
	public float yvelocity; //current rotational velocity (only REF)
	public float xvelocity;
	
	public float minimumTimeToReachTarget; //dampening factor, (lower means no smoothing, higher means there is some smoothing
	
	public Vector3 currentVelocity;//velocity vector of the unit (only REF)
	public float maxVelocity;//max velocity that the unit can move forward with
		
	public bool _moving; //is the unit moving? used for drawing UI elements
	
	public Vector3 angles; //angles for rotating (move function)
	public Vector3 newRotation; //new rotation added to current rotation (move function)
	
	
	
	void Start ()
	{
		goal = transform.position;
		gameObject.rigidbody.detectCollisions = false;
		gameObject.rigidbody.freezeRotation=true;
		currentVelocity = Vector3.zero;		
	}
	
	public void Move (Vector3 newGoal)
	{
		Stop(gameObject);
		goal = transform.position + newGoal;
		newRotation = Quaternion.FromToRotation(transform.forward,goal-transform.position).eulerAngles;
		newRotation.z = 0;
		newRotation.x = 0;
		angles = transform.rotation.eulerAngles + newRotation;
		angles.z = 0;
		angles.x = 0;
		
				
		if(angles.x>360)
		{
			angles.x = angles.x -360;
		}
		
		if(angles.y>360)
		{
			angles.y = angles.y -360;
		}
		
		Debug.Log(goal);
		Debug.Log(newRotation);
		
	}
	
	void Update ()
	{
		DrawOrders(goal);
		
		//seeing if the unit is at its goal position
		if(Vector3.Distance(transform.position,goal)<10)
			{			
			_moving = false;
			transform.eulerAngles = new Vector3( Mathf.SmoothDampAngle(transform.eulerAngles.x,0,ref xvelocity,minimumTimeToReachTarget,maxAngularVelocity),transform.eulerAngles.y,transform.eulerAngles.z);
			
			}
			else //unit is not at the goal and needs to rotate towards its target and move forward
			{	
				_moving = true;
				if(Vector3.Angle(transform.forward,goal-transform.position) >= 25.0f)//angle is greater than 5 degrees
				{
					//rotate towards target with max speed
				//Mathf.SmoothDampAngle(transform.eulerAngles.x,angles.x,ref xvelocity,minimumTimeToReachTarget,maxAngularVelocity)
				transform.eulerAngles = new Vector3(angles.x,Mathf.SmoothDampAngle(transform.eulerAngles.y,angles.y,ref yvelocity,minimumTimeToReachTarget,maxAngularVelocity),angles.z);
				}			
				else if(Vector3.Angle(transform.forward,goal-transform.position)>0.0f && Vector3.Angle (transform.forward,goal-transform.position)<=25.0f)//angle is between 5 and 0 degrees
				{	
					//unit is almost facing target, start moving forward and also finish rotating towards target
				transform.eulerAngles = new Vector3(angles.x,Mathf.SmoothDampAngle(transform.eulerAngles.y,angles.y,ref yvelocity,minimumTimeToReachTarget,maxAngularVelocity),angles.z);
				transform.position = Vector3.SmoothDamp(transform.position,goal,ref currentVelocity,minimumTimeToReachTarget,maxVelocity);
				}
				else //unit is facing target, need to only move forward
				{
				transform.position = Vector3.SmoothDamp(transform.position,goal,ref currentVelocity,minimumTimeToReachTarget,maxVelocity);
				}
			
			}//end if (distance check)
		
		
		
			
	}
	
		
	void DrawOrders(Vector3 goal)
	{
		if(_moving == true)
		{
			gameObject.GetComponent<LineRenderer>().SetVertexCount(2);
			gameObject.GetComponent<LineRenderer>().SetPosition(0,gameObject.transform.position);
			gameObject.GetComponent<LineRenderer>().SetPosition(1,goal);
		}
		else
		{
			gameObject.GetComponent<LineRenderer>().SetVertexCount(0);
		}
	}
	
	public void Stop(GameObject unit)
	{
		unit.GetComponent<UnitControls>().goal = transform.position;
	}
}
