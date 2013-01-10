using UnityEngine;
using System.Collections;

public class Console : MonoBehaviour {
	
	public UnitManager unitManager;
	public CameraControl cameraControl;
	public MovePlaneOverlay movePlane;
	// Use this for initialization
	void Start () {
	
		unitManager = GameObject.FindGameObjectWithTag("PlayerUnitManager").GetComponent<UnitManager> ();
		cameraControl = GameObject.Find("CameraManager").GetComponent<CameraControl>();
		movePlane = unitManager.GetComponentInChildren<MovePlaneOverlay>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI()
	{
		GUI.Label(new Rect(0,0,Screen.width,25),"averagePosition"+unitManager.averagePosition);
		GUI.Label(new Rect(0,25,Screen.width,25),"local mouse position"+movePlane.localMousePosition);
		GUI.Label(new Rect(0,50,Screen.width,25),"distance"+movePlane.distance);
		//GUI.Label(new Rect(0,75,Screen.width,25),"angle"+Vector3.Angle(transform.forward,goal-transform.position));
	}
}
