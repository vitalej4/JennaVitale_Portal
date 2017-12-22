using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class shows and hides doors (aka portals) when you walk into them. It listens for all OnPortalTransition events
// and manages the active portal.
public class DoorManager : MonoBehaviour {

	public delegate void DoorAction(Transform door);
	public static event DoorAction OnDoorOpen;

	public GameObject doorToVirtual2;

	public GameObject doorToVirtual;
	public GameObject doorToReality;

	public Camera mainCamera;
	public GameObject portalCamera;

	private GameObject currDoor;

	private bool isCurrDoorOpen = false;

	public int doornum = 0;
	public int prevdoornum;

	Vector3 startscale = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 endscale;

	public GameObject planegenerator;

	public GameObject throwball;

	void Start(){
//		PortalTransition.OnPortalTransition += OnDoorEntrance;
	}

	IEnumerator ScaleUp(GameObject portal, float duration){
		currDoor.transform.localScale = startscale;
		portal.SetActive (true);
		float t = 0;
		endscale = new Vector3(0.75f, 0.75f, 0.75f);
		while (t < duration) {
			t += Time.deltaTime;
			portal.transform.localScale = Vector3.Lerp (startscale, endscale, t / duration);

			yield return null;

		}
	}

	// This method is called from the Spawn Portal button in the UI. It spawns a portal in front of you.
	public void OpenDoorInFront(Vector3 doorpos){
//	public void OpenDoorInFront(){
		var camerachild = portalCamera.transform.GetChild (0);

		if (!isCurrDoorOpen) {

			switch (doornum)
			{
			case 0:

				currDoor = doorToVirtual;
				break;
			case 1:
				portalCamera.SetActive (true);
				camerachild.gameObject.SetActive (true);
				currDoor = doorToVirtual2;
				break;
			case 2:
				portalCamera.SetActive (false);
				camerachild.gameObject.SetActive (false);
				throwball.SetActive(false);
				currDoor = doorToReality;

				break;
			default:
				Debug.Log("Invalid");
				break;
			}
				


			StartCoroutine(ScaleUp(currDoor, 2.0f));


			if (doornum == 0) {

				currDoor.transform.position = (Vector3.ProjectOnPlane (doorpos, Vector3.up)).normalized
					+ mainCamera.transform.position;

				currDoor.transform.rotation = Quaternion.LookRotation (
					Vector3.ProjectOnPlane(currDoor.transform.position - mainCamera.transform.position, Vector3.up));

				currDoor.GetComponentInParent<Portal>().Source.transform.localPosition = currDoor.transform.position;
			} else {

				currDoor.transform.position = (Vector3.ProjectOnPlane (mainCamera.transform.forward, Vector3.up)).normalized
					+ mainCamera.transform.position;

				currDoor.transform.rotation = Quaternion.LookRotation (
					Vector3.ProjectOnPlane(currDoor.transform.position - mainCamera.transform.position, Vector3.up));

				currDoor.GetComponentInParent<Portal>().Source.transform.localPosition = currDoor.transform.position;

			}


			isCurrDoorOpen = true;

			if (OnDoorOpen != null) {
				OnDoorOpen (currDoor.transform);
			}
		}
	}

	// Respond to the player walking into the doorway. Since there are only two portals, we don't need to pass which
	// portal was entered.
	private void OnDoorEntrance() {

		currDoor.SetActive(false);
		isCurrDoorOpen = false;

		switch (doornum) {
		case 0:
			doornum = 1;
			break;
		case 1:
			doornum = 2;
			break;
		case 2:			
			doornum = 0;
			break;
		default:
			Debug.Log("Invalid");
			break;

		}
	}
}
