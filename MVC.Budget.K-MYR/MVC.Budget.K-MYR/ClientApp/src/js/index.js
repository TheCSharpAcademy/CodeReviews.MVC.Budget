import { PAGE_ROUTES } from './config';
import { importBootstrapModals } from './asyncComponents';
import { putFiscalPlan, deleteFiscalPlan, postFiscalPlan } from './api'
import messageBox from "./messageBox";

const cardsContainer = document.getElementById('cards-container');
formatDashboard();
window.addEventListener('countryChanged', () => {
    formatDashboard();
});

const modals = importBootstrapModals();
var modalsArray = await modals;
var addModal = modalsArray.find(m => m._element.id == 'addFiscalPlan-modal');
var updateModal = modalsArray.find(m => m._element.id == 'updateFiscalPlan-modal');
var updateModalLabel = document.getElementById('updateFiscalPlan-label');
var updateModalId = document.getElementById('updateFiscalPlan_id');
var updateModalName = document.getElementById('updateFiscalPlan_name');
var deleteModal = modalsArray.find(m => m._element.id == 'deleteFiscalPlan-modal');
var deleteModalLabel = document.getElementById('deleteFiscalPlan-label');
var deleteModalId = document.getElementById('deleteFiscalPlan_id');
setupModalHandlers(modals);

function formatDashboard() {
    var cards = $('.fiscalPlan-card');
    for (let i = 0; i < cards.length; i++) {
        let id = cards[i].dataset.id;
        let incomeText = document.getElementById(`fiscalPlan_income_${id}`);
        incomeText.textContent = `${window.userNumberFormat.format(incomeText.dataset.total)} / ${window.userNumberFormat.format(incomeText.dataset.budget)}`;
        let expensesText = document.getElementById(`fiscalPlan_expenses_${id}`);
        expensesText.textContent = `${window.userNumberFormat.format(expensesText.dataset.total)} / ${window.userNumberFormat.format(expensesText.dataset.budget)}`;
    }
}

function addFiscalPlan(fiscalPlan, beforeElement) {   
    var card = document.createElement('div');
    card.className = 'fiscalPlan-card';
    card.id = `fiscalPlan-card_${fiscalPlan.id}`;
    card.setAttribute('data-id', fiscalPlan.id);
    card.setAttribute('data-name', fiscalPlan.name);

    var headerContainer = document.createElement('div');
    headerContainer.className = 'd-flex justify-content-between gap-1';

    var editIcon = document.createElementNS("http://www.w3.org/2000/svg", "svg");
    editIcon.setAttribute('viewBox', '0 0 14 14');
    editIcon.setAttribute('height', '30');
    editIcon.setAttribute('width', '30');
    editIcon.setAttribute('class', 'fiscalPlan-icon');
    editIcon.setAttribute('fill', '#ffffff');
    editIcon.setAttribute('data-action', 'edit');
    var editUse = document.createElementNS("http://www.w3.org/2000/svg", "use");
    editUse.setAttribute('href', '#edit-icon');
    editIcon.appendChild(editUse);

    var heading = document.createElement('h1');
    heading.id = `fiscalPlan-header_${fiscalPlan.id}`;
    heading.class = 'fiscalPlan-heading';
    heading.textContent = fiscalPlan.name;

    var deleteIcon = document.createElementNS("http://www.w3.org/2000/svg", "svg");
    deleteIcon.setAttribute('viewBox', '0 0 14 14');
    deleteIcon.setAttribute('height', '30');
    deleteIcon.setAttribute('width', '30');
    deleteIcon.setAttribute('class', 'fiscalPlan-icon');
    deleteIcon.setAttribute('fill', '#ffffff');
    deleteIcon.setAttribute('data-action', 'delete');
    var deleteUse = document.createElementNS("http://www.w3.org/2000/svg", "use");
    deleteUse.setAttribute('href', '#trash-icon');
    deleteIcon.appendChild(deleteUse);

    headerContainer.appendChild(editIcon);
    headerContainer.appendChild(heading);
    headerContainer.appendChild(deleteIcon);

    var progressContainer = document.createElement('div');
    progressContainer.className = 'progress-container';

    var incomeSection = document.createElement('div');
    var incomeTitleContainer = document.createElement('div');
    incomeTitleContainer.className = 'd-flex justify-content-between';
    var incomeTitle = document.createElement('div');
    incomeTitle.textContent = 'Income';
    var incomeTotal = document.createElement('div');
    incomeTotal.id = `fiscalPlan_income_${fiscalPlan.id}`;
    incomeTotal.textContent = `${window.userNumberFormat.format(0)} /  ${window.userNumberFormat.format(0)}`;

    incomeTitleContainer.appendChild(incomeTitle);
    incomeTitleContainer.appendChild(incomeTotal);
    incomeSection.appendChild(incomeTitleContainer);

    var incomeProgressDiv = document.createElement('div');
    incomeProgressDiv.className = 'progress';
    var incomeProgressBar = document.createElement('div');
    incomeProgressBar.className = 'progress-bar bg-success';
    incomeProgressBar.setAttribute('role', 'progressbar');
    incomeProgressBar.style.width = '100%'; 
    incomeProgressBar.setAttribute('aria-valuenow', '100');
    incomeProgressBar.setAttribute('aria-valuemin', '0');
    incomeProgressBar.setAttribute('aria-valuemax', '100');
    incomeProgressBar.setAttribute('aria-labelledby', `fiscalPlan_balance_${fiscalPlan.id}`);

    incomeProgressDiv.appendChild(incomeProgressBar);
    incomeSection.appendChild(incomeProgressDiv);

    var expensesSection = document.createElement('div');
    var expensesTitleContainer = document.createElement('div');
    expensesTitleContainer.className = 'd-flex justify-content-between';
    var expensesTitle = document.createElement('div');
    expensesTitle.textContent = 'Expenses';
    var expensesTotal = document.createElement('div');
    expensesTotal.id = `fiscalPlan_expenses_${fiscalPlan.id}`;
    expensesTotal.textContent = `${window.userNumberFormat.format(0)} /  ${window.userNumberFormat.format(0)}`;
            
    expensesTitleContainer.appendChild(expensesTitle);
    expensesTitleContainer.appendChild(expensesTotal);
    expensesSection.appendChild(expensesTitleContainer);

    var expensesProgressDiv = document.createElement('div');
    expensesProgressDiv.className = 'progress';
    var expensesProgressBar = document.createElement('div');
    expensesProgressBar.className = 'progress-bar bg-danger';
    expensesProgressBar.setAttribute('role', 'progressbar');
    expensesProgressBar.style.width = '100%';
    expensesProgressBar.setAttribute('aria-valuenow', '100');
    expensesProgressBar.setAttribute('aria-valuemin', '0');
    expensesProgressBar.setAttribute('aria-valuemax', '100');
    expensesProgressBar.setAttribute('aria-labelledby', `fiscalPlan_balance_${fiscalPlan.id}`);

    expensesProgressDiv.appendChild(expensesProgressBar);
    expensesSection.appendChild(expensesProgressDiv);
    progressContainer.appendChild(incomeSection);
    progressContainer.appendChild(expensesSection);
    card.appendChild(headerContainer);
    card.appendChild(progressContainer);
    card.addEventListener('click', onFiscalPlanClick);

    cardsContainer.insertBefore(card, beforeElement);
}

