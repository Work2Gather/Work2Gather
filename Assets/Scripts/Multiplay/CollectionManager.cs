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
    // public JobCollectionManager jobCollectionManager = new JobCollectionManager();
    // public GameCollectionManager gameCollectionManager = new GameCollectionManager();
    // public CompanyCollectionManager companyCollectionManager = new CompanyCollectionManager();

    public void Initialize(IMongoDatabase database)
    {
        userCollectionManager.Initialize(database);
        // jobCollectionManager.Initialize(database);
        // gameCollectionManager.Initialize(database);
        // companyCollectionManager.Initialize(database);
    }
}
public class UserCollectionManager
{
    private IMongoCollection<UserClass> userCollection;
    public void Initialize(IMongoDatabase database)
    {
        userCollection = database.GetCollection<UserClass>("Users");
    }

    public async void CreateUser(UserClass user)
    {
        await userCollection.InsertOneAsync(user);
    }

    public async Task<UserClass> GetUserById(string userId)
    {
        // 이름으로 검색
        var filter = Builders<UserClass>.Filter.Eq("_id", ObjectId.Parse(userId));
        return await userCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<UserClass>> GetAllUsers()
    {
        return await userCollection.Find(_ => true).ToListAsync();
    }

    public async void UpdateUser(UserClass user)
    {
        var filter = Builders<UserClass>.Filter.Eq("_id", user.user_id);
        await userCollection.ReplaceOneAsync(filter, user);
    }

    public async void DeleteUser(string userId)
    {
        var filter = Builders<UserClass>.Filter.Eq("_id", ObjectId.Parse(userId));
        await userCollection.DeleteOneAsync(filter);
    }
}

// public class GameCollectionManager
// {
//     private IMongoCollection<GameClass> gameCollection;
//     public void Initialize(IMongoDatabase database)
//     {
//         gameCollection = database.GetCollection<GameClass>("Games");
//     }
// }



// public class JobCollectionManager
// {
//     private IMongoCollection<JobClass> jobCollection;

//     public void Initialize(IMongoDatabase database)
//     {
//         jobCollection = database.GetCollection<JobClass>("JobCategory");
//     }
// }



// public class CompanyCollectionManager
// {
//     private IMongoCollection<CompanyClass> companyCollection;

//     public void Initialize(IMongoDatabase database)
//     {
//         companyCollection = database.GetCollection<CompanyClass>("Companies");
//     }
// }

