using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;
public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public int speed = 5;
    public Animator animator;
    public bool isRight = true;
    public GameObject player;
    private bool floor = true;
    public ParticleSystem psBui;
    public GameObject menu;
    private bool isPlaying = true;
    private int countCoin = 0;
    public TMP_Text txtCoin;
    public AudioSource soundCoin;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //load điểm, vị trí
        if (Login.loginResponseModel.score >= 0)
        {
            countCoin = Login.loginResponseModel.score;
            txtCoin.text = countCoin + " x";
        }

        if (Login.loginResponseModel.positionX != "")
        {
            var posX = float.Parse(Login.loginResponseModel.positionX);
            var posY = float.Parse(Login.loginResponseModel.positionY);
            var posZ = float.Parse(Login.loginResponseModel.positionZ);
            transform.position = new Vector3(posX, posY, posZ);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ShootBullet();
    }

    protected void Move()
    {
        var playerX = player.transform.position.x;
        Vector2 scale = transform.localScale;
        animator.SetBool("isRunning", false);
        Quaternion rotation = psBui.transform.localRotation;
        //move right
        if (Input.GetKey(KeyCode.RightArrow) && playerX <= 30)
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

        //GetKey: Nhan giu nut
        //GetKeyDown: Nhan phim 1 lan
        //GetKeyUp: Tha phim ra

        //Pause game + show menu
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu();
        }

       
    }

    // Shooting bullet
    protected virtual void ShootBullet()
    {
         //Ban dan
        if (Input.GetKeyDown(KeyCode.S))
        {
            var x = transform.position.x + (isRight ? 1f : -1f);
            var y = transform.position.y - 0.5f;
            var z = transform.position.z;

            GameObject gameObject = (GameObject)Instantiate(Resources.Load("Prefab/bullet"),
                        new Vector3(x, y, z),
                        Quaternion.identity);

            gameObject.GetComponent<BulletScript>().setIsRight(isRight);
        }
    } 
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "nen_dat")
        {
            floor = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "coin")
        {
            soundCoin.Play();
            countCoin += 10;
            txtCoin.text = countCoin + " x";
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "checkpoint")
        {
            SavePosition();
        }
    }
    public void pauseMenu()
    {
        if (isPlaying)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
            isPlaying = false;
        }
        else
        {
            menu.SetActive(false);
            Time.timeScale = 1;
            isPlaying = true;
        }
    }

    public void SaveScore()
    {
        var user = Login.loginResponseModel.username;
        ScoreModel score = new ScoreModel(user, countCoin);

        StartCoroutine(SaveScoreAPI(score));
        SaveScoreAPI(score);
    }

    // API save score
    IEnumerator SaveScoreAPI(ScoreModel score)

    {
        string jsonStringRequest = JsonConvert.SerializeObject(score);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/save-score", "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)

        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(jsonString);
            if (responseModel.status == 1)
            {
                pauseMenu();
            }
            else
            {
                //Thong bao
            }
        }
        request.Dispose();
    }

    public void SavePosition()
    {
        var user = Login.loginResponseModel.username;
        var x = transform.position.x;
        var y = transform.position.y;
        var z = transform.position.z;

        PositionModel positionModel = new PositionModel(user, x.ToString(), y.ToString(), z.ToString());
        StartCoroutine(SavePositionAPI(positionModel));
        SavePositionAPI(positionModel);
    }
    // API luu position
    IEnumerator SavePositionAPI(PositionModel psModel)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(psModel);
        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/save-position", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStringRequest);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            var jsonString = request.downloadHandler.text.ToString();
            ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(jsonString);
            if (responseModel.status == 1)
            {
                Debug.Log("Done");
            }
            else
            {
                //goi lai API luu position
            }
        }
        request.Dispose();
    }
}
