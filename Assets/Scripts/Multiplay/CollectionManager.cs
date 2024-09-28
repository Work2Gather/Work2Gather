using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine.Rendering;


public class CollectionManager : MonoBehaviour
{

    public UserCollectionManager userCollectionManager = new UserCollectionManager();
    public JobCollectionManager jobCollectionManager = new JobCollectionManager();
    public GameCollectionManager gameCollectionManager = new GameCollectionManager();
    public CompanyCollectionManager companyCollectionManager = new CompanyCollectionManager();

    public void Initialize(IMongoDatabase database)
    {
        userCollectionManager.Initialize(database);
        jobCollectionManager.Initialize(database);
        gameCollectionManager.Initialize(database);
        companyCollectionManager.Initialize(database);
    }
}
public class UserCollectionManager
{
    private IMongoCollection<UserClass> userCollection;

    /// <summary>
    /// UserCollectionManager를 초기화하고 MongoDB 컬렉션을 설정합니다.
    /// </summary>
    /// <param name="database">연결된 MongoDB 데이터베이스</param>
    public void Initialize(IMongoDatabase database)
    {
        if (database == null)
        {
            Debug.LogError("데이터베이스가 null입니다. UserCollectionManager를 초기화할 수 없습니다.");
            return;
        }

        userCollection = database.GetCollection<UserClass>("Users");
        Debug.Log("UserCollectionManager 초기화 완료");
    }

