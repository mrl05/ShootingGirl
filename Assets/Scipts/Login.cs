using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;
public class Login : MonoBehaviour
{
    public TMP_InputField editUser, editPass;
    public TMP_Text txtError;
    public Selectable first;
    private EventSystem es;
    public Button btnLogin;
    public static LoginResponseModel loginResponseModel;
    // Start is called before the first frame update
    void Start()
    {
        es = EventSystem.current;
        first.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            btnLogin.onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Selectable next = es.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null) next.Select();
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            Selectable next = es.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
            if (next != null) next.Select();
        }
    }

    public void CheckLogin()
    {
        var user = editUser.text;
        var pass = editPass.text;

        UserModel userModel = new UserModel(user, pass);
        StartCoroutine(SignIn(userModel));
        SignIn(userModel);
        // call API
        // if (user.Equals("luanpc") && pass.Equals("123456"))
        // {
        //     SceneManager.LoadScene("StartGame");
        // }
        // else
        // {
        //     txtError.text = "Login failed !";
        // }
    }

    IEnumerator SignIn(UserModel userModel)

    {
        string jsonStringRequest = JsonConvert.SerializeObject(userModel);

        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/login", "POST");

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
            loginResponseModel = JsonConvert.DeserializeObject<LoginResponseModel>(jsonString);
            if (loginResponseModel.status == 1)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                txtError.text = loginResponseModel.notification;
            }
        }
    }
}
