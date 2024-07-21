import { Modal } from "./modal.js";

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

const searchFormElementId = "search-form";
const searchFormInputsClass = "search-form-input";
const searchFormPageNumberElementId = "search-form-page-number";

//======================== CATEGORY ========================\\

export async function showCreateCategoryModal() {
    await Modal.showPartialViewFormInModal(`${categoriesUri}/CreateCategoryModal`, "Create Category");
}

export async function createCategory(e) {
    e.preventDefault();

    await upsertEntity({
        uri: `${categoriesUri}/CreateCategory`,
        formElementId: createCategoryFormId,
        redirectUri: homePageCategoriesTab,
        method: "POST"
    });
}

export async function showEditCategoryModal(id) {
    await Modal.showPartialViewFormInModal(`${categoriesUri}/EditCategoryModal?id=${id}`, "Edit Category");
}

export async function updateCategory(e) {
    e.preventDefault();

    await upsertEntity({
        uri: `${categoriesUri}/UpdateCategory`,
        formElementId: updateCategoryFormId,
        redirectUri: `${homePageCategoriesTab}${window.location.search || ''}`,
        method: "PUT"
    });
}

export async function deleteCategory(id) {
    await deleteEntity("Category", `${categoriesUri}/Delete/${id}`, homePageCategoriesTab, "All transactions linked to this category will be deleted.");
}

//======================== TRANSACTIONS ========================\\

export async function showCreateTransactionModal() {
    await Modal.showPartialViewFormInModal(`${transactionsUri}/CreateTransactionModal`, "Create Transaction");
}

export async function createTransaction(e) {
    e.preventDefault();

    await upsertEntity({
        uri: `${transactionsUri}/CreateTransaction`,
        formElementId: createTransactionFormId,
        redirectUri: homePageTransactionsTab,
        method: "POST"
    });
}

export async function showEditTransactionModal(id) {
    await Modal.showPartialViewFormInModal(`${transactionsUri}/EditTransactionModal?id=${id}`, "Edit Transaction");
}

export async function updateTransaction(e) {
    e.preventDefault();

    await upsertEntity({
        uri: `${transactionsUri}/UpdateTransaction`,
        formElementId: updateTransactionFormId,
        redirectUri: `${homePageTransactionsTab}${window.location.search || ''}`,
        method: "PUT"
    });
}

export async function deleteTransaction(id) {
    await deleteEntity("Transaction", `${transactionsUri}/Delete/${id}`, homePageTransactionsTab);
}

export function clearSearchForm() {
    const searchFormEl = document.getElementById(searchFormElementId);
    const pageNumberEl = document.getElementById(searchFormPageNumberElementId);

    if (!searchFormEl) {
        return;
    }

    for (const input of searchFormEl.querySelectorAll(`.${searchFormInputsClass}`) || []) {
        input.value = null;
    }

    if (pageNumberEl) {
        pageNumberEl.value = 1;
    }
}

export function nextPage() {
    const pageNumberEl = document.getElementById(searchFormPageNumberElementId);

    if (pageNumberEl) {
        const pageNumberValue = parseInt(pageNumberEl.value);

        pageNumberEl.value = pageNumberValue + 1;
    }
}

export function previousPage() {
    const pageNumberEl = document.getElementById(searchFormPageNumberElementId);

    if (pageNumberEl && pageNumberEl.value > 1) {
        const pageNumberValue = parseInt(pageNumberEl.value);
        pageNumberEl.value = pageNumberValue - 1;
    }
}

//======================== COMMON ========================\\

export async function upsertEntity({ uri, formElementId, redirectUri, method }) {
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

export async function deleteEntity(entityName, uri, successRedirect, customMessage = '') {
    Modal.showConfirmationModal({
        title: `Delete ${entityName}`,
        body: `Are you sure you want to delete ${entityName.toLowerCase()}? ${customMessage}`,
        isDestructive: true,
        action: `Delete`,
        onConfirm: async () => {
            const response = await fetch(uri, {
                method: "DELETE"
            });

            if (!response.ok) {
                throw new Error();
            }

            window.location = successRedirect;
        }
    });
}
