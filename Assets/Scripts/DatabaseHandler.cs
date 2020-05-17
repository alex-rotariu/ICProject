// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System.Linq;

// Handler for UI buttons on the scene.  Also performs some
// necessary setup (initializing the firebase app, etc) on
// startup.
public class DatabaseHandler : MonoBehaviour
{

    ArrayList leaderBoard;

    private const int MaxScores = 10;
    public TextMeshProUGUI displayScores;
    private new string name = "";
    private uint score = 100;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    void Start()
    {
        leaderBoard = new ArrayList();
        InitializeFirebase();
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.
        app.SetEditorDatabaseUrl("https://icorrupt.firebaseio.com/");
        if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        if (GetComponent<SceneLoader>().GetActiveScene().Equals("LeaderBoard"))
        {
            StartListener();
        }

    }

    protected void StartListener()
    {
        Debug.Log("Started");
        FirebaseDatabase.DefaultInstance
          .GetReference("leaders").OrderByChild("score")
          .ValueChanged += (object sender2, ValueChangedEventArgs e2) =>
          {
              if (e2.DatabaseError != null)
              {
                  Debug.LogError(e2.DatabaseError.Message);
                  return;
              }
              leaderBoard.Clear();
              //Debug.Log("Received values for leaders.");
              e2.Snapshot.Children.Reverse();
              if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0)
              {
                  int place = (int)e2.Snapshot.ChildrenCount;
                  foreach (var childSnapshot in e2.Snapshot.Children)
                  {
                      if (childSnapshot.Child("score") == null || childSnapshot.Child("score").Value == null)
                      {
                          //Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
                          break;
                      }
                      else
                      {
                          //Debug.Log("leaders entry : " +
                         // childSnapshot.Child("name").Value.ToString() + " - " +
                        //  childSnapshot.Child("score").Value.ToString());
                          leaderBoard.Add(place + ".  " + childSnapshot.Child("name").Value.ToString() + " - " + childSnapshot.Child("score").Value.ToString());
                          place--;
                      }
                  }
                  
                  Debug.Log(leaderBoard.Count);
                  leaderBoard.Reverse();
                  string text = "";
                  foreach (string item in leaderBoard)
                  {
                      text += item + "\n";
                  }
                  displayScores.text = text;
              }
          };
    }

    // Exit if escape (or back, on mobile) is pressed.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    // A realtime database transaction receives MutableData which can be modified
    // and returns a TransactionResult which is either TransactionResult.Success(data) with
    // modified data or TransactionResult.Abort() which stops the transaction with no changes.
    TransactionResult AddScoreTransaction(MutableData mutableData)
    {
        bool exists = false;
        List<object> leaders = mutableData.Value as List<object>;
        if (leaders == null)
        {
            leaders = new List<object>();
        }
        foreach (var child in leaders)
        {
            string player = (string)((Dictionary<string, object>)child)["name"];
            if (name.Equals(player))
            {
                leaders.Remove(child);
                exists = true;
                break;
            }
        }
        if (!exists && mutableData.ChildrenCount >= MaxScores)
        {
            // If the current list of scores is greater or equal to our maximum allowed number,
            // we see if the new score should be added and remove the lowest existing score.
            long minScore = long.MaxValue;
            object minVal = null;
            foreach (var child in leaders)
            {
                if (!(child is Dictionary<string, object>))
                    continue;
                long childScore = (long)((Dictionary<string, object>)child)["score"];
                if (childScore < minScore)
                {
                    minScore = childScore;
                    minVal = child;
                }
            }
            // If the new score is lower than the current minimum, we abort.
            if (minScore > score)
            {
                return TransactionResult.Abort();
            }
            // Otherwise, we remove the current lowest to be replaced with the new score.
            leaders.Remove(minVal);
        }

        // Now we add the new score as a new entry that contains the email address and score.
        Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
        newScoreMap["score"] = score;
        newScoreMap["name"] = name;
        leaders.Add(newScoreMap);

        // You must set the Value to indicate data at that location has changed.
        mutableData.Value = leaders;
        //return and log success
        return TransactionResult.Success(mutableData);
    }

    public void AddScore(string username, uint money)
    {
        name = username;
        score = money;

        if (score < 0 || string.IsNullOrEmpty(name))
        {

            return;
        }

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("leaders");

        // Use a transaction to ensure that we do not encounter issues with
        // simultaneous updates that otherwise might create more than MaxScores top scores.
        reference.RunTransaction(AddScoreTransaction);
        /*          .ContinueWith(task => 
                  {
                      if (task.Exception != null)
                      {
                          Debug.Log(task.Exception.ToString());
                      }
                      else if (task.IsCompleted)
                      {
                          Debug.Log("Transaction complete.");
                      }
                  });*/
    }
}