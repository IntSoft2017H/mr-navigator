using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PathManager : MonoBehaviour {
    public GameObject cylinder;
    public List<Vector2> Path { get; private set; }
	public PlayerController player;
	public Text connectionText;

    private int pathUpdateInterval = 3; // in second
    private string pathServerUrl = "http://arcturu.com/mr-navigator-api/";
    private List<GameObject> pathObjs;
    private bool updatingPath = false;
    private float lastPathUpdate = 0;
    private float defaultCylinderHeight = 2;
    private float defaultCylinderRadius = 1;

	// Use this for initialization
	void Start () {
        Path = new List<Vector2>();
        pathObjs = new List<GameObject>();
        updatingPath = true;
        StartCoroutine(UpdatePath());
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastPathUpdate > pathUpdateInterval && !updatingPath)
        {
            updatingPath = true;
            StartCoroutine(UpdatePath());
        }
	}

    // サーバに HTTP でアクセスしてパス情報を取得する
    private IEnumerator UpdatePath()
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(pathServerUrl))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isHttpError || uwr.isNetworkError)
            {
                Debug.Log(uwr.error);
				connectionText.text = uwr.error;
            }
            else
            {
                var path_obj = new JSONObject(uwr.downloadHandler.text);
                Path = ConvertJsonPath(path_obj.GetField("path"));
				connectionText.text = "path update succeeded";
            }
            Debug.Log("Path updated.");
            DumpPath();
            updatingPath = false;
            lastPathUpdate = Time.time;
            ClearDrawnPath();
            DrawPath(Path, 0.2f);
        }
    }

    private void ClearDrawnPath()
    {
        foreach (var p in pathObjs)
        {
            Destroy(p);
        }
    }

    private List<Vector2> ConvertJsonPath(JSONObject raw)
    {
        var path = new List<Vector2>();
        foreach (var p in raw.list)
        {
			var p3 = new Vector3(p.GetField("x").n, 0, p.GetField("y").n);
			Debug.Log (player.yRotationOffset);
			p3 = Quaternion.Euler (player.yRotationOffset) * p3;
			path.Add(new Vector2(p3.x, p3.z));
        }
        return path;
    }

    private void DumpPath()
    {
        string str = "[";
        foreach (var p in Path)
        {
            str = string.Format("{0}[{1}, {2}], ", str, p.x, p.y);
        }
        Debug.Log(string.Format("{0}]", str));
    }

    private void DrawPath(List<Vector2> path, float r)
    {
        for (int i = 1; i < path.Count; i++)
        {
            DrawLine(path[i - 1], path[i], r);
        }
    }
    
    private void DrawLine(Vector2 from, Vector2 to, float r)
    {
        Vector3 position = new Vector3((to.x + from.x) / 2, 0, (to.y + from.y) / 2);
        var v = to - from;
        var xaxis = new Vector2(1, 0);
        var rotation = Quaternion.Euler(0, -Vector2.SignedAngle(xaxis, v), 90);
        var cyl = Instantiate(cylinder, position, rotation);
        cyl.transform.localScale = new Vector3(r / defaultCylinderRadius, v.magnitude / defaultCylinderHeight, r / defaultCylinderRadius);
        pathObjs.Add(cyl);
    }
}
