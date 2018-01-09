﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 0.2f;
    public GameObject player;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKey("up"))
        {
            transform.position += player.transform.forward * speed;
        }
        if (Input.GetKey("left"))
        {
            transform.position += -player.transform.right * speed;
        }
        if (Input.GetKey("right"))
        {
            transform.position += player.transform.right * speed;
        }
		if (Input.GetKey("down"))
        {
            transform.position += -player.transform.forward * speed;
        }

        if (Input.GetKey("w"))
        {
            transform.Rotate(-1, 0, 0);
			player.transform.Rotate(1, 0, 0);
        }
        if (Input.GetKey("a"))
        {
            transform.Rotate(0, -2, 0);
        }
        if (Input.GetKey("d"))
        {
            transform.Rotate(0, 2, 0);
        }
        if (Input.GetKey("s"))
        {
            transform.Rotate(1, 0, 0);
			player.transform.Rotate(-1, 0, 0);
        }
	}
}
