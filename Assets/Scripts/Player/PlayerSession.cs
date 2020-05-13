using Firebase.Auth;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSession : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseUser user;
    DatabaseHandler databaseHandler;

    private void Start()
    {
        InitializeFirebase();
        databaseHandler = FindObjectOfType<DatabaseHandler>();
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        auth.IdTokenChanged += IdTokenChanged;
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            user = auth.CurrentUser;
        }
    }

    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        if (senderAuth == auth && senderAuth.CurrentUser != null)
        {
            senderAuth.CurrentUser.TokenAsync(false);
                //.ContinueWith( task => Debug.Log(string.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
        }
    }

    public void SaveToDatabase()
    {
        Player player = ScenesData.GetPlayer();
        if(user != null)
        {
            RestClient.Put(url: "https://icorrupt.firebaseio.com/users/" + user.UserId + ".json", player);
            if(databaseHandler != null)
            {
                databaseHandler.AddScore(player.username, player.money);
            }
        }
            
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth.IdTokenChanged -= IdTokenChanged;
        auth = null;
    }
}
