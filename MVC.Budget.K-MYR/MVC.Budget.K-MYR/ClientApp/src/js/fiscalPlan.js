import { shortestAngle, resetStyle} from './utilities';
import { getCountrySelect, importChartDefaults, importBootstrapModals, importBootstrapCollapses } from './asyncComponents';
import 'jquery-validation';

const currentDate = new Date();
const chartDefaultsTask = importChartDefaults();

const incomeCategoriesAPI = "https://localhost:7246/api/IncomeCategories";
const expenseCategoriesAPI = "https://localhost:7246/api/ExpenseCategories";
const transactionsAPI = "https://localhost:7246/api/Transactions";
const fiscalPlanAPI = "https://localhost:7246/api/FiscalPlan";
const fiscalPlanId = document.getElementById("fiscalPlan_Id");
const menu = document.getElementById('menu-container');

const addCategoryModal = document.getElementById("addCategory-modal")
const addCategoryModalType = addCategoryModal.querySelector("#addCategory_type");
const updateCategoryModal = document.getElementById("updateCategory-modal")
const updateCategoryModalLabel = updateCategoryModal.querySelector("#updateCategory-label");
const updateCategoryModalId = updateCategoryModal.querySelector("#updateCategory_id");
const updateCategoryModalName = updateCategoryModal.querySelector("#updateCategory_name");
const updateCategoryModalBudget = updateCategoryModal.querySelector("#updateCategory_budget");
const updateCategoryModalType = updateCategoryModal.querySelector("#updateCategory_type");
const addTransactionModal = document.getElementById("addTransaction-modal")
const addTransactionModalCategoryId = addTransactionModal.querySelector("#addTransaction_categoryId");
const flipContainer = document.getElementById("flip-container-inner");

const statisticsDashboard = getStatisticsDashboard(fiscalPlanId.value, currentDate, fiscalPlanAPI);
const homeDashboard = getHomeDashboard(menu, fiscalPlanId.value, currentDate, fiscalPlanAPI);
const reevaluationDashboard = getReevaluationDashboard(fiscalPlanId.value, expenseCategoriesAPI, transactionsAPI);
const countrySelect = initializeCountrySelect();
const modals = importBootstrapModals().then((modalsArray) => {
    let addCategoryModal = modalsArray.find(m => m._element.id == "addCategory-modal")
    let addTransactionModal = modalsArray.find(m => m._element.id == "addTransaction-modal")
    let updateCategoryModal = modalsArray.find(m => m._element.id == "updateCategory-modal")

    $('#add-category-form').on("submit", async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            addCategoryModal.hide();
            await addCategory(new FormData(this));
        }
    });
    $('#add-transaction-form').on("submit", async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            addTransactionModal.hide();
            await addTransaction(new FormData(this));
        }
    });
    $('#update-category-form').on("submit", async function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            updateCategoryModal.hide();
            await updateCategory(new FormData(this));
        }
    });

    document.getElementById('close-menu').onclick = function () {
        menu.classList.remove('active');
        var id = menu.dataset.categoryid;
        var borderBox = document.getElementById(`category_${id}`).querySelector('.border-animation');
        borderBox.classList.remove('border-rotate');
        menu.dataset.categoryid = 0;
    };
    document.getElementById('add-menu').onclick = async function () {
        addTransactionModalCategoryId.value = menu.dataset.categoryid;
        addTransactionModal.show();
    };
    document.getElementById('edit-menu').onclick = function () {
        var category = document.getElementById(`category_${menu.dataset.categoryid}`);

        updateCategoryModalLabel.textContent = category.dataset.name;
        updateCategoryModalId.value = category.dataset.id;
        updateCategoryModalName.value = category.dataset.name;
        updateCategoryModalBudget.value = category.dataset.budget;
        updateCategoryModalType.value = category.dataset.type;

        updateCategoryModal.show();
    };
    document.getElementById('delete-menu').onclick = function () {
        var token = menu.querySelector('input').value;
        var id = menu.dataset.categoryid;
        var type = menu.dataset.type;
        if (deleteCategory(id, type, token)) {
            menu.classList.remove('active');
            menu.dataset.categoryid = 0;
        }
    };
    document.getElementById('details-menu').onclick = function () {
        var id = menu.dataset.categoryid;
        window.location.href = "Category/" + id;
    };
    return modalsArray
});
const collapses = importBootstrapCollapses().then(async function (collapses) {
    let modalsArray = await modals;    
    let modal = modalsArray.find(m => m._element.id == "addCategory-modal")
    $(".accordion-head").on("click", function (event) {
        if (event.target.closest("svg.add-icon")) {
            let type = $(this).closest('.accordion')[0].dataset.type;
            addCategoryModalType.value = type;
            modal.show();
        }

        else {            
            collapses.find(c => c._element.id == this.nextElementSibling.id).toggle();
            let caret = $('.accordion-caret', this)[0];
            caret.classList.toggle("rotate");
        }
    });
    return collapses;
});
const transactionsTable = getTransactionsTable();

var currentSideIndex = 0;
var currentDeg = 0;

let elements = document.querySelectorAll('.flip-content');
let observer = new ResizeObserver(entries => {
    entries.forEach(entry => {
        let width = entry.contentRect.width;
        let translateZValue = (width / 2);

        entry.target.style.transform = `rotateY(calc(90deg * var(--s))) translateZ(${translateZValue}px)`;
    });
});
elements.forEach(element => {
    observer.observe(element);
});

