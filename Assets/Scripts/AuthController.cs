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
    [SerializeField] Text errorText;

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
                    errorText.text = "Logged In";
                    GetComponent<SceneLoader>().loadSelectionScene();
                }
            });
    }
    

    public void RegisterUser()
    {
        if (emailInput == null || passwordInput == null)
            return;
        if (emailInput.text.Equals(""))
        {
            errorText.text = "Please enter a username";
            return;
        }

        if (passwordInput.text.Equals(""))
        {
            errorText.text = "Please enter a password";
            return;
        }

        if(passwordInput.text.Length > 35)
        {
            errorText.text = "Password too long";
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
                    emailInput.text = "";
                    passwordInput.text = "";
                    errorText.text = "Registration complete";
                    
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
       
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                msg = "Account with this email exists";
                break;
            case AuthError.EmailAlreadyInUse:
                msg = "Email is already in use";
                break;
            case AuthError.MissingPassword:
                msg = "Password is missing";
                break;
            case AuthError.WrongPassword:
                msg = "Wrong password";
                break;
            case AuthError.InvalidEmail:
                msg = "Invalid email";
                break;
            case AuthError.WeakPassword:
                msg = "Enter a better password";
                break;
            case AuthError.MissingEmail:
                msg = "Email is missing";
                break;
            case AuthError.UserNotFound:
                msg = "User not found";
                break;
            default:
                msg = "An error occured";
                break;
        }
        errorText.text = msg;
    }
}
