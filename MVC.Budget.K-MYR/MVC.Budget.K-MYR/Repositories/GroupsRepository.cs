using MVC.Budget.K_MYR.Data;
using MVC.Budget.K_MYR.Models;

namespace MVC.Budget.K_MYR.Repositories;

public class GroupsRepository : GenericRepository<Group>, IGroupsRepository
{
    public GroupsRepository(DatabaseContext context) : base(context)
    { }
}