$('#search-form').on("submit", async function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        await getTableData(new FormData(this));
    }
});

$('#action-sidebar').on("click", '.sidebar-button-container', async function (event) {
    if (currentSideIndex === this.dataset.index) {
        return;
    }

    var degreeDiff = shortestAngle(currentSideIndex, this.dataset.index);

    currentDeg += degreeDiff;

    flipContainer.style = `transform: rotateY(${currentDeg}deg)`;

    currentSideIndex = this.dataset.index;
});

flipContainer.addEventListener("transitionend", () => {

    currentDeg = currentDeg % 360;
    resetStyle(flipContainer, `transform: rotateY(${currentDeg}deg)`);
});

async function addTransaction(data) {
    try {
        var response = await fetch(`${transactionsAPI}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Title: data.get("Title"),
                Amount: parseFloat(data.get("Amount")),
                DateTime: data.get("DateTime"),
                IsHappy: data.get("IsHappy") === "true" ? true : false,
                IsNecessary: data.get("IsNecessary") === "true" ? true : false,
                CategoryId: parseInt(data.get("CategoryId"))
            })
        });

        if (response.ok) {
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
    };
}

async function addCategory(data) {
    try {
        var response = await fetch(`${data.get("type") == 1 ? incomeCategoriesAPI : expenseCategoriesAPI}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: data.get("Name"),
                Budget: parseFloat(data.get("Budget")),
                FiscalPlanId: parseInt(fiscalPlanId.value)
            })
        });

        if (response.ok) {
            return true;
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
            return false;
        }

    } catch (error) {
        console.error(error);
        return false;
    }
}

async function updateCategory(data) {
    try {
        var id = parseInt(data.get("Id"));     
        let queryParams = new URLSearchParams({
            Month: homeMonthPicker.datepicker('getUTCDate').toISOString()
        });
        var response = await fetch(`${data.get("type") == 1 ? incomeCategoriesAPI : expenseCategoriesAPI}/${id}?${queryParams}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: data.get("Name"),
                Budget: parseFloat(data.get("Budget")),
                GroupId: parseInt(data.get("GroupId")),
                Id: id,
                FiscalPlanId: parseInt(fiscalPlanId.value)
            })
        });

        if (response.ok) {
            return true;
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
            return false;
        }

    } catch (error) {
        console.error(error);
        return false;
    }
}

async function deleteCategory(id, type, token) {
    try {
        var response = await fetch(`${type == 1 ? incomeCategoriesAPI : expenseCategoriesAPI}/${id}`, {
            method: "DELETE",
            headers: {
                "RequestVerificationToken": token
            }
        });

        if (response.ok) {
            document.getElementById(`category_${id}`).remove();
            return true;
        } else {
            console.error(`HTTP Delete Error: ${response.status}`);
            return false;
        }

    } catch (error) {
        console.error(error);
        return false;
    }
}

async function getTableData(data) {
    try {
        let params = new URLSearchParams();

        for (let [key, value] of data.entries()) {
            if (value !== undefined && value !== '') {
                params.append(key, value);
            }
        }

        let query_string = params.toString();

        var response = await fetch(`${transactionsAPI}?${query_string}`, {
            method: "GET",
        });

        if (response.ok) {
            var data = await response.json();
            let table = await transactionsTable;
            table.clear();
            table.rows.add(data);
            table.draw();
        } else {
            table.clear();
            console.error(`HTTP GET Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
    }
}

async function getTransactionsTable() {
    const { default: DataTable, row, data } = await import(/* webpackChunkName: "datatables" */'datatables.net-bs5');
    let dataTable = new DataTable('#transactions-table', {
        info: false,
        dom: '<"pb-1" t<"d-flex justify-content-between mt-3"<"pt-1"l>p>>',
        columns: [
            { data: 'title' }, { data: 'dateTime' }, { data: 'amount' }, { data: 'category' }, {
                data: null,
                defaultContent:
                `<svg  width='20' height='20' fill='rgba(255, 255, 255, 1)' class='me-2 table-icon' viewBox='0 0 16 16'>
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
                </svg>`,
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
    dataTable.on('click', 'svg', function () {
        var row = dataTable.row($(this).parents('tr'));
        var data = row.data();
        console.log(data);
    });
    return dataTable;
}

async function getStatisticsDashboard(id, date, apiUrl) {
    await chartDefaultsTask;
    const { default: StatisticsDashboard } = await import(/* webpackChunkName: "statisticsDashboard" */'./statisticsDashboard');
  
    return new StatisticsDashboard(id, date, apiUrl);
}

async function getHomeDashboard(menu, id, date, apiUrl) {
    await chartDefaultsTask;
    const { default: HomeDashboard } = await import(/* webpackChunkName: "homeDashboard" */'./homeDashboard');

    return new HomeDashboard(menu, id, date, apiUrl);
} 

async function getReevaluationDashboard(id, categoriesAPI, transactionsAPI) {
    const { default: ReevaluationDashboard } = await import(/* webpackChunkName: "reevaluationDashboard" */'./reevaluationDashboard');

    return new ReevaluationDashboard(id, categoriesAPI, transactionsAPI);
} 

async function initializeCountrySelect() {
    let countrySelect = await getCountrySelect("#country");
    countrySelect.on('change', () => {
        let iso2Code = countrySelect.countrySelect("getSelectedCountryData").iso2;
       
    });
}
