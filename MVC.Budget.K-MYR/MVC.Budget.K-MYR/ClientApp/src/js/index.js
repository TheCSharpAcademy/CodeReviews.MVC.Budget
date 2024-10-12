import { PAGE_ROUTES } from './config';
import { importBootstrapModals } from './asyncComponents';
import { putFiscalPlan, deleteFiscalPlan, postFiscalPlan } from './api'

formatDashboard();

const modals = importBootstrapModals();
setupModalHandlers();

window.addEventListener('countryChanged', () => {
    formatDashboard();
});

function formatDashboard() {
    var cards = $('.fiscalPlan-card');
    for (let i = 0; i < cards.length; i++) {
        let id = cards[i].dataset.id;
        let incomeText = document.getElementById(`fiscalPlan_${id}_income`);
        incomeText.textContent = `${window.userNumberFormat.format(incomeText.dataset.total)} / ${window.userNumberFormat.format(incomeText.dataset.budget)}`;
        let expensesText = document.getElementById(`fiscalPlan_${id}_expenses`);
        expensesText.textContent = `${window.userNumberFormat.format(expensesText.dataset.total)} / ${window.userNumberFormat.format(expensesText.dataset.budget)}`;
    }
}

function updateFiscalPlan(formData) {
    var id = formData.get('Id');
    var name = formData.get('Name');
    var header = document.getElementById(`fiscalPlan-header_${id}`);
    header.textContent = name;
}

function removeFiscalPlan(id) {
    var element = document.getElementById(`fiscalPlan-card_${id}`);
    if (element) {
        $(element).off();
        element.remove();
    }
}

async function setupModalHandlers() {
    var modalsArray = await modals;
    var addModal = modalsArray.find(m => m._element.id == 'addFiscalPlan-modal');
    var updateModal = modalsArray.find(m => m._element.id == 'updateFiscalPlan-modal');
    var updateModalLabel = document.getElementById('updateFiscalPlan-label');
    var updateModalId = document.getElementById('updateFiscalPlan_id');
    var updateModalName = document.getElementById('updateFiscalPlan_name');
    var deleteModal = modalsArray.find(m => m._element.id == 'deleteFiscalPlan-modal');
    var deleteModalLabel = document.getElementById('deleteFiscalPlan-label');
    var deleteModalId = document.getElementById('deleteFiscalPlan_id');

    var addfiscalPlanCard = document.getElementById('addFiscalPlan-card');
    addfiscalPlanCard.addEventListener('click', function () {
        addModal.show();
    });
    var addFiscalPlanForm = document.getElementById('addFiscalPlan-form');
    addFiscalPlanForm.addEventListener('submit', async function (event) {
        event.preventDefault();
        if (addModal._isShown && $(this).valid()) {
            addModal.hide();
            await postFiscalPlan(new FormData(this));
        }
    });

    var updateFiscalPlanForm = document.getElementById('updateFiscalPlan-form');
    updateFiscalPlanForm.addEventListener('submit', async function (event) {
        event.preventDefault();
        if (updateModal._isShown && $(this).valid()) {
            updateModal.hide();
            let formData = new FormData(this);
            let isUpdated = await putFiscalPlan(formData);
            if (isUpdated) {
                updateFiscalPlan(formData);
            }
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
            var isDeleted = await deleteFiscalPlan(id, token);
            if (isDeleted) {
                removeFiscalPlan(id);
                }
        }

    });

    $('.fiscalPlan-card').on('click', function (event) {
        if (event.target.matches('.fiscalPlan-icon')) {
            switch (event.target.dataset.action) {
                case 'delete':
                    deleteModalLabel.textContent = `Delete '${this.dataset.name}'?`;
                    deleteModalId.value = this.dataset.id;
                    deleteModal.show();
                    break;
                case 'edit':
                    updateModalLabel.textContent = `Edit '${this.dataset.name}'`;
                    updateModalId.value = this.dataset.id;
                    updateModalName.value = this.dataset.name;
                    updateModal.show();
                    break;
            }
        }
        else {
            window.location.href = PAGE_ROUTES.FISCAL_PLAN(this.dataset.id);
        }
    });
}
