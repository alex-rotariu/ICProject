using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class AuthController : MonoBehaviour
{
    [SerializeField] Text emailInput, passwordInput;

    public void Login()
    {
        if (emailInput == null || passwordInput == null)
            return;
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(
            task => {
                if (task.IsCanceled)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    GetErrorMessage((AuthError)e.ErrorCode);
                    return;
                }

                if (task.IsFaulted)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    GetErrorMessage((AuthError)e.ErrorCode);
                    return;
                }

                if (task.IsCompleted)
                {
                    GetComponent<SceneLoader>().loadSelectionScene();
                }
            });
    }
    

    public void RegisterUser()
    {
        if (emailInput == null || passwordInput == null)
            return;
        if (emailInput.text.Equals("") && passwordInput.text.Equals(""))
        {
            print("Please enter an email and a password");
            return;
        }

        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(emailInput.text, passwordInput.text).ContinueWith(
            task => {
                if (task.IsCanceled)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    GetErrorMessage((AuthError)e.ErrorCode);
                    return;
                }

                if (task.IsFaulted)
                {
                    Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                    GetErrorMessage((AuthError)e.ErrorCode);
                    return;
                }

                if (task.IsCompleted)
                {
                    print("Registration complete");
                }
            });
    }

    public void Logout()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            FirebaseAuth.DefaultInstance.SignOut();
            Destroy(GameObject.Find("Music Player"));
            GetComponent<SceneLoader>().loadLogInScene();
        }
    }

    void GetErrorMessage(AuthError errorCode)
    {
        string msg = "";
        msg = errorCode.ToString();
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                //code
                break;
            case AuthError.MissingPassword:
                break;
            case AuthError.WrongPassword:
                break;
            case AuthError.InvalidEmail:
                break;
        }
        print(msg);
    }
}
