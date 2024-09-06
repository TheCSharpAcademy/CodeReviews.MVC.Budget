import { shortestAngle } from './utilities';
import { importChartDefaults, importBootstrapModals, importBootstrapCollapses } from './asyncComponents';
import { postTransaction, getTransactions, postCategory, putCategory, deleteCategory } from './api';

const currentDate = new Date();

const fiscalPlanId = document.getElementById("fiscalPlan_Id");
const menu = document.getElementById('menu-container');

const chartDefaultsTask = importChartDefaults();
const homeDashboardPromise = getHomeDashboard(menu, fiscalPlanId.value, currentDate, JSON.parse(fiscalPlanId.dataset.object));
const statisticsDashboardPromise = getStatisticsDashboard(fiscalPlanId.value, currentDate);

const reevaluationDashboardPromise = getReevaluationDashboard(fiscalPlanId.value);
const modalsPromise = importBootstrapModals();
const collapsesPromise = importBootstrapCollapses()
    .then((collapses) => {
    $(".accordion-head").on("click", function (event) {
        if (!event.target.classList.contains("add-category-icon")) {
            collapses.find(c => c._element.id == this.nextElementSibling.id).toggle();
            let caret = $('.accordion-caret', this)[0];
            caret.classList.toggle("rotate");
        }
    })
});

const transactionsTablePromise = getTransactionsTable()
    .then((table) => {
        document.getElementById('search_fiscalPlanId').value = fiscalPlanId.value;
        $('#search-form').on("submit", async function (event) {
            event.preventDefault();
            if ($(this).valid()) {
                table.ajax.reload(null, false);
            }
        });
        
        return table;
    });

setupFlipContainer();
setupModalHandlers();
setupRerenderHandlers();

async function setupRerenderHandlers() {
    var [homeDB, statisticsDB, reevaluationDB, transactionsTable] = await Promise.all(
        [homeDashboardPromise, statisticsDashboardPromise, reevaluationDashboardPromise, transactionsTablePromise]
    );
    window.addEventListener('countryChanged', () => {
        setTimeout(() => homeDB.formatDashboard(), 0);
        setTimeout(() => reevaluationDB.formatDashboard(), 0);
        setTimeout(() => statisticsDB.formatDashboard(), 0);
        setTimeout(() => transactionsTable.rows().invalidate().draw(), 0);
    })
}

async function setupModalHandlers() {
    const modals = await modalsPromise;
    const homeDashboard = await homeDashboardPromise;
    const addCategoryModal = modals.find(m => m._element.id == "addCategory-modal");
    const addCategoryModalType = document.getElementById("addCategory_type");
    const addCategoryFiscalPlanId = document.getElementById("addCategory_fiscalPlanId");

    const updateCategoryModal = modals.find(m => m._element.id == "updateCategory-modal");
    const updateCategoryModalLabel = document.getElementById("updateCategory-label");
    const updateCategoryModalId = document.getElementById("updateCategory_id");
    const updateCategoryModalName = document.getElementById("updateCategory_name");
    const updateCategoryModalBudget = document.getElementById("updateCategory_budget");
    const updateCategoryModalType = document.getElementById("updateCategory_type");
    const updateCategoryFiscalPlanId = document.getElementById("updateCategory_fiscalPlanId");

    const addTransactionModal = modals.find(m => m._element.id == "addTransaction-modal");
    const addTransactionModalCategoryId = document.getElementById("addTransaction_categoryId");

    document.getElementById('add-category-form').addEventListener("submit", async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            addCategoryModal.hide();
            let category = await postCategory(new FormData(this));

            if (category) {
                homeDashboard.addCategory(category);
            }
        }
    });
    document.getElementById('add-transaction-form').addEventListener("submit", async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            addTransactionModal.hide();
            let transaction = await postTransaction(new FormData(this));

            if (transaction) {
                homeDashboard.addTransaction(transaction);
            }
        }
    });
    document.getElementById('update-category-form').addEventListener("submit", async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            updateCategoryModal.hide();
            let month = homeDashboard.getCurrentMonth().toISOString();
            let formData = new FormData(this);
            let isUpdated = await putCategory(formData, month);

            if (isUpdated) {
                homeDashboard.editCategory(formData, month);
            }
        }
    });

    $(".add-category-icon").on("click", function () {
        var type = $(this).closest('.accordion')[0].dataset.type;
        addCategoryModalType.value = type;
        addCategoryFiscalPlanId.value = fiscalPlanId.value;
        addCategoryModal.show();
    });

    document.getElementById('close-menu').onclick = function () {
        menu.classList.remove('active');
        var id = menu.dataset.categoryid;
        var borderBox = document.getElementById(`category_${id}`).querySelector('.border-animation');
        borderBox.classList.remove('border-rotate');
        menu.dataset.categoryid = 0;
    }
    document.getElementById('add-menu').onclick = async function () {
        addTransactionModalCategoryId.value = menu.dataset.categoryid;
        addTransactionModal.show();
    }
    document.getElementById('edit-menu').onclick = function () {
        var data = homeDashboard.getCategory(menu.dataset.categoryid);
        updateCategoryModalLabel.textContent = data.name;
        updateCategoryModalId.value = data.id;
        updateCategoryModalName.value = data.name;
        updateCategoryModalBudget.value = data.budgetLimit?.budget ?? data.budget;
        updateCategoryModalType.value = data.categoryType;    
        updateCategoryFiscalPlanId.value = fiscalPlanId.value;
        updateCategoryModal.show();
    }
    document.getElementById('delete-menu').onclick = function () {
        var token = menu.querySelector('input').value;
        var id = parseInt(menu.dataset.categoryid);
        var data = homeDashboard.getCategory(menu.dataset.categoryid);
        var isDeleted = deleteCategory(id, data.categoryType, token)
        if (isDeleted) {
            homeDashboard.removeCategory(id, data.categoryType);
            menu.classList.remove('active');
            menu.dataset.categoryid = 0;
        }
    }
    document.getElementById('details-menu').onclick = function () {
        var id = menu.dataset.categoryid;
        window.location.href = "Category/" + id;
    }

    homeDashboard.setupMenuHandlers();
}

