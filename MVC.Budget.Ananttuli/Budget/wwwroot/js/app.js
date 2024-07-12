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
const searchFormDescriptionElementId = "search-form-description";
const searchFormCategoryElementId = "search-form-category";

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
        redirectUri: homePageCategoriesTab,
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
        redirectUri: homePageTransactionsTab,
        method: "PUT"
    });
}

export async function deleteTransaction(id) {
    await deleteEntity("Transaction", `${transactionsUri}/Delete/${id}`, homePageTransactionsTab);
}

export function clearSearchForm() {

    const descriptionSearchEl = document.getElementById(searchFormDescriptionElementId);
    const categorySearchEl = document.getElementById(searchFormCategoryElementId);

    if (descriptionSearchEl) {
        descriptionSearchEl.value = null;
    }

    if (categorySearchEl) {
        categorySearchEl.value = null;
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


//export async function showPartialViewFormInModal(uri, modalTitle) {
//    const modalViewResult = await fetch(uri);
//    const modalContent = await modalViewResult.text();

//    const { modalContentEl } = setModalContent(modalTitle, modalContent);

//    if (!modalContentEl) return;

//    const forms = modalContentEl?.getElementsByTagName("form");
//    var newForm = forms?.[forms.length - 1];
//    if (newForm) {
//        $.validator.unobtrusive.parse(newForm);
//    }
//}

//function resetModalContent() {
//    setModalContent("", "");
//}

//function setModalContent(title = "", content = "") {
//    const modalEl = document.getElementById("modal");
//    const modalTitleEl = modalEl?.querySelector(".modal-title");
//    const modalContentEl = modalEl?.querySelector(".modal-body");

//    if (!modalEl || !modalContentEl || !modalTitleEl) {
//        console.log("could not set modal content");
//        return {};
//    }

//    modalTitleEl.innerHTML = title;
//    modalContentEl.innerHTML = content;

//    return { modalTitleEl, modalContentEl };
//}

//function showConfirmationModal({ title, body, action, onConfirm, isDestructive, cancelable }) {
//    if (!document.getElementById("confirmModalContainer")) {
//        document.body.insertAdjacentHTML("beforeend", `<div id="confirmModalContainer"></div>`);
//    }

//    const container = document.getElementById("confirmModalContainer");
//    container.innerHTML = '';

//    const modalHtml = `
//        <div class="modal fade" id="confirmModal" tabindex="-1" aria-labelledby="confirmModalLabel" aria-hidden="true">
//            <div class="modal-dialog">
//                <div class="modal-content">
//                <div class="modal-header">
//                    <h5 class="modal-title" id="confirmModalLabel">${title}</h5>
//                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
//                </div>
//                <div class="modal-body">
//                    ${body}
//                </div>
//                <div class="modal-footer">
//                    ${cancelable ? '<button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>' : ''}
//                    <button type="button" class="btn ${isDestructive ? 'btn-danger' : ''}" id="confirmModalAction">${action}</button>
//                </div>
//                </div>
//            </div>
//          </div>
//    `;

//    container.innerHTML = modalHtml;

//    const confirmButton = document.getElementById('confirmModalAction');
//    const modalEl = document.getElementById('confirmModal');

//    if (!confirmButton || !modalEl) {
//        return;
//    }

//    const confirmModal = new bootstrap.Modal(modalEl);

//    confirmModal.show();

//    confirmButton.addEventListener('click', async function () {
//        try {
//            await onConfirm();
//            confirmModal?.hide();
//        } catch {
//            confirmModal?.hide();
//            showFailureModal();
//        } finally {
//            confirmModal?.hide();
//        }
//    });

//}

//function showFailureModal() {
//    showConfirmationModal({ title: "Error", body: "Operation failed", action: "OK", onConfirm: () => { }, cancelable: false });
//}