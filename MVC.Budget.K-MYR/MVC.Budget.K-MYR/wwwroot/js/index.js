class Statistics {
    #data;
    #sentimentChartYearly = document.getElementById('sentimentChartYear');
    #necessityChartYearly = document.getElementById('necessityChartYear');
    #sentimentBarChart = document.getElementById('sentimentLineChartYear');
    #necessityBarChart = document.getElementById('necessityLineChartYear');
    #overspendingChart = document.getElementById('overspendingChart');
    #totalSpentChart = document.getElementById('totalSpentChart');
    #overspendingHeading = document.getElementById('statistics-overspending');

    constructor(data) {
        this.#data = data;
    }

    async loadData() {
        this.#data = await getStatistics();
    }

    render() {
        new Chart(this.#sentimentChartYearly, {
            type: 'doughnut',
            data: {
                labels: [
                    'Happy',
                    'Unhappy'
                ],
                datasets: [{
                    label: 'Total Amount',
                    data: [this.#data.happyEvaluatedTotal, this.#data.unhappyEvaluatedTotal],
                    backgroundColor: [
                        'rgb(25,135,84)',
                        'rgb(220,53,69)'
                    ],
                    hoverOffset: 4
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';

                                if (label) {
                                    label += ': ';
                                }
                                if (context.parsed.y !== null) {
                                    label += numberFormat.format(context.parsed);
                                }
                                return label;
                            }
                        }
                    }
                }
            }
        });

        new Chart(this.#necessityChartYearly, {
            type: 'doughnut',
            data: {
                labels: [
                    'Necessary',
                    'Unnecessary'
                ],
                datasets: [{
                    label: 'Total Amount',
                    data: [this.#data.necessaryEvaluatedTotal, this.#data.unnecessaryEvaluatedTotal],
                    backgroundColor: [
                        'rgb(25,135,84)',
                        'rgb(220,53,69)'
                    ],
                    hoverOffset: 4
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';

                                if (label) {
                                    label += ': ';
                                }
                                if (context.parsed.y !== null) {
                                    label += numberFormat.format(context.parsed);
                                }
                                return label;
                            }
                        }
                    }
                }
            }
        });

        new Chart(this.#sentimentBarChart, {
            type: 'bar',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
                datasets: [{
                    label: 'Happy',
                    stack: 'Unevaluated',
                    data: this.#data.happyPerMonth,
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#20c997',
                },
                {
                    label: 'Unhappy',
                    stack: 'Unevaluated',
                    data: this.#data.unhappyPerMonth,
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: 'rgb(220,53,69)'
                },
                {
                    label: 'Happy (Eval.)',
                    stack: 'Evaluated',
                    data: this.#data.happyEvaluatedPerMonth,
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#0f7c5c',
                },
                {
                    label: 'Unhappy (Eval.)',
                    stack: 'Evaluated',
                    data: this.#data.unhappyEvaluatedPerMonth,
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#881d27',
                },
                {
                    label: 'Unevaluated',
                    stack: 'Evaluated',
                    data: this.#data.unevaluatedPerMonth,
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#1c1c1c',
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        border: {
                            color: '#d3d3d3',
                        },
                        grid: {
                            color: '#d3d3d3',
                            lineWidth: 0.2,
                        },
                        ticks: {
                            color: '#d3d3d3',
                            callback: function (value, index, ticks) {
                                return numberFormat.format(value);
                            }
                        }
                    },
                    x: {
                        border: {
                            color: '#d3d3d3',
                        },
                        grid: {
                            display: false,
                            tickColor: '#d3d3d3',
                        },
                        ticks: {
                            color: '#d3d3d3',
                        }
                    },
                },
                plugins: {
                    emptypiechart: false,
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';

                                if (label) {
                                    label += ': ';
                                }
                                if (context.parsed.y !== null) {
                                    label += numberFormat.format(context.parsed.y);
                                }
                                return label;
                            }
                        }
                    }
                }
            }
        });

        new Chart(this.#necessityBarChart, {
            type: 'bar',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
                datasets: [{
                    label: 'Necessary',
                    stack: 'Unevaluated',
                    data: this.#data.necessaryPerMonth,
                    backgroundColor: '#20c997',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',

                },
                {
                    label: 'Unnecessary',
                    stack: 'Unevaluated',
                    data: this.#data.unnecessaryPerMonth,
                    backgroundColor: 'rgb(220,53,69)',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',

                },
                {
                    label: 'Necessary (Eval.)',
                    stack: 'Evaluated',
                    data: this.#data.necessaryEvaluatedPerMonth,
                    backgroundColor: '#0f7c5c',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',

                },
                {
                    label: 'Unnecessary (Eval.)',
                    stack: 'Evaluated',
                    data: this.#data.unnecessaryEvaluatedPerMonth,
                    backgroundColor: '#881d27',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                },
                {
                    label: 'Unevaluated',
                    stack: 'Evaluated',
                    data: this.#data.unevaluatedPerMonth,
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#1c1c1c'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        border: {
                            color: '#d3d3d3',
                        },
                        grid: {
                            color: '#d3d3d3',
                            lineWidth: 0.2,
                        },
                        ticks: {
                            color: '#d3d3d3',
                            callback: function (value, index, ticks) {
                                return numberFormat.format(value);
                            }
                        }
                    },
                    x: {
                        border: {
                            color: '#d3d3d3',
                        },
                        grid: {
                            display: false,
                            tickColor: '#d3d3d3',
                        },
                        ticks: {
                            color: '#d3d3d3',
                        }
                    },
                },
                plugins: {
                    emptypiechart: false,
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';

                                if (label) {
                                    label += ': ';
                                }
                                if (context.parsed.y !== null) {
                                    label += numberFormat.format(context.parsed.y);
                                }
                                return label;
                            }
                        }
                    }
                }
            }
        });

        var datasets = [];

        for (var i = 0; i < this.#data.monthlyOverspendingPerCategory.length; i++) {
            var categoryData = this.#data.monthlyOverspendingPerCategory[i];
            datasets.push({
                label: categoryData.category,
                data: categoryData.overspendingPerMonth,
                borderWidth: 2,
                borderColor: '#d3d3d3',
                backgroundColor: getRandomColor()
            });
        }

        new Chart(this.#overspendingChart, {
            type: 'bar',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
                datasets: datasets
            },
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: 'Monthly Overspending Per Category'
                    }
                },
                responsive: true,
                maintainAspectRatio: false,
                indexAxis: 'y',
                scales: {
                    x: {
                        stacked: true,
                        border: {
                            color: '#d3d3d3',
                        },
                        grid: {
                            color: '#d3d3d3',
                            lineWidth: 0.2,
                        },
                        ticks: {
                            color: '#d3d3d3',
                            callback: function (value, index, ticks) {
                                return numberFormat.format(value);
                            }
                        }
                    },
                    y: {
                        stacked: true,
                        border: {
                            color: '#d3d3d3',
                        },
                        grid: {
                            display: false,
                            tickColor: '#d3d3d3',

                        },
                        ticks: {
                            color: '#d3d3d3',
                        }
                    }
                },
                plugins: {
                    emptypiechart: false,
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                let label = context.dataset.label || '';

                                if (label) {
                                    label += ': ';
                                }
                                if (context.parsed.y !== null) {
                                    label += numberFormat.format(context.parsed.x);
                                }
                                return label;
                            }
                        }
                    }
                }
            }
        });

        this.#overspendingHeading.textContent = `Overspending: ${numberFormat.format(this.#data.overspendingTotal)}`;

        new Chart(this.#totalSpentChart, {
            type: 'line',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
                datasets: [{
                    label: 'Total Spent Per Month',
                    data: this.#data.totalPerMonth,
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#20c997'

                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    y: {
                        border: {
                            color: '#d3d3d3',
                        },
                        grid: {
                            color: '#d3d3d3',
                            lineWidth: 0.2,
                        },
                        ticks: {
                            color: '#d3d3d3',
                            callback: function (value, index, ticks) {
                                return numberFormat.format(value);
                            }
                        }
                    },
                    x: {
                        border: {
                            color: '#d3d3d3',
                        },
                        grid: {
                            display: false,
                            tickColor: '#d3d3d3',
                        },
                        ticks: {
                            color: '#d3d3d3',
                        }
                    },
                },
                plugins: {
                    emptypiechart: false,
                }
            }
        });
    }
}

