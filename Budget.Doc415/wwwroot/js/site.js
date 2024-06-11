function deleteCategory(id) {

    fetch(`/Category/Delete/${id}`, {
        method: 'POST'
    }).then(response => {
        window.location.href = `/Transaction/Index`;
    })
}

function deleteTransaction(id) {

    fetch(`/Transaction/Delete/${id}`, {
        method: 'POST'
    }).then(response => {
        window.location.href = `/Transaction/Index`;
    })
}

function openDeleteModal(itemId) {
    $('#itemIdToDelete').val(itemId);
    $('#deleteConfirmationModal').modal('show');
}

$('#confirmDeleteBtn').click(function () {
    let id= $('#itemIdToDelete').val();
    fetch(`/Transaction/Delete/${id}`, {
        method: 'POST'
    }).then(response => {
        window.location.href = `/Transaction/Index`;
    })
   
});

function openEditForm(id) {
    let form = document.getElementById(id);
    form.style.display = "block";
}
function closeEditForm(id) {
    let form = document.getElementById(id);
    form.style.display = "none";
}