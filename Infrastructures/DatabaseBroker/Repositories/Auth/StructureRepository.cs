using DatabaseBroker.DataContext;
using Entity.Models;
using Entity.Models.Auth;

namespace DatabaseBroker.Repositories.Auth;

public class StructureRepository(PlanetaDataContext dbContext)
    : RepositoryBase<Structure, long>(dbContext), IStructureRepository;