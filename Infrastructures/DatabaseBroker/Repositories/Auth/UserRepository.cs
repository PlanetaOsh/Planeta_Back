using DatabaseBroker.DataContext;
using Entity.Models;
using Entity.Models.Auth;

namespace DatabaseBroker.Repositories.Auth;

public class UserRepository(PlanetaDataContext dbContext) : RepositoryBase<User, long>(dbContext), IUserRepository;