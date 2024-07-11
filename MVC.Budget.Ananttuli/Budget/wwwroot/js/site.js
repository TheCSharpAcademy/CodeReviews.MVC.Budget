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
    await showPartialViewFormInModal(`${categoriesUri}/CreateCategoryModal`, "Create Category");
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
    await showPartialViewFormInModal(`${categoriesUri}/EditCategoryModal?id=${id}`, "Edit Category");
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
    await showPartialViewFormInModal(`${transactionsUri}/CreateTransactionModal`, "Create Transaction");
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
    await showPartialViewFormInModal(`${transactionsUri}/EditTransactionModal?id=${id}`, "Edit Transaction");
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

async function showPartialViewFormInModal(uri, modalTitle) {
    const modalViewResult = await fetch(uri);
    const modalContent = await modalViewResult.text();

    const { modalContentEl } = setModalContent(modalTitle, modalContent);

    if (!modalContentEl) return;

    const forms = modalContentEl?.getElementsByTagName("form");
    var newForm = forms?.[forms.length - 1];
    if (newForm) {
        $.validator.unobtrusive.parse(newForm);
    }
}

function resetModalContent() {
    setModalContent("", "");
}

function setModalContent(title = "", content = "") {
    const modalEl = document.getElementById("modal");
    const modalTitleEl = modalEl?.querySelector(".modal-title");
    const modalContentEl = modalEl?.querySelector(".modal-body");

    if (!modalEl || !modalContentEl || !modalTitleEl) {
        console.log("could not set modal content");
        return {};
    }

    modalTitleEl.innerHTML = title;
    modalContentEl.innerHTML = content;

    return { modalTitleEl, modalContentEl };
}