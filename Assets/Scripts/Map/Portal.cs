using System.Collections;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum E_PORTAL_TYPE
{
    None = 0,
    Scene = 1,
    Position = 2,
}

public class Portal : MonoBehaviour
{
    [SerializeField] string NextSceneName;
    [SerializeField] string FImageText;
    [SerializeField] string JobiText;
    [SerializeField] string NextJobiText;
    [SerializeField] Vector3 NextPosition;
    [SerializeField] E_PORTAL_TYPE currentPortalType;
    [SerializeField] bool IsActive;
    [SerializeField] GameObject Canvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IsActive = true;
            GameManager.Instance.UIManager.FImageText.text = FImageText;
            GameManager.Instance.UIManager.FImage.SetActive(true);
            GameManager.Instance.UIManager.JobiText.text = JobiText;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            IsActive = false;
            GameManager.Instance.UIManager.FImage.SetActive(false);
            //GameManager.Instance.UIManager.JobiText.text = "";
        }
    }

    void Awake()
    {
        // if (FImageText != "")
        //     FImageText = "F를 눌러 방문하기";
    }

    void Update()
    {
        if (IsActive)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ActivatePortal();
            }
        }
    }

    void ActivatePortal()
    {
        switch (currentPortalType)
        {
            case E_PORTAL_TYPE.None:
                break;
            case E_PORTAL_TYPE.Scene:
                Canvas.SetActive(true);
                GameManager.Instance.PlayerObject.GetComponent<ExamplePlayer>().enabled = false;
                GameManager.Instance.AudioManager.BGM.Stop();
                break;
            case E_PORTAL_TYPE.Position:
                GameManager.Instance.PlayerObject.transform.GetChild(0).GetComponent<KinematicCharacterMotor>().SetPosition(NextPosition);
                GameManager.Instance.UIManager.JobiText.text = NextJobiText;
                break;
        }
    }
}
