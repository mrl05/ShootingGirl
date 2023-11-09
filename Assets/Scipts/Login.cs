using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Login : MonoBehaviour
{
    public TMP_InputField editUser, editPass;
    public TMP_Text txtError;
    public Selectable first;
    private EventSystem es;
    public Button btnLogin;
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

        // call API
        if (user.Equals("luanpc") && pass.Equals("123456"))
        {
            SceneManager.LoadScene("StartGame");
        }
        else
        {
            txtError.text = "Login failed !";
        }
    }
}
