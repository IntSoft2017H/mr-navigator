using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 0.2f;
    public GameObject eye;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("up"))
        {
            transform.position += transform.forward * speed;
        }
        if (Input.GetKey("left"))
        {
            transform.Rotate(0, -5, 0);
        }
        if (Input.GetKey("right"))
        {
            transform.Rotate(0, 5, 0);
        }
		if (Input.GetKey("down"))
        {
            transform.position += -transform.forward * speed;
        }

        if (Input.GetKey("w"))
        {
            eye.transform.Rotate(-2, 0, 0);
        }
        if (Input.GetKey("s"))
        {
            eye.transform.Rotate(2, 0, 0);
        }
	}
}
