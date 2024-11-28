const uriTransaction = 'api/Transaction';
const uriCategory = 'api/Category';
let transactions = [];
let balance;

async function populateCategoriesDropMenu(targetSelect, url) {
    targetSelect.innerHTML = '';

    const defaultOption = document.createElement('option');
    defaultOption.value = '';
    defaultOption.text = "--select category--";
    targetSelect.appendChild(defaultOption);

    let fetchPromise;

    if (typeof url === 'string') {
        // If categoriesOrUrl is a string, assume it's a URL and fetch the data
        fetchPromise = fetch(url);
    } else {
        // If it's not a string, assume it's the array of categories
        fetchPromise = Promise.resolve(url);
    }

    try {
        const response = await fetchPromise;
        if (!response.ok) {
            throw new Error(`Failed to fetch categories. Status: ${response.status}`);
        }

        const categories = await response.json();

        categories.forEach(category => {
            const option = document.createElement('option');
            option.value = category.categoryId;
            option.text = category.categoryName;
            targetSelect.appendChild(option);
        });
    } catch (error) {
        console.error('Error during category fetch or processing:', error);
        console.error('Error details:', error.message);
    }
}
function getTransactions() {
    fetch(uriTransaction)
        .then(response => response.json())
        .then(data => {
            transactions = data;
            displayTransactions(data);
        })
        .catch(error => console.error('Unable to get items.', error));
}
function displayTransactions(data) {
    const tList = document.getElementById('transactionList');
    const tBody = tList.querySelector('tBody');
    tBody.innerHTML = '';
    balance = 0;
    data.forEach(item => {
        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNodeId = document.createTextNode(item.transactionId);
        td1.appendChild(textNodeId);

        let td2 = tr.insertCell(1);
        let date = new Date(item.transactionDate);
        let dateString = formatDate(date);
        let textNodeDate = document.createTextNode(dateString);
        td2.appendChild(textNodeDate);

        let td3 = tr.insertCell(2);
        let textNodeSource = document.createTextNode(item.transactionSource);
        td3.appendChild(textNodeSource);

        let td4 = tr.insertCell(3);
        let decimalAmount = item.transactionAmount;
        let formattedAmount = `${formatAmount(decimalAmount)} €`;
        let textNodeAmount = document.createTextNode(formattedAmount);
        td4.appendChild(textNodeAmount);

        let td5 = tr.insertCell(4);
        fetch(uriCategory + "/" + item.categoryId)
            .then(response => response.json())
            .then(categoryData => {
                let textNodeCategory = document.createTextNode(categoryData.categoryName);
                td5.appendChild(textNodeCategory);
            })
            .catch(error => console.error('Unable to get the category name'));

        let td6 = tr.insertCell(5);
        let editTransactionButton = document.createElement('button');
        editTransactionButton.type = 'button';
        editTransactionButton.classList.add('btn');
        editTransactionButton.innerHTML = `
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
            <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
            <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z" />
        </svg>`;
        editTransactionButton.addEventListener('click', () => {
            editTransactionModal(item);
        });
        td6.appendChild(editTransactionButton);

        let td7 = tr.insertCell(6);
        let deleteTransactionButton = document.createElement('button');
        deleteTransactionButton.type = 'button';
        deleteTransactionButton.classList = ('btn');
        deleteTransactionButton.innerHTML = `
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash3" viewBox="0 0 16 16">
            <path d="M6.5 1h3a.5.5 0 0 1 .5.5v1H6v-1a.5.5 0 0 1 .5-.5M11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3A1.5 1.5 0 0 0 5 1.5v1H2.506a.58.58 0 0 0-.01 0H1.5a.5.5 0 0 0 0 1h.538l.853 10.66A2 2 0 0 0 4.885 16h6.23a2 2 0 0 0 1.994-1.84l.853-10.66h.538a.5.5 0 0 0 0-1h-.995a.59.59 0 0 0-.01 0zm1.958 1-.846 10.58a1 1 0 0 1-.997.92h-6.23a1 1 0 0 1-.997-.92L3.042 3.5zm-7.487 1a.5.5 0 0 1 .528.47l.5 8.5a.5.5 0 0 1-.998.06L5 5.03a.5.5 0 0 1 .47-.53Zm5.058 0a.5.5 0 0 1 .47.53l-.5 8.5a.5.5 0 1 1-.998-.06l.5-8.5a.5.5 0 0 1 .528-.47ZM8 4.5a.5.5 0 0 1 .5.5v8.5a.5.5 0 0 1-1 0V5a.5.5 0 0 1 .5-.5"/>
        </svg>`;
        deleteTransactionButton.addEventListener('click', () => {
            deleteTransaction(item.transactionId);
        });
        td7.appendChild(deleteTransactionButton);

        balance += decimalAmount;
    });
    document.getElementById('balanceTotal').textContent = balance.toFixed(2);
}
function searchOnTransactionName() {
    const searchString = document.getElementById('searchTransactionName').value.toLowerCase();
    const searchTransactionResult = transactions.filter(transaction =>
        transaction.transactionSource.toLowerCase().includes(searchString)
    );
    displayTransactions(searchTransactionResult);
}
function sortOnCategory() {
    fetch(uriTransaction)
        .then(response => response.json())
        .then(data => {
            const sortedTransactions = data.sort(function (a, b) {
                if (a.categoryId < b.categoryId) {
                    return -1;
                }
                if (a.categoryId > b.categoryId) {
                    return 1;
                }
                return 0;
            });
            displayTransactions(sortedTransactions);
        })
}
function sortOnDate() {
    fetch(uriTransaction)
        .then(response => response.json())
        .then(data => {
            const sortedOnDate = data.sort(function (a, b) {
                var dateA = new Date(a.transactionDate);
                var dateB = new Date(b.transactionDate);
                return dateA - dateB;
            });
            displayTransactions(sortedOnDate);
        });
}
function sortOnDateDesc() {
    fetch(uriTransaction)
        .then(response => response.json())
        .then(data => {
            const sortedOnDate = data.sort(function (a, b) {
                var dateA = new Date(a.transactionDate);
                var dateB = new Date(b.transactionDate);
                return dateB - dateA;
            });
            displayTransactions(sortedOnDate);
        });
}
function addTransaction() {
    const addDate = document.getElementById('add-date');
    const addSource = document.getElementById('add-source');
    const addAmount = document.getElementById('add-amount');
    const addCategorySelect = document.getElementById('add-selectedcategory');


    if (!addDate.value || !addSource.value || !addAmount.value || !addCategorySelect.value) {
        document.getElementById('error-message').innerText = 'Please fill in all the required fields';
        document.getElementById('error-message').style.display = 'block';
        return;
    }
    if (isNaN(parseFloat(addAmount.value))) {
        document.getElementById('error-message').innerText = 'Please fill in valid number for amount';
        document.getElementById('error-message').style.display = 'block';
        return;
    }
    document.getElementById('error-message').innerText = '';
    document.getElementById('error-message').style.display = 'none';

    const selectedCategoryOption = addCategorySelect.options[addCategorySelect.selectedIndex];
    const selectedCategory = {
        CategoryId: parseInt(selectedCategoryOption.value),
        CategoryName: selectedCategoryOption.text,
    };

    const item = {
        TransactionDate: addDate.value,
        TransactionSource: addSource.value.trim(),
        TransactionAmount: parseFloat(addAmount.value),
        Category: selectedCategory,
        CategoryId: selectedCategory.CategoryId
    };

    fetch(uriTransaction, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            addDate.value = '';
            addSource.value = '';
            addAmount.value = '';
            addCategorySelect.value = '';
            location.reload();
        })
        .catch(error => console.error('Unable to add item.'));
}
function editTransaction() {

    if (!editDate.value || !editSource.value || !editAmount.value || !editCategory.value) {
        document.getElementById('error-message-edit').innerText = 'Please fill in all the required fields';
        document.getElementById('error-message-edit').style.display = 'block';
        return;
    }
    if (isNaN(parseFloat(editAmount.value))) {
        document.getElementById('error-message-edit').innerText = 'Please fill in valid number for amount';
        document.getElementById('error-message-edit').style.display = 'block';
        return;
    }

    const selectedEditCategoryOption = editCategory.options[editCategory.selectedIndex];
    const selectedEditCategory = {
        CategoryId: parseInt(selectedEditCategoryOption.value),
        CategoryName: selectedEditCategoryOption.text,
    };
    const editedItem = {
        TransactionId: editTransactionId,
        TransactionDate: editDate.value,
        TransactionSource: editSource.value.trim(),
        TransactionAmount: parseFloat(editAmount.value),
        Category: selectedEditCategory,
        CategoryId: selectedEditCategory.CategoryId
    }

    fetch(uriTransaction + "/" + editTransactionId, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(editedItem)
    })
        .then(response => {
            if (response.ok || response.status === 204) {
                return true;
            }
            else {
                throw new Error(`Failed to update category. Status: ${response.status}`);
            }
        })
        .then(success => {
            if (success) {
                location.reload();
            }
        })
        .catch(error => console.error('Could not update the transaction', error.message));
}
function deleteTransaction(transactionId) {
    const askForConfirmation = confirm("Delete transaction with id " + transactionId + " ?");
    if (askForConfirmation) {
        fetch(uriTransaction + "/" + transactionId, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }
                location.reload();
            })
            .catch(error => {
                console.error('An error occured while deleting transaction: ', error.message);
            });
    }
}
function addCategory() {
    const addCategory = document.getElementById('add-categoryname');
    const input = addCategory.value.trim();
    if (input === '') {
        alert('Category name can\'t be empty');
        return;
    }
    const item = { CategoryName: input };

    fetch(uriCategory, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            addCategory.value = '';
            location.reload();
        })
        .catch(error => console.error('Unabale to add category.'));
}
function updateCategory() {
    const getUpdatedCategoryName = document.getElementById('update-categoryname');
    const newCategoryName = getUpdatedCategoryName.value.trim();
    const getCategoryId = document.getElementById('edit-selected-category');
    const categoryId = getCategoryId.value;
    const item = { CategoryId: categoryId, CategoryName: newCategoryName };
    fetch(uriCategory + "/" + categoryId, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => {
            if (response.ok || response.status === 204) {
                return true;
            }
            else {
                throw new Error(`Failed to update category. Status: ${response.status}`);
            }
        })
        .then(success => {
            if (success) {
                //const editCategorySelect = document.getElementById('edit-selected-category');
                //populateCategoriesDropMenu(editCategorySelect, uriCategory);
                document.getElementById('update-categoryname').value = '';
                hideEditForm();
                location.reload();
            }
        })
        .catch(error => console.error('Could not update the category name', error.message));
}
function deleteCategory() {
    const deleteCategoryId = document.getElementById('delete-selectedcategory');
    const warning = confirm("By deleting a Category, all transactions linked to it will also be deleted ! \nAre you sure you want to delete this category ?");
    if (warning) {
        fetch(uriCategory + "/" + deleteCategoryId.value, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Failed to delete the category. Status: ${response.status}`);
                }
            })
            .then(() => {
                const deleteCategorySelect = document.getElementById('delete-selectedcategory');
                populateCategoriesDropMenu(deleteCategorySelect, uriCategory);
                hideDeleteForm();
                location.reload();
            })
            .catch(error => {
                console.error('Error deleting category:', error);
            });
    }
}
const transactionEditModal = document.getElementById('editTransactionModal');
const editDate = document.getElementById('edit-date');
const editSource = document.getElementById('edit-source');
const editAmount = document.getElementById('edit-amount');
const editCategory = document.getElementById('edit-selectedcategory');
let editTransactionId;
function editTransactionModal(item) {

    populateCategoriesDropMenu(editCategory, uriCategory);

    transactionEditModal.style.display = 'block';
    editTransactionId = item.transactionId;
    editDate.value = convertDate(item.transactionDate);
    editSource.value = item.transactionSource;
    editAmount.value = item.transactionAmount;
    editCategory.value = item.categoryId;
    setTimeout(function () {
        editCategory.value = item.categoryId;
    }, 100); // had to add a small delay else the categoryvalue wasn't updating
}
function convertDate(dateString) {
    const date = new Date(dateString);
    //converting takes a day off so adding a day first
    const plusOneDay = new Date(date.getTime() + 24 * 60 * 60 * 1000);
    const convertedDate = plusOneDay.toISOString().split('T')[0];
    return convertedDate;
}
function formatDate(date) {
    const options = { day: 'numeric', month: 'short', year: 'numeric' };
    return date.toLocaleDateString('nl-BE', options);
}
function formatAmount(decimalAmount) {
    return decimalAmount.toFixed(2);
}

const transactionModal = document.getElementById('transactionModal');
const openTransactionModal = document.getElementById('openTransactionModal');
const closeModalTransaction = document.getElementById('closeModalTransaction');
const categoryModal = document.getElementById('categoryModal');
const openCategoryModal = document.getElementById('openCategoryModal');
const closeModalCategory = document.getElementById('closeModalCategory');

openTransactionModal.addEventListener('click', () => {
    transactionModal.style.display = 'block';
});
closeModalTransaction.addEventListener('click', () => {
    transactionModal.style.display = 'none';
})
window.addEventListener('click', (event) => {
    if (event.target === transactionModal) {
        transactionModal.style.display = 'none';
    }
});
openCategoryModal.addEventListener('click', () => {
    categoryModal.style.display = 'block';
    categoryActionSelect.value = 'add-category';

});
closeModalCategory.addEventListener('click', () => {
    categoryModal.style.display = 'none';
    categoryActionSelect.value = 'add-category';
});
window.addEventListener('click', (event) => {
    if (event.target === categoryModal) {
        categoryModal.style.display = 'none';
        renderAddCategoryForm();
    }
});
window.addEventListener('click', (event) => {
    if (event.target === transactionEditModal) {
        transactionEditModal.style.display = 'none';
    }
});

const categoryActionSelect = document.getElementById('categoryAction');
const categoryDisplay = document.getElementById('categoryModal-display');
let updateCategoryNameInput;
let listOfCategoriesSelect;

categoryActionSelect.addEventListener('change', manageCategoryModal);
function manageCategoryModal() {
    const selectedCategoryAction = categoryActionSelect.value;

    if (selectedCategoryAction === 'add-category') {
        renderAddCategoryForm();
    } else if (selectedCategoryAction === 'edit-category') {
        renderEditCategoryForm();
    } else if (selectedCategoryAction === 'delete-category') {
        renderDeleteCategoryForm();
    }
}
function renderAddCategoryForm() {
    categoryDisplay.innerHTML = `
        <form id="add-category-form">
            Add Category: <br>
            <input type="text" id="add-categoryname" placeholder="Name">
            <input type="submit" value="Add Category">                     
        </form>
        <div id="add-form-message" style="display: none;"></div>
    `;
    const addCategoryForm = document.getElementById('add-category-form');
    addCategoryForm.addEventListener('submit', function (event) {
        event.preventDefault();
        addCategory();
    });
}
function renderEditCategoryForm() {
    categoryDisplay.innerHTML = `
        <div id="edit-category-container">Select the category name you wish to edit:
            <select id="edit-selected-category" required></select>
        </div>
        <form id="edit-category-form">
            <input type="text" id="update-categoryname" placeholder="" />
            <input type="submit" value="Update Name" />
        </form>
    `;
    const editCategoryForm = document.getElementById('edit-category-form');
    editCategoryForm.addEventListener('submit', function (event) {
        event.preventDefault();
        updateCategory();
    });

    const editCategorySelect = document.getElementById('edit-selected-category');
    populateCategoriesDropMenu(editCategorySelect, uriCategory);
    listOfCategoriesSelect = editCategorySelect;
    listOfCategoriesSelect.addEventListener('change', function () {
        updatePlaceholder();
        toggleEditFormVisibility();
    });
    updateCategoryNameInput = document.getElementById('update-categoryname');
    hideEditForm();
}
function renderDeleteCategoryForm() {
    categoryDisplay.innerHTML = `
        <div id="delete-category-container">Select the category that you wish to delete:
            <select id="delete-selectedcategory" required></select>
        </div>
        <form id="delete-category-form">
            <input type="submit" value="Delete Category" />
        </form>
    `;

    const deleteCategoryForm = document.getElementById('delete-category-form');
    deleteCategoryForm.addEventListener('submit', function (event) {
        event.preventDefault();
        deleteCategory();
    });

    const deleteCategorySelect = document.getElementById('delete-selectedcategory');

    populateCategoriesDropMenu(deleteCategorySelect, uriCategory);
    listOfCategoriesSelect = deleteCategorySelect;
    listOfCategoriesSelect.addEventListener('change', function () {
        toggleDeleteFormVisibility();
    });
    hideDeleteForm();
}
function updatePlaceholder() {
    const selectedOption = listOfCategoriesSelect.options[listOfCategoriesSelect.selectedIndex];
    updateCategoryNameInput.placeholder = `${selectedOption.text}`;
}
function hideEditForm() {
    const formContainer = document.getElementById('edit-category-form');
    formContainer.style.display = 'none';
}
function showEditForm() {
    const formContainer = document.getElementById('edit-category-form');
    formContainer.style.display = 'block';
}
function toggleEditFormVisibility() {
    const selectedOption = listOfCategoriesSelect.options[listOfCategoriesSelect.selectedIndex];
    if (selectedOption.value) {
        showEditForm();
    }
    else {
        hideEditForm();
    }
}
function hideDeleteForm() {
    const formContainer = document.getElementById('delete-category-form');
    formContainer.style.display = 'none';
}
function showDeleteForm() {
    const formContainer = document.getElementById('delete-category-form');
    formContainer.style.display = 'block';
}
function toggleDeleteFormVisibility() {
    const selectedOption = listOfCategoriesSelect.options[listOfCategoriesSelect.selectedIndex];
    if (selectedOption.value) {
        showDeleteForm();
    }
    else {
        hideDeleteForm();
    }
}