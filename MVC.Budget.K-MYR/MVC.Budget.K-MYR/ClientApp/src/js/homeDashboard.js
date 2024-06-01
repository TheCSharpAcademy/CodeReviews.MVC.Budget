import { Chart } from 'chart.js';

export default class HomeDashboard {
    #data;
    #isLoading;
    #sentimentChartMonthly;
    #necessityChartMonthly;
    #overspendingHeading;
    #incomeBalanceHeader;
    #incomeAccordionBody;
    #expenseBalanceHeader;
    #expenseAccordionBody;
    #menu;


    constructor(menu) {
        this.#data = null;
        this.#menu = menu;
        this.init();
    }
    init() {
        
        this.#sentimentChartMonthly = new Chart(document.getElementById('sentimentChart'), {
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

        this.#necessityChartMonthly = new Chart(document.getElementById('necessityChart'), {
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

        this.#overspendingHeading = document.getElementById('home-overspending');
        this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(0)}`;

        this.#incomeBalanceHeader = document.getElementById('incomeBalanceHeader');
        this.#incomeAccordionBody = document.getElementById('incomeAccordionBody');

        this.#expenseBalanceHeader = document.getElementById('expensesBalanceHeader');
        this.#expenseAccordionBody = document.getElementById('expensesAccordionBody');
    }

    async refresh(id, date) {
        if (this.#isLoading) {
            console.log("Statistics are loading...")
            return;
        }

        this.#isLoading = true;
        let data = await this.#getData(id, date);
        this.#data = data;
        if (data) {
            let hasUpdated = this.#updateCharts(data);
            let hasCreated = this.#createCategoryElements(data);
            await hasUpdated;
            await hasCreated;
        }
        
        this.#isLoading = false;
    }

    async #getData(id, date) {
        try {
            let response = await fetch(`https://localhost:7246/api/FiscalPlan/${id}/Month?Month=${date}`, {
                method: "GET",
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

    async #updateCharts(data) {
        if (!data) {
            return false;
        }

        this.#sentimentChartMonthly.data.datasets[0].data = [data.expensesHappyTotal, data.expensesTotal - data.expensesHappyTotal, Number.MIN_VALUE];
        this.#sentimentChartMonthly.update();

        this.#necessityChartMonthly.data.datasets[0].data = [data.expensesNecessaryTotal, data.expensesTotal - data.expensesNecessaryTotal, Number.MIN_VALUE]
        this.#necessityChartMonthly.update();

        this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(data.overspending)}`

        this.#data = data;
    }

    #createCategoryElements(data) {
        let fragment = document.createDocumentFragment();
        let orderedCategories = data.incomeCategories.sort(function (a, b) {
            return a.name.localeCompare(b.name);
        });

        for (var i = 0; i < orderedCategories.length; i++) {
            fragment.appendChild(this.#createCategoryELement(orderedCategories[i], data.id))
        }

        this.#incomeAccordionBody.textContent = "";
        this.#incomeAccordionBody.appendChild(fragment);

        fragment = document.createDocumentFragment();
        orderedCategories = data.expenseCategories.sort(function (a, b) {
            return a.name.localeCompare(b.name);
        });

        for (var i = 0; i < orderedCategories.length; i++) {
            fragment.appendChild(this.#createCategoryELement(orderedCategories[i], data.id))
        }

        this.#expenseAccordionBody.textContent = "";
        this.#expenseAccordionBody.appendChild(fragment);
        return true;
    }   

    #createCategoryELement(category, fiscalPlanId) {
        let mainDiv = document.createElement('div');
        mainDiv.id = `category_${category.id}`;
        mainDiv.className = 'category';
        mainDiv.dataset.id = `${category.id}`;
        mainDiv.dataset.type = `${category.categoryType}`;
        mainDiv.dataset.name = `${category.name}`;
        mainDiv.dataset.budget = `${category.budget}`;
        mainDiv.dataset.fiscalplanid = `${fiscalPlanId}`;  

        let menu = this.#menu;
        mainDiv.addEventListener("click", function (event) {
            
            if (menu.dataset.categoryid != 0) {
                var borderBox = document.getElementById(`category_${menu.dataset.categoryid}`).querySelector('.border-animation');
                borderBox.classList.remove('border-rotate');
            }
            menu.dataset.categoryid = `${category.id}`;
            menu.dataset.type = `${ category.categoryType }`;
            menu.style.left = `${mainDiv.style.left + event.pageX - 100}px`;
            menu.style.top = `${event.pageY - 100}px`;
            menu.classList.add('active');

            this.querySelector('.border-animation').classList.add('border-rotate');
        });

        let borderContainerDiv = document.createElement('div');
        borderContainerDiv.className = 'border-container';

        let contentDiv = document.createElement('div');
        contentDiv.className = 'content';

        let categoryBodyDiv = document.createElement('div');
        categoryBodyDiv.className = 'category-body';

        let workDiv = document.createElement('div');
        workDiv.textContent = `${category.name}`;

        let msAutoDiv = document.createElement('div');
        msAutoDiv.className = 'ms-auto';

        let balanceTextSpan = document.createElement('span');
        balanceTextSpan.className = 'balance-text';
        balanceTextSpan.textContent = `${window.userNumberFormat.format(category.total)} / ${window.userNumberFormat.format(category.budget)}`;

        msAutoDiv.appendChild(balanceTextSpan);

        categoryBodyDiv.appendChild(workDiv);
        categoryBodyDiv.appendChild(msAutoDiv);

        let progressDiv = document.createElement('div');
        progressDiv.className = 'progress';

        let progressBarDivPercentage = Math.floor(category.total * 100 / category.budget);
        let color = "bg-success";
        if (category.categoryType == 2) {
            color = progressBarDivPercentage < 50 ? "bg-success" : progressBarDivPercentage < 85 ? "bg-warning" : "bg-danger";
        }

        let progressBarDiv = document.createElement('div');
        
        progressBarDiv.className = `progress-bar progress-bar-striped progress-bar-animated ${color}`;
        progressBarDiv.role = 'progressbar';
        progressBarDiv.style.width = `${progressBarDivPercentage}%`;
        progressBarDiv.ariaValuenow = `${progressBarDivPercentage}`;
        progressBarDiv.ariaValuemin = '0';
        progressBarDiv.ariaValuemax = '100';

        // Append progress-bar div to progress div
        progressDiv.appendChild(progressBarDiv);

        // Append category-body div and progress div to content div
        contentDiv.appendChild(categoryBodyDiv);
        contentDiv.appendChild(progressDiv);

        // Create border-animation div
        let borderAnimationDiv = document.createElement('div');
        borderAnimationDiv.className = 'border-animation';

        // Append content div and border-animation div to border-container div
        borderContainerDiv.appendChild(contentDiv);
        borderContainerDiv.appendChild(borderAnimationDiv);

        // Append border-container div to main div
        mainDiv.appendChild(borderContainerDiv);

        return mainDiv;
    }
}