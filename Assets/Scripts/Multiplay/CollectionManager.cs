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

    // 유저 생성
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

    // 유저 이름으로 단일 조회
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

    // 유저 ID로 단일 조회
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

    // 유저 id로 유저 조회 후 게임 히스토리 업데이트
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
    public async void CreateGame(GameClass game)
    {
        await gameCollection.InsertOneAsync(game);
    }
}



public class JobCollectionManager
{
    private IMongoCollection<JobClass> jobCollection;

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
}



public class CompanyCollectionManager
{
    private IMongoCollection<CompanyClass> companyCollection;

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
}

