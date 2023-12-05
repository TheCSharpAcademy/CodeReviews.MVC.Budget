const uriTransaction = 'api/Transaction';
const uriCategory = 'api/Category';
let transactions = [];

function getTransactions() {
    fetch(uriTransaction)
        .then(response => response.json())
        .then(data => displayTransactions(data))
        .catch(error => console.error('Unable to get items.', error));
}
function populateCategoriesDropMenu() {
    const addCategorySelect = document.getElementById('add-selectedcategory');
    fetch(uriCategory)
        .then(response => response.json())
        .then(categories => {
            addCategorySelect.innerHTML = '';
            categories.forEach(category => {
                const option = document.createElement('option');
                option.value = category.categoryId;
                option.text = category.categoryName;
                addCategorySelect.appendChild(option);
            });
        })
        .catch(error => console.error('Unable to fetch categories.', error));
}
function populateCategoriesDropMenu3(targetSelect, url) {
    targetSelect.innerHTML = '';

    const defaultOption = document.createElement('option');
    defaultOption.value = '';
    defaultOption.text = "--select category--";
    targetSelect.appendChild(defaultOption);

    let fetchPromise;
    if (typeof url === 'string') {
        // If categoriesOrUrl is a string, assume it's a URL and fetch the data
        fetchPromise = fetch(url)
            .then(response => response.json());
    } else {
        // If it's not a string, assume it's the array of categories
        fetchPromise = Promise.resolve(url);
    }

    fetchPromise
        .then(categories => {
            categories.forEach(category => {
                const option = document.createElement('option');
                option.value = category.categoryId;
                option.text = category.categoryName;
                targetSelect.appendChild(option);
            });
        })
        .catch(error => console.error('Unable to fetch or process categories.', error));
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
            getTransactions();
            addDate.value = '';
            addSource.value = '';
            addAmount.value = '';
            addCategorySelect.value = '';
        })
        .catch(error => console.error('Unable to add item.'));
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
            populateCategoriesDropMenu();
        })
        .catch(error => console.error('Unabale to add category.'));
}
function updateCategory() {
    const getUpdatedCategoryName = document.getElementById('update-categoryname');
    const newCategoryName = getUpdatedCategoryName.value.trim();
    const getCategoryId = document.getElementById('edit-selectedcategory');
    const categoryId = getCategoryId.value;
    const item = { CategoryId: categoryId, CategoryName: newCategoryName };
    const statusMessage = document.getElementById('edit-form-message');
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
                console.log('Category updated succesfully');
                statusMessage.innerText = 'Categoryname successfully updated';
                statusMessage.style.color = 'green';
                statusMessage.style.display = 'block';
                return true;
            }
            else {
                throw new Error(`Failed to update category. Status: ${response.status}`);
            }
        })
        .then(success => {
            if (success) {
                const editCategorySelect = document.getElementById('edit-selectedcategory');
                populateCategoriesDropMenu3(editCategorySelect, uriCategory);
                document.getElementById('update-categoryname').value = '';
                hideEditForm();
            }
        })
        .catch(error => console.error('Could not update the category name', error.message));
}
function deleteCategory() {
    const deleteCategoryId = document.getElementById('delete-selectedcategory');
    const statusDeleteMessage = document.getElementById('delete-form-message');
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
                if (response.ok) {
                    statusDeleteMessage.innerText = 'Category successfully deleted';
                    statusDeleteMessage.style.color = 'green';
                    statusDeleteMessage.style.display = 'block';
                    return true;
                } 
            })
            .then(success => {
                if (success) {
                    const deleteCategorySelect = document.getElementById('delete-selectedcategory');
                    populateCategoriesDropMenu3(deleteCategorySelect, uriCategory);
                    hideDeleteForm();
                }
            })
            .catch(error => {
                console.error('Error deleting category:', error);
            });
    }
    else {
        console.log("user clicked cancel");
    }
}

function displayTransactions(data) {
    const tList = document.getElementById('transactionList');
    const tBody = tList.querySelector('tBody');
    tBody.innerHTML = '';

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
        let formattedAmount = formatAmount(decimalAmount);
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
    });

   transactions = data;
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
});
closeModalCategory.addEventListener('click', () => {
    categoryModal.style.display = 'none';
});
window.addEventListener('click', (event) => {
    if (event.target === categoryModal) {
        categoryModal.style.display = 'none';
    }
});

// start of category modal content code

const categoryActionSelect = document.getElementById('categoryAction');
const categoryDisplay = document.getElementById('categoryModal-display');
let updateCategoryNameInput;
let listOfCategoriesSelect;

categoryActionSelect.addEventListener('change', manageCategoryModal);

function manageCategoryModal() {
   const selectedCategoryAction = categoryActionSelect.value;

    if (selectedCategoryAction === 'add-category') {
        categoryDisplay.innerHTML = `
          <form id="add-category-form" action="javascript:void(0);" method="POST" onsubmit="addCategory()">
              Add Category: <br>
              <input type="text" id="add-categoryname" placeholder="Name">
              <input type="submit" value="Add Category">                     
          </form>
          <div id="add-form-message" style="display: none;"></div>
          `;
    }
   else if (selectedCategoryAction === 'edit-category') {

        categoryDisplay.innerHTML = `
          <div id="edit-category-container">Select the category name you wish to edit: <select id="edit-selectedcategory" required>
    </select></div>
    <form id="edit-category-form" action ="javascript:void(0)" method="POST" onsubmit="updateCategory()">
        <input type="text" id="update-categoryname" placeholder="add new categoryname" />
        <input type="submit" value="Update Name" />
    </form>
    <div id="edit-form-message" style="display: none;"></div>
    `;
       
       const editCategorySelect = document.getElementById('edit-selectedcategory');
       populateCategoriesDropMenu3(editCategorySelect, uriCategory);

       listOfCategoriesSelect = editCategorySelect;
       listOfCategoriesSelect.addEventListener('change', function () {
           updatePlaceholder();
           toggleEditFormVisibility();
           const statusEditMessage = document.getElementById('edit-form-message');
           statusEditMessage.style.display = 'none';
       });
       updateCategoryNameInput = document.getElementById('update-categoryname');
       hideEditForm();
   }
   else if (selectedCategoryAction === 'delete-category') {
       categoryDisplay.innerHTML = `
       <div id="delete-category-container">Select the category that you wish to delete:
       <select id="delete-selectedcategory" required>
       </select></div>
       <form id="delete-category-form" action ="javascript:void(0)" method="POST" onsubmit="deleteCategory()">
        <input type="submit" value="Delete Category" />
        </form>
        <div id ="delete-form-message" style="display: none;"></div>
       `;
        const deleteCategorySelect = document.getElementById('delete-selectedcategory');
        populateCategoriesDropMenu3(deleteCategorySelect, uriCategory);
        listOfCategoriesSelect = deleteCategorySelect;
        listOfCategoriesSelect.addEventListener('change', function () {
            toggleDeleteFormVisibility();
            const statusDeleteMessage = document.getElementById('delete-form-message');
            statusDeleteMessage.style.display = 'none';
        });
        hideDeleteForm();
    }
}

function updatePlaceholder() {
    const selectedOption = listOfCategoriesSelect.options[listOfCategoriesSelect.selectedIndex];
    updateCategoryNameInput.placeholder = `${selectedOption.text}`;
    (updateCategoryNameInput.placeholder);
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



//TODO list: - instead of an alert window try to show error in the modal
//- when closing and re-opening the category modal the add/edit/delete button doesnt reset
// replace original dropdownmenu with dropdownmenu 3