using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour
{
	private LineRenderer lineRenderer;
	private UnitManager unitManger;
	public MovePlaneOverlay movePlane;
	private UnitManager unitManager;
	// Use this for initialization
	void Start ()
	{
	lineRenderer = gameObject.GetComponent<LineRenderer>();
	lineRenderer.SetVertexCount(3);
	movePlane = transform.parent.GetComponent<MovePlaneOverlay>();
	unitManager = GameObject.FindGameObjectWithTag("PlayerUnitManager").GetComponent<UnitManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		drawLine(movePlane.localMousePosition);
		
	}
	
void drawLine(Vector3 mousePosition)
	{
	if((unitManager.heightVector.y - movePlane.localMousePosition.y) > 5.0f ||(unitManager.heightVector.y - movePlane.localMousePosition.y)< -5.0f)
		{
			lineRenderer.SetVertexCount(4);
		lineRenderer.SetPosition(0,transform.position);
		lineRenderer.SetPosition(1,mousePosition);
		lineRenderer.SetPosition(2,new Vector3(mousePosition.x,mousePosition.y + unitManager.heightVector.y,mousePosition.z));
		lineRenderer.SetPosition(3,transform.position);
			
		}
		else
		{
			lineRenderer.SetVertexCount(3);
		lineRenderer.SetPosition(0,transform.position);
		lineRenderer.SetPosition(1,mousePosition);
		lineRenderer.SetPosition(2,transform.position);
		}
				
	}
}
