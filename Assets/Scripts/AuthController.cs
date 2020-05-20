using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using Models;
using System.ComponentModel;
using Proyecto26;
using System.Resources;
using System;
using UnityEditor;


public class AuthController : MonoBehaviour
{
    private DatabaseHandler databaseHandler;
    [SerializeField] Text emailInput, passwordInput;
    [SerializeField] Text errorText;


    private void Start()
    {
        databaseHandler = GetComponent<DatabaseHandler>();
    }

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

                    FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;                
                    Player player = new Player();
                    RestClient.Get<Player>(url: "https://icorrupt.firebaseio.com/users/" + user.UserId + ".json").Then(response =>
                    {
                        player = response;
                        ScenesData.SetPlayer(player);
                        NextScene(player);
                    });
                }
            });
    }
    
    private void NextScene(Player player)
    {
        if (player.character == -1)
            GetComponent<SceneLoader>().loadSelectionScene();
        else
            GetComponent<SceneLoader>().loadGameSceneLoggedIn();
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
        GameObject username = GameObject.Find("Username Input");
        Text usernameText = username.GetComponent<Text>();

        if (usernameText.text.Equals(""))
        {
            errorText.text = "Please enter a username";
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
                    if (usernameText != null)
                    {
                        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
                        UserProfile profile = new UserProfile();
                        profile.DisplayName = usernameText.text;
                        user.UpdateUserProfileAsync(profile);
                        Player player = new Player();
                        player.username = usernameText.text;
                        player.money = 0;
                        player.character = -1;
                        player.moneyPerSecond = 0;
                        player.moneyPerClick = 1;
                        player.upgradeBought = new bool[50];
                        player.lastSaveTime = System.DateTime.UtcNow.ToString();

                        RestClient.Put(url: "https://icorrupt.firebaseio.com/users/" + user.UserId + ".json", player);

                        databaseHandler.AddScore(player.username, player.money);
                    }
                    errorText.text = "Registration complete";
                    
                }
            });
    }


    public void Logout()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            GameObject player = FindObjectOfType<MusicPlayer>().gameObject;
            GameObject options = FindObjectOfType<OptionsController>().gameObject;
            Destroy(player);
            Destroy(options);
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
