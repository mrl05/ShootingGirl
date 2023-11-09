using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float start, end;
    private bool isRight; // moving right
    public GameObject player;
    private float positionEnemy;
    private float positionPlayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        EnemyFollowPlayer();

        CheckTurnRight();

        MoveEnemy();
    }

    protected virtual void EnemyFollowPlayer()
    {
        positionEnemy = transform.position.x;
        //Enemy follow player 
        if (player != null)
        {
            positionPlayer = player.transform.position.x;
            if (positionPlayer > start && positionPlayer < end)
            {
                if (positionPlayer < positionEnemy) isRight = false;
                if (positionPlayer > positionEnemy) isRight = true;
            }
        }
    }

    protected virtual void CheckTurnRight()
    {
        if (positionEnemy < start)
        {
            isRight = true;
        }
        if (positionEnemy > end)
        {
            isRight = false;
        }
    }

    protected virtual void MoveEnemy()
    {
        Vector2 scale = transform.localScale;
        if (isRight)
        {
            scale.x = -1;
            transform.Translate(Vector3.right * 2f * Time.deltaTime);
        }
        else
        {
            scale.x = 1;
            transform.Translate(Vector3.left * 2f * Time.deltaTime);
        }
        transform.localScale = scale;
    }

    //check 2 enemy touching other
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "left")
        {
            isRight = isRight ? false : true;
        }
    }

    public void SetStart(float start)
    {
        this.start = start;
    }
    public void SetEnd(float end)
    {
        this.end = end;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
