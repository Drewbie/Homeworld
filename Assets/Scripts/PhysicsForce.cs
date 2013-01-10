using UnityEngine;
using System.Collections;

public class PhysicsForce : MonoBehaviour
{
	public float thrust;
	private float engineThrust;
	private float rotationThrust;
	public float length;
	
	

	public Vector3 goal;


	
	void Start ()
	{
		goal = transform.position;
		
	}
	
	public void Move (Vector3 newGoal)
	{
		goal = transform.position + newGoal;
		
	}
	
	

	void FixedUpdate ()
	{
		engineThrust = 0.8f*thrust;
		rotationThrust = 0.2f*thrust;
		Vector3 tempVector = (goal-transform.position);
		
		if(Vector3.Distance(goal, transform.position)>10)
		{
			rigidbody.AddForceAtPosition(new Vector3(0,0,engineThrust),new Vector3(0,0,0));
			
			
			if(tempVector.x <0)
			{
				rigidbody.AddForceAtPosition(new Vector3(-rotationThrust/2,0,0),new Vector3(0,0,length/2));
				rigidbody.AddForceAtPosition(new Vector3(rotationThrust/2,0,0),new Vector3(0,0,-length/2));
			}
			else
			{
				rigidbody.AddForceAtPosition(new Vector3(rotationThrust/2,0,0),new Vector3(0,0,length/2));
				rigidbody.AddForceAtPosition(new Vector3(-rotationThrust/2,0,0),new Vector3(0,0,-length/2));
			}
			
			if(tempVector.y > 0)
			{
				//rigidbody.AddForceAtPosition(new Vector3(0,rotationThrust/2,0),new Vector3(0,0,length/2));
				
			}
			else
			{
				//rigidbody.AddForceAtPosition(new Vector3(0,-rotationThrust/2,0),new Vector3(0,0,length/2));
				
			}
			
			
		}
		
		
		
	}
	
	
}
