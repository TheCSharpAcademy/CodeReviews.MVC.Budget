const editCategory = (id) => {
    fetch(`Categories/Details/${id}`)
        .then(response => {
            if (!response.ok) throw new Error('Server problem occured.');
            return response.json();
        })
        .then(data => {
            $('#editCategoryModalLabel').text(`Update Category - ${data.name}`)
            $('#editCategoryModal #CategoryViewModel_Id').val($.trim(data.id));
            $('#editCategoryModal #CategoryViewModel_Name').val($.trim(data.name));
            $('#editCategoryModal').modal('show');
        })
}

const deleteCategory = (id, name) => {
    if (confirm(`Are you sure you want to delete category '${name}'?`)) {
        fetch(`Categories/Delete/${id}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (!response.ok) throw new Error('Server problem occured.');
                return response;
            })
            .then(() => window.location.reload())
            .catch(error => console.error('Deleting category failed.', error))
    }
}

const editTransaction = (id) => {
    fetch(`Transactions/Details/${id}`)
        .then(response => {
            if (!response.ok) throw new Error('Server problem occured.');
            return response.json();
        })
        .then(data => {
            $('#editTransactionModalLabel').text(`Update Transaction - ${data.description}`)
            $('#editTransactionModal #TransactionViewModel_Id').val($.trim(data.id));
            $('#editTransactionModal #TransactionViewModel_TransactionType').val($.trim(data.transactionType));
            $('#editTransactionModal #TransactionViewModel_Date').val($.trim(data.date).substring(0, 10));
            $('#editTransactionModal #TransactionViewModel_Description').val($.trim(data.description));
            $('#editTransactionModal #TransactionViewModel_Amount').val($.trim(data.amount));
            $('#editTransactionModal #TransactionViewModel_CategoryId').val($.trim(data.categoryId));
            $('#editTransactionModal').modal('show');
        })
}

const deleteTransaction = (id) => {
    if (confirm(`Are you sure you want to delete transaction? ID:${id}`)) {
        fetch(`Transactions/Delete/${id}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (!response.ok) throw new Error('Server problem occured.');
                return response;
            })
            .then(() => window.location.reload())
            .catch(error => console.error('Deleting transaction failed.', error))
    }
}