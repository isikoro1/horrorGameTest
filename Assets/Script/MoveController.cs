using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveController : MonoBehaviour
{
    private Animator animator;

    public float stamina; 
    public Slider stamina_slider; 
    public bool tired;

    public bool GameOver; 
    public float walkSpeed = 0.08f;
    public float runSpeed = 0.12f;
    public int staminaDown = 15;
    private bool running = false;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift))
        {
            running = true;
        }
        else
        {
            running = false;
        }

        //プレイヤー移動処理
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Walk", true);
            
            if (running && !tired)
            {
                transform.position += transform.forward * runSpeed;
                stamina = stamina - (Time.deltaTime * staminaDown);
            }
            else
            {
                transform.position += transform.forward * walkSpeed;
            }

        }
        else
        {
            animator.SetBool("Walk", false);
        }

        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Back", true);
            transform.position -= transform.forward * walkSpeed;
        }
        else
        {
            animator.SetBool("Back", false);
        }

        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
         
            transform.position += transform.right * walkSpeed;
        }
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
           
            transform.position -= transform.right * walkSpeed;
        }

        
        if (Input.GetKey(KeyCode.Space ) && tired == false)
        {
            
            animator.SetBool("Run", true);        
            
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            stamina = stamina + (Time.deltaTime * 5);
        }
        const float MIN = 0;
        const float MAX = 100;
        stamina = Mathf.Clamp(stamina, MIN, MAX);

        stamina_slider.value = stamina;
        

        if (GameOver == true)
        {
            SceneManager.LoadScene("GameOver");
        }

        

        //スタミナが０になった時
        if (stamina == 0)
        {
            tired = true;
        }
        else if(stamina > 40)
        {
            tired = false;
        }
    }

    private Rigidbody rb;
    private float cameraSpeed = 10f;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W) ||
            Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.D) ||
            Input.GetKeyUp(KeyCode.A))
        {
            rb.velocity = Vector3.zero;
        }

        //マウスでカメラの視点を操作する
        if (Input.GetMouseButton(0))
        {
            float x = Input.GetAxis("Mouse X") * cameraSpeed;

            if (Mathf.Abs(x) > 0.1f)
            {
                transform.RotateAround(transform.position, Vector3.up, x);
            }
            float Y = Input.GetAxis("Mouse Y") * cameraSpeed;
            if (Mathf.Abs(Y) > 0.1f)
            {
                transform.RotateAround(transform.position, Vector3.up, Y);
            }
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameOver = true;
        }
    }

}
