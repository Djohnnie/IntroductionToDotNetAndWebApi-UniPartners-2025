using Takenlijst.DataAccess;
using Takenlijst.Models;

namespace Takenlijst.Repositories;

public class TakenRepository : Repository<Taak>
{
    public TakenRepository(TakenDbContext dbContext) : base(dbContext, dbContext.Taken) { }
}