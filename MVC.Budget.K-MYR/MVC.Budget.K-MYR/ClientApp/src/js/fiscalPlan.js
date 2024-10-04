import { API_ROUTES, PAGE_ROUTES } from './config';
import { shortestAngle } from './utilities';
import { importChartDefaults, importBootstrapModals, importBootstrapCollapses } from './asyncComponents';
import { postTransaction, putTransaction, deleteTransaction, postCategory, putCategory, deleteCategory } from './api';

const currentDate = new Date();

const fiscalPlanId = document.getElementById('fiscalPlan_Id');
const menu = document.getElementById('menu-container');

const chartDefaultsTask = importChartDefaults();
const homeDashboardPromise = getHomeDashboard(menu, fiscalPlanId.value, currentDate, JSON.parse(fiscalPlanId.dataset.object));
const statisticsDashboardPromise = getStatisticsDashboard(fiscalPlanId.value, currentDate);
const reevaluationDashboardPromise = getReevaluationDashboard();
const modalsPromise = importBootstrapModals();
const collapsesPromise = importBootstrapCollapses()
    .then(() => {
        $('.accordion-head').on('click', function (event) {
            if (!event.target.classList.contains('add-category-icon')) {
                let collapse = $(this).next();                
                if (!collapse[0].classList.contains('collapsing')) {
                    collapse.collapse('toggle');
                    let caret = $('.accordion-caret', this)[0];
                    caret.classList.toggle('rotate');
                }
            }
        });
    });
const transactionsTablePromise = getTransactionsTable();    

setupFlipContainer();
setupMenuHandlers(modalsPromise, homeDashboardPromise);
setupDataTableHandlers(transactionsTablePromise);
setupRerenderHandlers(homeDashboardPromise, statisticsDashboardPromise, reevaluationDashboardPromise, transactionsTablePromise);

async function setupRerenderHandlers(homeDBPromise, statisticsDBPromise, reevaluationDBPromise, tablePromise) {
    var [homeDB, statisticsDB, reevaluationDB, transactionsTable] = await Promise.all(
        [homeDBPromise, statisticsDBPromise, reevaluationDBPromise, tablePromise]
    );
    window.addEventListener('countryChanged', () => {
        setTimeout(() => homeDB.formatDashboard(), 0);
        setTimeout(() => reevaluationDB.formatDashboard(), 0);
        setTimeout(() => statisticsDB.formatDashboard(), 0);
        setTimeout(() => transactionsTable.rows().invalidate().draw(), 0);
    })
}

async function setupDataTableHandlers(tablePromise) {
    var table = await tablePromise;

    $('#search-form').on('submit', async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            table.page(0);
            table.ajax.reload(null, false);
        }
    });

    var modals = await modalsPromise;
    var updateTransactionModal = modals.find(m => m._element.id == 'updateTransaction-modal');
    var deleteTransactionModal = modals.find(m => m._element.id == 'deleteTransaction-modal');

    initUpdateTransactionModal(updateTransactionModal, table);
    initDeleteTransactionModal(deleteTransactionModal, table);

    var idUpdate= document.getElementById('updateTransaction_id');
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

    table.on('click', 'svg', function (event) {
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
        }
    });

    var tableContainer = document.getElementById('table-container');
    tableContainer.style = '';
    table.columns.adjust();
}

async function setupMenuHandlers(modalsPromise, homeDBPromise) {
    var modals = await modalsPromise;
    var homeDashboard = await homeDBPromise;    
    var addCategoryModal = modals.find(m => m._element.id == 'addCategory-modal');
    var updateCategoryModal = modals.find(m => m._element.id == 'updateCategory-modal');
    var deleteCategoryModal = modals.find(m => m._element.id == 'deleteCategory-modal');
    var addTransactionModal = modals.find(m => m._element.id == 'addTransaction-modal');

    initAddCategoryModal(addCategoryModal, homeDashboard);
    initUpdateCategoryModal(updateCategoryModal, homeDashboard);
    initDeleteCategoryModal(deleteCategoryModal, homeDashboard);  
    initAddTransactionModal(addTransactionModal, homeDashboard); 

    document.getElementById('close-menu').onclick = function () {
        menu.classList.remove('active');
        var id = menu.dataset.categoryid;
        var borderBox = document.getElementById(`category_${id}`).querySelector('.border-animation');
        borderBox.classList.remove('border-rotate');
        menu.dataset.categoryid = 0;
    }    
    document.getElementById('details-menu').onclick = function () {
        var id = menu.dataset.categoryid;
        window.location.href = PAGE_ROUTES.CATEGORY(id);
    }
    homeDashboard.attachMenuHandlers();
}