    /// <summary>
    /// 새로운 사용자를 데이터베이스에 추가합니다.
    /// </summary>
    /// <param name="user">추가할 UserClass 객체</param>
    /// <returns>비동기 작업</returns>
    public async Task CreateUser(UserClass user)
    {
        try
        {
            await userCollection.InsertOneAsync(user);
            Debug.Log($"유저 '{user.user_name}'이(가) 성공적으로 생성되었습니다.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"유저 생성 실패: {ex.Message}");
        }
    }

    /// <summary>
    /// 주어진 사용자 이름이 이미 존재하는지 확인합니다.
    /// </summary>
    /// <param name="user_name">확인할 사용자 이름</param>
    /// <returns>사용자 이름이 존재하면 true, 그렇지 않으면 false</returns>
    public async Task<bool> CheckUserNameExists(string user_name)
    {
        try
        {
            var filter = Builders<UserClass>.Filter.Eq("user_name", user_name);
            var user = await userCollection.Find(filter).FirstOrDefaultAsync();
            return user != null;
        }
        catch (Exception ex)
        {
            Debug.LogError($"유저 이름 조회 실패: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// 주어진 ID에 해당하는 사용자를 조회합니다.
    /// </summary>
    /// <param name="user_id">조회할 사용자의 ID</param>
    /// <returns>조회된 UserClass 객체, 없으면 null</returns>
    public async Task<UserClass> GetUserById(string user_id)
    {
        try
        {
            var filter = Builders<UserClass>.Filter.Eq("_id", ObjectId.Parse(user_id));
            var user = await userCollection.Find(filter).FirstOrDefaultAsync();
            if (user != null)
            {
                Debug.Log($"유저 ID '{user_id}'를 성공적으로 조회했습니다.");
            }
            else
            {
                Debug.Log($"유저 ID '{user_id}'를 찾을 수 없습니다.");
            }
            return user;
        }
        catch (Exception ex)
        {
            Debug.LogError($"유저 조회 실패: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 주어진 ID에 해당하는 사용자의 게임 히스토리를 업데이트합니다.
    /// </summary>
    /// <param name="user_id">업데이트할 사용자의 ID</param>
    /// <param name="newGameHistory">추가할 새로운 게임 히스토리</param>
    /// <returns>비동기 작업</returns>
    public async Task UpdateUserGameHistory(string user_id, GameHistoryClass newGameHistory)
    {
        try
        {
            var filter = Builders<UserClass>.Filter.Eq("_id", ObjectId.Parse(user_id));
            var update = Builders<UserClass>.Update.Push("user_game_histories", newGameHistory);
            var result = await userCollection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                Debug.Log($"유저 ID '{user_id}'의 게임 히스토리가 성공적으로 업데이트되었습니다.");
            }
            else
            {
                Debug.Log($"유저 ID '{user_id}'를 찾을 수 없거나 업데이트하지 못했습니다.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"유저 게임 히스토리 업데이트 실패: {ex.Message}");
        }
    }
}

public class GameCollectionManager
{
    private IMongoCollection<GameClass> gameCollection;

    /// <summary>
    /// GameCollectionManager를 초기화하고 MongoDB 컬렉션을 설정합니다.
    /// </summary>
    /// <param name="database">연결된 MongoDB 데이터베이스</param>
    public void Initialize(IMongoDatabase database)
    {
        if (database == null)
        {
            Debug.LogError("데이터베이스가 null입니다. GameCollectionManager를 초기화할 수 없습니다.");
            return;
        }

        gameCollection = database.GetCollection<GameClass>("Games");
        Debug.Log("GameCollectionManager 초기화 완료");
    }

    /// <summary>
    /// 새로운 게임을 데이터베이스에 추가합니다.
    /// </summary>
    /// <param name="game">추가할 GameClass 객체</param>
    public async Task CreateGame(GameClass game)
    {
        await gameCollection.InsertOneAsync(game);
    }

    /// <summary>
    /// 주어진 ID에 해당하는 게임을 조회합니다.
    /// </summary>
    /// <param name="id">조회할 게임의 ObjectId</param>
    /// <returns>조회된 GameClass 객체, 없으면 null</returns>
    public async Task<GameClass> GetGameById(ObjectId id)
    {
        return await gameCollection.Find(g => g.game_id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 주어진 이름에 해당하는 게임을 조회합니다.
    /// </summary>
    /// <param name="name">조회할 게임의 이름</param>
    /// <returns>조회된 GameClass 객체, 없으면 null</returns>
    public async Task<GameClass> GetGameByName(string name)
    {
        return await gameCollection.Find(g => g.game_name == name).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 모든 게임을 조회합니다.
    /// </summary>
    /// <returns>모든 GameClass 객체의 리스트</returns>
    public async Task<List<GameClass>> GetAllGames()
    {
        return await gameCollection.Find(_ => true).ToListAsync();
    }
}

public class JobCollectionManager
{
    private IMongoCollection<JobClass> jobCollection;

    /// <summary>
    /// JobCollectionManager를 초기화하고 MongoDB 컬렉션을 설정합니다.
    /// </summary>
    /// <param name="database">연결된 MongoDB 데이터베이스</param>
    public void Initialize(IMongoDatabase database)
    {
        if (database == null)
        {
            Debug.LogError("데이터베이스가 null입니다. JobCollectionManager를 초기화할 수 없습니다.");
            return;
        }

        jobCollection = database.GetCollection<JobClass>("JobCategory");
        Debug.Log("JobCollectionManager 초기화 완료");
    }

    /// <summary>
    /// 새로운 직업을 데이터베이스에 추가합니다.
    /// </summary>
    /// <param name="job">추가할 JobClass 객체</param>
    public async Task CreateJob(JobClass job)
    {
        await jobCollection.InsertOneAsync(job);
    }

    /// <summary>
    /// 주어진 ID에 해당하는 직업을 조회합니다.
    /// </summary>
    /// <param name="id">조회할 직업의 ObjectId</param>
    /// <returns>조회된 JobClass 객체, 없으면 null</returns>
    public async Task<JobClass> GetJobById(ObjectId id)
    {
        return await jobCollection.Find(j => j.job_category_id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 주어진 이름에 해당하는 직업을 조회합니다.
    /// </summary>
    /// <param name="name">조회할 직업의 이름</param>
    /// <returns>조회된 JobClass 객체, 없으면 null</returns>
    public async Task<JobClass> GetJobByName(string name)
    {
        return await jobCollection.Find(j => j.job_category_name == name).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 모든 직업을 조회합니다.
    /// </summary>
    /// <returns>모든 JobClass 객체의 리스트</returns>
    public async Task<List<JobClass>> GetAllJobs()
    {
        return await jobCollection.Find(_ => true).ToListAsync();
    }
}

public class CompanyCollectionManager
{
    private IMongoCollection<CompanyClass> companyCollection;

    /// <summary>
    /// CompanyCollectionManager를 초기화하고 MongoDB 컬렉션을 설정합니다.
    /// </summary>
    /// <param name="database">연결된 MongoDB 데이터베이스</param>
    public void Initialize(IMongoDatabase database)
    {
        if (database == null)
        {
            Debug.LogError("데이터베이스가 null입니다. CompanyCollectionManager를 초기화할 수 없습니다.");
            return;
        }
        companyCollection = database.GetCollection<CompanyClass>("Companies");
        Debug.Log("CompanyCollectionManager 초기화 완료");
    }

    /// <summary>
    /// 새로운 회사를 데이터베이스에 추가합니다.
    /// </summary>
    /// <param name="company">추가할 CompanyClass 객체</param>
    public async Task CreateCompany(CompanyClass company)
    {
        await companyCollection.InsertOneAsync(company);
    }

    /// <summary>
    /// 주어진 ID에 해당하는 회사를 조회합니다.
    /// </summary>
    /// <param name="id">조회할 회사의 ObjectId</param>
    /// <returns>조회된 CompanyClass 객체, 없으면 null</returns>
    public async Task<CompanyClass> GetCompanyById(ObjectId id)
    {
        return await companyCollection.Find(c => c.company_id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 주어진 이름에 해당하는 회사를 조회합니다.
    /// </summary>
    /// <param name="name">조회할 회사의 이름</param>
    /// <returns>조회된 CompanyClass 객체, 없으면 null</returns>
    public async Task<CompanyClass> GetCompanyByName(string name)
    {
        return await companyCollection.Find(c => c.company_name == name).FirstOrDefaultAsync();
    }

    /// <summary>
    /// 모든 회사를 조회합니다.
    /// </summary>
    /// <returns>모든 CompanyClass 객체의 리스트</returns>
    public async Task<List<CompanyClass>> GetAllCompanies()
    {
        return await companyCollection.Find(_ => true).ToListAsync();
    }
}
