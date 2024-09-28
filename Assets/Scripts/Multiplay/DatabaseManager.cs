using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

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

    [BsonElement("user_jobs")]
    public List<string> user_jobs { get; set; }

    [BsonElement("user_game_histories")]
    public List<GameHistoryClass> user_game_histories { get; set; }

    [BsonElement("created_at")]
    public DateTime created_at { get; set; }

    public UserClass(string user_name, int user_character_id, int user_age, string user_gender, List<string> user_current_job_list)
    {
        this.user_id = ObjectId.GenerateNewId();
        this.user_name = user_name;
        this.user_character_id = user_character_id;
        this.user_age = user_age;
        this.user_gender = user_gender;
        user_jobs = user_current_job_list;
        user_game_histories = new List<GameHistoryClass>();
        created_at = DateTime.UtcNow;
    }
    #endregion
}

public class JobClass
{
    #region Job Class
    [BsonId]
    public ObjectId job_category_id { get; set; }

    [BsonElement("job_category_name")]
    public string job_category_name { get; set; }

    public JobClass(string job_category_name)
    {
        this.job_category_id = ObjectId.GenerateNewId();
        this.job_category_name = job_category_name;
    }
    #endregion
}

public class GameClass
{
    #region Game Class
    [BsonId]
    public ObjectId game_id { get; set; }

    [BsonElement("game_name")]
    public string game_name { get; set; }

    [BsonElement("game_level")]
    public int game_level { get; set; }

    public GameClass(string game_name, int game_level)
    {
        this.game_id = ObjectId.GenerateNewId();
        this.game_name = game_name;
        this.game_level = game_level;
    }
    #endregion
}

public class GameHistoryClass
{
    #region Game History Class
    [BsonElement("game_history_id")]
    public ObjectId game_history_id { get; set; }

    [BsonElement("game_name")]
    public string game_name { get; set; }

    [BsonElement("game_level")]
    public int game_level { get; set; }

    [BsonElement("game_score")]
    public int game_score { get; set; }

    [BsonElement("game_created_at")]
    public DateTime game_created_at { get; set; }


