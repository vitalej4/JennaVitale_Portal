using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.iOS{
	// This component lives on the camera parent object and triggers a transition when you walk through a portal 
	[RequireComponent(typeof(Rigidbody))]
	public class PortalTransition : MonoBehaviour {

		public delegate void PortalTransitionAction();
		public static event PortalTransitionAction OnPortalTransition;
	//	public GameObject doorToVirtual;
		public GameObject doorToReality;
		public Camera mainCamera;
		public GameObject portalcamera2;
		public GameObject hittest;
		public GameObject generateplanes;

		// The main camera is surrounded by a SphereCollider with IsTrigger set to On
		void OnTriggerEnter(Collider portal){
			Portal logic = portal.GetComponentInParent<Portal> ();
			transform.position = logic.PortalCameras[1].transform.position - GetComponentInChildren<Camera>().transform.localPosition;


			if (logic.name == "VRDoor") {
				doorToReality.transform.position = new Vector3(-0.97f, 200.968f, 0.33f);
				doorToReality.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane (doorToReality.transform.position - mainCamera.transform.position, Vector3.up));
				doorToReality.GetComponentInParent<Portal> ().Source.transform.localPosition = doorToReality.transform.position;
				hittest.SetActive (false);
				logic.gameObject.SetActive (false);

			}

			if (logic.name == "VRDoor2") {
				doorToReality.transform.position = new Vector3(8.83f, 904.77f, 4.0f);
				doorToReality.transform.rotation = Quaternion.LookRotation (Vector3.ProjectOnPlane (doorToReality.transform.position - mainCamera.transform.position, Vector3.up));
				doorToReality.GetComponentInParent<Portal> ().Source.transform.localPosition = doorToReality.transform.position;

				portalcamera2.SetActive (false);
				hittest.SetActive (false);

				logic.gameObject.SetActive (false);

			} 

			if (logic.name == "RealWorldDoor") {
				hittest.SetActive (true);
				ARKitWorldTrackingSessionConfiguration sessionConfig = new ARKitWorldTrackingSessionConfiguration ( UnityARAlignment.UnityARAlignmentGravity, UnityARPlaneDetection.Horizontal);
				UnityARSessionNativeInterface.GetARSessionNativeInterface().RunWithConfigAndOptions(sessionConfig, UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors | UnityARSessionRunOption.ARSessionRunOptionResetTracking);


			}


			if (OnPortalTransition != null) {
				// Emit a static OnPortalTransition event every time the camera enters a portal. The DoorManager listens for this event.
				OnPortalTransition ();
			}


		}

	}
}
