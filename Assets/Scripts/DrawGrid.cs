using UnityEngine;
using System.Collections;

public class DrawGrid : MonoBehaviour {

	private LineRenderer lineRenderer;
	public MovePlaneOverlay movePlane;
	private UnitManager unitManager;
	
	// Use this for initialization
	void Start ()
	{
	lineRenderer = gameObject.GetComponent<LineRenderer>();
	
	movePlane = transform.parent.GetComponent<MovePlaneOverlay>();
	unitManager = GameObject.FindGameObjectWithTag("PlayerUnitManager").GetComponent<UnitManager>();
	}
	
	// Update is called once per frame
	void Update ()
	{		
		drawGrid(movePlane.distance*1000);
	}
	
	void drawGrid(float distance)
	{
		
		lineRenderer.SetVertexCount(7);
		lineRenderer.SetPosition(0,gameObject.transform.position);
		lineRenderer.SetPosition(1,gameObject.transform.position+(transform.right*distance));
		lineRenderer.SetPosition(2,gameObject.transform.position+(transform.right*-distance));
		lineRenderer.SetPosition(3,gameObject.transform.position);
		lineRenderer.SetPosition(4,gameObject.transform.position+(transform.forward*distance));
		lineRenderer.SetPosition(5,gameObject.transform.position+(transform.forward*-distance));
		lineRenderer.SetPosition(6,gameObject.transform.position);
	}
}
