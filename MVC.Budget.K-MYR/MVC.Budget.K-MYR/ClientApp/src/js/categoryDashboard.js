import { ArcElement, Chart, DoughnutController } from 'chart.js';
Chart.register(DoughnutController, ArcElement);
import { getDatePicker } from './asyncComponents'
import {  getCategoryDataByMonth } from './api';

export default class CategoryDashboard {
    #data;
    #isLoading;
    #initPromise;
    #monthPicker;
    #sentimentChart;
    #necessityChart;    
    #table;
    #budgetHeader;
    #differenceHeader;
    #totalHeader;

    constructor(id, date, data) {
        this.#data = data;
        this.#initPromise = this.#init(id, date);
    }

    async #init(id, date) {
        try {
            this.#isLoading = true;
            var datepickerPromise = this.#initializeDatePicker(id, date);
            var tablePromise = this.#initializeTable(datepickerPromise);
            this.#initializeCharts();

            this.#budgetHeader = document.getElementById('budget-header');
            this.#totalHeader = document.getElementById('total-header');
            this.#differenceHeader = document.getElementById('difference-header');

            this.#table = await tablePromise;
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

    async #initializeTable(promise) {
        await promise;
        try {
            const { default: DataTable } = await import(/* webpackChunkName: "datatables" */'datatables.net-bs5');
            var lastAjaxData = {
                start: 0,
                lastId: null,
                lastValue: null
            };
            var table = new DataTable('#transactions-table', {
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
                        MinDate: minDate.length > 0 ? minDate : null,
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
            var tableContainer = document.getElementById('table-container');
            tableContainer.style = '';
            table.columns.adjust();

            return table;
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
            var data = await this.#getData(id, date, this.#data.categoryType);

            if (data) {
                this.#renderData(data);
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
        var differenceHeading = isIncomeCategory ? 'Missing' : 'Overspending';
        var budget = dataObj.budgetLimit?.budget ?? dataObj.budget;
        var difference = budget - dataObj.total;
        var differenceHeading;
        if (difference < 0) {
            differenceHeading = isIncomeCategory ? "Surplus" : "Overspending";
        } else {
            differenceHeading = isIncomeCategory ? "Missing" : "Available";
        }       

        this.#budgetHeader.textContent = `${budgetHeading}: ${window.userNumberFormat.format(budget)}`;
        this.#totalHeader.textContent = `${totalHeading}: ${window.userNumberFormat.format(dataObj.total)}`;
        this.#differenceHeader.textContent = `${differenceHeading}: ${window.userNumberFormat.format(Math.abs(difference))}`;

        return true;
    }

    #renderData(data) {
        var dataObj = data ?? this.#data;

        if (dataObj == null) {
            return false;
        }
        this.#formatCharts(dataObj);
        this.#formatHeaders(dataObj);
        //setTimeout(() => this.#table.rows().invalidate().draw(), 0);

        return true;
    }

    getCurrentMonthUTC = () => this.#monthPicker.datepicker('getUTCDate');
    getCurrentMonth = () => this.#monthPicker.datepicker('getDate');
}