﻿using MVC.Budget.K_MYR.Repositories;

namespace MVC.Budget.K_MYR.Data
{
    public interface IUnitOfWork
    {
        ICategoriesRepository CategoriesRepository { get; }
        ITransactionsRepository TransactionsRepository { get; }

        void Dispose();
        Task Save();
    }
}