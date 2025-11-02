using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IMongoRepository<TDocument> where TDocument : IBaseDocument
    {
        IQueryable<TDocument> AsQueryable();

        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression);

        Task<IEnumerable<TDocument>> FilterByAsync( Expression<Func<TDocument, bool>> filterExpression);

        Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression, int limit);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);

        TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        TDocument FindById(string id);

        Task<TDocument> FindByIdAsync(string id);

        void InsertOne(TDocument document);

        Task InsertOneAsync(TDocument document);

        void InsertMany(ICollection<TDocument> documents);

        Task InsertManyAsync(ICollection<TDocument> documents);

        void ReplaceOne(TDocument document);

        Task ReplaceOneAsync(TDocument document);

        void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        void DeleteById(string id);

        Task DeleteByIdAsync(string id);

        void DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);

        void UpdateOne(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> updateDefinition);
        Task UpdateOneAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> updateDefinition, UpdateOptions updateOptions = null);

        Task UpdateManyAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> updateDefinition, UpdateOptions updateOptions = null);

        Task DeleteManyAsync(ICollection<TDocument> documents);

        void UpdateMany(ICollection<TDocument> documents);

        Task UpdateManyAsync(ICollection<TDocument> documents);

        IMongoCollection<TDocument> GetCollection();

        /// <summary>
        /// Author : Chung Thanh Phuoc
        /// Check data has been exists
        /// Parameter is lambda expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> GetAnyAsync(Expression<Func<TDocument, bool>> predicate);

        Task<bool> GetAnyAsync<TDerivedDocument>(Expression<Func<TDerivedDocument, bool>> predicate) where TDerivedDocument : TDocument;
    }

    public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IBaseDocument
    {
        protected readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IMongoDbConnection connection)
        {
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public async virtual Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return await _collection.Find(filterExpression).ToListAsync();
        }

        public async virtual Task<IEnumerable<TDocument>> FilterByAsync(Expression<Func<TDocument, bool>> filterExpression, int limit)
        {
            return await _collection.Find(filterExpression).Limit(limit).ToListAsync();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TDocument FindById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            if (typeof(TDocument).GetInterfaces().Contains(typeof(ISoftDelete)))
            {
                filter &= Builders<TDocument>.Filter.Ne(nameof(ISoftDelete.IsDeleted), true);
            }
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                if (typeof(TDocument).GetInterfaces().Contains(typeof(ISoftDelete)))
                {
                    filter &= Builders<TDocument>.Filter.Ne(nameof(ISoftDelete.IsDeleted), true);
                }
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }

        public virtual Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            _collection.InsertMany(documents);
        }

        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, id);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }

        public void UpdateOne(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> updateDefinition)
        {
            _collection.UpdateOne(filterExpression, updateDefinition);
        }

        public async Task UpdateOneAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> updateDefinition, UpdateOptions updateOptions = null)
        {
            await _collection.UpdateOneAsync(filterExpression, updateDefinition, updateOptions);
        } 
        
        public async Task UpdateManyAsync(Expression<Func<TDocument, bool>> filterExpression, UpdateDefinition<TDocument> updateDefinition, UpdateOptions updateOptions = null)
        {
            await _collection.UpdateManyAsync(filterExpression, updateDefinition, updateOptions);
        }

        public Task DeleteManyAsync(ICollection<TDocument> documents)
        {
            return Task.Run(() =>
            {
                var listOfReplaceOneModels = documents.Select(document => new DeleteOneModel<TDocument>(Builders<TDocument>.Filter.Where(x => x.Id == document.Id)));
                return _collection.BulkWriteAsync(listOfReplaceOneModels);
            });
        }

        public void UpdateMany(ICollection<TDocument> documents)
        {
            var listOfReplaceOneModels = documents.Select(document => new ReplaceOneModel<TDocument>(Builders<TDocument>.Filter.Where(x => x.Id == document.Id), document));
            _collection.BulkWrite(listOfReplaceOneModels);
        }

        public Task UpdateManyAsync(ICollection<TDocument> documents)
        {
            return Task.Run(() =>
            {
                var listOfReplaceOneModels = documents.Select(document => new ReplaceOneModel<TDocument>(Builders<TDocument>.Filter.Where(x => x.Id == document.Id), document));
                return _collection.BulkWriteAsync(listOfReplaceOneModels);
            });
        }

        public IMongoCollection<TDocument> GetCollection()
        {
            return _collection;
        }

        /// <summary>
        /// Author : Chung Thanh Phuoc
        /// Check data has been exists
        /// Parameter is lambda expression
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> GetAnyAsync(Expression<Func<TDocument, bool>> predicate)
        {
            return await _collection.Find(predicate).AnyAsync();
        }

        public async Task<bool> GetAnyAsync<TDerivedDocument>(Expression<Func<TDerivedDocument, bool>> predicate) where TDerivedDocument : TDocument
        {
            return await _collection.OfType<TDerivedDocument>().Find(predicate).AnyAsync();
        }
    }
}