function initAddCategoryModal(modal, homeDashboard) {
    var addCategoryModalType = document.getElementById('addCategory_type');
    var addCategoryModalFiscalPlanId = document.getElementById('addCategory_fiscalPlanId');
    var form = document.getElementById('addCategory-form');
    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            modal.hide();
            let category = await postCategory(new FormData(this));
            if (category) {
                homeDashboard.addCategory(category);
            }
        }
    });

    $('.add-category-icon').on('click', function () {
        var type = $(this).closest('.accordion')[0].dataset.type;
        addCategoryModalType.value = type;
        addCategoryModalFiscalPlanId.value = fiscalPlanId.value;
        modal.show();
    });
}

function initUpdateCategoryModal(modal, homeDashboard) {
    var updateCategoryModalLabel = document.getElementById('updateCategory-label');
    var updateCategoryModalId = document.getElementById('updateCategory_id');
    var updateCategoryModalName = document.getElementById('updateCategory_name');
    var updateCategoryModalBudget = document.getElementById('updateCategory_budget');
    var updateCategoryModalType = document.getElementById('updateCategory_type');
    var updateCategoryModalFiscalPlanId = document.getElementById('updateCategory_fiscalPlanId');
    var form = document.getElementById('updateCategory-form');
    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            modal.hide();
            let month = homeDashboard.getCurrentMonth();
            let formData = new FormData(this);
            let isUpdated = await putCategory(formData, month);
            if (isUpdated) {
                homeDashboard.editCategory(formData, month);
            }
        }
    });

    var editIcon = document.getElementById('edit-menu');
    editIcon.addEventListener('click', function () {
        let data = homeDashboard.getCategory(menu.dataset.categoryid);
        updateCategoryModalLabel.textContent = `Edit '${data.name}'`;
        updateCategoryModalId.value = data.id;
        updateCategoryModalName.value = data.name;
        updateCategoryModalBudget.value = data.budgetLimit?.budget ?? data.budget;
        updateCategoryModalType.value = data.categoryType;
        updateCategoryModalFiscalPlanId.value = fiscalPlanId.value;
        modal.show();
    });
}

function initDeleteCategoryModal(modal, homeDashboard) {
    var deleteCategoryModalLabel = document.getElementById('deleteCategory-label');
    var deleteCategoryModalId = document.getElementById('deleteCategory_id');
    var deleteCategoryModalType = document.getElementById('deleteCategory_type');
    var form = document.getElementById('deleteCategory-form');
    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        modal.hide();
        var formData = new FormData(this);
        var id = parseInt(formData.get('Id'));
        var type = parseInt(formData.get('Type'));
        var token = formData.get('__RequestVerificationToken');
        var isDeleted = await deleteCategory(id, type, token);
        if (isDeleted) {
            homeDashboard.removeCategory(id, type);
            menu.classList.remove('active');
            menu.dataset.categoryid = 0;
        }
    });
    var deleteIcon = document.getElementById('delete-menu');
    deleteIcon.addEventListener('click', function () {
        var data = homeDashboard.getCategory(menu.dataset.categoryid);
        deleteCategoryModalLabel.textContent = `Delete '${data.name}'`;
        deleteCategoryModalType.value = data.categoryType;
        deleteCategoryModalId.value = menu.dataset.categoryid;
        modal.show();
    });
}

function initAddTransactionModal(modal, homeDashboard) {
    var addTransactionModalCategoryId = document.getElementById('addTransaction_categoryId');
    var form = document.getElementById('addTransaction-form');
    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            modal.hide();
            let transaction = await postTransaction(new FormData(this));

            if (transaction) {
                homeDashboard.addTransaction(transaction);
            }
        }
    });

    var addIcon = document.getElementById('add-menu');
    addIcon.addEventListener('click', function () {
        addTransactionModalCategoryId.value = menu.dataset.categoryid;
        modal.show();
    });
}

