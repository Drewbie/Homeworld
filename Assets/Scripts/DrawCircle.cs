using UnityEngine;
using System.Collections;

public class DrawCircle : MonoBehaviour {
	
	private int numberOfLines;
	private float radiansPerArc;
	private LineRenderer lineRenderer;
	
	private float[] pointsX;
	private float[] pointsY;
	private float radians;
	public MovePlaneOverlay movePlane;
	private UnitManager unitManager;
	public int lines;
	private float distance;
	
	
		
	// Use this for initialization
	void Start () 
	{	
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		movePlane = transform.parent.GetComponent<MovePlaneOverlay>();
		unitManager = GameObject.FindGameObjectWithTag("PlayerUnitManager").GetComponent<UnitManager> ();
		
	}
	
	// Update is called once per frame
	void Update ()
{
		distance = movePlane.distance;	
		drawCircle(lines,distance);
		
		
}//end update
	
	
	
	
	
	void drawCircle(int numberOfLines, float distance)
	{
		radiansPerArc = (2*Mathf.PI)/numberOfLines;
		lineRenderer.SetVertexCount(numberOfLines+1);
		pointsX = new float[numberOfLines+1];
		pointsY = new float[numberOfLines+1];
		
		
		for (int i = 1; i < (numberOfLines+1); i++) 
		{
			pointsX[i] = Mathf.Cos(radiansPerArc*i);
			pointsY[i] = Mathf.Sin(radiansPerArc*i);
		}
		
		
		
		
		lineRenderer.SetPosition(0,((transform.localPosition+Vector3.right)*distance));
		
		for (int j = 1; j < (numberOfLines+1); j++)
		{
			Vector3 vector = new Vector3(pointsX[j],0,pointsY[j]);
			lineRenderer.SetPosition(j,((transform.localPosition+vector)*distance));
			
		}
		
		
		
		
	}
	
}
