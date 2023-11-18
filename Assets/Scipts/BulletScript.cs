using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private bool isRight;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((isRight ? Vector3.right : Vector3.left) * Time.deltaTime * 5f);
    }

    public void setIsRight(bool isRight)
    {
        this.isRight = isRight;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "left")
        {
            Destroy(gameObject);
            var name = other.attachedRigidbody.name;
            Destroy(GameObject.Find(name));
        }
    }
}
