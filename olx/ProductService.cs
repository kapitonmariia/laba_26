using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using MongoDB.Bson;

public class ProductService
{
    private readonly IMongoCollection<Product> _products;

    public ProductService(IConfiguration config)
    {
        var client = new MongoClient(config.GetSection("MongoDB:ConnectionString").Value);
        var database = client.GetDatabase(config.GetSection("MongoDB:DatabaseName").Value);
        _products = database.GetCollection<Product>(config.GetSection("MongoDB:ProductCollectionName").Value);
    }

    public void Create(Product product) => _products.InsertOne(product);

    public List<Product> Get() => _products.Find(product => true).ToList();

    public Product Get_by_id(ObjectId id) => _products.Find<Product>(product => product.Id == id).FirstOrDefault();

    public void Update(ObjectId id, Product productIn) =>
        _products.ReplaceOne(product => product.Id == id, productIn);

    public void Remove(ObjectId id) =>
        _products.DeleteOne(product => product.Id == id);

    public List<Product> GetByCategory(ObjectId categoryId) =>
        _products.Find(product => product.CategoryId == categoryId).ToList();
}
