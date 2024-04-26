import Chart from 'chart.js/auto';
import { getRandomColor } from './utilities';

export default class Statistics {
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
                                    label += window.userNumberFormat.format(context.parsed);
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
                                    label += window.userNumberFormat.format(context.parsed);
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

        this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(this.#data.overspendingTotal)}`;

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
                }
            }
        });
    }
}