    public GameHistoryClass(string game_name, int game_level, int game_score)
    {
        this.game_history_id = ObjectId.GenerateNewId();
        this.game_name = game_name;
        this.game_level = game_level;
        this.game_score = game_score;
        game_created_at = DateTime.UtcNow;
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

#region Mongo DB Context

public class MongoDBContext
{
    private IMongoClient mongoClient;
    public IMongoDatabase database{get; private set;}

    private string mongoDBUserName = "work2gather";

    private string mongoDBPassword = "BqaqH3zsAL3xSM1o";

    private string mongoDBName = "work2gather";

    public async Task ConnectToMongoDB()
    {
        // MongoDB 연결 문자열
        string connectionString = $"mongodb+srv://{mongoDBUserName}:{mongoDBPassword}@qualificationalitated.rc7ev.mongodb.net/";

        // MongoDB 클라이언트 생성
        mongoClient = new MongoClient(connectionString);

        // 데이터베이스 선택
        database = mongoClient.GetDatabase(mongoDBName);

        try
        {
            await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            Debug.Log("MongoDB에 연결되었습니다.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"MongoDB 연결 실패: {ex.Message}");
            database = null;
        }
    }
}

#endregion

public class DatabaseManager : MonoBehaviour
{
    public MongoDBContext mongoDBContext;
    [SerializeField] public CollectionManager collectionManager;

    // 비동기적으로 데이터베이스 연결
    async void Start()
    {
        mongoDBContext = new MongoDBContext();
        await InitializeDatabaseConnection();
    }

    private async Task InitializeDatabaseConnection()
    {
        await mongoDBContext.ConnectToMongoDB();
        if (mongoDBContext.database != null)
        {
            Debug.Log("MongoDB 연결 성공");
            InitializeCollectionManager();

            // MongoDB 연결 후 CRUD 테스트 수행, 테스트 해보려면 아래 주석 해제하기
            // await PerformAllCRUDOperations();
        }
        else
        {
            Debug.LogError("MongoDB 연결 실패");
        }
    }

    private void InitializeCollectionManager()
    {
        collectionManager.Initialize(mongoDBContext.database);
    }

    // 모든 컬렉션에 대한 CRUD 테스트를 수행하는 메서드
    private async Task PerformAllCRUDOperations()
    {
        await PerformUserCRUDOperations();
        await PerformGameCRUDOperations();
        await PerformJobCRUDOperations();
        await PerformCompanyCRUDOperations();
    }

    // User collection test CRUD operation
    private async Task PerformUserCRUDOperations()
    {
        Debug.Log("=== User CRUD Test ===");

        // 1. 새로운 유저 생성
        List<string> jobs = new List<string> { "Developer", "Designer" };
        UserClass newUser = new UserClass("JohnDoe", 101, 25, "Male", jobs);
        
        await collectionManager.userCollectionManager.CreateUser(newUser);

        // 2. 유저가 존재하는지 확인
        bool userExists = await collectionManager.userCollectionManager.CheckUserNameExists("JohnDoe");
        Debug.Log(userExists ? "JohnDoe 유저가 존재합니다." : "JohnDoe 유저가 존재하지 않습니다.");

        // 3. 유저 ID로 조회
        string userId = newUser.user_id.ToString();
        UserClass foundUser = await collectionManager.userCollectionManager.GetUserById(userId);

        if (foundUser != null)
        {
            Debug.Log($"조회된 유저 이름: {foundUser.user_name}, 나이: {foundUser.user_age}");
        }

        // 4. 유저의 게임 히스토리 업데이트
        GameHistoryClass newGameHistory = new GameHistoryClass("Game1", 5, 1000);
        await collectionManager.userCollectionManager.UpdateUserGameHistory(userId, newGameHistory);

        // 5. 유저 업데이트 후 확인
        foundUser = await collectionManager.userCollectionManager.GetUserById(userId);
        if (foundUser != null && foundUser.user_game_histories.Count > 0)
        {
            Debug.Log($"업데이트된 게임 히스토리: {foundUser.user_game_histories[0].game_name}");
        }
    }

    // Game collection test CRUD operation
    private async Task PerformGameCRUDOperations()
    {
        Debug.Log("=== Game CRUD Test ===");

        // 1. 새로운 게임 생성
        GameClass newGame = new GameClass("TestGame", 1);
        await collectionManager.gameCollectionManager.CreateGame(newGame);
        Debug.Log($"게임 생성됨: {newGame.game_name}");

        // 2. 게임 ID로 조회
        GameClass foundGame = await collectionManager.gameCollectionManager.GetGameById(newGame.game_id);
        if (foundGame != null)
        {
            Debug.Log($"ID로 조회된 게임: {foundGame.game_name}");
        }

        // 3. 게임 이름으로 조회
        foundGame = await collectionManager.gameCollectionManager.GetGameByName("TestGame");
        if (foundGame != null)
        {
            Debug.Log($"이름으로 조회된 게임: {foundGame.game_name}");
        }

        // 4. 모든 게임 조회
        List<GameClass> allGames = await collectionManager.gameCollectionManager.GetAllGames();
        Debug.Log($"전체 게임 수: {allGames.Count}");
    }

    // Job collection test CRUD operation
    private async Task PerformJobCRUDOperations()
    {
        Debug.Log("=== Job CRUD Test ===");

        // 1. 새로운 직업 생성
        JobClass newJob = new JobClass("TestJob");
        await collectionManager.jobCollectionManager.CreateJob(newJob);
        Debug.Log($"직업 생성됨: {newJob.job_category_name}");

        // 2. 직업 ID로 조회
        JobClass foundJob = await collectionManager.jobCollectionManager.GetJobById(newJob.job_category_id);
        if (foundJob != null)
        {
            Debug.Log($"ID로 조회된 직업: {foundJob.job_category_name}");
        }

        // 3. 직업 이름으로 조회
        foundJob = await collectionManager.jobCollectionManager.GetJobByName("TestJob");
        if (foundJob != null)
        {
            Debug.Log($"이름으로 조회된 직업: {foundJob.job_category_name}");
        }

        // 4. 모든 직업 조회
        List<JobClass> allJobs = await collectionManager.jobCollectionManager.GetAllJobs();
        Debug.Log($"전체 직업 수: {allJobs.Count}");
    }

    // Company collection test CRUD operation
    private async Task PerformCompanyCRUDOperations()
    {
        Debug.Log("=== Company CRUD Test ===");

        // 1. 새로운 회사 생성
        UserClass dummyOwner = new UserClass("OwnerName", 1, 30, "Male", new List<string> { "CEO" });
        CompanyClass newCompany = new CompanyClass("TestCompany", dummyOwner, "Test Description", "test@email.com", "123-456-7890");
        await collectionManager.companyCollectionManager.CreateCompany(newCompany);
        Debug.Log($"회사 생성됨: {newCompany.company_name}");

        // 2. 회사 ID로 조회
        CompanyClass foundCompany = await collectionManager.companyCollectionManager.GetCompanyById(newCompany.company_id);
        if (foundCompany != null)
        {
            Debug.Log($"ID로 조회된 회사: {foundCompany.company_name}");
        }

        // 3. 회사 이름으로 조회
        foundCompany = await collectionManager.companyCollectionManager.GetCompanyByName("TestCompany");
        if (foundCompany != null)
        {
            Debug.Log($"이름으로 조회된 회사: {foundCompany.company_name}");
        }

        // 4. 모든 회사 조회
        List<CompanyClass> allCompanies = await collectionManager.companyCollectionManager.GetAllCompanies();
        Debug.Log($"전체 회사 수: {allCompanies.Count}");
    }
}
