using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

	public GameObject ball;
	public float ballthrowingforce = 5f;
	public Camera mainCamera;
	public float balldistance = 2f;
	private bool isballthrown = false;
	Vector3 startposition;

	// Use this for initialization
	void Start () {
		ball.GetComponent<Rigidbody> ().useGravity = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isballthrown == true) {
			ball.GetComponent<Rigidbody> ().useGravity = false;
		}
	}

	public void ThrowBall(){
		ball.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		ball.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		ball.transform.position = mainCamera.transform.position + mainCamera.transform.forward * balldistance;
		ball.GetComponent<Rigidbody> ().useGravity = true;
		ball.GetComponent<Rigidbody> ().AddForce (mainCamera.transform.forward * ballthrowingforce);
		isballthrown = true;
	}
}
