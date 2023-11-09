using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public int speed = 5;
    public Animator animator;
    public bool isRight = true;
    public GameObject player;
    private bool floor = true;
    public ParticleSystem psBui;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected void Move()
    {
        var playerX = player.transform.position.x;
        Vector2 scale = transform.localScale;
        animator.SetBool("isRunning", false);
        Quaternion rotation = psBui.transform.localRotation;
        //move right
        if (Input.GetKey(KeyCode.RightArrow) && playerX <= 17)
        {
            rotation.y = 180;
            psBui.transform.localRotation = rotation;
            psBui.Play();
            isRight = true;
            animator.SetBool("isRunning", true);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            scale.x *= scale.x > 0 ? 1 : -1;
            transform.localScale = scale;
        }
        //move left
        if (Input.GetKey(KeyCode.LeftArrow) && playerX >= -8.7)
        {
            rotation.y = 0;
            psBui.transform.localRotation = rotation;
            psBui.Play();
            isRight = false;
            animator.SetBool("isRunning", true);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            scale.x *= scale.x > 0 ? -1 : 1;
            transform.localScale = scale;
        }
        //jump
        if (Input.GetKey(KeyCode.Space))
        {
            if (floor)
            {
                if (isRight)
                {
                    //transform.Translate(Time.deltaTime * speed, Time.deltaTime * 2 * speed, 0);
                    //rb.AddForce(new Vector2(0, 2 * speed));
                    rb.velocity = new Vector2(rb.velocity.x, 1.5f * speed);
                    scale.x *= scale.x > 0 ? 1 : -1;
                    transform.localScale = scale;
                }
                else
                {
                    //transform.Translate(-Time.deltaTime * speed, Time.deltaTime * 2 * speed, 0);
                    //rb.AddForce(new Vector2(0, 2 * speed));
                    rb.velocity = new Vector2(rb.velocity.x, 1.5f * speed);
                    scale.x *= scale.x > 0 ? -1 : 1;
                    transform.localScale = scale;
                }
                floor = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "nen_dat")
        {
            floor = true;
        }
    }
}
