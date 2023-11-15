using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserModel
{
    public string username { get; set; }
    public string password { get; set; }
    public UserModel(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    // private string username;
    // private string password;

    // public UserModel(string username, string password)
    // {
    //     this.username = username;
    //     this.password = password;
    // }

    // public string getUserName()
    // {
    //     return username;
    // }
    // public void setUserName(string username)
    // {
    //     this.username = username;
    // }
}
