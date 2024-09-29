using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] public UIManager UIManager;
    [SerializeField] public DatabaseManager DatabaseManager;
    [SerializeField] public RPCManager RPCManager;
    [SerializeField] public ClientRegistration ClientRegistration;
    private static TitleManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static TitleManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindFirstObjectByType(typeof(TitleManager)) as TitleManager;

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
        WelcomeSequence();
    }

    #region Title Flow

    private void WelcomeSequence()
    {
        UIManager.FadePanel.OnComplete(() =>
        {
            UIManager.WelcomeText.FadeText(0, 1, 0.5f);
        });
        UIManager.WelcomeText.OnComplete(() =>
        {
            foreach (var button in UIManager.TitleButtonArray)
                button.gameObject.SetActive(true);
        });


        UIManager.FadePanel.FadePanel(0, 0.8f, 2);
    }

    public void ConnectToWorld()
    {
        SceneManager.LoadScene("1. MainTown");
    }

    #endregion
}
