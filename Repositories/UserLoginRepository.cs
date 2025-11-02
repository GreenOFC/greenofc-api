using _24hplusdotnetcore.ModelDtos.Roles;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Repositories
{
    public interface IUserLoginRepository: IMongoRepository<UserLogin>
    {
    }
    public class UserLoginRepository: MongoRepository<UserLogin>, IUserLoginRepository, IScopedLifetime
    {
        public UserLoginRepository(IMongoDbConnection mongoDbConnection) : base(mongoDbConnection)
        {

        }

    }
}
