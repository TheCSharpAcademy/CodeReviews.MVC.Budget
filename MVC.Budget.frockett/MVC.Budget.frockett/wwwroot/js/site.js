
fetchDefaultTransactions();
handleScroll();

let allTransactions = [];
const sortDirections = {};
function fetchDefaultTransactions() {
    fetch('/api/TransactionsAPI')
        .then(response => response.json())
        .then(data => {
            console.log(data);
            allTransactions = data.$values;
            renderTable(allTransactions);
        });
}

function renderTable(transactions) {
    handleFocus(null);
    fetchCategories();
    const list = document.getElementById('transactionList');
    list.innerHTML = '';

    const tableHead = document.createElement('thead');
    tableHead.classList.add('table-head')
    const headerRow = document.createElement('tr');

    const headerTitle = document.createElement('th');
    headerTitle.scope = 'col';
    headerTitle.innerText = 'Name';
    headerTitle.addEventListener('click', () => {
        sortTable(0);
        handleFocus(headerTitle);
    });

    const headerCat = document.createElement('th');
    headerCat.scope = 'col';
    headerCat.addEventListener('click', () => {
        sortTable(1);
        handleFocus(headerCat);
    });
    headerCat.innerText = 'Category';

    const headerAmount = document.createElement('th');
    headerAmount.scope = 'col';
    headerAmount.addEventListener('click', () => {
        sortTable(2);
        handleFocus(headerAmount);
    });
    headerAmount.innerText = 'Amount';

    const headerDate = document.createElement('th');
    headerDate.scope = 'col';
    headerDate.addEventListener('click', () => {
        sortTable(3);
        handleFocus(headerDate);
    });
    headerDate.innerText = 'Date';

    const buttonCell = document.createElement('th');
    buttonCell.scope = 'col';

    headerRow.appendChild(headerTitle);
    headerRow.appendChild(headerCat);
    headerRow.appendChild(headerAmount);
    headerRow.appendChild(headerDate);
    headerRow.appendChild(buttonCell);

    tableHead.appendChild(headerRow);

    list.appendChild(tableHead);

    const tbody = document.createElement('tbody');
    tbody.classList.add('table-body');

    transactions.forEach(transactionEntity => {
        const transaction = document.createElement('tr');
        transaction.className = 'transaction';

        const title = document.createElement('td');
        title.textContent = transactionEntity.title;

        const category = document.createElement('td');
        category.textContent = transactionEntity.categoryName;

        const value = document.createElement('td');
        value.textContent = transactionEntity.amount.toFixed(2);

        const date = document.createElement('td');
        date.textContent = new Date(transactionEntity.dateTime).toLocaleDateString();

        const buttonContainer = document.createElement('td');
        buttonContainer.className = 'button-container';

        const editButton = document.createElement('button');
        editButton.innerHTML = '<i class="fa-regular fa-pen-to-square"></i>'
        editButton.onclick = () => showEditForm(transactionEntity);
        editButton.className = 'btn btn-light btn-sm';

        const deleteButton = document.createElement('button');
        deleteButton.innerHTML = '<i class="fa-solid fa-trash"></i>'
        deleteButton.onclick = () => showDeleteModal(transactionEntity);
        editButton.className = 'btn btn-light btn-sm';

        buttonContainer.appendChild(editButton);
        buttonContainer.appendChild(deleteButton);

        transaction.appendChild(title);
        transaction.appendChild(category);
        transaction.appendChild(value);
        transaction.appendChild(date);
        transaction.appendChild(buttonContainer);

        tbody.appendChild(transaction)

    });

    list.appendChild(tbody)
}

function filterTransactions() {
    const nameFilterValue = document.getElementById('nameFilter').value.toLowerCase();
    const amountFilterValue = parseFloat(document.getElementById('amountFilter').value);
    const categoryFilterValue = document.getElementById('categoryFilter').value.toLowerCase();


    const filteredTransactions = allTransactions.filter(transaction => {
        const matchesName = transaction.title.toLowerCase().includes(nameFilterValue);
        const matchesAmount = isNaN(amountFilterValue) || transaction.amount === amountFilterValue;
        const matchesCategory = transaction.categoryName.toLowerCase().includes(categoryFilterValue);

        return matchesName && matchesAmount && matchesCategory;
    });

    renderTable(filteredTransactions);
}

