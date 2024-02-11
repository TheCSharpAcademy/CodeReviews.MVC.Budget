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
          $('#editTransactionModalLabel').text(`Update Transaction - ${data.id}`)
          $('#editTransactionModal #TransactionViewModel_Id').val($.trim(data.id));
          $('#editTransactionModal #TransactionViewModel_TransactionType').val($.trim(data.transactionType));
          $('#editTransactionModal #TransactionViewModel_Date').val($.trim(data.date).substring(0,10));
          $('#editTransactionModal #TransactionViewModel_Description').val($.trim(data.description));
          $('#editTransactionModal #TransactionViewModel_Amount').val($.trim(data.amount));
          $('#editTransactionModal #TransactionViewModel_CategoryId').val($.trim(data.categoryId));
          $('#editTransactionModal').modal('show');
       })
}