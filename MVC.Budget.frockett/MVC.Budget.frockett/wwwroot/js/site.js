// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

fetchDefaultTransactions();

let allTransactions = [];

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
    const list = document.getElementById('transactionList');
    list.innerHTML = '';

    const tableHead = document.createElement('thead');
    const headerRow = document.createElement('tr');

    const headerTitle = document.createElement('th');
    headerTitle.scope = 'col';
    headerTitle.innerText = 'Name';

    const headerCat = document.createElement('th');
    headerCat.scope = 'col';
    headerCat.innerText = 'Category';

    const headerAmount = document.createElement('th');
    headerAmount.scope = 'col';
    headerAmount.innerText = 'Amount';

    const headerDate = document.createElement('th');
    headerDate.scope = 'col';
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
        editButton.onclick = () => showEditForm(todo);
        editButton.className = 'btn btn-light btn-sm';

        const deleteButton = document.createElement('button');
        deleteButton.innerHTML = '<i class="fa-solid fa-trash"></i>'
        deleteButton.onclick = () => showDeleteModal(todo);
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

function fetchCategoryName(id) {
        return fetch(`/api/CategoriesAPI/${id}`)
            .then(response => response.json())
            .then(data => {
                return data;
            })
            .catch(error => {
                console.error('Error fetching category name:', error);
                return null;
            });
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
    }
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
            const categories = data.$values
            const categorySelect = document.getElementById('add-transactionCategory');
            categorySelect.innerHTML = '';
            categories.forEach(category => {
                const option = document.createElement('option');
                option.value = category.id;
                option.text = category.name;
                categorySelect.appendChild(option);
            });
        })
        .catch(error => console.error('Error fetching categories:', error));
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
