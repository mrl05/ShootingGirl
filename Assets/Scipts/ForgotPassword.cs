using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;
using TMPro;
public class ForgotPassword : MonoBehaviour
{
    public TMP_InputField txtUser, txtOTP, txtNewPass, txtRenewPass;
    public GameObject resetPassword, sendOTP, login;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SendOTP()
    {
        var user = txtUser.text;
        OTPModel oTPModel = new OTPModel(user);
        StartCoroutine(SendOTPAPI(oTPModel));
        SendOTPAPI(oTPModel);
    }

    //Goi API send OTP
    IEnumerator SendOTPAPI(OTPModel otpModel)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(otpModel);
        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/send-otp", "POST");
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
                // load panel reset
                resetPassword.SetActive(true);
                sendOTP.SetActive(false);
            }
            else
            {
                // hien thi thong bao
            }
        }
    }

    public void ResetPass()
    {
        var newPass = txtNewPass.text;
        var renewPass = txtRenewPass.text;
        if (newPass.Equals(renewPass))
        {
            var user = txtUser.text;
            int otp = int.Parse(txtOTP.text);
            ResetPassModel resetPassModel = new ResetPassModel(user, otp, newPass);
            StartCoroutine(ResetPassAPI(resetPassModel));
            ResetPassAPI(resetPassModel);
        }
        else
        {
            //hien thi thong bao
        }
    }
    //Goi API reset pass
    IEnumerator ResetPassAPI(ResetPassModel resetPassModel)
    {
        string jsonStringRequest = JsonConvert.SerializeObject(resetPassModel);
        var request = new UnityWebRequest("https://hoccungminh.dinhnt.com/fpt/reset-password", "POST");
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
                // return login / main menu
                resetPassword.SetActive(false);
                login.SetActive(true);
            }
            else
            {
                // hien thi thong bao
            }
        }
    }
}
