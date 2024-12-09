using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MongoDB.Bson;

public class CategoryService
{
    private readonly IMongoCollection<Category> _categories;

    public CategoryService(IConfiguration config)
    {
        var client = new MongoClient(config.GetSection("MongoDB:ConnectionString").Value);
        var database = client.GetDatabase(config.GetSection("MongoDB:DatabaseName").Value);
        _categories = database.GetCollection<Category>(config.GetSection("MongoDB:CategoryCollectionName").Value);
    }

    public void Create(Category category) => _categories.InsertOne(category);

    public List<Category> Get() => _categories.Find(category => true).ToList();

    public Category Get(ObjectId id) => _categories.Find<Category>(category => category.Id == id).FirstOrDefault();

    public void Update(ObjectId id, Category categoryIn) =>
        _categories.ReplaceOne(category => category.Id == id, categoryIn);

    public void Remove(ObjectId id) =>
        _categories.DeleteOne(category => category.Id == id);
}
