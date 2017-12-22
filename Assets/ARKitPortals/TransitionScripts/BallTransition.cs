using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTransition : MonoBehaviour {
	float constantSpeed = 2.0f;
	Transform portalhit;
	Transform portalcameraparent;
	Transform portalcamera;
	Vector3 newpos;
	public Camera maincamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnTriggerEnter (Collider portal){

//			portalhit = portal.transform.root;
//			portalcameraparent = portalhit.GetChild (0);
//			portalcamera = portalcameraparent.GetChild (0);
//			gameObject.transform.position = portalcamera.position;

		Portal logic = portal.GetComponentInParent<Portal> ();
		transform.position = logic.PortalCameras [0].transform.position;

	}

}
