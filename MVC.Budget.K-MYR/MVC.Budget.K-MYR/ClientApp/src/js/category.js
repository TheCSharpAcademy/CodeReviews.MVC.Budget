import { importChartDefaults, importBootstrapCollapses, importBootstrapModals } from './asyncComponents';
import { postTransaction, putTransaction, deleteTransaction } from './api'
import messageBox from './messageBox';
import { setupRefocusHandlers } from './utilities'

const chartDefaultsTask = importChartDefaults();
const smallScreenSize = 576;
const currentDate = new Date();
const categoryId = document.getElementById('category_Id');

const categoryDashboardPromise = getCategoryDashboard(currentDate, JSON.parse(categoryId.dataset.object));
const modalsPromise = importBootstrapModals()
    .catch(() => {
        messageBox.addAndShow('A critical error occurred. Please reload the page', '#cross-icon', false);
    });
const collapsesPromise = importBootstrapCollapses()
    .then(() => {
        $('.accordion-head').on('click keydown', function (event) {
            if (event.type === 'click' || event.type === 'keydown' && event.key === 'Enter') {
                if (event.target.id !== 'addTransaction-button') {
                    let collapse = $(this).next();
                    if (!collapse[0].classList.contains('collapsing')) {
                        collapse.collapse('toggle');
                        let caret = $('.accordion-caret', this)[0];
                        caret.classList.toggle('rotate');
                    }
                }
            }
        });
    })
    .catch(() => {
        messageBox.addAndShow('A critical error occurred. Please reload the page.', '#cross-icon', false);
    });

const tooltipsPromise = getTooltips();
setupDataTableHandlers(categoryDashboardPromise, modalsPromise)
initAddTransactionModal(categoryDashboardPromise, modalsPromise)
setupRerenderHandlers(categoryDashboardPromise);
setupRefocusHandlers();

async function getCategoryDashboard(id, date, data) {
    try {
        const { default: CategoryDashboard } = await import(/* webpackChunkName: "categoryDashboard"*/ './categoryDashboard');
        await chartDefaultsTask;

        return new CategoryDashboard(id, date, data);

    } catch(error) {
        messageBox.addAndShow('A critical error occurred. Please reload the page', '#cross-icon', false);
    }
} 

async function setupRerenderHandlers(dashboardPromise) {
    var dashBoard = await dashboardPromise;   
    await dashBoard.initPromise;
    window.addEventListener('countryChanged', () => {
        setTimeout(() => dashBoard.formatDashboard(), 0);
    })
}

async function setupDataTableHandlers(dashboardPromise, modalsPromise) {
    var dashBoard = await dashboardPromise;   
    await dashBoard.initPromise;
    var table = dashBoard.table;
    var modals = await modalsPromise;

    var updateTransactionModal = modals.find(m => m._element.id == 'updateTransaction-modal');
    var deleteTransactionModal = modals.find(m => m._element.id == 'deleteTransaction-modal');

    initUpdateTransactionModal(dashBoard, updateTransactionModal, table);
    initDeleteTransactionModal(dashBoard, deleteTransactionModal, table);

    var idUpdate = document.getElementById('updateTransaction_id');
    var labelUpdate = document.getElementById('updateTransaction-label');
    var title = document.getElementById('updateTransaction_title');
    var dateTime = document.getElementById('updateTransaction_datetime');
    var amount = document.getElementById('updateTransaction_amount');
    var isHappy = document.getElementById('updateTransaction_isHappyTrue');
    var isUnhappy = document.getElementById('updateTransaction_isHappyFalse');
    var isNecessary = document.getElementById('updateTransaction_isNecessaryTrue');
    var isUnnecessary = document.getElementById('updateTransaction_isNecessaryFalse');

    var labelDelete = document.getElementById('deleteTransaction-label');
    var idDelete = document.getElementById('deleteTransaction_id');
    table.on('click keydown', 'svg', function (event) {
        if (event.type === 'click' || event.type === 'keydown' && event.key === 'Enter') {

            var row = table.row(event.target.closest('tr'));
            var data = row.data();
            switch (this.dataset.icon) {
                case 'edit':
                    idUpdate.value = data.id;
                    labelUpdate.textContent = `Edit '${data.title}'`;
                    title.value = data.title;
                    dateTime.value = data.dateTime.slice(0, 19);
                    amount.value = data.amount;
                    let element = data.isHappy ? isHappy : isUnhappy;
                    element.checked = true;
                    element = data.isNecessary ? isNecessary : isUnnecessary;
                    element.checked = true;
                    updateTransactionModal.show();
                    break;
                case 'delete':
                    idDelete.value = data.id;
                    labelDelete.textContent = `Delete '${data.title}'`;
                    deleteTransactionModal.show();
                    break;
            }
        }
    });

    var tableContainer = document.getElementById('table-container');
    tableContainer.style = '';
    table.columns.adjust();
}

