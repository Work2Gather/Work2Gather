using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatManager : MonoBehaviour
{
    public void OnClickReturnButton()
    {
        transform.parent.gameObject.SetActive(false);
        GameManager.Instance.PlayerObject.GetComponent<ExamplePlayer>().enabled = true;
        GameManager.Instance.AudioManager.BGM.Play();
        //SceneManager.LoadScene("1. MainTown");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
