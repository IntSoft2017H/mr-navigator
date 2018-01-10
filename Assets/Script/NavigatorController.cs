using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NavigatorController : MonoBehaviour
{
    public PathManager pm;
    public GameObject player;
	public PlayerController playerController;
    public float speed = 0.05f;
    public float sight = 0.3f; // この半径以内に入った点は訪れたとみなす
    public float playerSight = 5; // この半径以内にプレイヤーがいる場合に限り先に進む (nav が player に期待する視力)
    public float randomness = 0.1f;
    public int randomDirUpdateCycle = 30;

    private int lastVisited = -1;
    private Vector2 randomDir;

    enum NavigatorState
    {
        Idle,
        Walking,
        Finish,
    }

    // Use this for initialization
    void Start()
    {
        randomDir = new Vector2(Random.value, Random.value);
    }

    // Update is called once per frame
    void Update()
    {
		if (!playerController.calibDone) {
			SetState (NavigatorState.Idle);
			return;
		}
        // プレイヤーを待ちながらパスを追う
        if (lastVisited < pm.Path.Count - 1)
        {
            var navpos = new Vector2(transform.position.x, transform.position.z);
            var player_pos = new Vector2(player.transform.position.x, player.transform.position.z);
            float dist_nav_point = Vector2.Distance(navpos, pm.Path[lastVisited + 1]);
            float dist_nav_player = Vector2.Distance(navpos, player_pos);
            if (dist_nav_point < sight)
            {
                lastVisited++;
                if (lastVisited >= pm.Path.Count - 1)
                {
                    return;
                }
            }
            var nav2next = pm.Path[lastVisited + 1] - navpos;
            var nav_forward = nav2next.normalized;
            nav_forward = nav_forward * (1 - randomness) + randomDir * randomness;
            if (dist_nav_player < playerSight)
            {
                transform.position += new Vector3(nav_forward.x * speed, 0, nav_forward.y * speed);
                transform.forward = new Vector3(nav_forward.x, transform.forward.y, nav_forward.y).normalized;
                transform.Rotate(0, 20, 0); // kitten モデルは向きが傾いているのでそれを補正する
                SetState(NavigatorState.Walking);
            }
            else
            {
                SetState(NavigatorState.Idle);
            }
            Debug.Log(string.Format("target = {0}, dist_nav_point = {1}, dist_nav_player = {2}", lastVisited + 1, dist_nav_point, dist_nav_player));
        }
        else
        {
            SetState(NavigatorState.Finish);
        }

        // update random dir
        if (Time.frameCount % randomDirUpdateCycle == 0)
        {
            randomDir = new Vector2(Random.value, Random.value);
        }
    }

    void SetState(NavigatorState state)
    {
        var animator = GetComponent<Animator>();
        switch (state)
        {
            case NavigatorState.Idle:
                animator.SetBool("walking", false);
                break;
            case NavigatorState.Walking:
                animator.SetBool("walking", true);
                break;
            case NavigatorState.Finish:
                animator.SetBool("walking", false);
                break;
            default:
                throw new System.InvalidOperationException("This must not be fired!");
        }
    }

    class IndexDist
    {
        public int index;
        public float dist;
    }

    private IndexDist FindNearestPoint(int from)
    {
        // パス中の from 以降の最近接 *点* を見つける
        var pos = new Vector2(transform.position.x, transform.position.z);
        float min_len = float.PositiveInfinity;
        int next_point = -1;
        for (int i = from; i < pm.Path.Count; i++)
        {
            var d = Vector2.Distance(pos, pm.Path[i]);
            if (d < min_len)
            {
                min_len = d;
                next_point = i;
            }
        }
        return new IndexDist() { index = next_point, dist = min_len };
    }

    private IndexDist FindNextPoint(int from)
    {
        // パス中の最近接部分を見つける
        var pos = new Vector2(transform.position.x, transform.position.z);
        float min_len = float.PositiveInfinity;
        int next_point = -1;
        for (int i = from + 1; i < pm.Path.Count; i++)
        {
            var p = pm.Path[i] - pm.Path[i - 1];
            var x = pos - pm.Path[i - 1];
            var xscale = Vector2.Dot(p.normalized, x);
            // TODO FIXME 無駄がある (始点終点以外の点に対する判断が重複している)
            if (xscale > p.magnitude)
            {
                var d = Vector2.Distance(pos, pm.Path[i]);
                if (d < min_len)
                {
                    min_len = d;
                    next_point = i;
                }
            }
            else if (xscale < 0)
            {
                var d = Vector2.Distance(pos, pm.Path[i] + xscale * p);
                if (d < min_len)
                {
                    min_len = d;
                    next_point = i - 1;
                }
            }
            else
            {
                var d = Vector2.Distance(pos, pm.Path[i - 1]);
                if (d < min_len)
                {
                    min_len = d;
                    next_point = i;
                }
            }
        }
        return new IndexDist() { index = next_point, dist = min_len };

    }
}
