using System.Collections;
using KinematicCharacterController.Examples;
using UnityEngine;

public class Booth : MonoBehaviour
{
    [SerializeField] bool IsActive;
    [SerializeField] GameObject BoothImage;
    [SerializeField] Camera BoothCamera;
    [SerializeField] ExamplePlayer Player;
    bool IsVisited;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameManager.Instance.PlayerObject.GetComponentInChildren<ExamplePlayer>();
        if(Player == null)
        {
            Player = FindFirstObjectByType<ExamplePlayer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ActivateBooth();
            }
        }

        if (IsVisited)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape))
            {
                DeActivateBooth();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IsActive = true;
            GameManager.Instance.UIManager.FImageText.text = "F를 눌러 방문하기";
            GameManager.Instance.UIManager.FImage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            IsActive = false;
            GameManager.Instance.UIManager.FImage.SetActive(false);
        }
    }

    void ActivateBooth()
    {
        IsActive = false;
        GetComponent<SphereCollider>().enabled = false;
        Player.enabled = false;
        
        GameManager.Instance.UIManager.FImage.SetActive(false);
        BoothImage.SetActive(true);
        BoothCamera.gameObject.SetActive(true);

        if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;

        StartCoroutine(DoDelayActivateBooth());
    }

    void DeActivateBooth()
    {
        BoothImage.SetActive(false);
        BoothCamera.gameObject.SetActive(false);
        GetComponent<SphereCollider>().enabled = true;

        Player.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

        IsVisited = false;
    }

    private IEnumerator DoDelayActivateBooth()
    {
        yield return new WaitForSeconds(0.25f);

        IsVisited = true;
    }
}
