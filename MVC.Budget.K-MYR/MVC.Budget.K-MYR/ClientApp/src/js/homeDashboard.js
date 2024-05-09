import Chart from 'chart.js/auto';

export default class HomeDashboard {
    #data;
    #initPromise;
    #sentimentChartMonthly;
    #necessityChartMonthly;    
    #overspendingHeading;
    #incomeBalanceHeader;
    #incomeAccordionBody;
    #expenseBalanceHeader;
    #expenseAccordionBody;
    

    constructor() {
        this.data = null;
        this.initPromise = this.init();
    }

    async init() {
        await this.getData();

        this.#sentimentChartMonthly = new Chart(document.getElementById('sentimentChart'), {
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
                                    label += window.userNumberFormat.format(context.parsed);
                                }
                                return label;
                            }
                        }
                    }
                }
            }
        });

        this.#sentimentChartMonthly = new Chart(document.getElementById('necessityChart'), {
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
                                    label += window.userNumberFormat.format(context.parsed);
                                }
                                return label;
                            }
                        }
                    }
                }
            }
        });

        this.#overspendingHeading = document.getElementById('statistics-overspending');
        this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(this.#data.overspendingTotal)}`;

        this.#incomeBalanceHeader = document.getElementById('incomeBalanceHeader');        
        this.#incomeAccordionBody = document.getElementById('incomeAccordionBody');

        this.#expenseBalanceHeader = document.getElementById('expensesBalanceHeader');
        this.#expenseAccordionBody = document.getElementById('expensesAccordionBody');
    }

    async getData() {

    }
}