function sortTable(index) {
    let table, rows, switching, i, x, y, shouldSwitch, direction, switchCount = 0;
    table = document.getElementById("transactionList");
    switching = true;

    if (!sortDirections[index]) {
        sortDirections[index] = "asc";
    }

    // Toggle the sorting direction:
    direction = sortDirections[index] === "asc" ? "desc" : "asc";
    sortDirections[index] = direction;

    while (switching) {
        switching = false;
        rows = table.rows;

        for (i = 1; i < rows.length - 1; i++) {
            shouldSwitch = false;
            x = rows[i].getElementsByTagName("td")[index];
            y = rows[i + 1].getElementsByTagName("td")[index];

            if (direction === "asc") {
                // Sort alphabetically if not a number
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase() && isNaN(x.innerHTML)) {
                    shouldSwitch = true;
                    break;
                }
                // Sort numerically if it is a number
                else if ((!isNaN(x.innerHTML) && !isNaN(y.innerHTML)) && parseFloat(x.innerHTML) > parseFloat(y.innerHTML)) {
                    shouldSwitch = true;
                    break;
                }
            } else if (direction === "desc") {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase() && isNaN(x.innerHTML)) {
                    shouldSwitch = true;
                    break;
                }
                else if ((!isNaN(x.innerHTML) && !isNaN(y.innerHTML)) && parseFloat(x.innerHTML) < parseFloat(y.innerHTML)) {
                    shouldSwitch = true;
                    break;
                }
            }
        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            switchCount++;
        }
    }
}

function handleFocus(header) {
    const allHeaders = document.getElementsByTagName("th");
    for (let i = 0; i < allHeaders.length; i++) {
        allHeaders[i].classList.remove('th-focus');
    }
    if (header !== null) {
        header.classList.add('th-focus');
    }
    
}
function toggleAddModal() {
    const addModal = document.getElementById('addTransactionModal');
    const overlay = document.querySelector('.overlay');

    if (addModal.style.display === 'block') {
        overlay.classList.add("hidden");
        addModal.style.display = 'none';
        document.getElementById('add-transactionTitle').value = '';
        document.getElementById('add-transactionAmount').value = '';
        document.getElementById('add-transactionDate').value = '';
    }
    else {
        addModal.style.display = 'block';
        overlay.classList.remove("hidden");
    }
}

function toggleAddCategoryModal() {
    const addCategoryModal = document.getElementById('addCategoryModal');
    const overlay = document.querySelector('.overlay');

    if (addCategoryModal.style.display === 'block') {
        overlay.classList.add("hidden");
        addCategoryModal.style.display = 'none';
        document.getElementById('add-categoryName').value = '';
    } else {
        addCategoryModal.style.display = 'block';
        overlay.classList.remove("hidden");
        fetchCategories();
    }
}

function toggleEditModal() {
    const editModal = document.getElementById('editTransactionModal');
    const overlay = document.querySelector('.overlay');

    if (editModal.style.display === 'block') {
        overlay.classList.add("hidden");
        editModal.style.display = 'none';
        document.getElementById('edit-transactionTitle').value = '';
        document.getElementById('edit-transactionAmount').value = '';
        document.getElementById('edit-transactionDate').value = '';
    } else {
        editModal.style.display = 'block';
        overlay.classList.remove("hidden");
    }
}

function showEditForm(transaction) {
    document.getElementById('edit-transactionTitle').value = transaction.title;
    document.getElementById('edit-transactionCategory').value = transaction.categoryId;
    document.getElementById('edit-transactionAmount').value = transaction.amount.toFixed(2);
    document.getElementById('edit-transactionDate').value = transaction.dateTime.split('T')[0];
    document.getElementById('editTransactionForm').onsubmit = function (e) {
        e.preventDefault();
        updateTransaction(transaction.id);
    };

    toggleEditModal();
}