function initUpdateTransactionModal(modal, table) {
    document.getElementById('updateTransaction-form').addEventListener('submit', async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            modal.hide();
            let formData = new FormData(this);
            let isUpdated = await putTransaction(formData);
            if (isUpdated) {
                let row = table.row((_, data) => data.id === parseInt(formData.get('Id')));
                if (row) {
                    let data = row.data();
                    data.amount = parseFloat(formData.get('Amount'));
                    data.title = formData.get('Title');
                    data.dateTime = formData.get('DateTime');
                    data.isHappy = formData.get('IsHappy') === 'true';
                    data.isNecessary = formData.get('IsNecessary') === 'true';
                    data.isEvaluated = formData.get('IsEvaluated') === 'true';
                    data.previousIsHappy = formData.get('PreviousIsHappy') === 'true';
                    data.PreviousIsNecessary = formData.get('PreviousIsNecessary') === 'true';
                    row.invalidate();
                }              
            }
        }
    });
}

function initDeleteTransactionModal(modal, table) {
    var form = document.getElementById('deleteTransaction-form');
    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        modal.hide();
        var formData = new FormData(this);
        var id = parseInt(formData.get('Id'));
        var token = formData.get('__RequestVerificationToken');
        var isDeleted = await deleteTransaction(id, token);
        if (isDeleted) {
            let row = table.row((_, data) => data.id === parseInt(formData.get('Id')));
            if (row) {
                row.remove().draw();
            }    
        }
    });  
}

function setupFlipContainer() {
    var faces = ['face_0', 'face_1', 'face_2', 'face_3'];
    var flipContainer = document.getElementById('flip-container-inner');
    var currentSideIndex = 0;
    var currentDeg = 0;

    $('#action-sidebar').on('click', '.sidebar-button-container', function () {
        var index = parseInt(this.dataset.index);
        if (currentSideIndex === index) {
            return;
        }

        var currentFace = document.getElementById(faces[currentSideIndex]);
        var nextFace = document.getElementById(faces[index]);

        var degreeDiff = shortestAngle(currentSideIndex, index);
        currentDeg += degreeDiff;

        flipContainer.style = `transform: rotateY(${currentDeg}deg)`;
        currentFace.classList.remove('visible-face');
        nextFace.classList.add('visible-face');
        currentSideIndex = index;
    });

    flipContainer.addEventListener('transitionend', () => {
        currentDeg = currentDeg % 360;
        flipContainer.style = `transform: rotateY(${currentDeg}deg); transition: transform 0s`;
    });
}

