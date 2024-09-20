using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserClass
{
    public string user_name;
    public int user_character_id;
    public int user_age;
    public string user_gender;
    public List<long> user_job_id;

    public UserClass(string user_name, int user_character_id, int user_age, string user_gender, List<long> user_job_id)
    {
        this.user_name = user_name;
        this.user_character_id = user_character_id;
        this.user_age = user_age;
        this.user_gender = user_gender;
        this.user_job_id = user_job_id;
    }
}

public class HTTPManager : MonoBehaviour
{
    public UserClass CurrentUserClass = null;
    [SerializeField] private string baseURL;
    void Start()
    {

    }

    public void PostUserClass(UserClass user)
    {
        CurrentUserClass = user;
        //StartCoroutine(DoPostUserClass(CurrentUserClass));
    }

    private IEnumerator DoPostUserClass(UserClass user)
    {
        string json = JsonUtility.ToJson(user);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(baseURL, json))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.error == null)
            {
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("error");
            }
        }
    }

    public string GetUserClass()
    {
        string user = "";

        return user;
    }
}
