using MVC.Budget.K_MYR.Repositories;

namespace MVC.Budget.K_MYR.Data
{
    public interface IUnitOfWork
    {
        ICategoriesRepository CategoriesRepository { get; }
        ITransactionsRepository TransactionsRepository { get; }
        IGroupsRepository GroupsRepository { get; }
        ICategoryBudgetsRepository CategoryBudgetRepository { get; }

        void Dispose();
        Task Save();
    }
}