using DatabaseBroker.DataContext;
using Entity.Models;
using Entity.Models.Auth;

namespace DatabaseBroker.Repositories.Auth;

public class PermissionRepository(PlanetaDataContext dbContext)
    : RepositoryBase<Permission, long>(dbContext), IPermissionRepository;
