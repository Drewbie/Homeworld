using UnityEngine;
using System.Collections;

public class MovePlaneOverlay : MonoBehaviour
{
	LayerMask layerMask = 1 << 8;
	Ray ray;
	RaycastHit hit;
	public Vector3 localMousePosition;
	public bool _draw;
	public float distance;
	private Transform drawLine;
	private Transform drawCircle;
	private Transform drawGrid;
	private UnitManager unitManager;
	// Use this for initialization
	void Start ()
	{
		//get unit manager for selection and orders
		unitManager = GameObject.FindGameObjectWithTag("PlayerUnitManager").GetComponent<UnitManager> ();
		drawLine = transform.FindChild("drawLine");
		drawCircle = transform.FindChild("drawCircle");
		drawGrid = transform.FindChild("drawGrid");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	if(_draw)
		{
			
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
			if(unitManager._moveSequence == true && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
			{
				
			}
			else if(Physics.Raycast(ray,out hit, Mathf.Infinity,layerMask))
			{
				localMousePosition = hit.point;
			}
		
		
			distance = (Vector3.Distance(localMousePosition,transform.position)/1000);
			drawLine.gameObject.SetActive(true);
			drawCircle.gameObject.SetActive(true);
			drawGrid.gameObject.SetActive(true);
			
		}
		else
		{
			drawLine.gameObject.SetActive(false);
			drawCircle.gameObject.SetActive(false);
			drawGrid.gameObject.SetActive(false);
		}
		
	}
	
	void SetDraw(bool trueOrFalse)
	{
		_draw = trueOrFalse;
	}
}