async function initAddTransactionModal(dashboardPromise, modalsPromise) {
    var modals = await modalsPromise;
    var dB = await dashboardPromise;
    var modal = modals.find(m => m._element.id == 'addTransaction-modal');
    var addTransactionModalCategoryId = document.getElementById('addTransaction_categoryId');
    var form = document.getElementById('addTransaction-form');
    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        if (modal._isShown && $(this).valid()) {
            modal.hide();
            let response = await postTransaction(new FormData(this));
            if (response.isSuccess) {
                dB.addTransaction(response.data);
            }
            messageBox.addAndShow(response.message, response.isSuccess ? '#check-icon' : '#cross-icon'); 
        }
    });

    var addIcon = document.getElementById('addTransaction-button');
    $(addIcon).on('click keydown', function (event) {
        if (event.type === 'click' || event.type === 'keydown' && event.key === 'Enter') {
            addTransactionModalCategoryId.value = categoryId.value;
            modal.show();
        }
    });
}

function initUpdateTransactionModal(dashboard, modal, table) {
    var form = document.getElementById('updateTransaction-form');
    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        if (modal._isShown && $(this).valid()) {
            modal.hide();
            let formData = new FormData(this);
            let response = await putTransaction(formData);
            if (response.isSuccess) {
                let row = table.row((_, data) => data.id === parseInt(formData.get('Id')));
                if (row) {
                    let data = row.data();
                    let newAmount = parseFloat(formData.get('Amount'));
                    let newIsHappy = formData.get('IsHappy') === 'true';
                    let newIsNecessary = formData.get('IsNecessary') === 'true'; 
                    let newDateTime = new Date(formData.get('DateTime')); 
                    dashboard.editTransaction(data, newAmount, newIsHappy, newIsNecessary, newDateTime);
                    data.amount = newAmount;
                    data.title = formData.get('Title');
                    data.dateTime = formData.get('DateTime');
                    data.isHappy = newIsHappy;
                    data.isNecessary = newIsNecessary;
                    data.isEvaluated = formData.get('IsEvaluated') === 'true';
                    data.previousIsHappy = formData.get('PreviousIsHappy') === 'true';
                    data.PreviousIsNecessary = formData.get('PreviousIsNecessary') === 'true';
                    row.invalidate();
                }
                messageBox.addAndShow(response.message, response.isSuccess ? '#check-icon' : '#cross-icon'); 
            }
        }
    });
}

function initDeleteTransactionModal(dashboard, modal, table) {
    var form = document.getElementById('deleteTransaction-form');
    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        if (modal._isShown) {
            modal.hide();
            var formData = new FormData(this);
            var id = parseInt(formData.get('Id'));
            var token = formData.get('__RequestVerificationToken');
            var response = await deleteTransaction(id, token);
            if (response.isSuccess) {              
                let row = table.row((_, data) => data.id === parseInt(formData.get('Id')));
                if (row) {
                    dashboard.removeTransaction(row.data());
                    row.remove().draw();
                }
            }
            messageBox.addAndShow(response.message, response.isSuccess ? '#check-icon' : '#cross-icon'); 
        }
    });
}

async function getTooltips() {
    const Tooltip = (await import(/* webpackChunkName: "bootstrap-tooltips" */'bootstrap/js/dist/tooltip')).default;
    var tooltipElements = document.querySelectorAll('.tooltipped');
    var tooltips = [...tooltipElements].map(element => new Tooltip(element, {
        container: 'body',
        delay: { show: 500, hide: 0 },
        offset: [0, 10],
        placement: (instance, _) => {
            var query = window.matchMedia(`(min-width: ${smallScreenSize}px)`);
            return instance._element.classList.contains('sidebar-button-container')
                && query.matches ? 'right' : 'top';
        },
    }));
    return tooltips;
}