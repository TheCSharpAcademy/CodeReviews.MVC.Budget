import { Chart, BarController, BarElement, CategoryScale, LinearScale, LineController, LineElement, PointElement, DoughnutController, ArcElement} from 'chart.js';
Chart.register(BarController, BarElement, CategoryScale, LinearScale, LineController, LineElement, PointElement, DoughnutController, ArcElement);
import { getRandomColor } from './utilities';
import { getDatePicker } from './asyncComponents'

export default class StatisticsDashboard {
    #data;
    #initPromise;
    #apiUrl;
    #isLoading;
    #yearPicker
    #sentimentChartYearly;
    #necessityChartYearly;
    #sentimentBarChart;
    #necessityBarChart;
    #overspendingChart;
    #totalSpentChart;
    #overspendingHeading; 

    constructor(id, year, apiUrl) {
        this.#data = null;
        this.#apiUrl = apiUrl;
        this.#initPromise = this.#init(id, year);
    }

    async #init(id, year) {
        try {
            this.#isLoading = true;
            this.#initializeDatePicker(id, year);
            this.#sentimentChartYearly = new Chart(document.getElementById('sentimentChartYear'), {
                type: 'doughnut',
                data: {
                    labels: [
                        'Happy',
                        'Unhappy'
                    ],
                    datasets: [{
                        label: 'Total Amount',
                        data: [0, 0],
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
                                        label += window.userNumberFormat.format(context.parsed);
                                    }
                                    return label;
                                }
                            }
                        }
                    }
                }
            });

            this.#necessityChartYearly = new Chart(document.getElementById('necessityChartYear'), {
                type: 'doughnut',
                data: {
                    labels: [
                        'Necessary',
                        'Unnecessary'
                    ],
                    datasets: [{
                        label: 'Total Amount',
                        data: [0, 0],
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
                                        label += window.userNumberFormat.format(context.parsed);
                                    }
                                    return label;
                                }
                            }
                        }
                    }
                }
            });

            this.#sentimentBarChart = new Chart(document.getElementById('sentimentLineChartYear'), {
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
                                        label += window.userNumberFormat.format(context.parsed.y);
                                    }
                                    return label;
                                }
                            }
                        }
                    }
                }
            });

            this.#necessityBarChart = new Chart(document.getElementById('necessityLineChartYear'), {
                type: 'bar',
                data: {
                    labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
                    datasets: [{
                        label: 'Necessary',
                        stack: 'Unevaluated',
                        backgroundColor: '#20c997',
                        borderWidth: 2,
                        borderColor: '#d3d3d3',

                    },
                    {
                        label: 'Unnecessary',
                        stack: 'Unevaluated',
                        backgroundColor: 'rgb(220,53,69)',
                        borderWidth: 2,
                        borderColor: '#d3d3d3',

                    },
                    {
                        label: 'Necessary (Eval.)',
                        stack: 'Evaluated',
                        backgroundColor: '#0f7c5c',
                        borderWidth: 2,
                        borderColor: '#d3d3d3',

                    },
                    {
                        label: 'Unnecessary (Eval.)',
                        stack: 'Evaluated',
                        backgroundColor: '#881d27',
                        borderWidth: 2,
                        borderColor: '#d3d3d3',
                    },
                    {
                        label: 'Unevaluated',
                        stack: 'Evaluated',
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
                                        label += window.userNumberFormat.format(context.parsed.y);
                                    }
                                    return label;
                                }
                            }
                        }
                    }
                }
            });

            this.#overspendingChart = new Chart(document.getElementById('overspendingChart'), {
                type: 'bar',
                data: {
                    labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dez",],
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
                                        label += window.userNumberFormat.format(context.parsed.x);
                                    }
                                    return label;
                                }
                            }
                        }
                    }
                }
            });

            this.#overspendingHeading = document.getElementById('statistics-overspending');
            this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(0)}`;

            this.#totalSpentChart = new Chart(document.getElementById('totalSpentChart'), {
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
                                    if (context.parsed.x !== null) {
                                        label += window.userNumberFormat.format(context.parsed.y);
                                    }
                                    return label;
                                }
                            }
                        }
                    }
                }
            });            

            let data = await this.#getData(id, year);
            this.#renderData(data);

            $('.yearPicker .calendar-button').on('click', function () {
                let input = $(this).siblings('.yearSelector');
                if (!input.data('datepicker').picker.is(':visible')) {
                    input.datepicker('show');
                } else {
                    input.datepicker('hide');
                }
            });

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
        try {
            let response = await fetch(`${this.#apiUrl}/${id}/${year.getFullYear()}`, {
                method: "GET"
            });

            if (response.ok) {
                return await response.json();
            } else {
                console.error(`HTTP GET Error: ${response.status}`);
                return null;
            }
        } catch (error) {
            console.error(error);
            return null;
        }
    }

    rerenderDashboard() {
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
                return false;
            }

            this.#renderData();
        } finally {
            this.#isLoading = false;
        }
    }

    #renderData(data) {
        let dataObj = data ?? this.#data;

        if (dataObj == null) {
            return false;
        }

        this.#sentimentChartYearly.data.datasets[0].data = [data.happyEvaluatedTotal, data.unhappyEvaluatedTotal, Number.MIN_VALUE];
        this.#sentimentChartYearly.update();

        this.#necessityChartYearly.data.datasets[0].data = [data.necessaryEvaluatedTotal, data.unnecessaryEvaluatedTotal, Number.MIN_VALUE]
        this.#necessityChartYearly.update();

        this.#sentimentBarChart.data.datasets[0].data = data.happyPerMonth;
        this.#sentimentBarChart.data.datasets[1].data = data.unhappyPerMonth;
        this.#sentimentBarChart.data.datasets[2].data = data.happyEvaluatedPerMonth;
        this.#sentimentBarChart.data.datasets[3].data = data.unhappyEvaluatedPerMonth;
        this.#sentimentBarChart.data.datasets[4].data = data.unevaluatedPerMonth;
        this.#sentimentBarChart.update();

        this.#necessityBarChart.data.datasets[0].data = data.necessaryPerMonth;
        this.#necessityBarChart.data.datasets[1].data = data.unnecessaryPerMonth;
        this.#necessityBarChart.data.datasets[2].data = data.necessaryEvaluatedPerMonth;
        this.#necessityBarChart.data.datasets[3].data = data.unnecessaryEvaluatedPerMonth;
        this.#necessityBarChart.data.datasets[4].data = data.unevaluatedPerMonth;
        this.#necessityBarChart.update();

        let datasets = [];

        for (let i = 0; i < data.monthlyOverspendingPerCategory.length; i++) {
            let categoryData = data.monthlyOverspendingPerCategory[i];
            datasets.push({
                label: categoryData.category,
                data: categoryData.overspendingPerMonth,
                borderWidth: 2,
                borderColor: '#d3d3d3',
                backgroundColor: getRandomColor()
            });
        }

        this.#overspendingChart.data.datasets = datasets;
        this.#overspendingChart.update();

        this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(data.overspendingTotal)}`;

        this.#totalSpentChart.data.datasets[0].data = data.totalPerMonth;
        this.#totalSpentChart.update();
        
        this.#data = data;
    }

    async #initializeDatePicker(id, year) {
        let self = this;
        this.#yearPicker = await getDatePicker("#statistics-yearSelector");
        this.#yearPicker.datepicker('setDate', year.toISOString());

        this.#yearPicker.on('changeDate', async function () {
            self.refresh(id, self.#yearPicker.datepicker('getUTCDate'))
        });
    }
}
