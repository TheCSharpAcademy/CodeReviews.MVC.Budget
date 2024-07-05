import { ArcElement, Chart, DoughnutController } from 'chart.js';
Chart.register(DoughnutController, ArcElement);
import { getDatePicker } from './asyncComponents'
import { getFiscalPlanDataByMonth } from './api';


export default class HomeDashboard {
    #data;
    #isLoading;
    #initPromise;
    #monthPicker;
    #sentimentChartMonthly;
    #necessityChartMonthly;
    #overspendingHeading;
    #incomeBalanceHeader;
    #incomeAccordionBody;
    #expenseBalanceHeader;
    #expenseAccordionBody;
    #menu;
    #dashboardContainer;


    constructor(menu, id, date, data) {
        this.#data = data;
        this.#menu = menu;      
        this.#initPromise = this.#init(id, date);
    }

    async #init(id, date) {
        try {
            this.#isLoading = true;
            this.#initializeDatePicker(id, date);

            var sentimentChart= document.getElementById('sentimentChart');
            this.#sentimentChartMonthly = new Chart(sentimentChart, {
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

            var necessityChart = document.getElementById('necessityChart');
            this.#necessityChartMonthly = new Chart(document.getElementById('necessityChart'), {
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

            this.#incomeBalanceHeader = document.getElementById('incomeBalanceHeader');
            this.#incomeAccordionBody = document.getElementById('incomeAccordionBody');

            this.#expenseBalanceHeader = document.getElementById('expensesBalanceHeader');
            this.#expenseAccordionBody = document.getElementById('expensesAccordionBody');  

            this.#dashboardContainer = document.getElementById('home-container');
            
            this.formatInitialCategories();   

        } finally {
            this.#isLoading = false;
        }        
    }

    setupMenuHandlers() {
        let categoryElements = this.#dashboardContainer.querySelectorAll('.category');

        for (let i = 0; i < categoryElements.length; i++) {
            let category = categoryElements[i];
            let menu = this.#menu;
            let id = category.dataset.id
            let type = category.dataset.type;
            category.addEventListener("click", function (event) {
                if (menu.dataset.categoryid != 0) {
                    var borderBox = document.getElementById(`category_${menu.dataset.categoryid}`).querySelector('.border-animation');
                    borderBox.classList.remove('border-rotate');
                }
                let y = Math.max(Math.min(event.pageY - 100, window.innerHeight - 200), 66);
                menu.dataset.categoryid = id;
                menu.dataset.type = type;
                menu.style.left = `${category.style.left + event.pageX - 100}px`;
                menu.style.top = `${y}px`;
                menu.classList.add('active');

                this.querySelector('.border-animation').classList.add('border-rotate');
            });
        }
    }

    formatInitialCategories()
    {
        let overspending = parseInt(this.#overspendingHeading.dataset.overspending);
        this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(overspending)}`;
        let incomeTotal = parseInt(this.#incomeBalanceHeader.dataset.total);
        let incomeBudget = parseInt(this.#incomeBalanceHeader.dataset.budget);
        let expenseTotal = parseInt(this.#expenseBalanceHeader.dataset.total);
        let expenseBudget = parseInt(this.#expenseBalanceHeader.dataset.budget);
        this.#incomeBalanceHeader.textContent = `${window.userNumberFormat.format(incomeTotal)} / ${window.userNumberFormat.format(incomeBudget)}`;
        this.#expenseBalanceHeader.textContent = `${window.userNumberFormat.format(expenseTotal)} / ${window.userNumberFormat.format(expenseBudget)}`;

        let categoryElements = this.#dashboardContainer.querySelectorAll('.category');

        for (let i = 0; i < categoryElements.length; i++) {
            let category = categoryElements[i];
            let id = parseInt(category.dataset.id);
            let total = parseInt(category.dataset.total);
            let budget = parseInt(category.dataset.budget);
            let type = parseInt(category.dataset.type);

            let balanceElement = category.querySelector(`#category_${id}_balance`)
            balanceElement.textContent = `${window.userNumberFormat.format(total)} / 
            ${window.userNumberFormat.format(budget)}`;

            if (total > budget) {
                let deviationAmount = total - budget;
                let deviationSpan = category.querySelector(`#category_${id}_deviationText`);
                deviationSpan.textContent = type === 1
                    ? `Windfall: ${window.userNumberFormat.format(deviationAmount)}`
                    : `Overspending: ${window.userNumberFormat.format(deviationAmount)}`;
            }
        }
    }

    async #initializeDatePicker(id, date) {
        let self = this;
        this.#monthPicker = await getDatePicker("#home-monthSelector", "month")
        this.#monthPicker.datepicker('setDate', date.toISOString());
        this.#monthPicker.on('changeDate', async function () {
            var date = self.#monthPicker.datepicker('getUTCDate')
            self.refresh(id, date);
        });

        $('.monthPicker .calendar-button').on('click', function () {
            let input = $(this).siblings('.monthSelector');
            if (!input.data('datepicker').picker.is(':visible')) {
                input.datepicker('show');
            } else {
                input.datepicker('hide');
            }
        });
    }

    async refresh(id, date) {        
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
                return false;
            }

            this.#isLoading = true;

            let data = await this.#getData(id, date);

            this.#renderData(data);            

            this.#data = data;
        } finally {
            this.#isLoading = false;
        }
    }   

    async #getData(id, date) {
        var data = await getFiscalPlanDataByMonth(id, date);
        return data;
    }

    rerenderDashboard() {
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
                return false;
            }
            
        } finally {
            this.#isLoading = false;
        }
    }

    #renderData(data) {
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

        this.#updateCategories(data.incomeCategories)
        this.#updateCategories(data.expenseCategories)

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
        if (!category) {
            return false;
        }

        let accordion = category.categoryType == 1 ? this.#incomeAccordionBody : this.#expenseAccordionBody;
        let categoryElement = accordion.querySelector(`#category_${category.id}`);

        if (!categoryElement) {
            this.addCategory(category);
            return true;
        }

        let categoryNameElement = categoryElement.querySelector(`#category_${category.id}_name`);
        categoryNameElement.textContent = decodeURIComponent(category.name);        

        let progressBarElement = categoryElement.querySelector(`#category_${category.id}_progressbar`)
        let balanceElement = categoryElement.querySelector(`#category_${category.id}_balance`)
        balanceElement.textContent = `${window.userNumberFormat.format(category.total)} / 
            ${window.userNumberFormat.format(category.budgetLimit?.budget ?? category.budget)}`;

        let deviationDiv = categoryElement.querySelector(`#category_${category.id}_deviation`)

        if (category.total > category.budget) {
            let deviationSpan;
            let deviationAmount = category.total - category.budget;
            if (!deviationDiv) {
                let categoryBodyDiv = categoryElement.querySelector('.category-body');
                deviationDiv = document.createElement('div');
                deviationDiv.className = 'me-2';
                deviationDiv.id = `category_${category.id}_deviation`;

                deviationSpan = document.createElement('span');
                deviationSpan.id = `category_${category.id}_deviationText`;
                deviationSpan.className = 'deviation-text';

                deviationDiv.appendChild(deviationSpan);
                categoryBodyDiv.insertBefore(deviationDiv, balanceElement.parentElement);
                balanceElement.parentElement.className = "";
            } else {
                deviationSpan = deviationDiv.querySelector(`#category_${category.id}_deviationText`);
            }
            deviationSpan.textContent = category.categoryType === 1
                ? `Windfall: ${window.userNumberFormat.format(deviationAmount)}`
                : `Overspending: ${window.userNumberFormat.format(deviationAmount)}`;

        } else if (deviationDiv) {
            deviationDiv.remove();
        }

        let progressBarELementPercentage = Math.floor(category.total * 100 / category.budget);
        let color = "bg-success";
        if (category.categoryType == 2) {
            color = progressBarELementPercentage < 50 ? "bg-success" : progressBarELementPercentage < 85 ? "bg-warning" : "bg-danger";
        }

        progressBarElement.className = `progress-bar progress-bar-striped progress-bar-animated ${color}`;
        progressBarElement.style.width = `${progressBarELementPercentage}%`;
        progressBarElement.ariaValuenow = `${progressBarELementPercentage}`;

        return true;
    }

    #createCategoryElements(data) {
        let fragment = document.createDocumentFragment();
        let orderedCategories = data.incomeCategories.sort(function (a, b) {
            return a.name.localeCompare(b.name);
        });

        for (var i = 0; i < orderedCategories.length; i++) {
            fragment.appendChild(this.#createCategoryElement(orderedCategories[i], data.id))
        }

        this.#incomeAccordionBody.textContent = "";
        this.#incomeAccordionBody.appendChild(fragment);

        fragment = document.createDocumentFragment();
        orderedCategories = data.expenseCategories.sort(function (a, b) {
            return a.name.localeCompare(b.name);
        });

        for (var i = 0; i < orderedCategories.length; i++) {
            fragment.appendChild(this.#createCategoryElement(orderedCategories[i], data.id))
        }

        this.#expenseAccordionBody.textContent = "";
        this.#expenseAccordionBody.appendChild(fragment);
        return true;
    }   

    #createCategoryElement(category) {
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
            let y = Math.max(Math.min(event.pageY - 100, window.innerHeight - 200), 66);
            menu.dataset.categoryid = `${category.id}`;
            menu.dataset.type = `${ category.categoryType }`;
            menu.style.left = `${mainDiv.style.left + event.pageX - 100}px`;
            menu.style.top = `${y}px`;
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
        categoryNameDiv.className = "me-auto"
        categoryNameDiv.textContent = decodeURIComponent(category.name);

        let budget = category.budgetLimit?.budget ?? category.budget;

        let categoryBalanceDiv = document.createElement('div');

        let categoryBalanceSpan = document.createElement('span');
        categoryBalanceSpan.id = `category_${category.id}_balance`;
        categoryBalanceSpan.className = 'balance-text';
        categoryBalanceSpan.textContent = `${window.userNumberFormat.format(category.total)} / 
            ${window.userNumberFormat.format(budget)}`;

        categoryBalanceDiv.appendChild(categoryBalanceSpan);
        categoryBodyDiv.appendChild(categoryNameDiv);

        if (category.total > category.budget) {
            let deviationAmount = category.total - category.budget;
            let deviationDiv = document.createElement('div');
            deviationDiv.className = 'me-2';
            deviationDiv.id = `category_${category.id}_deviation`;

            let deviationSpan = document.createElement('span');
            deviationSpan.id = `category_${category.id}_deviationText`;
            deviationSpan.className = 'deviation-text';
            deviationSpan.textContent = category.categoryType === 1
                ? `Windfall: ${window.userNumberFormat.format(deviationAmount)}`
                : `Overspending: ${window.userNumberFormat.format(deviationAmount)}`;

            deviationDiv.appendChild(deviationSpan);
            categoryBodyDiv.appendChild(deviationDiv);
        } 

        categoryBodyDiv.appendChild(categoryBalanceDiv);

        let progressDiv = document.createElement('div');
        progressDiv.className = 'progress';

        let progressBarDivPercentage = Math.floor(category.total * 100 / budget);
        let color = "bg-success";
        if (category.categoryType == 2) {
            color = progressBarDivPercentage < 50 ? "bg-success" : progressBarDivPercentage < 85 ? "bg-warning" : "bg-danger";
        }

        let progressBarDiv = document.createElement('div');
        progressBarDiv.id = `category_${category.id}_progressbar`;
        progressBarDiv.className = `progress-bar progress-bar-striped progress-bar-animated ${color}`;
        progressBarDiv.role = 'progressbar';
        progressBarDiv.style.width = `${progressBarDivPercentage}%`;
        progressBarDiv.ariaValuenow = `${progressBarDivPercentage}`;
        progressBarDiv.ariaValuemin = '0';
        progressBarDiv.ariaValuemax = '100';

        progressDiv.appendChild(progressBarDiv);

        contentDiv.appendChild(categoryBodyDiv);
        contentDiv.appendChild(progressDiv);

        let borderAnimationDiv = document.createElement('div');
        borderAnimationDiv.className = 'border-animation';

        borderContainerDiv.appendChild(contentDiv);
        borderContainerDiv.appendChild(borderAnimationDiv);

        mainDiv.appendChild(borderContainerDiv);

        return mainDiv;
    }

    addCategory(category) {        
        var categoryDTO =
        {
            id: category.id,
            name: category.name,
            budget: category.budget,            
            categoryType: category.categoryType,
            happyTotal: 0,
            necessaryTotal: 0,           
            total: 0
        }

        var categoryElement = this.#createCategoryElement(categoryDTO);

        var accordion = categoryDTO.categoryType == 1 ? this.#incomeAccordionBody : this.#expenseAccordionBody;
        var array = categoryDTO.categoryType == 1 ? this.#data.incomeCategories : this.#data.expenseCategories;
        var insertIndex = array.findIndex((object) => categoryDTO.name.localeCompare(object.name) < 0);

        if (insertIndex === -1) {
            array.push(categoryDTO);
            accordion.appendChild(categoryElement);
        } else {
            array.splice(insertIndex, 0, categoryDTO);      
            accordion.insertBefore(categoryElement, accordion.children[insertIndex]);
        }
    }

    removeCategory(id, type) {        
        var categoryElement = document.getElementById(`category_${id}`);
        categoryElement.remove();

        if (!this.#data) {
            return;
        }

        var array;        

        switch (type) {
            case 1:
                array = this.#data.incomeCategories;
                break;
            case 2:
                array = this.#data.expenseCategories;
                break;
        }

        var index = array.findIndex(item => item.id === id);

        if (index !== -1) {
            array.splice(index, 1)
        }
    }

    addTransaction(transaction) {
        var transactionDate = new Date(transaction.dateTime)
        var transactionYear = transactionDate.getYear();
        var transactionMonth = transactionDate.getMonth();
        var currentDate = new Date(this.#data.month);
        var currentYear = currentDate.getYear();
        var currentMonth = currentDate.getMonth();

        if (transactionYear === currentYear && transactionMonth == currentMonth) {
            var array = this.#data.incomeCategories.concat(this.#data.expenseCategories);
            let category = array.find((element) => element.id === transaction.categoryId);
            category.total += transaction.amount;
            this.#updateCategory(category);
        }        
    }
}