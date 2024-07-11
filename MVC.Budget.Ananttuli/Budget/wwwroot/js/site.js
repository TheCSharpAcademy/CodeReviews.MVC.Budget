//======================== CONFIG ========================\\
const homePage = "/Home";
const homePageCategoriesTab = `${homePage}?activeTab=Categories`;
const homePageTransactionsTab = `${homePage}?activeTab=Transactions`;

const categoriesUri = "/Categories";
const createCategoryFormId = "create-category-form";
const updateCategoryFormId = "update-category-form";

const transactionsUri = "/Transactions";
const createTransactionFormId = "create-transaction-form";
const updateTransactionFormId = "update-transaction-form";

//======================== CATEGORY ========================\\

async function showCreateCategoryModal() {
    await getModalPartialView(`${categoriesUri}/CreateCategoryModal`, "modal");
}

async function createCategory(e) {
    e.preventDefault();

    await upsertEntity({
        uri: `${categoriesUri}/CreateCategory`,
        formElementId: createCategoryFormId,
        redirectUri: homePageCategoriesTab,
        method: "POST"
    });
}

async function showEditCategoryModal(id) {
    await getModalPartialView(`${categoriesUri}/EditCategoryModal?id=${id}`, "modal");
}

async function updateCategory(e) {
    e.preventDefault();

    await upsertEntity({
        uri: `${categoriesUri}/UpdateCategory`,
        formElementId: updateCategoryFormId,
        redirectUri: homePageCategoriesTab,
        method: "PUT"
    });
}

//======================== TRANSACTIONS ========================\\

async function showCreateTransactionModal() {
    await getModalPartialView(`${transactionsUri}/CreateTransactionModal`, "modal");
}

async function createTransaction(e) {
    e.preventDefault();

    await upsertEntity({
        uri: `${transactionsUri}/CreateTransaction`,
        formElementId: createTransactionFormId,
        redirectUri: homePageTransactionsTab,
        method: "POST"
    });
}

async function showEditTransactionModal(id) {
    await getModalPartialView(`${transactionsUri}/EditTransactionModal?id=${id}`, "modal");
}

async function updateTransaction(e) {
    e.preventDefault();

    await upsertEntity({
        uri: `${transactionsUri}/UpdateTransaction`,
        formElementId: updateTransactionFormId,
        redirectUri: homePageTransactionsTab,
        method: "PUT"
    });
}


//======================== COMMON ========================\\

async function upsertEntity({ uri, formElementId, redirectUri, method }) {
    try {
        const formObject = $(`#${formElementId}`);

        const isFormValid = formObject.valid();

        if (!isFormValid) {

            return;
        }

        const formElement = formObject[0];

        const response = await fetch(uri, {
            method,
            body: new URLSearchParams(new FormData(formElement))
        });

        if (!response.ok) {
            throw new Error();
        }

        window.location = redirectUri;
    } catch (error) {
        console.log("Error", error);
        const errorEl = document.querySelector(`#${formElementId} .error-message`);
        if (errorEl) errorEl.innerText = `Could not ${method == "POST" ? "create" : "update"}`
    }
}

async function getModalPartialView(uri, modalHtmlId) {
    const modalViewResult = await fetch(uri);
    const modalMarkup = await modalViewResult.text();

    const modalEl = document.getElementById(modalHtmlId);
    const modalContentEl = modalEl?.querySelector(".modal-body")
    if (!modalEl || !modalContentEl) {
        throw new Error("Could not find modal");
    }
    modalContentEl.innerHTML = modalMarkup;
    const forms = modalContentEl.getElementsByTagName("form");
    var newForm = forms[forms.length - 1];
    $.validator.unobtrusive.parse(newForm);
}