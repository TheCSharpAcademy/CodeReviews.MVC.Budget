import { ArcElement, Chart, DoughnutController } from 'chart.js';
Chart.register(DoughnutController, ArcElement);
import { getDatePicker } from './asyncComponents'

export default class HomeDashboard {
    #data;
    #isLoading;
    #initPromise;
    #monthPicker
    #sentimentChartMonthly;
    #necessityChartMonthly;
    #overspendingHeading;
    #incomeBalanceHeader;
    #incomeAccordionBody;
    #expenseBalanceHeader;
    #expenseAccordionBody;
    #menu;


    constructor(menu, id, date) {
        this.#data = null;
        this.#menu = menu;       
        this.#initPromise = this.#init(id, date);
    }

    async #init(id, date) {
        try {
            let self = this;
            this.#isLoading = true;

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

            this.#monthPicker = await getDatePicker("#home-monthSelector", "month");
            this.#monthPicker.datepicker('setDate', date.toISOString());
            this.#monthPicker.on('changeDate', async function () {
                self.refresh(id, self.#monthPicker.datepicker('getUTCDate'))
            });

            let data = await this.#getData(id, date);
            this.#data = data;

            this.#incomeBalanceHeader.textContent = `${window.userNumberFormat.format(data.incomeTotal)} / ${window.userNumberFormat.format(data.incomeBudget)}`;
            this.#expenseBalanceHeader.textContent = `${window.userNumberFormat.format(data.expensesTotal)} / ${window.userNumberFormat.format(data.expensesBudget)}`;

            if (data) {
                let hasUpdated = this.#updateCharts(data);
                let hasCreated = this.#createCategoryElements(data);
                hasUpdated;
                hasCreated;
            }
        } finally {
            this.#isLoading = false;
        }        
    }

    async refresh(id, date) {        
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
                return false;
            }

            this.#isLoading = true;
            let data = await this.#getData(id, date);
            this.#updateCharts(data);
            this.#updateCategories(data.incomeCategories)
            this.#updateCategories(data.expenseCategories)
        } finally {
            this.#isLoading = false;
        }
    }

    async #getData(id, date) {
        try {
            let response = await fetch(`https://localhost:7246/api/FiscalPlan/${id}/Month?Month=${date.toISOString() ?? new Date().toISOString()}`, {
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

    #updateCharts(data) {
        let dataObj = data ?? this.#data;       

        if (dataObj == null) {
            return false;
        }

        this.#sentimentChartMonthly.data.datasets[0].data = [dataObj.expensesHappyTotal, dataObj.expensesTotal - dataObj.expensesHappyTotal, Number.MIN_VALUE];
        this.#sentimentChartMonthly.update();

        this.#necessityChartMonthly.data.datasets[0].data = [dataObj.expensesNecessaryTotal, dataObj.expensesTotal - dataObj.expensesNecessaryTotal, Number.MIN_VALUE]
        this.#necessityChartMonthly.update();

        this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(dataObj.overspending)}`

        this.#incomeBalanceHeader.textContent = `${window.userNumberFormat.format(data.incomeTotal)} / ${window.userNumberFormat.format(data.incomeBudget)}`;
        this.#expenseBalanceHeader.textContent = `${window.userNumberFormat.format(data.expensesTotal)} / ${window.userNumberFormat.format(data.expensesBudget)}`;


        return true;
    }

    #updateCategories(categories) {
        if (categories.length == 0) {
            return false;
        }

        for (let i = 0; i < categories.length; i++) {
            this.#updateCategory(categories[i]);
        }
    }

    #updateCategory(category) {
        let categoryElement;

        if (!category) {
            return false;
        }

        switch (category.categoryType) {
            case 1:
                categoryElement = this.#incomeAccordionBody.querySelector(`#category_${category.id}`);

                break;
            case 2:
                categoryElement = this.#expenseAccordionBody.querySelector(`#category_${category.id}`);
                break;
        }

        if (!categoryElement) {
            return false;
        }

        categoryElement.querySelector(`#category_${category.id}_name`).textContent = decodeURIComponent(category.name);

        let progressBarELement = categoryElement.querySelector(`#category_${category.id}_progress`)
        let balanceELement = categoryElement.querySelector(`#category_${category.id}_balance`)
        balanceELement.textContent = `${window.userNumberFormat.format(category.total)} / 
            ${window.userNumberFormat.format(category.budgetLimit?.budget ?? category.budget)}`;

        let progressBarELementPercentage = Math.floor(category.total * 100 / category.budget);
        let color = "bg-success";
        if (category.categoryType == 2) {
            color = progressBarELementPercentage < 50 ? "bg-success" : progressBarELementPercentage < 85 ? "bg-warning" : "bg-danger";
        }

        progressBarELement.className = `progress-bar progress-bar-striped progress-bar-animated ${color}`;
        progressBarELement.style.width = `${progressBarELementPercentage}%`;
        progressBarELement.ariaValuenow = `${progressBarELementPercentage}`;

        return true;
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

    #createCategoryELement(category) {
        let mainDiv = document.createElement('div');
        mainDiv.id = `category_${category.id}`;
        mainDiv.className = 'category';
        mainDiv.dataset.id = `${category.id}`;
        mainDiv.dataset.type = `${category.categoryType}`;
        mainDiv.dataset.name = `${category.name}`;
        mainDiv.dataset.budget = `${category.budget}`;
        mainDiv.dataset.fiscalplanid = `${category.fiscalPlanId}`;  

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

        let categoryNameDiv = document.createElement('div');
        categoryNameDiv.id = `category_${category.id}_name`;
        categoryNameDiv.textContent = decodeURIComponent(category.name);

        let categoryBalanceDiv = document.createElement('div');
        categoryBalanceDiv.className = 'ms-auto';        

        let categoryBalanceSpan = document.createElement('span');
        categoryBalanceSpan.id = `category_${category.id}_balance`;
        let budget = category.budgetLimit?.budget ?? category.budget;
        categoryBalanceSpan.className = 'balance-text';
        categoryBalanceSpan.textContent = `${window.userNumberFormat.format(category.total)} / 
            ${window.userNumberFormat.format(budget)}`;


        categoryBalanceDiv.appendChild(categoryBalanceSpan);

        categoryBodyDiv.appendChild(categoryNameDiv);
        categoryBodyDiv.appendChild(categoryBalanceDiv);

        let progressDiv = document.createElement('div');
        progressDiv.className = 'progress';

        let progressBarDivPercentage = Math.floor(category.total * 100 / budget);
        let color = "bg-success";
        if (category.categoryType == 2) {
            color = progressBarDivPercentage < 50 ? "bg-success" : progressBarDivPercentage < 85 ? "bg-warning" : "bg-danger";
        }

        let progressBarDiv = document.createElement('div');
        progressBarDiv.id = `category_${category.id}_progress`;

        
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