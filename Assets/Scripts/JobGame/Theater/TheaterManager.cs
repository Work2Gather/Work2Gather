using TicketGame;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum E_TICKET_DIFFICULTY
{
    Low,
    Mid,
    High,
}

public class TheaterManager : MonoBehaviour
{
    [SerializeField] public TicketGameManager TicketGameManager;
    [SerializeField] public UIManager UIManager;
    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static TheaterManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static TheaterManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindFirstObjectByType(typeof(TheaterManager)) as TheaterManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            Cursor.lockState = CursorLockMode.None;
    }


    #region UI

    public void OnClickReturnButton()
    {
        SceneManager.LoadScene("1. MainTown");
    }

    public void ReturnToMainSelect()
    {
        UIManager.TicketGame.SetActive(false);
        UIManager.MainSelect.SetActive(true);
        UIManager.InGameUI.SetActive(true);
    }

    public void OnClickStartButton(int difficulty)
    {
        UIManager.MainSelect.SetActive(false);
        UIManager.InGameUI.SetActive(false);
        switch (difficulty)
        {
            case (int)E_TICKET_DIFFICULTY.Low:
                UIManager.TicketGame.SetActive(true);
                TicketGameManager.StartTicketGame();
                break;
            case (int)E_TICKET_DIFFICULTY.Mid:
                UIManager.MainSelect.SetActive(true);
                UIManager.InGameUI.SetActive(true);
                break;
            case (int)E_TICKET_DIFFICULTY.High:
                UIManager.MainSelect.SetActive(true);
                UIManager.InGameUI.SetActive(true);
                break;
        }
    }

    #endregion
}
