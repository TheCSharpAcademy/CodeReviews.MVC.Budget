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
    const item = { CategoryName: addCategory.value.trim() }
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

categoryActionSelect.addEventListener('change', updateCategoryModal);

    function updateCategoryModal() {
        const selectedCategoryAction = categoryActionSelect.value;

        if (selectedCategoryAction === 'add-category') {
            categoryDisplay.innerHTML = `
                <form action="javascript:void(0);" method="POST" onsubmit="addCategory()">
                    Add Category: <br>
                    <input type="text" id="add-categoryname" placeholder="Name"><span class="btn-close" id="closeModalCategory"></span>
                    <input type="submit" value="Add Category">
                </form>`;
        }
        else if (selectedCategoryAction === 'edit-category') {
        
            categoryDisplay.innerHTML = `
          <div> select category that you would like to change: <select id="edit-selectedcategory" required>
    </select>
    <form action ="javascript:void(0)" method="POST" onsubmit="updateCategory()">
            <input type="text" id="update-categoryname" placeholder="add new categoryname" />
        <input type="submit" value="Edit Category" />
    </form></div>
    `;
            const editCategorySelect = document.getElementById('edit-selectedcategory');
            populateCategoriesDropMenu3(editCategorySelect, uriCategory);

            listOfCategoriesSelect = editCategorySelect;
            listOfCategoriesSelect.addEventListener('change', function () {
                console.log('listOfCategoriesSelect changed');
                updatePlaceholder();
            });
            updateCategoryNameInput = document.getElementById('update-categoryname');
        }
        else if (selectedCategoryAction === 'delete-category') {
            categoryDisplay.innerHTML = `
        <p> here comes the delete text</p> `;
        }
    }

    function updatePlaceholder() {
        const selectedOption = listOfCategoriesSelect.options[listOfCategoriesSelect.selectedIndex];
        console.log(selectedOption);
        updateCategoryNameInput.placeholder = `${selectedOption.text}`;
        console.log(updateCategoryNameInput.placeholder);
    }


//TODO list: - display error when trying to add empty as category
            // - make the updatePlaceholder work