import { ArcElement, Chart, DoughnutController } from 'chart.js';
Chart.register(DoughnutController, ArcElement);
import { getDatePicker } from './asyncComponents'
import { getCategoryDataByMonth } from './api';
import { API_ROUTES } from './config'

export default class CategoryDashboard {
    #data;
    #isLoading;
    initPromise;
    #monthPicker;
    #sentimentChart;
    #necessityChart;    
    table;
    #budgetHeader;
    #differenceHeader;
    #totalHeader;

    constructor(id, date, data) {
        this.#data = data;
        this.initPromise = this.#init(id, date);
    }

    async #init(id, date) {
        try {
            this.#isLoading = true;
            var token = document.getElementById('antiforgeryToken').value;
            var datepickerPromise = this.#initializeDatePicker(id, date);
            var tablePromise = this.#initializeTable(token);
            this.#initializeCharts();
            this.#budgetHeader = document.getElementById('budget-header');
            this.#totalHeader = document.getElementById('total-header');
            this.#differenceHeader = document.getElementById('difference-header');

            await tablePromise;
            await datepickerPromise;
            this.table.ajax.reload(null, false);
        } finally {
            this.#isLoading = false;
        }
    }

    #initializeCharts() {
        var options = {
            responsive: true,
            layout: {
                padding: 2
            },
            maintainAspectRatio: false,
            plugins: {
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            var label = context.dataset.label || '';

                            if (label) {
                                label += ': ';
                            }
                            if (context.parsed.y !== null) {
                                label += window.userNumberFormat.format(context.parsed);
                            }
                            return label;
                        }
                    }
                }
            }
        };
        var sentimentChart = document.getElementById('sentimentChart');
        this.#sentimentChart = new Chart(sentimentChart, {
            type: 'doughnut',
            data: {
                labels: [
                    'Happy',
                    'Unhappy'
                ],
                datasets: [{
                    label: 'Total Amount',
                    data: [sentimentChart.dataset.happy, sentimentChart.dataset.unhappy],
                    backgroundColor: [
                        'rgb(25,135,84)',
                        'rgb(220,53,69)'
                    ],
                    hoverOffset: 4
                }]
            },
            options: options
        });

        var necessityChart = document.getElementById('necessityChart');
        this.#necessityChart = new Chart(necessityChart, {
            type: 'doughnut',
            data: {
                labels: [
                    'Necessary',
                    'Unnecessary'
                ],
                datasets: [{
                    label: 'Total Amount',
                    data: [necessityChart.dataset.necessary, necessityChart.dataset.unnecessary],
                    backgroundColor: [
                        'rgb(25,135,84)',
                        'rgb(220,53,69)'
                    ],
                    hoverOffset: 4
                }]
            },
            options: options
        });
    }

    async #initializeDatePicker(id, date) {
        this.#monthPicker = await getDatePicker("#monthSelector", "month")
        this.#monthPicker.datepicker('setDate', date.toISOString());
        this.#monthPicker.on('changeDate', async () => {
            this.#refresh(id, this.#monthPicker.datepicker('getUTCDate'));            
        });

        $('.monthPicker .calendar-button').on('click', function () {
            var input = $(this).siblings('.monthSelector');
            if (!input.data('datepicker').picker.is(':visible')) {
                input.datepicker('show');
            } else {
                input.datepicker('hide');
            }
        });
    }

    async #initializeTable(token) {
        try {
            const { default: DataTable } = await import(/* webpackChunkName: "datatables" */'datatables.net-bs5');
            var lastAjaxData = {
                start: 0,
                lastId: null,
                lastValue: null
            };
            var self = this;
            var table = new DataTable('#transactions-table', {
                processing: true,
                serverSide: true,
                deferLoading: 0,
                order: [[1, 'desc']],
                ajax: function (data, callback, settings) {
                    var table = new $.fn.dataTable.Api(settings);

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

                    var date = self.#monthPicker.datepicker('getUTCDate');
                    var start = new Date(Date.UTC(date.getFullYear(), date.getMonth()));
                    var end = new Date(Date.UTC(date.getFullYear(), date.getMonth() + 1, 1));                  
                    end.setMilliseconds(-1);
                    var requestData = {
                        draw: data.draw,
                        start: data.start,
                        pageSize: data.length,
                        orderBy: orderBy,
                        orderDirection: orderDirection === 'asc' ? 0 : 1,
                        lastId: lastId,
                        lastValue: lastValue,
                        isPrevious: isPrevious,
                        categoryId: self.#data.id,
                        MinDate: start.toISOString(),
                        MaxDate: end.toISOString(),
                    };
                    $.ajax({
                        url: API_ROUTES.transactions.GET_SEARCH,
                        type: 'POST',
                        contentType: 'application/json',
                        headers: {
                            'RequestVerificationToken': token
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
                lengthMenu: [10, 25, 50],
                columns: [
                    { data: 'title', render: DataTable.render.text(), name: 'title' },
                    { data: 'dateTime', name: 'dateTime' },
                    { data: 'amount', name: 'amount' },
                    {
                        data: null,
                        defaultContent:
                            `<div class="d-flex justify-content-center align-items-center flex-wrap gap-2">
                        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" class="table-icon" fill="rgba(255, 255, 255, 1)" data-icon="edit">
                            <use href="#edit-icon"/>
                        </svg >
                        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" class="table-icon" viewBox="0 0 16 16" fill="rgba(255, 255, 255, 1)" data-icon="delete">
                            <use href="#trash-icon"/>
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
            this.table = table;
            var tableContainer = document.getElementById('table-container');
            tableContainer.style = '';
            table.columns.adjust();
        } catch (error) {
            console.error('Error loading Datatable:', error);
            throw error;
        }
    }

    async #refresh(id, date) {
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
            }
            this.#isLoading = true;
            this.table.ajax.reload(null, true)
            var data = await this.#getData(id, date, this.#data.categoryType);

            if (data) {
                this.#formatCharts(data);
                this.#formatHeaders(data);
                this.#data = data;
            }
            
        } finally {
            this.#isLoading = false;
        }
    } 

    async #getData(id, date, type) {
        var data = await getCategoryDataByMonth(id, date, type);
        return data;
    }

    #formatCharts(data) {
        var dataObj = data ?? this.#data;

        if (dataObj == null) {
            return false;
        }
        this.#sentimentChart.data.datasets[0].data = [dataObj.happyTotal, dataObj.total - dataObj.happyTotal, Number.MIN_VALUE];
        this.#sentimentChart.update();

        this.#necessityChart.data.datasets[0].data = [dataObj.necessaryTotal, dataObj.total - dataObj.necessaryTotal, Number.MIN_VALUE];
        this.#necessityChart.update();

        return true;
    }

    #formatHeaders(data) {
        var dataObj = data ?? this.#data;
        if (dataObj == null) {
            return false;
        }
        var isIncomeCategory = dataObj.categoryType === 1;
        var budgetHeading = isIncomeCategory ? 'Goal' : 'Budget';
        var totalHeading = isIncomeCategory ? 'Income' : 'Expenses';
        var budget = dataObj.budgetLimit?.budget ?? dataObj.budget;
        var difference = budget - dataObj.total;
        var differenceHeading;
        if (difference < 0) {
            differenceHeading = isIncomeCategory ? "Surplus" : "Overspending";
        } else {
            differenceHeading = isIncomeCategory ? "Pending" : "Available";
        }       

        this.#budgetHeader.textContent = `${budgetHeading}: ${window.userNumberFormat.format(budget)}`;
        this.#totalHeader.textContent = `${totalHeading}: ${window.userNumberFormat.format(dataObj.total)}`;
        this.#differenceHeader.textContent = `${differenceHeading}: ${window.userNumberFormat.format(Math.abs(difference))}`;

        return true;
    }

    formatDashboard(data) {
        var dataObj = data ?? this.#data;

        if (dataObj == null) {
            return false;
        }
        this.#formatCharts(dataObj);
        this.#formatHeaders(dataObj);
        this.table.rows().invalidate().draw();

        return true;
    }

    addTransaction(transaction) {
        var transactionDate = new Date(transaction.dateTime)
        var transactionYear = transactionDate.getYear();
        var transactionMonth = transactionDate.getMonth();
        var currentDate = this.getCurrentMonthUTC();
        var currentYear = currentDate.getYear();
        var currentMonth = currentDate.getMonth();

        if (transactionYear === currentYear && transactionMonth == currentMonth) {
            this.#data.total += transaction.amount;

            if (transaction.isHappy) {
                this.#data.happyTotal += transaction.amount;
            }
            if (transaction.isNecessary) {
                this.#data.necessaryTotal += transaction.amount;
            }     

            this.#formatHeaders();
            this.#formatCharts();
            this.table.ajax.reload(null, false);
        }
    }

    getCurrentMonthUTC = () => this.#monthPicker.datepicker('getUTCDate');
    getCurrentMonth = () => this.#monthPicker.datepicker('getDate');
}