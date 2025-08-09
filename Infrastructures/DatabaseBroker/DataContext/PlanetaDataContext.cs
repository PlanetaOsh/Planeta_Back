using Entity.Enums;
using Entity.Models.Auth;
using Entity.Models.Common;
using Entity.Models.ReferenceBook;
using Entity.Models.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace DatabaseBroker.DataContext;

public class PlanetaDataContext : DbContext
{
    public PlanetaDataContext(DbContextOptions<PlanetaDataContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    /*private T GetService<T>() where T : class
    {
        var serviceProvider = this.GetInfrastructure<IServiceProvider>();
        return serviceProvider.GetService<T>();
    }
    
    private int GetCurrentUserId()
    {
        var httpContextAccessor = GetService<IHttpContextAccessor>();
        var httpContextAccessor2 = GetService<IMosqueRepository>();
        var userIdClaim = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
    
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            return userId;
    
        return -1;
    }*/
    
    private void TrackActionsAt()
    {
        var dateTimeNow = DateTime.Now;
        //var userId = GetCurrentUserId();
        
        foreach (var entity in ChangeTracker.Entries()
                     .Where(x => x is { State: EntityState.Added, Entity: AuditableModelBase<long> }))
        {
            var model = (AuditableModelBase<long>)entity.Entity;
            model.CreatedAt = dateTimeNow;
            //model.CreatedBy = userId;
        }

        foreach (var entity in ChangeTracker.Entries()
                     .Where(x => x is { State: EntityState.Modified, Entity: AuditableModelBase<long> }))
        {
            var model = (AuditableModelBase<long>)entity.Entity;
            model.UpdatedAt = dateTimeNow;
            //model.UpdatedBy = userId;
        }
    }

    public override int SaveChanges()
    {
        TrackActionsAt();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        TrackActionsAt();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new CancellationToken())
    {
        TrackActionsAt();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        TrackActionsAt();
        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //Configuring all MultiLanguage fields over entities
        var multiLanguageFields = modelBuilder
            .Model
            .GetEntityTypes()
            .SelectMany(x => x.ClrType.GetProperties())
            .Where(x => x.PropertyType == typeof(MultiLanguageField));

        foreach (var multiLanguageField in multiLanguageFields)
            modelBuilder
                .Entity(multiLanguageField.DeclaringType!)
                .Property(multiLanguageField.PropertyType, multiLanguageField.Name)
                .HasColumnType("jsonb");

        modelBuilder
            .Entity<SignMethod>()
            .HasOne(x => x.User)
            .WithMany(x => x.SignMethods)
            .HasForeignKey(x => x.UserId);

        modelBuilder
            .Entity<SignMethod>()
            .HasDiscriminator(x => x.Type)
            .HasValue<DefaultSignMethod>(SignMethods.Normal);
    }
    
    #region Auth schema
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Structure> Structures { get; set; }
    public DbSet<StructurePermission> StructurePermissions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<SignMethod> UserSignMethods { get; set; }
    public DbSet<TokenModel> Tokens { get; set; }
    #endregion

    #region Static file schema
    public DbSet<StaticFile> StaticFiles { get; set; }
    #endregion

    #region Reference Book schema
    public DbSet<Country> Countries { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<District> Districts { get; set; }
    #endregion
}