function updateTransaction(id) {
    const title = document.getElementById('edit-transactionTitle').value;
    const categoryId = document.getElementById('edit-transactionCategory').value;
    const amount = document.getElementById('edit-transactionAmount').value;
    const date = document.getElementById('edit-transactionDate').value;

    const updatedTransaction = {
        id: id,
        title: title,
        amount: parseFloat(amount),
        dateTime: new Date(date).toISOString(),
        categoryId: parseInt(categoryId),
    };

    fetch(`/api/TransactionsAPI/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(updatedTransaction)
    })
        .then(response => {
            if (response.status === 204) {
                return null;
            }
            return response.json();
        })
        .then(data => {
            toggleEditModal();
            fetchDefaultTransactions();
        })
        .catch(error => console.error('Error updating transaction:', error));
}

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('addCategoryForm').addEventListener('submit', function (e) {
        e.preventDefault();
        addCategory();
    });
});

function addCategory() {
    const name = document.getElementById('add-categoryName').value;

    const category = {
        name: name
    };

    fetch('/api/CategoriesAPI', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(category)
    })
        .then(response => response.json())
        .then(data => {
            toggleAddCategoryModal();
            fetchCategories();
        })
        .catch(error => console.error('Error adding category:', error));
}

document.addEventListener('DOMContentLoaded', (event) => {
    fetchCategories();

    document.getElementById('addTransactionForm').addEventListener('submit', function (e) {
        e.preventDefault();
        addTransaction();
    });
});

function fetchCategories() {
    fetch('/api/CategoriesAPI')
        .then(response => response.json())
        .then(data => {
            const categories = data.$values;
            const categoryList = document.getElementById('categoryList').getElementsByTagName('tbody')[0];
            categoryList.innerHTML = '';

            categories.forEach(category => {
                const row = document.createElement('tr');
                const nameCell = document.createElement('td');
                nameCell.textContent = category.name;
                row.appendChild(nameCell);

                const actionsCell = document.createElement('td');
                const editButton = document.createElement('button');
                editButton.className = 'btn btn-light btn-sm';
                editButton.innerHTML = '<i class="fa-regular fa-pen-to-square"></i>';
                editButton.onclick = () => showEditCategoryForm(category);
                actionsCell.appendChild(editButton);

                const deleteButton = document.createElement('button');
                deleteButton.className = 'btn btn-light btn-sm';
                deleteButton.innerHTML = '<i class="fa-solid fa-trash"></i>';
                deleteButton.onclick = () => confirmDelete(category);
                actionsCell.appendChild(deleteButton);

                row.appendChild(actionsCell);
                categoryList.appendChild(row);
            });
            populateCategoryDropdowns(categories);
        })
        .catch(error => console.error('Error fetching categories:', error));
}

function populateCategoryDropdowns(categories) {
    const categorySelect = document.getElementById('add-transactionCategory');
    const editCategorySelect = document.getElementById('edit-transactionCategory');

    if (categorySelect) {
        categorySelect.innerHTML = '';
        categories.forEach(category => {
            const option = document.createElement('option');
            option.value = category.id;
            option.text = category.name;
            categorySelect.appendChild(option);
        });
    }

    if (editCategorySelect) {
        editCategorySelect.innerHTML = '';
        categories.forEach(category => {
            const option = document.createElement('option');
            option.value = category.id;
            option.text = category.name;
            editCategorySelect.appendChild(option);
        });
    }
}

function showAddCategoryForm() {
    toggleCategoryForm();
}

function toggleCategoryForm() {
    const categoryForm = document.getElementById('addCategoryForm');
    categoryForm.style.display = categoryForm.style.display === 'none' ? 'block' : 'none';
}

function addCategory() {
    const name = document.getElementById('add-categoryName').value;

    const category = {
        name: name
    };

    fetch('/api/CategoriesAPI', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(category)
    })
        .then(response => response.json())
        .then(data => {
            toggleAddCategoryModal();
            fetchCategories();
        })
        .catch(error => console.error('Error adding category:', error));
}

function showEditCategoryForm(category) {
    toggleCategoryForm();

    document.getElementById('add-categoryName').value = category.name;
    document.getElementById('addCategoryForm').onsubmit = function (e) {
        e.preventDefault();
        updateCategory(category.id);
    };
}

function updateCategory(id) {
    const name = document.getElementById('add-categoryName').value;

    const category = {
        id: id,
        name: name
    };

    fetch(`/api/CategoriesAPI/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(category)
    })
        .then(response => response.json())
        .then(data => {
            toggleAddCategoryModal();
            fetchCategories();
        })
        .catch(error => console.error('Error updating category:', error));
}

let categoryIdToDelete = null;

function confirmDelete(category) {
    categoryIdToDelete = category.id;
    console.log(category);
    const confirmDeleteBody = document.getElementById('confirm-delete-modal-body');
    confirmDeleteBody.innerText = `Are you sure you want to delete ${category.name} and ${category.transactions.length} transactions?`;

    $('#confirmDeleteModal').modal('show');

    document.getElementById('confirmDeleteButton').addEventListener('click', function () {
        if (categoryIdToDelete !== null) {
            fetch(`/api/CategoriesAPI/${categoryIdToDelete}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        fetchCategories(fetchCategories);
                        $('#confirmDeleteModal').modal('hide');
                    } else {
                        console.error('Error deleting category');
                    }
                })
                .catch(error => console.error('Error deleting category:', error));
        }
    });

    document.getElementById('cancelDeleteButton').addEventListener('click', function () {
        $('#confirmDeleteModal').modal('hide');
    })
}



function addTransaction() {
    const transaction = {
        title: document.getElementById('add-transactionTitle').value,
        amount: parseFloat(document.getElementById('add-transactionAmount').value),
        dateTime: new Date(document.getElementById('add-transactionDate').value).toISOString(),
        categoryId: parseInt(document.getElementById('add-transactionCategory').value)
    }

    fetch('api/TransactionsAPI', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(transaction)
    })
    .then(response => response.json())
    .then(data => {
        toggleAddModal();
        fetchDefaultTransactions();
    })
        .catch(error => console.error('Error adding transaction: ', error));
}

// Track scroll position, change background color of filter form once the user has scrolled
function handleScroll() {
    $(window).scroll(function () {
        const form = document.getElementById('filter-form')
        let scroll = $(window).scrollTop();
        if (scroll >= 100) {
            form.classList.add('scrolled');
        }
        else {
            form.classList.remove('scrolled');
        }
    })
}