using KinematicCharacterController.Examples;
using Michsky.DreamOS;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public RPCManager RPCManager;
    [SerializeField] public ClientManager ClientManager;
    [SerializeField] public UIManager UIManager;
    [SerializeField] public GameObject[] PlayerObjectArray;

    [SerializeField] private Vector3 SpawnPosition = Vector3.zero;
    public GameObject PlayerObject;
    private int playerId = 1;
    private static GameManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindFirstObjectByType(typeof(GameManager)) as GameManager;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RPCManager = FindFirstObjectByType<RPCManager>();
        ClientManager = FindFirstObjectByType<ClientManager>();

        if (RPCManager.Server == false)
        {
            if (SceneManager.GetActiveScene().name == "1. MainTown")
            {
                SetPlayer();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerObject == null)
        {
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
            if (SceneManager.GetActiveScene().name == "1. MainTown")
            {
                SetUI();
            }
        }
    }

    void SetPlayer()
    {
        if (RPCManager != null)
        {
            RPCManager.StartClient();
        }
        // else
        // {
        //     PlayerObject = Instantiate(PlayerObjectArray[playerId - 1], SpawnPosition, Quaternion.identity);
        // }
    }

    void SetUI()
    {
        //UIManager.CurrentMapText.text = SceneManager.GetActiveScene().name;
        UIManager.IndicatorRenderer.mainCam = PlayerObject.transform.GetChild(1).GetComponent<Camera>();
    }
}