var locale = new Intl.Locale("en-at");
var numberFormat = new Intl.NumberFormat("en-at", { style: 'currency', currency: "EUR" });

const categoriesAPI = "https://localhost:7246/api/Categories";
const transactionsAPI = "https://localhost:7246/api/Transactions";
const groupsAPI = "https://localhost:7246/api/Groups";

const menu = document.getElementById('menu-container');
const sidebar = document.getElementById("sidebar");

const sentimentChart = document.getElementById('sentimentChart');
const necessityChart = document.getElementById('necessityChart');

const updateCategoryModal = $("#updateCategory-modal");
const categoryModalLabel = updateCategoryModal.find("#updateCategory-label");
const categoryModalId = updateCategoryModal.find("#updateCategory_id");
const categoryModalName = updateCategoryModal.find("#updateCategory_name");
const categoryModalBudget = updateCategoryModal.find("#updateCategory_budget");
const categoryModalGroupId = updateCategoryModal.find("#updateCategory_groupId");
const addCategoryModal = $("#add-category-modal");
const addTransactionModal = $("#add-transaction-modal");

const flipContainer = document.getElementById("flip-container-inner");

const reevaluationContainer = document.getElementById("reevalCategories-container");
const reevaluatioInfo = document.getElementById("reevalInfo");

Chart.defaults.color = '#ffffff';
Chart.defaults.scales.linear.min = 0;
Chart.defaults.plugins.legend.labels.filter = (item) => item.text !== undefined;
Chart.defaults.plugins.tooltip.filter = (item) => item.label !== "";    

Chart.register({
    id: "emptypiechart",
    beforeInit: function (chart) {        
        chart.data.datasets[0].backgroundColor.push('#d2dee2');
        chart.data.datasets[0].data.push(Number.MIN_VALUE);               
    }
});

var currentSideIndex = 0;
var currentDeg = 0;

createReevaluationElements();
initializeStatisticsDashboard();

