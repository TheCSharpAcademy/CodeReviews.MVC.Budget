
const editCategoryBtns = document.querySelectorAll('#EditCategoryBtn');

if (editCategoryBtns) {
    editCategoryBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            const categoryId = btn.getAttribute('data-id');
            const categoryName = btn.getAttribute('data-name');

            document.querySelector('#AddCategoryForm #AddCategory_Id').value = categoryId;
            document.querySelector('#AddCategoryForm #AddCategory_Name').value = categoryName;

        });
    });
};


const addCategoryBtn = document.querySelector('.open-addCategory');
addCategoryBtn.addEventListener('click', () => {
    document.querySelector('#AddCategoryForm #AddCategory_Id').value = 0;
    document.querySelector('#AddCategoryForm #AddCategory_Name').value = '';
    document.querySelector('#AddCategoryForm #AddCategory_Name').classList.remove('is-invalid');
    document.querySelector('#AddCategoryForm #AddCategoryBtn').disabled = false;
});

const deleteCategoryBtns = document.querySelectorAll('#DeleteCategoryBtn');

if (deleteCategoryBtns) {
    deleteCategoryBtns.forEach(btn => {
        btn.addEventListener("click", () => {
            const categoryId = btn.getAttribute('data-id');

            document.querySelector('#DeleteCategoryForm #DeleteCategoryId').value = categoryId;
        })
    });
}

const editTransactionBtns = document.querySelectorAll('#EditTransactionBtn');

if (editTransactionBtns) {
    editTransactionBtns.forEach(btn => {
        btn.addEventListener('click', () => {
            const transactionId = btn.getAttribute('data-id');
            const transactionName = btn.getAttribute('data-name');
            const transactionAmount = btn.getAttribute('data-amount');
            const transactionDate = btn.getAttribute('data-date');
            const transactionType = btn.getAttribute('data-transactionType');
            const transactionCategory = btn.getAttribute('data-category');

            const addTransactionForm = document.querySelector('#AddTransactionForm');

            addTransactionForm.querySelector('#AddTransaction_Id').value = transactionId;
            addTransactionForm.querySelector('#AddTransaction_Name').value = transactionName;
            addTransactionForm.querySelector('#AddTransaction_Amount').value = +transactionAmount;
            addTransactionForm.querySelector('.AddTransactionForm-type-input').value = transactionType;
            addTransactionForm.querySelector('.AddTransactionForm-category-input').value = transactionCategory;
            addTransactionForm.querySelector('#AddTransaction_Date').value = new Date(transactionDate).toISOString().substring(0, 10);
        });
    });
}

const addTransactionBtn = document.querySelector('.open-addTransaction');
addTransactionBtn.addEventListener('click', () => {

    const addTransactionForm = document.querySelector('#AddTransactionForm');

    addTransactionForm.querySelector('#AddTransaction_Id').value = 0;
    addTransactionForm.querySelector('#AddTransaction_Name').value = '';
    addTransactionForm.querySelector('#AddTransaction_Amount').value = 0.00;
    addTransactionForm.querySelector('.AddTransactionForm-type-input').value = '';
    addTransactionForm.querySelector('.AddTransactionForm-category-input').value = '';
    addTransactionForm.querySelector('#AddTransaction_Date').value = '';
});

const typeSelect = document.querySelector('.AddTransactionForm-type-input');

typeSelect.addEventListener('change', () => {
    if (typeSelect.value == 1) {
        document.querySelector('.AddTransactionForm-category-input').value = 7;
        document.querySelector('.AddTransactionForm-category-input').classList.add("hidden");
    } else {
        document.querySelector('.AddTransactionForm-category-input').classList.remove("hidden");
        document.querySelector('.AddTransactionForm-category-input').value = '';
    }
});

const deleteTransactionBtns = document.querySelectorAll('#DeleteTransactionBtn');

if (deleteTransactionBtns) {
    deleteTransactionBtns.forEach(btn => {
        btn.addEventListener("click", () => {
            const transactionId = btn.getAttribute('data-id');

            document.querySelector('#DeleteTransactionForm #DeleteTransactionId').value = transactionId;
        })
    });
}

const transactionsTabBtn = document.querySelector('#transactions-tab');
const transactionsTab = document.querySelector('#transactions');

const categoriesTabBtn = document.querySelector('#categories-tab');
const categoriesTab = document.querySelector('#categories');


transactionsTabBtn.addEventListener('click', () => {
    localStorage.setItem('activeTab', 'transactions');
});

categoriesTabBtn.addEventListener('click', () => {
    localStorage.setItem('activeTab', 'categories');
});


window.addEventListener('load', () => {
    setTimeout(() => {
        const activeTab = localStorage.getItem('activeTab');

        if (activeTab == null) {
            transactionsTabBtn.classList.add('active');
            transactionsTab.classList.add('show', 'active');
        }

        if (activeTab == "transactions") {
            transactionsTabBtn.classList.add('active');
            transactionsTab.classList.add('show', 'active');

            categoriesTabBtn.classList.remove('active');
            categoriesTab.classList.remove('show', 'active');
        } else if (activeTab == "categories") {
            categoriesTabBtn.classList.add('active');
            categoriesTab.classList.add('show', 'active');

            transactionsTabBtn.classList.remove('active');
            transactionsTab.classList.remove('show', 'active');
        } else {
            transactionsTabBtn.classList.add('active');
            transactionsTab.classList.add('show', 'active');
        }
    }, 100)


});

const categories = [];

editCategoryBtns.forEach(btn => {
    categories.push(btn.getAttribute('data-name').toLocaleLowerCase());
});


const categoryInput = document.querySelector('#AddCategory_Name');

categoryInput.addEventListener('input', (e) => {
    if (e.target.value != "" && categories.includes(e.target.value.toLocaleLowerCase())) {
        document.querySelector('#AddCategoryForm #AddCategory_Name').classList.add('is-invalid');
        document.querySelector('#AddCategoryForm #AddCategoryBtn').disabled = true;
        document.querySelector('#AddCategoryForm #AddCategory_Name').focus();
        document.querySelector(".category-name-validator").insertAdjacentHTML("beforebegin", '<span id="category-exist-error" class="text-danger">The Category already exists!.</span>');
    } else {
        document.querySelector('#AddCategoryForm #AddCategory_Name').classList.remove('is-invalid');
        document.querySelector('#AddCategoryForm #AddCategoryBtn').disabled = false;
        document.querySelector('#category-exist-error').remove();
    }
})

const tableRows = document.querySelectorAll('.table-row');
const searchInput = document.querySelector('#SearchInput');

searchInput.addEventListener('keyup', (e) => {
    tableRows.forEach(row => {
        const name = row.querySelector("td:nth-child(2)").textContent.toLowerCase();
        console.log(name);

        if (name.includes(e.target.value.toLowerCase())) {
            row.classList.remove('hidden');
        } else {
            row.classList.add('hidden');
        }
    });

});

