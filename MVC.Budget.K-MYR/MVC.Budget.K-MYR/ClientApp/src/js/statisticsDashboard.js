import { Chart, BarController, BarElement, CategoryScale, LinearScale, LineController, LineElement, PointElement, DoughnutController, ArcElement, Title} from 'chart.js';
Chart.register(BarController, BarElement, CategoryScale, LinearScale, LineController, LineElement, PointElement, DoughnutController, ArcElement, Title);
import { getColor } from './utilities';
import { getDatePicker } from './asyncComponents';
import { getFiscalPlanDataByYear } from './api';

export default class StatisticsDashboard {
    #data;
    #initPromise;
    #isLoading;
    #yearPicker
    #sentimentChartYearly;
    #necessityChartYearly;
    #sentimentBarChart;
    #necessityBarChart;
    #overspendingChart;
    #totalSpentChart;
    #overspendingHeading; 
    #totalSpentHeading;

    constructor(id, year) {
        this.#data = null;
        this.#initPromise = this.#init(id, year);
    }

    async #init(id, year) {
        try {
            var chartsPromise = this.#initializeCharts();            

            this.#overspendingHeading = document.getElementById('statistics-overspending');
            this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(0)}`;
            this.#totalSpentHeading = document.getElementById('statistics-totalSpent');
            this.#totalSpentHeading.textContent = `Total Spent: ${window.userNumberFormat.format(0)}`;

            var data = await this.#getData(id, year);
            await chartsPromise;           

            this.#renderData(data);
            this.#initializeDatePicker(id, year)

            $('.yearPicker .calendar-button').on('click', function () {
                let input = $(this).siblings('.yearSelector');
                if (!input.data('datepicker').picker.is(':visible')) {
                    input.datepicker('show');
                } else {
                    input.datepicker('hide');
                }
            });

            this.#data = data;

        } finally {
            this.#isLoading = false;
        }
    }

    async refresh(id, year) {
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
                return false;
            }

            this.#isLoading = true;
            let data = await this.#getData(id, year);
            this.#renderData(data);
        } finally {
            this.#isLoading = false;
        }
    }

    async #getData(id, year) {
        var data = await getFiscalPlanDataByYear(id, year);
        return data;
    }

    formatDashboard() {
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
                return false;
            }
            this.#isLoading = true;
            this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(this.#data.overspendingTotal)}`;
            this.#totalSpentHeading.textContent = `Total Spent: ${window.userNumberFormat.format(this.#data.totalSpent)}`;
            this.#sentimentBarChart.update('none');
            this.#necessityBarChart.update('none');
            this.#overspendingChart.update('none');
            this.#totalSpentChart.update('none');
            
        } finally {
            this.#isLoading = false;
        }
    }

    async #renderData(data) {
        var dataObj = data ?? this.#data;

        if (dataObj == null) {
            return false;
        }

        var tasks = [];

        this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(data.overspendingTotal)}`;
        this.#totalSpentHeading.textContent = `Total Spent: ${window.userNumberFormat.format(data.totalSpent)}`;

        this.#sentimentChartYearly.data.datasets[0].data = [data.happyEvaluatedTotal, data.unhappyEvaluatedTotal, Number.MIN_VALUE];
        tasks.push(this.#updateChartTask(this.#sentimentChartYearly));

        this.#necessityChartYearly.data.datasets[0].data = [data.necessaryEvaluatedTotal, data.unnecessaryEvaluatedTotal, Number.MIN_VALUE]
        tasks.push(this.#updateChartTask(this.#necessityChartYearly));

        this.#sentimentBarChart.data.datasets[0].data = data.happyPerMonth;
        this.#sentimentBarChart.data.datasets[1].data = data.unhappyPerMonth;
        this.#sentimentBarChart.data.datasets[2].data = data.happyEvaluatedPerMonth;
        this.#sentimentBarChart.data.datasets[3].data = data.unhappyEvaluatedPerMonth;
        this.#sentimentBarChart.data.datasets[4].data = data.unevaluatedPerMonth;
        tasks.push(this.#updateChartTask(this.#sentimentBarChart));

        this.#necessityBarChart.data.datasets[0].data = data.necessaryPerMonth;
        this.#necessityBarChart.data.datasets[1].data = data.unnecessaryPerMonth;
        this.#necessityBarChart.data.datasets[2].data = data.necessaryEvaluatedPerMonth;
        this.#necessityBarChart.data.datasets[3].data = data.unnecessaryEvaluatedPerMonth;
        this.#necessityBarChart.data.datasets[4].data = data.unevaluatedPerMonth;
        tasks.push(this.#updateChartTask(this.#necessityBarChart));

        var datasets = [];

        for (let i = 0; i < data.monthlyOverspendingPerCategory.length; i++) {
            let categoryData = data.monthlyOverspendingPerCategory[i];
            datasets.push({
                label: categoryData.category,
                data: categoryData.overspendingPerMonth,
                borderWidth: 2,
                borderColor: '#d3d3d3',
                backgroundColor: getColor(i)
            });
        }

        this.#overspendingChart.data.datasets = datasets;
        tasks.push(this.#updateChartTask(this.#overspendingChart));

        this.#totalSpentChart.data.datasets[0].data = data.totalPerMonth;
        tasks.push(this.#updateChartTask(this.#totalSpentChart));

        await Promise.all(tasks);
        this.#data = data;
    }

    async #initializeDatePicker(id, year) {
        this.#yearPicker = await getDatePicker("#statistics-yearSelector");
        this.#yearPicker.datepicker('setDate', year.toISOString());
        this.#yearPicker.on('changeDate', async () => {
            this.refresh(id, this.#yearPicker.datepicker('getUTCDate'));
        });

    }

    async #initializeCharts() {
        var tasks = [];
        var pieChartDatasets = [{
            label: 'Total Amount',
            data: [0, 0],
            backgroundColor: [
                'rgb(25,135,84)',
                'rgb(220,53,69)'
            ],
            hoverOffset: 4
        }];
        var tooltip = {
            callbacks: {
                label: function (context) {
                    let label = context.dataset.label || '';

                    if (label) {
                        label += ': ';
                    }
                    if (context.parsed !== null) {
                        label += window.userNumberFormat.format(context.parsed);
                    }
                    return label;
                }
            }
        };
        var options = {
            responsive: true,
            layout: {
                padding: 2
            },
            maintainAspectRatio: false,
            plugins: {
                tooltip: tooltip
            }
        };
        var scales = {
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
                        return window.userNumberFormat.format(value);
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
        };

        tasks.push(this.#createChartTask('sentimentChartYear', {
            type: 'doughnut',
            data: {
                labels: ['Happy', 'Unhappy'],
                datasets: pieChartDatasets
            },
            options: options
        }).then(chart => this.#sentimentChartYearly = chart));

        tasks.push(this.#createChartTask('necessityChartYear', {
            type: 'doughnut',
            data: {
                labels: ['Necessary', 'Unnecessary'],
                datasets: pieChartDatasets
            },
            options: options
        }).then(chart => this.#necessityChartYearly = chart));

        var defaultTooltip =
        {
            callbacks: {
                label: function (context) {
                    let label = context.dataset.label || '';

                    if (label) {
                        label += ': ';
                    }
                    if (context.parsed.y !== null) {
                        label += window.userNumberFormat.format(context.parsed.y);
                    }
                    return label;
                }
            }
        };

        var defaultOptions =
        {
            ...options,
            scales: scales
        };
        defaultOptions.plugins = {
            emptypiechart: false,
            tooltip: defaultTooltip            
        };

        tasks.push(this.#createChartTask('sentimentBarChartYear', {
            type: 'bar',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
                datasets: [{
                    label: 'Happy',
                    stack: 'Unevaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#20c997',
                },
                {
                    label: 'Unhappy',
                    stack: 'Unevaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: 'rgb(220,53,69)'
                },
                {
                    label: 'Happy (Eval.)',
                    stack: 'Evaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#0f7c5c',
                },
                {
                    label: 'Unhappy (Eval.)',
                    stack: 'Evaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#881d27',
                },
                {
                    label: 'Unevaluated',
                    stack: 'Evaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#1c1c1c',
                }]
            },
            options: defaultOptions
        }).then(chart => this.#sentimentBarChart = chart));

        tasks.push(this.#createChartTask('necessityBarChartYear', {
            type: 'bar',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
                datasets: [{
                    label: 'Necessary',
                    stack: 'Unevaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#20c997',
                },
                {
                    label: 'Unnecessary',
                    stack: 'Unevaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: 'rgb(220,53,69)'
                },
                {
                    label: 'Necessary (Eval.)',
                    stack: 'Evaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#0f7c5c',
                },
                {
                    label: 'Unnecessary (Eval.)',
                    stack: 'Evaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#881d27',
                },
                {
                    label: 'Unevaluated',
                    stack: 'Evaluated',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#1c1c1c',
                }]
            },
            options: defaultOptions
        }).then(chart => this.#necessityBarChart = chart));

        tasks.push(this.#createChartTask('overspendingChart', {
            type: 'bar',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
            },
            options: {
                plugins: {
                    emptypiechart: false,
                    tooltip: defaultTooltip,
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
                                return window.userNumberFormat.format(value);
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
                }
            }
        }).then(chart => this.#overspendingChart = chart));

        tasks.push(this.#createChartTask('totalSpentChart', {
            type: 'line',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
                datasets: [{
                    label: 'Total Spent Per Month',
                    borderWidth: 2,
                    borderColor: '#d3d3d3',
                    backgroundColor: '#20c997'

                }]
            },
            options: defaultOptions
        }).then(chart => this.#totalSpentChart = chart));

        await Promise.all(tasks);
    }

    #createChartTask(elementId, config) {
        return new Promise((resolve) => {
            setTimeout(() => {
                let chart = new Chart(document.getElementById(elementId), config);
                resolve(chart);
            }, 0);
        });
    }

    #updateChartTask(chart) {
        return new Promise(() => {
            setTimeout(() => {
                chart.update();
            }, 0);
        });
    }
}