const transactionsTable = $('#transactions-table').DataTable({
    info: false,
    dom: '<"pb-1" t<"d-flex justify-content-between mt-3"<"pt-1"l>p>>',
    columns: [
        { data: 'title' }, { data: 'dateTime' }, { data: 'amount' }, { data: 'category' }, {
            data: null,
            defaultContent:
                `<svg  width='20' height='20' fill='rgba(255, 255, 255, 1)' class='me-1' viewBox='0 0 16 16'>
                    <path d='M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z'/>
                    <path fill-rule="evenodd" d='M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z'>
                </svg >
                <svg width="20" height="20" fill="rgba(255, 255, 255, 1)" viewBox="0 0 16 16">
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
            return numberFormat.format(data);
        }
    }, {
        targets: 1,
        render: function (data, type, row) {
            return new Date(data).toLocaleString(locale);
        }
    }],
    scrollX: true,
    scrollCollapse: true
});

$("#country").countrySelect({
    preferredCountries: ["at", "us"]
});

new Chart(sentimentChart, {
    type: 'doughnut',
    data: {
        labels: [
            'Happy',
            'Unhappy'
        ],
        datasets: [{
            label: 'Total Amount',
            data: [parseFloat(sentimentChart.dataset.happy), parseFloat(sentimentChart.dataset.unhappy)],
            backgroundColor: [
                'rgb(25,135,84)',
                'rgb(220,53,69)'
            ],
            hoverOffset: 4
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            tooltip: {
                callbacks: {
                    label: function (context) {
                        let label = context.dataset.label || '';

                        if (label) {
                            label += ': ';
                        }
                        if (context.parsed.y !== null) {
                            label += numberFormat.format(context.parsed);
                        }
                        return label;
                    }
                }
            }
        }
    }
});

new Chart(necessityChart, {
    type: 'doughnut',
    data: {
        labels: [
            'Necessary',
            'Unnecessary'
        ],
        datasets: [{
            label: 'Total Amount',
            data: [parseFloat(necessityChart.dataset.necessary), parseFloat(necessityChart.dataset.unnecessary)],
            backgroundColor: [
                'rgb(25,135,84)',
                'rgb(220,53,69)'
            ],
            hoverOffset: 4
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            tooltip: {
                callbacks: {
                    label: function (context) {
                        let label = context.dataset.label || '';

                        if (label) {
                            label += ': ';
                        }
                        if (context.parsed.y !== null) {
                            label += numberFormat.format(context.parsed);
                        }
                        return label;
                    }
                }
            }
        }
    }
});

/*
document.getElementById("sidebar-caret").addEventListener("click", () => {
    sidebar.classList.toggle('collapsed');
});
*/

transactionsTable.on('click', 'svg', function () {
    var row = transactionsTable.row($(this).parents('tr'));
    var data = row.data();
    console.log(data);
});

$(".accordion-head").on("click", function (event) {
    if (event.target.matches("svg.add-icon")) {
        var id = $(this).closest('.accordion').data("id");
        addCategoryModal.modal('show');
        addCategoryModal.find("#GroupId").val(id);
    }
    else {
        $(this).next().collapse('toggle');
        var caret = $('.accordion-caret', this)[0];
        caret.classList.toggle("rotate");
    }
});

$('#add-category-form').on("submit", async function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        addCategoryModal.modal('hide');
        await addCategory(new FormData(this));
    }
});

$('#add-transaction-form').on("submit", async function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        addTransactionModal.modal('hide');
        await addTransaction(new FormData(this));
    }
});

$('#update-category-form').on("submit", async function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        updateCategoryModal.modal('hide');
        await updateCategory(new FormData(this));
    }
});

$('#search-form').on("submit", async function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        await populateTable(new FormData(this));
    }
});

$('.category').on("click", function (event) {
    if (menu.dataset.category != 0) {
        var borderBox = document.getElementById(`category_${menu.dataset.category}`).querySelector('.border-animation');
        borderBox.classList.remove('border-rotate');
    }

    menu.dataset.category = this.dataset.id;
    menu.style.left = `${this.style.left + event.pageX - 100}px`;
    menu.style.top = `${event.pageY - 100}px`;
    menu.classList.add('active');

    this.querySelector('.border-animation').classList.add('border-rotate');
});

document.getElementById('close-menu').onclick = function () {
    menu.classList.remove('active');
    var id = menu.dataset.category;
    var borderBox = document.getElementById(`category_${id}`).querySelector('.border-animation');
    borderBox.classList.remove('border-rotate');
    menu.dataset.category = 0;
    menu.dataset.groupId = 0;
};

document.getElementById('delete-menu').onclick = function () {
    var token = menu.querySelector('input').value;
    var id = menu.dataset.category;
    if (deleteCategory(id, token)) {
        menu.classList.remove('active');
        menu.dataset.category = 0;
        menu.dataset.groupId = 0;
    }
};

document.getElementById('add-menu').onclick = function () {
    var id = menu.dataset.category;
    addTransactionModal.find("#CategoryId").val(id);
    addTransactionModal.modal('show');
};

document.getElementById('edit-menu').onclick = function () {
    var category = document.getElementById(`category_${menu.dataset.category}`);

    categoryModalLabel.text(`Edit ${category.dataset.name}`);
    categoryModalId.val(category.dataset.id);
    categoryModalName.val(category.dataset.name);
    categoryModalBudget.val(category.dataset.budget);
    categoryModalGroupId.val(category.dataset.groupid);

    updateCategoryModal.modal('show');
};

document.getElementById('details-menu').onclick = function () {
    var id = menu.dataset.category;
    window.location.href = "Category/" + id;
};

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

async function reevaluateTransaction(data, transactionElement, accordionBody, accordion) {
    try {
        var id = parseInt(data.get("PageModel.Transaction.Id"));
        console.log("Removed Transaction with id " + id);
        var transaction = document.getElementById(`reeval_transaction_${id}`);

        var patchDoc =
            [{
                op: "replace",
                path: "/IsHappy",
                value: data.get("PageModel.Transaction.IsHappy") === "true" ? true : false
            },
            {
                op: "replace",
                path: "/IsNecessary",
                value: data.get("PageModel.Transaction.IsNecessary") === "true" ? true : false
            }, {
                op: "replace",
                path: "/PreviousIsHappy",
                value: transaction.dataset.isHappy === "true" ? true : false
            },
            {
                op: "replace",
                path: "/PreviousIsNecessary",
                value: transaction.dataset.isNecessary === "true" ? true : false
            },
            {
                op: "replace",
                path: "/Evaluated",
                value: true
            }];

        var response = await fetch(`${transactionsAPI}/${id}`, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json-patch+json"
            },
            body: JSON.stringify(patchDoc)
        });

        if (response.ok) {
            transactionElement.remove();
            if (accordionBody.childElementCount == 0) {
                accordion.remove();
            }
            showReevaluationInfo();
        } else {
            console.error(`HTTP Patch Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
    };
}

async function getFilteredCategories() {
    try {
        var response = await fetch(`${categoriesAPI}/filteredByEvaluation`, {
            method: "GET"
        });

        if (response.ok) {
            var categories = await response.json();
            return categories;
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
    };
}

async function addCategory(data) {
    try {
        var response = await fetch(`${categoriesAPI}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: data.get("Name"),
                Budget: parseFloat(data.get("Budget")),
                GroupId: parseInt(data.get("GroupId"))
            })
        });

        if (response.ok) {
            document.querySelector(`#group_${data.get("GroupId")} .accordion-body`).appendChild(createCategoryElement(await response.json()));
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
        var response = await fetch(`${categoriesAPI}/${id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": data.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: data.get("Name"),
                Budget: parseFloat(data.get("Budget")),
                GroupId: parseInt(data.get("GroupId")),
                Id: id
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

async function deleteCategory(id, token) {
    try {
        var response = await fetch(`${categoriesAPI}/${id}`, {
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

async function getStatistics() {
    try {
        var response = await fetch(`${groupsAPI}/2?year=2024`, { /////// FIX GROUPD ID HERE
            method: "GET"
        });

        if (response.ok) {
            return await response.json();
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
        }
    } catch (error) {
        console.error(error);
    }
}

async function populateTable(data) {
    try {
        let params = new URLSearchParams();

        for (let [key, value] of data.entries()) {
            if (value !== undefined && value !== '') {
                params.append(key, value);
            }
        }

        let query_string = params.toString();
        console.log(`${transactionsAPI}?${query_string}`);

        var response = await fetch(`${transactionsAPI}?${query_string}`, {
            method: "GET",
        });

        if (response.ok) {
            var data = await response.json();
            transactionsTable.clear();
            transactionsTable.rows.add(data);
            transactionsTable.draw();
        } else {
            transactionsTable.clear();
            console.error(`HTTP GET Error: ${response.status}`);
        }

    } catch (error) {
        console.error(error);
    }
}

function createCategoryElement(category) {
    let div = document.createElement('div');
    div.className = 'category border p-2';

    let innerDiv = document.createElement('div');
    innerDiv.className = 'd-flex';

    let nameDiv = document.createElement('div');
    nameDiv.textContent = decodeURIComponent(category.name);

    let balanceDiv = document.createElement('div');
    balanceDiv.className = 'ms-auto';
    balanceDiv.textContent = 'Balance: 700 / 700';

    innerDiv.appendChild(nameDiv);
    innerDiv.appendChild(balanceDiv);

    let progressDiv = document.createElement('div');
    progressDiv.className = 'progress';

    let progressBarDiv = document.createElement('div');
    progressBarDiv.className = 'progress-bar progress-bar-striped progress-bar-animated bg-success';
    progressBarDiv.role = 'progressbar';
    progressBarDiv.style.width = '100%';
    progressBarDiv.setAttribute('aria-valuenow', '100');
    progressBarDiv.setAttribute('aria-valuemin', '0');
    progressBarDiv.setAttribute('aria-valuemax', '100');

    progressDiv.appendChild(progressBarDiv);

    div.appendChild(innerDiv);
    div.appendChild(progressDiv);

    return div;
}

function createEvaluationElement(category) {
    var accordion = document.createElement("div");
    accordion.classList.add("accordion");

    var accordionItem = document.createElement("div");
    accordionItem.classList.add("accordion-item");

    var accordionHeader = document.createElement("h5");
    accordionHeader.id = `accordionHeader_${category.id}`;
    accordionHeader.textContent = `${decodeURIComponent(category.name)}`;
    accordionHeader.classList.add("accordion-header", "me-auto")

    var accordionCaret = document.createElement("div");
    accordionCaret.classList.add("accordion-caret", "rotate");

    var accordionHead = document.createElement("div");
    accordionHead.id = `accordion-head_${category.id}`;
    accordionHead.classList.add("accordion-head", "d-flex");
    accordionHead.addEventListener("click", () => {
        $(accordionHead).next().collapse('toggle');
        accordionCaret.classList.toggle("rotate");
    });

    var accordionCollapse = document.createElement("div");
    accordionCollapse.classList.add("accordion-collapse", "collapse", "show");
    accordionCollapse.setAttribute("aria-labelledby", `accordionHeader_${category.id}`)

    var accordionBody = document.createElement("div");
    accordionBody.classList.add("accordion-body");

    for (var i = 0; i < category.transactions.length; i++) {
        let transaction = category.transactions[i];

        let transactionBody = document.createElement('div');

        transactionBody.id = `reeval_transaction_${transaction.id}`;
        transactionBody.className = 'transaction-body';
        transactionBody.setAttribute('data-id', transaction.id);
        transactionBody.setAttribute('data-ishappy', transaction.isHappy);
        transactionBody.setAttribute('data-isnecessary', transaction.isNecessary);

        var titleDiv = document.createElement('div');
        titleDiv.textContent = decodeURIComponent(transaction.title);

        var dateDiv = document.createElement('div');
        dateDiv.className = 'ms-2';
        dateDiv.textContent = new Date(transaction.dateTime).toLocaleDateString(locale);

        var amountDiv = document.createElement('div');
        amountDiv.className = 'ms-2 me-auto';
        amountDiv.textContent = numberFormat.format(transaction.amount);

        var transactionForm = document.createElement('form');
        transactionForm.id = `reevaluate-transaction-form_${transaction.id}`;
        transactionForm.className = 'reevaluate-transaction-form';
        transactionForm.setAttribute('novalidate', 'novalidate');
        transactionForm.addEventListener("submit", async function (event) {
            event.preventDefault();
            if ($(transactionForm).valid()) {
                let element = transactionBody;
                await reevaluateTransaction(new FormData(this), element, accordionBody, accordion);
            }
        });

        transactionForm.innerHTML =
            `<input type="hidden" value="${transaction.id}" data-val="true" data-val-required="The Id field is required." id="reeval_transaction_${transaction.id}" name="PageModel.Transaction.Id">
                            <div class="d-flex justify-content-around">
                                <div class="me-3 d-flex align-items-center">
                                    <input type="radio" value="true" id="isHappyTrue_${transaction.id}" class="iconRadioButton" data-val="true" data-val-required="The IsHappy field is required." name="PageModel.Transaction.IsHappy">
                                    <label class="reevalIconLabel" for="isHappyTrue_${transaction.id}">
                                        <div class="reevalIconContainer">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="50px" height="50px" viewBox="0 0 2050 2050" class="reevalIcon">
                                                <g data-name="Layer 3" id="Layer_3">
                                                    <circle fill="#ffc500" cx="1025" cy="1025" r="613.7"></circle>
                                                    <path fill="#ff9f1e" d="M1068.5,1551.8c-339,0-613.7-274.8-613.7-613.7a613.5,613.5,0,0,1,44.4-229.8A610.7,610.7,0,0,0,411.3,1025c0,338.9,274.8,613.7,613.7,613.7,257.7,0,478.3-158.8,569.2-383.9C1486.8,1432.8,1291.5,1551.8,1068.5,1551.8Z"></path>
                                                    <path fill="#ffe059" d="M1025,411.3A610.4,610.4,0,0,0,659.9,531.7a706.9,706.9,0,0,1,453.2-18.9,27.8,27.8,0,0,0-3.3,5.1c-18.2,33.9,18.9,89.1,82.7,123.3s130.4,34.6,148.6.8a38.6,38.6,0,0,0,3.7-10.5c165.2,128,271.9,328.1,272.9,553.1a613.3,613.3,0,0,0,21-159.6C1638.7,686.1,1363.9,411.3,1025,411.3Z"></path>
                                                    <ellipse fill="#ffe059" cx="1339.4" cy="698.8" rx="30.8" ry="52.8" transform="translate(396.9 1879.5) rotate(-79)"></ellipse>
                                                    <ellipse fill="#ffe059" cx="1235.1" cy="718.7" rx="17.2" ry="25" transform="translate(-4.9 1429) rotate(-60)"></ellipse>
                                                    <path fill="#00305f" d="M1146.1,883.2a30.6,30.6,0,0,1-13.7-3.2,30,30,0,0,1-13-40.4c26.2-51.3,63.4-79.5,104.7-79.5s78.5,28.2,104.7,79.5a30,30,0,1,1-53.4,27.3c-15-29.3-34.2-46.8-51.3-46.8s-36.3,17.5-51.3,46.8A29.9,29.9,0,0,1,1146.1,883.2Z"></path>
                                                    <path fill="#00305f" d="M903.9,883.2a29.9,29.9,0,0,1-26.7-16.3c-15-29.3-34.2-46.8-51.3-46.8s-36.3,17.5-51.3,46.8a30,30,0,1,1-53.4-27.3c26.2-51.3,63.4-79.5,104.7-79.5s78.5,28.2,104.7,79.5a30,30,0,0,1-13,40.4A30.6,30.6,0,0,1,903.9,883.2Z"></path>
                                                    <path fill="#00305f" d="M863.1,1280.8a29.4,29.4,0,0,1-15.8-4.5,29.9,29.9,0,0,1-9.6-41.3,89.9,89.9,0,0,0,13.4-47.4,88.9,88.9,0,0,0-16.9-52.5,30,30,0,0,1,48.6-35.2,150.2,150.2,0,0,1,5.8,166.8A30.1,30.1,0,0,1,863.1,1280.8Z"></path>
                                                    <path fill="#00305f" d="M1186.9,1280.8a30.1,30.1,0,0,1-25.5-14.1,150.2,150.2,0,0,1,5.8-166.8,30,30,0,1,1,48.6,35.2,88.9,88.9,0,0,0-16.9,52.5,89.9,89.9,0,0,0,13.4,47.4,29.9,29.9,0,0,1-9.6,41.3A29.4,29.4,0,0,1,1186.9,1280.8Z"></path>
                                                    <path fill="#00305f" d="M1025,1247.3a293.2,293.2,0,0,1-135.5-33.1,30,30,0,1,1,27.6-53.2,234.3,234.3,0,0,0,215.8,0c46.2-19.3,34.3,19.6,27.6,53.2A293.2,293.2,0,0,1,1025,1247.3Z"></path>
                                                </g>
                                            </svg>
                                        </div>
                                    </label>
                                    <input type="radio" value="false" id="isHappyFalse_${transaction.id}" class="iconRadioButton" checked="" name="PageModel.Transaction.IsHappy">
                                    <label class="reevalIconLabel" for="isHappyFalse_${transaction.id}">
                                        <div class="reevalIconContainer">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="50px" height="50px" viewBox="0 0 2050 2050" class="reevalIcon">
                                                <g data-name="Layer 3" id="Layer_3">
                                                    <circle fill="#ffc500" cx="1025" cy="1025" r="613.7"></circle>
                                                    <path fill="#ff9f1e" d="M1068.5,1551.8c-339,0-613.7-274.8-613.7-613.7a613.5,613.5,0,0,1,44.4-229.8A610.7,610.7,0,0,0,411.3,1025c0,338.9,274.8,613.7,613.7,613.7,257.7,0,478.3-158.8,569.2-383.9C1486.8,1432.8,1291.5,1551.8,1068.5,1551.8Z"></path>
                                                    <ellipse fill="#00305f" cx="773.9" cy="896.3" rx="94.9" ry="60.2" transform="translate(-155.1 1630.7) rotate(-87.5)"></ellipse>
                                                    <circle fill="#00305f" cx="803.6" cy="857.7" r="16.6"></circle>
                                                    <ellipse fill="#00305f" cx="1276.1" cy="896.3" rx="60.2" ry="94.9" transform="translate(-37.7 56.1) rotate(-2.5)"></ellipse>
                                                    <circle fill="#00305f" cx="1296.4" cy="857.7" r="16.6"></circle>
                                                    <path fill="#ffe059" d="M1025,411.3A610.4,610.4,0,0,0,659.9,531.7a706.9,706.9,0,0,1,453.2-18.9,27.8,27.8,0,0,0-3.3,5.1c-18.2,33.9,18.9,89.1,82.7,123.3s130.4,34.6,148.6.8a38.6,38.6,0,0,0,3.7-10.5c165.2,128,271.9,328.1,272.9,553.1a613.3,613.3,0,0,0,21-159.6C1638.7,686.1,1363.9,411.3,1025,411.3Z"></path>
                                                    <ellipse fill="#ffe059" cx="1339.4" cy="698.8" rx="30.8" ry="52.8" transform="translate(396.9 1879.5) rotate(-79)"></ellipse>
                                                    <ellipse fill="#ffe059" cx="1235.1" cy="718.7" rx="17.2" ry="25" transform="translate(-4.9 1429) rotate(-60)"></ellipse>
                                                    <path fill="#00305f" d="M1296.4,1314.5a29.7,29.7,0,0,1-23-10.7c-32.3-38.4-70-68.5-112-89.5s-88.8-32.4-136-32.4c-91.6,0-178.2,41.6-243.9,117.1a30,30,0,0,1-45.3-39.4c37.5-43,81-76.8,129.5-100.5a361.4,361.4,0,0,1,322.6,1.6c49.3,24.7,93.4,59.8,131,104.5a29.9,29.9,0,0,1-22.9,49.3Z"></path>
                                                </g>
                                            </svg>
                                        </div>
                                    </label>
                                </div>
                                <div class="me-3  ms-1 d-flex align-items-center">
                                    <input type="radio" value="true" id="isNecessaryTrue_${transaction.id}" class="iconRadioButton" data-val="true" data-val-required="The IsNecessary field is required." name="PageModel.Transaction.IsNecessary">
                                    <label class="reevalIconLabel" for="isNecessaryTrue_${transaction.id}">
                                        <div class="reevalIconContainer">
                                            <svg xmlns="http://www.w3.org/2000/svg" height="30px" width="30px" viewBox="0 0 512 512" class="reevalIconNecessity">
                                                <path style="fill:#FFC61B;" d="M452.656,499.209c-7.881,0-14.269-6.39-14.269-14.269V234.027l-32.225,30.763
	                                                c-5.701,5.442-14.732,5.233-20.176-0.468c-5.441-5.701-5.233-14.733,0.468-20.175l56.347-53.791
	                                                c4.131-3.944,10.215-5.046,15.467-2.797c5.251,2.246,8.656,7.407,8.656,13.118V484.94
	                                                C466.925,492.819,460.537,499.209,452.656,499.209z"></path>
                                                <g>
                                                    <path style="fill:#FEE187;" d="M338.188,309.953c-2.499,2.386-5.849,3.683-9.302,3.584c-3.455-0.093-6.729-1.56-9.097-4.075
		                        l-51.775-54.994l-21.981,22.543V484.94h103.312V299.302L338.188,309.953z"></path>
                                                    <polygon style="fill:#FEE187;" points="121.207,318.315 37.615,404.139 37.615,484.94 140.927,484.94 140.927,340.529"></polygon>
                                                </g>
                                                <path style="fill:#FFC61B;" d="M396.309,470.671h-32.695V299.302c0-0.645-0.059-1.276-0.141-1.899
	                        c-0.02-0.148-0.043-0.295-0.067-0.444c-0.098-0.595-0.224-1.179-0.394-1.747c-0.007-0.023-0.011-0.047-0.019-0.07
	                        c-0.184-0.606-0.417-1.19-0.676-1.759c-0.051-0.114-0.107-0.225-0.163-0.338c-0.251-0.512-0.525-1.01-0.833-1.485
	                        c-0.033-0.05-0.06-0.103-0.093-0.153c-0.337-0.507-0.711-0.983-1.107-1.441c-0.098-0.113-0.2-0.221-0.301-0.332
	                        c-0.367-0.397-0.751-0.775-1.16-1.127c-0.061-0.053-0.118-0.111-0.181-0.164c-0.471-0.392-0.972-0.746-1.49-1.077
	                        c-0.116-0.073-0.233-0.143-0.35-0.213c-0.541-0.323-1.097-0.622-1.681-0.872l0,0l0,0c-0.588-0.253-1.201-0.454-1.828-0.625
	                        c-0.127-0.036-0.255-0.067-0.384-0.098c-0.559-0.137-1.13-0.248-1.714-0.317c-0.054-0.006-0.108-0.019-0.163-0.024
	                        c-0.502-0.053-1.01-0.084-1.525-0.084c-0.104,0-0.205,0.013-0.31,0.016c-0.161,0.003-0.321,0.016-0.481,0.024
	                        c-0.511,0.029-1.015,0.081-1.51,0.163c-0.116,0.019-0.23,0.031-0.345,0.053c-0.616,0.116-1.219,0.27-1.804,0.462
	                        c-0.086,0.029-0.168,0.063-0.253,0.093c-0.522,0.183-1.029,0.395-1.521,0.635c-0.107,0.051-0.215,0.1-0.322,0.156
	                        c-0.548,0.284-1.076,0.598-1.58,0.949c-0.093,0.064-0.181,0.137-0.274,0.204c-0.44,0.321-0.86,0.668-1.26,1.036
	                        c-0.061,0.057-0.133,0.101-0.194,0.16l-10.208,9.747l-50.879-54.041c-2.667-2.832-6.374-4.453-10.264-4.489
	                        c-3.843-0.016-7.626,1.523-10.341,4.308l-21.981,22.543c-0.305,0.314-0.592,0.642-0.866,0.98c-0.334,0.287-0.661,0.585-0.972,0.906
	                        l-54.225,56.092c-5.478,5.666-5.325,14.699,0.341,20.177c5.666,5.478,14.702,5.325,20.177-0.341l29.697-30.718v156.528h-74.773
	                        v-130.14c0-0.461-0.026-0.92-0.07-1.374c-0.019-0.194-0.053-0.382-0.078-0.574c-0.036-0.254-0.067-0.508-0.116-0.759
	                        c-0.044-0.231-0.103-0.458-0.158-0.685c-0.05-0.205-0.098-0.41-0.157-0.612c-0.07-0.238-0.15-0.471-0.231-0.703
	                        c-0.066-0.187-0.131-0.374-0.204-0.558c-0.091-0.228-0.19-0.452-0.293-0.675c-0.084-0.186-0.171-0.368-0.264-0.549
	                        c-0.107-0.21-0.22-0.415-0.337-0.619c-0.108-0.188-0.221-0.372-0.337-0.555c-0.12-0.187-0.241-0.372-0.37-0.554
	                        c-0.133-0.188-0.271-0.371-0.414-0.554c-0.13-0.167-0.261-0.332-0.398-0.492c-0.059-0.068-0.11-0.141-0.168-0.208l-19.719-22.215
	                        c-2.631-2.964-6.378-4.7-10.341-4.792c-3.974-0.113-7.787,1.47-10.552,4.309L27.4,394.188c-2.596,2.664-4.047,6.237-4.047,9.957
	                        v80.8c0,7.88,6.388,14.269,14.269,14.269h103.31h103.312c0.303,0,0.599-0.027,0.898-0.046c0.298,0.019,0.595,0.046,0.898,0.046
	                        h103.312h46.965c7.881,0,14.269-6.39,14.269-14.269C410.585,477.066,404.19,470.671,396.309,470.671z M51.884,409.94l68.837-70.676
	                        l5.935,6.687v124.72H51.884V409.94z M260.302,282.815l7.531-7.723l41.567,44.151c4.94,5.247,11.904,8.365,19.078,8.556
	                        c2.24,0.057,4.455-0.148,6.597-0.615v143.484h-74.773V282.815z"></path>
                                                <polygon style="fill:#FEE187;" points="407.582,51.21 497.729,27.055 473.575,117.203 "></polygon>
                                                <path style="fill:#FFC61B;" d="M507.82,16.965c-3.606-3.603-8.861-5.014-13.784-3.693l-90.147,24.155
	                        c-4.924,1.32-8.77,5.166-10.09,10.09c-1.32,4.923,0.089,10.178,3.693,13.783l23.831,23.831l-91.555,87.464l-51.201-54.382
	                        c-2.667-2.832-6.374-4.453-10.264-4.489c-3.878-0.006-7.626,1.523-10.341,4.308l-95.528,97.966l-30.114-33.924
	                        c-2.631-2.964-6.378-4.7-10.341-4.792c-3.988-0.098-7.787,1.468-10.552,4.309L4.048,291.838c-5.499,5.645-5.38,14.679,0.265,20.178
	                        c2.774,2.704,6.366,4.048,9.956,4.048c3.716,0,7.427-1.443,10.224-4.312l96.672-99.254l30.11,33.92
	                        c2.631,2.964,6.376,4.7,10.337,4.792c3.958,0.108,7.784-1.465,10.551-4.304L268,148.622l50.846,54.005
	                        c2.607,2.768,6.21,4.382,10.011,4.483c3.766,0.118,7.484-1.318,10.234-3.947l102.419-97.845l21.976,21.976
	                        c2.711,2.71,6.356,4.18,10.09,4.18c1.231,0,2.471-0.158,3.693-0.487c4.924-1.32,8.77-5.166,10.09-10.09l24.155-90.147
	                        C512.833,25.825,511.424,20.571,507.82,16.965z M466.189,89.636L435.15,58.598l42.4-11.361L466.189,89.636z"></path>
                                            </svg>
                                        </div>
                                    </label>
                                    <input type="radio" value="false" id="isNecessaryFalse_${transaction.id}" class="iconRadioButton" checked="" name="PageModel.Transaction.IsNecessary">
                                    <label class="reevalIconLabel" for="isNecessaryFalse_${transaction.id}">
                                        <div class="reevalIconContainer">
                                            <svg xmlns="http://www.w3.org/2000/svg" height="30px" width="30px" viewBox="0 0 512 512" class="reevalIconNecessity">
                                                <path style="fill:#FFC61B;" d="M14.183,471.406C6.35,471.406,0,465.055,0,457.223V174.677c0-5.678,3.386-10.806,8.603-13.039
	                        c5.219-2.235,11.267-1.14,15.373,2.78l56.006,53.466c5.666,5.408,5.875,14.387,0.465,20.054c-5.408,5.665-14.386,5.875-20.052,0.465
	                        l-32.03-30.577v249.398C28.366,465.055,22.016,471.406,14.183,471.406z"></path>
                                                <g>
                                                    <path style="fill:#FEE187;" d="M127.957,283.293c2.483,2.371,5.814,3.661,9.246,3.563c3.434-0.092,6.689-1.55,9.042-4.051
		                        l51.462-54.662l21.848,22.407v206.672H116.87V272.708L127.957,283.293z"></path>
                                                    <polygon style="fill:#FEE187;" points="343.628,291.605 426.715,376.91 426.715,457.223 324.028,457.223 324.028,313.686 	"></polygon>
                                                </g>
                                                <path style="fill:#FFC61B;" d="M436.875,367.013l-83.086-85.305c-2.749-2.821-6.527-4.402-10.49-4.283
	                        c-3.939,0.091-7.663,1.817-10.279,4.763l-19.6,22.08c-0.06,0.067-0.111,0.14-0.169,0.208c-0.136,0.159-0.265,0.322-0.394,0.488
	                        c-0.142,0.182-0.279,0.363-0.411,0.55c-0.128,0.182-0.251,0.367-0.37,0.555c-0.113,0.179-0.224,0.36-0.33,0.543
	                        c-0.119,0.206-0.233,0.414-0.34,0.625c-0.089,0.176-0.174,0.353-0.257,0.532c-0.104,0.225-0.204,0.452-0.296,0.685
	                        c-0.071,0.177-0.133,0.357-0.197,0.538c-0.084,0.237-0.165,0.474-0.235,0.716c-0.057,0.194-0.104,0.391-0.152,0.589
	                        c-0.057,0.231-0.116,0.461-0.162,0.698c-0.047,0.245-0.078,0.492-0.113,0.74c-0.027,0.194-0.061,0.386-0.081,0.582
	                        c-0.044,0.452-0.07,0.908-0.07,1.364v129.354h-74.32V287.457l29.518,30.532c5.445,5.632,14.423,5.784,20.055,0.339
	                        c5.632-5.445,5.784-14.423,0.339-20.055l-53.897-55.753c-0.309-0.321-0.635-0.617-0.969-0.903c-0.271-0.335-0.555-0.66-0.858-0.97
	                        l-21.848-22.407c-2.671-2.739-6.333-4.282-10.155-4.282c-0.041,0-0.084,0-0.123,0c-3.868,0.034-7.553,1.645-10.202,4.462
	                        l-50.571,53.716l-10.147-9.687c-0.061-0.058-0.13-0.102-0.191-0.157c-0.399-0.367-0.817-0.712-1.255-1.033
	                        c-0.089-0.065-0.176-0.136-0.267-0.2c-0.502-0.349-1.028-0.662-1.574-0.945c-0.104-0.054-0.21-0.101-0.315-0.152
	                        c-0.491-0.24-0.997-0.451-1.518-0.634c-0.084-0.03-0.165-0.064-0.25-0.091c-0.582-0.191-1.18-0.345-1.793-0.46
	                        c-0.115-0.023-0.231-0.034-0.347-0.054c-0.491-0.079-0.987-0.132-1.493-0.16c-0.162-0.009-0.322-0.021-0.485-0.024
	                        c-0.102-0.001-0.201-0.016-0.305-0.016c-0.512,0-1.018,0.03-1.516,0.084c-0.057,0.006-0.112,0.018-0.167,0.026
	                        c-0.58,0.069-1.146,0.179-1.702,0.315c-0.125,0.031-0.248,0.061-0.372,0.095c-1.245,0.342-2.42,0.848-3.503,1.496
	                        c-0.111,0.067-0.221,0.132-0.33,0.201c-0.519,0.33-1.021,0.685-1.492,1.078c-0.058,0.048-0.111,0.102-0.167,0.152
	                        c-0.411,0.355-0.797,0.735-1.164,1.132c-0.099,0.108-0.2,0.214-0.295,0.325c-0.396,0.455-0.767,0.93-1.102,1.435
	                        c-0.033,0.048-0.06,0.101-0.091,0.149c-0.306,0.474-0.58,0.969-0.83,1.479c-0.054,0.112-0.109,0.221-0.162,0.335
	                        c-0.258,0.566-0.489,1.146-0.672,1.749c-0.007,0.023-0.011,0.045-0.017,0.068c-0.169,0.563-0.294,1.146-0.391,1.736
	                        c-0.024,0.148-0.047,0.292-0.067,0.441c-0.082,0.618-0.14,1.245-0.14,1.888V443.04H70.19c-7.833,0-14.183,6.351-14.183,14.183
	                        s6.35,14.183,14.183,14.183h46.681h102.686c0.301,0,0.596-0.027,0.892-0.045c0.296,0.018,0.591,0.045,0.892,0.045h102.686h102.686
	                        c7.832,0,14.183-6.351,14.183-14.183V376.91C440.897,373.214,439.455,369.662,436.875,367.013z M205.373,443.04h-74.32V300.422
	                        c1.882,0.411,3.818,0.621,5.784,0.621c0.003,0,0.004,0,0.007,0c0.255,0,0.511-0.004,0.738-0.011
	                        c7.161-0.19,14.084-3.29,18.991-8.506l41.315-43.883l7.486,7.676V443.04H205.373z M412.531,443.04h-74.32V319.072l5.899-6.646
	                        l68.421,70.249V443.04z"></path>
                                                <polygon style="fill:#FEE187;" points="408.215,298.51 497.817,322.519 473.808,232.917 "></polygon>
                                                <path style="fill:#FFC61B;" d="M511.516,318.847l-24.009-89.602c-1.312-4.895-5.134-8.717-10.029-10.029
	                        c-4.896-1.315-10.117,0.089-13.699,3.671l-22.947,22.947l-87.482-90.125c-2.749-2.822-6.55-4.37-10.488-4.283
	                        c-3.939,0.091-7.662,1.817-10.279,4.763l-29.932,33.719l-94.95-97.374c-2.699-2.767-6.414-4.275-10.28-4.282
	                        c-3.866,0.034-7.551,1.645-10.202,4.462l-50.892,54.055L29.3,44.521c-5.663-5.409-14.641-5.205-20.052,0.457
	                        C3.837,50.642,4.041,59.62,9.706,65.031l117.357,112.118c2.733,2.613,6.404,4.048,10.172,3.923c3.778-0.099,7.36-1.703,9.951-4.456
	                        l50.539-53.68l95.26,97.691c2.749,2.82,6.543,4.344,10.487,4.278c3.937-0.091,7.659-1.817,10.274-4.763l29.928-33.715l77.1,79.467
	                        l-22.587,22.587c-3.583,3.583-4.983,8.806-3.671,13.699c1.312,4.895,5.134,8.717,10.029,10.029l89.602,24.009
	                        c1.213,0.325,2.447,0.484,3.669,0.484c3.713,0,7.336-1.459,10.03-4.154C511.43,328.964,512.828,323.742,511.516,318.847z
	                        M435.615,291.167l30.851-30.851l11.293,42.144L435.615,291.167z"></path>
                                            </svg>
                                        </div>
                                    </label>
                                </div>
                                <div>
                                    <button class="reeval-submit-button" type="submit" value="">
                                        <svg class="reeval-submit-svg" width="40px" height="40px" viewBox="0 0 24 24">
                                            <circle cx="12" cy="12" r="10" stroke-width="1.5"></circle>
                                            <path d="M8.5 12.5L10.5 14.5L15.5 9.5" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                        </svg>
                                    </button>
                                </div>
                            </div>
                            <span class="text-danger field-validation-valid" data-valmsg-for="PageModel.Transaction.IsHappy" data-valmsg-replace="true"></span>
                            <span class="text-danger field-validation-valid" data-valmsg-for="PageModel.Transaction.IsNecessary" data-valmsg-replace="true"></span>`

        transactionBody.appendChild(titleDiv);
        transactionBody.appendChild(dateDiv);
        transactionBody.appendChild(amountDiv);
        transactionBody.appendChild(transactionForm);

        accordionBody.appendChild(transactionBody);
    }

    accordionHead.appendChild(accordionHeader);
    accordionHead.appendChild(accordionCaret);
    accordionCollapse.appendChild(accordionBody);
    accordionItem.appendChild(accordionHead);
    accordionItem.appendChild(accordionCollapse);
    accordion.appendChild(accordionItem);

    return accordion;
}

async function createReevaluationElements() {
    var categories = await getFilteredCategories();
    var frag = document.createDocumentFragment();

    for (var i = 0; i < categories.length; i++) {
        if (categories[i].transactions.length > 0) {
            frag.appendChild(createEvaluationElement(categories[i]));
        }
    }

    reevaluationContainer.innerHTML = "";
    reevaluationContainer.appendChild(frag);

    showReevaluationInfo();
}

async function initializeStatisticsDashboard() {

    var data = await getStatistics()
    var statistics = new Statistics(data);
    statistics.render();
}

function showReevaluationInfo() {
    if (reevaluationContainer.childElementCount == 0) {
        reevaluatioInfo.style.display = 'block';
    } else {
        reevaluatioInfo.style.display = 'none';
    }
}

function shortestAngle(index1, index2) {
    var diff = (index2 - index1 + 4) % 4;

    if (diff === 1) {
        return -90;
    } else if (diff === 2) {
        return -180;
    } else if (diff === 3) {
        return 90;
    } else {
        return 0;
    }
}

function resetStyle(element, style) {
    element.style = style + '; transition: transform 0s';
}

function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

