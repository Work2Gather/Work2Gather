using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;
using UnityEngine.Networking;

#region Collections


public class UserClass
{
    #region User Class
    [BsonId]
    public ObjectId user_id { get; set; }

    [BsonElement("user_name")]
    public string user_name { get; set; }

    [BsonElement("user_character_id")]
    public int user_character_id { get; set; }

    [BsonElement("user_age")]
    public int user_age { get; set; }

    [BsonElement("user_gender")]
    public string user_gender { get; set; }

    [BsonElement("created_at")]
    public DateTime created_at { get; set; }

    [BsonElement("jobs")]
    public List<string> user_job_id { get; set; }

    [BsonElement("games")]
    public List<GameClass> games { get; set; }

    public UserClass(string user_name, int user_character_id, int user_age, string user_gender, List<string> user_current_job_list)
    {
        this.user_id = ObjectId.GenerateNewId();
        this.user_name = user_name;
        this.user_character_id = user_character_id;
        this.user_age = user_age;
        this.user_gender = user_gender;
        created_at = DateTime.UtcNow;
        user_job_id = user_current_job_list;
        games = new List<GameClass>();
    }
    #endregion
}

public class JobClass
{
    #region Job Class
    [BsonElement("job_id")]
    public ObjectId job_id { get; set; }

    [BsonElement("category_name")]
    public string category_name { get; set; }

    public JobClass(string job_id, string category_name)
    {
        this.job_id = ObjectId.GenerateNewId();
        this.category_name = category_name;
    }
    #endregion
}

public class GameClass
{
    #region Game Class
    [BsonElement("game_id")]
    public ObjectId game_id { get; set; }

    [BsonElement("game_name")]
    public string game_name { get; set; }

    [BsonElement("game_level")]
    public int game_level { get; set; }

    [BsonElement("game_score")]
    public int game_score { get; set; }

    [BsonElement("game_created_at")]
    public DateTime game_created_at { get; set; }

    [BsonElement("game_play_time")]
    public TimeSpan game_play_time { get; set; }


    public GameClass(string gameId, string gameName, int gameLevel, int gameScore)
    {
        this.game_id = ObjectId.GenerateNewId();
        this.game_name = game_name;
        this.game_level = game_level;
        this.game_score = game_score;
        game_created_at = DateTime.UtcNow;
        game_play_time = TimeSpan.Zero;
    }
    #endregion
}


public class CompanyClass
{
    #region Company Class
    [BsonId]
    public ObjectId company_id { get; set; }

    [BsonElement("company_name")]
    public string company_name { get; set; }

    [BsonElement("company_owner")]
    public UserClass company_owner { get; set; }

    [BsonElement("company_introduces")]
    public string company_introduces { get; set; }

    [BsonElement("company_contacts_email")]
    public string company_contacts_email { get; set; }

    [BsonElement("company_contacts_phone")]
    public string company_contacts_phone { get; set; }

    [BsonElement("hiring_positions")]
    public List<string> hiring_positions { get; set; }

    public CompanyClass(string company_name, UserClass company_owner, string company_introduces, string contacts_email, string contacts_phone)
    {
        this.company_id = ObjectId.GenerateNewId();
        this.company_name = company_name;
        this.company_owner = company_owner;
        this.company_introduces = company_introduces;
        company_contacts_email = contacts_email;
        company_contacts_phone = contacts_phone;
        hiring_positions = new List<string>();
    }
    #endregion
}

#endregion

public class DatabaseManager : MonoBehaviour
{
    public UserClass CurrentUserClass = null;
    #region Collection Name
    public readonly string CompaniesCollection = "Companies";
    public readonly string UsersCollection = "Users";
    public readonly string GameCategoryCollection = "GameCategory";
    public readonly string JobCategoryCollection = "JobCategory";
    #endregion
    void Start()
    {
        
    }

    public void PostUserClass(UserClass user)
    {
        CurrentUserClass = user;
        //StartCoroutine(DoPostUserClass(CurrentUserClass));
    }

    // private IEnumerator DoPostUserClass(UserClass user)
    // {
    //     string json = JsonUtility.ToJson(user);

    //     using (UnityWebRequest www = UnityWebRequest.PostWwwForm(baseURL, json))
    //     {
    //         byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
    //         www.uploadHandler = new UploadHandlerRaw(jsonToSend);
    //         www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //         www.SetRequestHeader("Content-Type", "application/json");

    //         yield return www.SendWebRequest();

    //         if (www.error == null)
    //         {
    //             Debug.Log(www.downloadHandler.text);
    //         }
    //         else
    //         {
    //             Debug.Log("error");
    //         }
    //     }
    // }

    public string GetUserClass()
    {
        string user = "";

        return user;
    }
}
