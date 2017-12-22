using System;
using System.Collections.Generic;
using System.Collections;

namespace UnityEngine.XR.iOS
{
	public class UnityARHitTestExample : MonoBehaviour
	{
		public Transform doortovirtual;
		public Transform doortovirtual2;
		Vector3 newdoorpos;
		public Camera mainCamera;
		Vector3 startscale = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 endscale;
		public GameObject portalcamera;
		public GameObject portalcamera2;


		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
        {
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
            
			if(point.y > .15f){

					if (Input.touchCount == 1) {

						doortovirtual2.gameObject.SetActive (false);

						if (hitResults.Count > 0) {

							foreach (var hitResult in hitResults) {
								Debug.Log ("ONE TOUCH");
								portalcamera.SetActive (true);
								portalcamera2.SetActive (false);

								StartCoroutine (ScaleUp (doortovirtual, 2.0f));
								doortovirtual.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
								doortovirtual.rotation = Quaternion.LookRotation (Vector3.ProjectOnPlane (doortovirtual.transform.position - mainCamera.transform.position, Vector3.up));
								
								doortovirtual.GetComponentInParent<Portal> ().Source.transform.localPosition = doortovirtual.transform.position;
								

								return true;
							}
						}
					}

					else if (Input.touchCount == 2) {

							doortovirtual.gameObject.SetActive (false);

						if (hitResults.Count > 0) {

							foreach (var hitResult in hitResults) {
								Debug.Log ("TWO TOUCH");
								portalcamera2.SetActive (true);
								portalcamera.SetActive (false);
								StartCoroutine (ScaleUp (doortovirtual2, 2.0f));

								doortovirtual2.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
								doortovirtual2.rotation = Quaternion.LookRotation (Vector3.ProjectOnPlane (doortovirtual2.position - mainCamera.transform.position, Vector3.up));

								doortovirtual2.GetComponentInParent<Portal> ().Source.transform.localPosition = doortovirtual2.position;



								return true;
							}
						}

					}


			}
            return false;
        }
		
		// Update is called once per frame
		void Update () {

			if (Input.touchCount > 0)
			{
				var touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
				{
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					ARPoint point = new ARPoint {
						x = screenPosition.x,
						y = screenPosition.y
					};

					// prioritize reults types
					ARHitTestResultType[] resultTypes = {
						ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
						// if you want to use infinite planes use this:
//						ARHitTestResultType.ARHitTestResultTypeExistingPlane,
//						ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
//						ARHitTestResultType.ARHitTestResultTypeFeaturePoint
					}; 

					foreach (ARHitTestResultType resultType in resultTypes)
					{
						if (HitTestWithResultType (point, resultType)) {
							return;
						} 


						}
					}
				}


			}

			IEnumerator ScaleUp(Transform portal, float duration){
				portal.localScale = startscale;
				portal.gameObject.SetActive (true);
				float t = 0;
				endscale = new Vector3(2.5f, 2.5f, 2.5f);
				while (t < duration) {
					t += Time.deltaTime;
					portal.localScale = Vector3.Lerp (startscale, endscale, t / duration);

					yield return null;

				}
			}

		}

	
	}


