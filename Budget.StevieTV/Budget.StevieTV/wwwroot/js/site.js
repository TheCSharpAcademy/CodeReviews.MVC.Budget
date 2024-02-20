// TRANSACTIONS
function editTransaction(id) {
    fetch(`Transaction/Details/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Invalid response from server when getting Transaction');
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
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

function deleteTransaction(id) {
    if (confirm(`Are you sure you wish to delete transaction number ${id}`)) {
        fetch(`Transaction/Delete/${id}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Invalid response from server when getting Transaction');
                }
                return response;
            })
            .then(() => window.location.reload())
            .catch(error => console.error('Unable to delete the transaction', error))
    }
}

// CATEGORIES
function editCategory(id) {
    fetch(`Category/Details/${id}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Invalid response from server when getting Transaction');
            }
            return response.json();
        })
        .then(data => {
            console.log(data);
            $('#editCategoryModalLabel').text(`Update Category - ${data.name}`)
            $('#editCategoryModal #CategoryViewModel_Id').val($.trim(data.id));
            $('#editCategoryModal #CategoryViewModel_Name').val($.trim(data.name));
            $('#editCategoryModal').modal('show');
        })
}

function deleteCategory(id, name) {
    debugger;
    if (confirm(`Are you sure you wish to delete category ${name} [${id}]`)) {
        fetch(`Category/Delete/${id}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Invalid response from server when getting Transaction');
                }
                return response;
            })
            .then(() => window.location.reload())
            .catch(error => console.error('Unable to delete the category', error))
    }
}