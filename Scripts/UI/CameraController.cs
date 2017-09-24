using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
	public float radius = 1f;
	public float speedModifier = .5f;
    
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update ()
    {
		Vector2 differenceVector = new Vector2 (target.position.x - transform.position.x, target.position.y - transform.position.y);
		if (differenceVector.magnitude > radius) {			
			float xValue = transform.position.x + differenceVector.x * speedModifier * Time.deltaTime;
			float yValue = transform.position.y + differenceVector.y * speedModifier * Time.deltaTime;
			transform.position = new Vector3 (xValue, yValue, -10);
		}
	}
}
