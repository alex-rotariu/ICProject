using Firebase.Auth;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSession : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseUser user;

    private void Start()
    {
        InitializeFirebase();
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
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

    public void SaveToDatabase()
    {
        Player player = ScenesData.GetPlayer();
        if(user != null)
            RestClient.Put(url: "https://icorrupt.firebaseio.com/users/" + user.UserId + ".json", player);
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
}
