using MVC.Budget.K_MYR.Repositories;

namespace MVC.Budget.K_MYR.Data
{
    public interface IUnitOfWork
    {
        ICategoriesRepository CategoriesRepository { get; }
        ITransactionsRepository TransactionsRepository { get; }
        IGroupsRepository GroupsRepository { get; }
        ICategoryBudgetsRepository CategoryBudgetsRepository { get; }
        IFiscalPlansRepository FiscalPlansRepository { get; }

        void Dispose();
        Task Save();
    }
}