async function getTransactionsTable() {
    try {
        const { default: DataTable } = await import(/* webpackChunkName: "datatables" */'datatables.net-bs5');
        var lastAjaxData = {
            start: 0,     
            lastId: null,
            lastValue: null
        };
        var dataTable = new DataTable('#transactions-table', {
            processing: true,
            serverSide: true,
            deferLoading: 0,
            ajax: function (data, callback, settings) {
                var formData = new FormData(document.getElementById('search-form'));
                var table = new $.fn.dataTable.Api(settings);

                var searchString = formData.get('SearchString');
                var minDate = formData.get('MinDate');
                var maxDate = formData.get('MaxDate');
                var fiscalPlanId = formData.get('FiscalPlanId');
                var categoryId = formData.get('CategoryId');
                var minAmount = formData.get('MinAmount');
                var maxAmount = formData.get('MaxAmount');

                var isPrevious = false;
                var lastId = null;
                var lastValue = null;                
                var orderBy = null;
                var orderDirection = null;

                if (data.order?.[0]) {
                    orderBy = data.order[0].name;
                    orderDirection = data.order[0].dir;
                }

                if (data.start !== 0) {
                    let rowData = null;

                    if (lastAjaxData.start !== data.start) {
                        isPrevious = lastAjaxData.start > data.start;
                        rowData = isPrevious ? table.row(':first').data() : table.row(':last').data();
                    } else {
                        lastId = lastAjaxData.lastId;
                        lastValue = lastAjaxData.lastValue;
                    }

                    if (rowData) {
                        lastId = rowData.id;
                        if (orderBy) {
                            lastValue = rowData[orderBy];
                        }
                    }
                }
                                
                var requestData = {
                    draw: data.draw,
                    start: data.start,
                    pageSize: data.length,
                    orderBy: orderBy,
                    orderDirection: orderDirection === 'asc' ? 0 : 1,
                    lastId: lastId,
                    lastValue: lastValue,
                    isPrevious: isPrevious,
                    FiscalPlanId: fiscalPlanId.length > 0 ? parseInt(fiscalPlanId) : null,
                    SearchString: searchString.length > 0 ? searchString : null,
                    CategoryId: categoryId.length > 0 ? parseInt(categoryId) : null,
                    MinDate: minDate.length > 0 ? minDate: null,
                    MaxDate: maxDate.length > 0 ? maxDate : null,
                    MinAmount: minAmount.length > 0 ? parseFloat(minAmount) : null,
                    MaxAmount: maxAmount.length > 0 ? parseFloat(maxAmount) : null
                };
                $.ajax({
                    url: API_ROUTES.transactions.GET_SEARCH,
                    type: 'POST',
                    contentType: 'application/json',
                    headers: {
                        'RequestVerificationToken': formData.get('__RequestVerificationToken')
                    },
                    data: JSON.stringify(requestData),
                    success: function (response) {
                        callback({
                            draw: response.draw,
                            recordsFiltered: data.start + response.transactions.length + (response.hasNext === true ? 1 : 0),
                            data: response.transactions
                        });
                        lastAjaxData = {
                            start: requestData.start,
                            lastId: requestData.lastId,
                            lastValue: requestData.lastValue
                        };                      
                    },
                    error: function (xhr, status, error) {
                        console.error(`Couldn't fetch transactions'`, error);
                    }
                });
              
            },
            info: false,
            layout: {
                topStart: null,
                topEnd: null,
                bottomStart: 'pageLength',
                bottomEnd: {
                    paging: {
                        type: 'simple',
                        numbers: false 
                    }
                }
            },
            lengthMenu: [10, 25, 50, 100],
            columns: [
                { data: 'title', render: DataTable.render.text(), name: 'title' },
                { data: 'dateTime', name: 'dateTime' },
                { data: 'amount', name: 'amount' },
                { data: 'category', render: DataTable.render.text(), name: 'category.name' },
                {
                    data: null,
                    defaultContent:
                    `<div class="d-flex justify-content-center align-items-center flex-wrap gap-2">
                        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" class="table-icon" fill="rgba(255, 255, 255, 1)" data-icon="edit">
                            <use href="#edit-icon" xlink:href="#edit-icon"/>
                        </svg >
                        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" class="table-icon" viewBox="0 0 16 16" fill="rgba(255, 255, 255, 1)" data-icon="delete">
                            <use href="#trash-icon" xlink:href="#trash-icon"/>
                        </svg>
                    </div>`,
                    targets: -1,
                    sortable: false
                },
            ],
            columnDefs: [{
                targets: 2,
                render: function (data, type, row) {
                    if (type === 'display') {
                        return window.userNumberFormat.format(data);
                    } else {
                        return data;
                    }
                }
            }, {
                targets: 1,
                render: function (data, type, row) {
                    if (type === 'display') {
                        return new Date(data).toLocaleString(window.userLocale);
                    } else {
                        return data;
                    }
                }
            }],
            scrollX: true,
            scrollCollapse: true
        });      
        return dataTable;
    } catch (error) {
        console.error('Error loading Datatable:', error);
        throw error;
    }
}

async function getStatisticsDashboard(id, date) {
    try {
        const { default: StatisticsDashboard } = await import(/* webpackChunkName: "statisticsDashboard" */'./statisticsDashboard');
        await chartDefaultsTask;

        return new StatisticsDashboard(id, date);
    } catch (error) {
        console.error('Error loading statistics dashboard:', error);
        throw error;
    }    
}

async function getHomeDashboard(menu, id, date, data) {
    try {
        const { default: HomeDashboard } = await import(/* webpackChunkName: "homeDashboard"*/ './homeDashboard');
        await chartDefaultsTask;

        return new HomeDashboard(menu, id, date, data);

    } catch (error) {
        console.error('Error loading home dashboard:', error);
        throw error;
    }
} 

async function getReevaluationDashboard() {
    try {
        const { default: ReevaluationDashboard } = await import(/* webpackChunkName: "reevaluationDashboard" */'./reevaluationDashboard');

        return new ReevaluationDashboard();
    } catch (error) {
        console.error('Error loading reevaluation dashboard:', error);
        throw error;
    }    
} 