function updateFiscalPlan(formData) {
    var id = formData.get('Id');
    var name = formData.get('Name');
    var header = document.getElementById(`fiscalPlan-header_${id}`);
    header.textContent = name;
    var card = document.getElementById(`fiscalPlan-card_${id}`);
    card.dataset.name = name;
}

function removeFiscalPlan(id) {
    var element = document.getElementById(`fiscalPlan-card_${id}`);
    if (element) {
        element.removeEventListener('click', onFiscalPlanClick);
        element.remove();
    }
}

async function setupModalHandlers() {   
    var addfiscalPlanCard = document.getElementById('addFiscalPlan-card');
    
    var addFiscalPlanForm = document.getElementById('addFiscalPlan-form');
    addFiscalPlanForm.addEventListener('submit', async function (event) {
        event.preventDefault();
        if (addModal._isShown && $(this).valid()) {
            addModal.hide();
            var response = await postFiscalPlan(new FormData(this));
            if (response.isSuccess) {
                addFiscalPlan(response.data, addfiscalPlanCard);
            }
            messageBox.addMessage({ text: response.message, iconId: response.isSuccess ? '#check-icon' : '#cross-icon' });
            messageBox.show();
        }
    });
    addfiscalPlanCard.addEventListener('click', function () {
        addModal.show();
    });

    var updateFiscalPlanForm = document.getElementById('updateFiscalPlan-form');
    updateFiscalPlanForm.addEventListener('submit', async function (event) {
        event.preventDefault();
        if (updateModal._isShown && $(this).valid()) {
            updateModal.hide();
            let formData = new FormData(this);
            let response = await putFiscalPlan(formData);
            if (response.isSuccess) {
                updateFiscalPlan(formData);
            }            
            messageBox.addMessage({ text: response.message, iconId: response.isSuccess ? '#check-icon' : '#cross-icon' });
            messageBox.show();
        }
    });

    var deleteFiscalPlanForm = document.getElementById('deleteFiscalPlan-form');
    deleteFiscalPlanForm.addEventListener('submit', async function (event) {
        event.preventDefault();
        if (deleteModal._isShown) {
            deleteModal.hide();
            var formData = new FormData(this);
            var id = formData.get('Id');
            var token = formData.get('__RequestVerificationToken');
            var response = await deleteFiscalPlan(id, token);
            if (response.isSuccess) {
                removeFiscalPlan(id);
            }
            messageBox.addMessage({ text: response.message, iconId: response.isSuccess ? '#check-icon' : '#cross-icon' });
            messageBox.show();
        }
    });

    cardsContainer.querySelectorAll('.fiscalPlan-card')
                  .forEach(element => element.addEventListener("click", onFiscalPlanClick))   
}

function onFiscalPlanClick(event) {
    var fiscalPlanCard = event.currentTarget;
    var id = parseInt(fiscalPlanCard.dataset.id);

    if (event.target.matches('.fiscalPlan-icon')) {        
        switch (event.target.dataset.action) {
            case 'delete':              
                deleteModalLabel.textContent = `Delete '${fiscalPlanCard.dataset.name}'?`;
                deleteModalId.value = id;
                deleteModal.show();
                break;
            case 'edit':              
                updateModalLabel.textContent = `Edit '${fiscalPlanCard.dataset.name}'`;
                updateModalId.value = id;
                updateModalName.value = fiscalPlanCard.dataset.name;
                updateModal.show();
                break;
        }
    }
    else {        
        window.location.href = PAGE_ROUTES.FISCAL_PLAN(id);
    }
}