function setupFlipContainer() {
    var faces = ["face_0", "face_1", "face_2", "face_3"];
    var flipContainer = document.getElementById("flip-container-inner");
    var currentSideIndex = 0;
    var currentDeg = 0;

    $('#action-sidebar').on("click", '.sidebar-button-container', async function () {
        var index = parseInt(this.dataset.index);
        if (currentSideIndex === index) {
            return;
        }

        var currentFace = document.getElementById(faces[currentSideIndex]);
        var nextFace = document.getElementById(faces[index]);

        var degreeDiff = shortestAngle(currentSideIndex, index);
        currentDeg += degreeDiff;

        flipContainer.style = `transform: rotateY(${currentDeg}deg)`;
        currentFace.classList.remove("visible-face");
        nextFace.classList.add("visible-face");
        currentSideIndex = index;
    });

    flipContainer.addEventListener("transitionend", () => {

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
                var formData = new FormData(document.getElementById("search-form"));
                var table = new $.fn.dataTable.Api(settings);

                var searchString = formData.get("SearchString");
                var minDate = formData.get("MinDate");
                var maxDate = formData.get("MaxDate");
                var fiscalPlanId = formData.get("FiscalPlanId");
                var categoryId = formData.get("CategoryId");
                var minAmount = formData.get("MinAmount");
                var maxAmount = formData.get("MaxAmount");

                var isPrevious = lastAjaxData.start > data.start;
                var lastId = lastAjaxData.lastId;
                var lastValue = lastAjaxData.lastValue;
                var orderBy = null;
                var orderDirection = null;

                if (data.order[0]) {
                    orderBy = data.order[0].name;
                    orderDirection = data.order[0].dir;
                }

                if (data.start !== 0) {
                    var rowData = null;

                    if (lastAjaxData.start > data.start) {
                        rowData = table.row(':first').data();
                    }
                    else if (lastAjaxData.start < data.start) {
                        rowData = table.row(':last').data();
                    }                    
                    lastId = rowData.id;

                    if (orderBy) {
                        lastValue = rowData[orderBy];
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
                console.log(requestData);
                $.ajax({
                    url: 'https://localhost:7246/api/Transactions/search',
                    type: 'POST',
                    contentType: 'application/json',
                    headers: {
                        "RequestVerificationToken": formData.get('__RequestVerificationToken')
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
            columns: [
                { data: 'title', render: DataTable.render.text(), name: 'title' },
                { data: 'dateTime', name: 'dateTime' },
                { data: 'amount', name: 'amount' },
                { data: 'category', render: DataTable.render.text(), name: 'category' },
                {
                    data: null,
                    defaultContent:
                    `<div class="d-flex justify-content-center align-items-center flex-wrap gap-2">
                        <svg  width='20' height='20' fill='rgba(255, 255, 255, 1)' class='table-icon' viewBox='0 0 16 16'>
                            <defs>
                            <linearGradient id="icon_gradient" x1="0%" y1="0%" x2="100%" y2="0%">
                                <stop offset="0%" style="stop-color:#CCFBE5;stop-opacity:1" />
                                <stop offset="50%" style="stop-color:#A2D6CB;stop-opacity:1" />
                                <stop offset="100%" style="stop-color:#7EB1B1;stop-opacity:1" />
                            </linearGradient>
                            </defs>
                            <path d='M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z'/>
                            <path fill-rule="evenodd" d='M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z'>
                        </svg >
                        <svg width="20" height="20" fill="rgba(255, 255, 255, 1)" viewBox="0 0 16 16" class="table-icon">
                            <defs>
                                <linearGradient id="icon_gradient" x1="0%" y1="0%" x2="100%" y2="0%">
                                    <stop offset="0%" style="stop-color:#CCFBE5;stop-opacity:1" />
                                    <stop offset="50%" style="stop-color:#A2D6CB;stop-opacity:1" />
                                    <stop offset="100%" style="stop-color:#7EB1B1;stop-opacity:1" />
                                </linearGradient>
                            </defs>
                          <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                          <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
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
        dataTable.on('click', 'svg', function (event) {
            var row = dataTable.row(event.target.closest('tr'));
            var data = row.data();
            console.log(data, this);
        });
        var table = document.getElementById("table-container");
        table.style = "";
        dataTable.columns.adjust();
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

async function getReevaluationDashboard(id) {
    try {
        const { default: ReevaluationDashboard } = await import(/* webpackChunkName: "reevaluationDashboard" */'./reevaluationDashboard');

        return new ReevaluationDashboard(id);
    } catch (error) {
        console.error('Error loading reevaluation dashboard:', error);
        throw error;
    }    
} 
