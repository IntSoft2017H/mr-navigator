using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed = 0.2f;
    public GameObject player;
	public GameObject calibPoint;
	public Text calibCountDownText;
	public Vector3 yRotationOffset;
	public bool calibDone = false;

	private float calibCount = 0;
	private Vector2 north;
	private float calibCountThreshold = 3; // seconds

	// Use this for initialization
	void Start () {
		north = new Vector2 (0, 0);
		yRotationOffset = new Vector3 (0, 0, 0);
	}

    // Update is called once per frame
    void Update() {
		if (!calibDone) {
			var cp = new Vector2 (calibPoint.transform.position.x, calibPoint.transform.position.z);
			var pl = new Vector2 (transform.position.x, transform.position.z);
			if (Vector2.Distance (cp, pl) < calibPoint.transform.localScale.x) {
				Debug.Log ("inside!");
				calibCount += Time.deltaTime;
				north += new Vector2 (transform.forward.x, transform.forward.z);
			} else {
				calibCount = 0;
				north = new Vector2 (0, 0);
			}
			calibCountDownText.text = string.Format ("{0} ({1}, {2})", calibCountThreshold - calibCount, cp.x - pl.x, cp.y - pl.y);
			if (calibCount > calibCountThreshold) {
				Vector2 zplus = north.normalized;
				float rot2 = Vector2.SignedAngle (zplus, new Vector2 (0, 1));
				yRotationOffset = new Vector3 (0, rot2, 0);
				calibDone = true;
				calibCountDownText.text = string.Format ("");
			}
		}

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
