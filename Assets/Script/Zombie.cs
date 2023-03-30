using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI; 
public class Zombie : MonoBehaviour
{
    private Animator animator;
    public GameObject player;
    public float distance;

    public bool search;
    [SerializeField]
    private SphereCollider searchArea;
    [SerializeField]
    private float searchAngle = 230f;

    
    private NavMeshAgent agent;
    [SerializeField] private Vector3 _forward = Vector3.forward;
    public Transform[] points;
    public int destPoint; //現在の目的地
    public float zWalk = 0.00018f;
    public float zRun = 0.0002f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        
        agent = GetComponent<NavMeshAgent>(); //NMAを情報取得
        // 最初の目的地を入れる
        agent.SetDestination(points[0].position);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
            distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance < 1.0)
        {
            animator.SetTrigger("Attack");
        }

        if (search == true)
        {
            animator.SetBool("Run", true);
            transform.position += transform.forward * zRun;
            
            var dir = player.transform.position - this.transform.position;

            // ターゲットの方向への回転
            var lookAtRotation = Quaternion.LookRotation(dir, Vector3.up);
            // 回転補正
            var offsetRotation = Quaternion.FromToRotation(_forward, Vector3.forward);

            // 回転補正->ターゲット方向への回転の順に、自身の向きを操作する
            transform.rotation = lookAtRotation * offsetRotation * Quaternion.Euler(0, 90, 0);
            animator.SetBool("Move", true);
           
            this.transform.LookAt(player.transform);

            agent.SetDestination(player.transform.position);
        }
        else
        {
            animator.SetBool("Run", false);
            transform.position += transform.forward * zWalk;

            if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();
            
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //主人公の方向
            var playerDirection = other.transform.position - transform.position;
            // 敵の前方からの主人公の方向
            var angle = Vector3.Angle(transform.forward, playerDirection);

            //　サーチする角度内だったら発見
            if (angle > searchAngle)
            {
                Debug.Log("主人公発見:");

                search = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            search = false;
        }
    }

    // サーチする角度表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
    }

    //6ch
    void GotoNextPoint()
    {
        // 地点が何も設定されていないときに返します
        if (points.Length == 0)
            return;

        // エージェントが現在設定された目標地点に行くように設定します
        agent.destination = points[destPoint].position;

        //配列内の次の位置を目標地点に設定し、
        //必要ならば出発地点に戻ります
        destPoint = (destPoint + 1) % points.Length;

        agent.SetDestination(points[destPoint].position);
    }
}
