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
                        data: [this.#data.expensesHappyTotal, this.#data.expensesTotal - this.#data.expensesHappyTotal],
                        backgroundColor: [
                            'rgb(25,135,84)',
                            'rgb(220,53,69)'
                        ],
                        hoverOffset: 4
                    }]
                },
                options: {
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
                        data: [this.#data.expensesNecessaryTotal, this.#data.expensesTotal - this.#data.expensesNecessaryTotal],
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
                }
            });


            this.#overspendingHeading = document.getElementById('home-overspending');

            this.#incomeBalanceHeader = document.getElementById('incomeBalanceHeader');
            this.#incomeAccordionBody = document.getElementById('incomeAccordionBody');

            this.#expenseBalanceHeader = document.getElementById('expensesBalanceHeader');
            this.#expenseAccordionBody = document.getElementById('expensesAccordionBody');  
            
            this.#formatHeaders();   
            this.#formatCategories();

        } finally {
            this.#isLoading = false;
        }        
    }

    setupMenuHandlers() {
        var menu = this.#menu;
        var categories = this.#data.incomeCategories.concat(this.#data.expenseCategories);

        for (let i = 0; i < categories.length; i++) {
            let category = categories[i];
            let element = document.getElementById(`category_${category.id}`);

            if (element) {
                element.addEventListener("click", function (event) {
                    if (menu.dataset.categoryid != 0) {
                        let borderBox = document.getElementById(`category_${menu.dataset.categoryid}`).querySelector('.border-animation');
                        borderBox.classList.remove('border-rotate');
                    }
                    let y = Math.max(Math.min(event.pageY - 100, window.innerHeight - 200), 66);
                    menu.dataset.categoryid = category.id;                   
                    menu.style.left = `${element.style.left + event.pageX - 100}px`;
                    menu.style.top = `${y}px`;
                    menu.classList.add('active');

                    element.querySelector('.border-animation').classList.add('border-rotate');
                });
            }     
        }
    }

    async #initializeDatePicker(id, date) {
        var self = this;
        this.#monthPicker = await getDatePicker("#home-monthSelector", "month")
        this.#monthPicker.datepicker('setDate', date.toISOString());
        this.#monthPicker.on('changeDate', async function () {
            var date = self.#monthPicker.datepicker('getUTCDate')
            self.refresh(id, date);
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

    async refresh(id, date) {        
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
                return false;
            }

            this.#isLoading = true;

            var data = await this.#getData(id, date);

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

    formatDashboard(data) {
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
                return false;
            }
            this.#isLoading = true;

            this.#formatHeaders(data);
            this.#formatCategories(data); 
            this.#formatCharts(data);
            
        } finally {
            this.#isLoading = false;
        }
    }

    #formatCategories(data) {
        var dataObj = data ?? this.#data;

        if (dataObj == null) {
            return false;
        }

        var categoryElements = this.#data.incomeCategories.concat(this.#data.expenseCategories);

        for (let i = 0; i < categoryElements.length; i++) {
            this.#formatCategory(categoryElements[i]);
        }  
        return true;
    }

    #formatCharts(data) {
        var dataObj = data ?? this.#data;

        if (dataObj == null) {
            return false;
        }
        this.#sentimentChartMonthly.data.datasets[0].data = [dataObj.expensesHappyTotal, dataObj.expensesTotal - dataObj.expensesHappyTotal, Number.MIN_VALUE];
        this.#sentimentChartMonthly.update();

        this.#necessityChartMonthly.data.datasets[0].data = [dataObj.expensesNecessaryTotal, dataObj.expensesTotal - dataObj.expensesNecessaryTotal, Number.MIN_VALUE];
        this.#necessityChartMonthly.update();

        return true;
    }

    #formatHeaders(data) {
        var dataObj = data ?? this.#data;

        if (dataObj == null) {
            return false;
        }
       
        this.#overspendingHeading.textContent = `Overspending: ${window.userNumberFormat.format(dataObj.overspending)}`;
        this.#incomeBalanceHeader.textContent = `${window.userNumberFormat.format(dataObj.incomeTotal)} / ${window.userNumberFormat.format(dataObj.incomeBudget)}`;
        this.#expenseBalanceHeader.textContent = `${window.userNumberFormat.format(dataObj.expensesTotal)} / ${window.userNumberFormat.format(dataObj.expensesBudget)}`;

        return true;
    }

    #formatCategory(category) {
        if (!category) {
            return false;
        }
        var balanceElement = document.getElementById(`category_${category.id}_balance`);
        balanceElement.textContent = `${window.userNumberFormat.format(category.total)} / 
            ${window.userNumberFormat.format(category.budget)}`;

        if (category.total > category.budget) {
            let deviationAmount = category.total - category.budget;
            let deviationSpan = document.getElementById(`category_${category.id}_deviationText`);
            deviationSpan.textContent = category.type === 1
                ? `Windfall: ${window.userNumberFormat.format(deviationAmount)}`
                : `Overspending: ${window.userNumberFormat.format(deviationAmount)}`;
        }
        return true;
    }

    #renderData(data) {
        var dataObj = data ?? this.#data;       

        if (dataObj == null) {
            return false;
        }      

        this.#formatCharts(dataObj);
        this.#formatHeaders(dataObj);

        this.#updateCategories(dataObj.incomeCategories);
        this.#updateCategories(dataObj.expenseCategories);

        return true;
    }

    #updateCategories(categories) {
        if (categories.length == 0) {
            return false;
        }

        for (let i = 0; i < categories.length; i++) {
            this.#updateCategory(categories[i]);
        }
        return true;
    }

    #updateCategory(category) {
        if (!category) {
            return false;
        }
        var categoryElement = document.getElementById(`category_${category.id}`);

        if (!categoryElement) {
            this.addCategory(category);
            return true;
        }

        var categoryNameElement = document.getElementById(`category_${category.id}_name`);
        categoryNameElement.textContent = decodeURIComponent(category.name);        

        var budget = category.budgetLimit?.budget ?? category.budget;
        var progressBarElement = document.getElementById(`category_${category.id}_progressbar`)
        var balanceElement = document.getElementById(`category_${category.id}_balance`)
        balanceElement.textContent = `${window.userNumberFormat.format(category.total)} / 
            ${window.userNumberFormat.format(budget)}`;

        var deviationDiv = document.getElementById(`category_${category.id}_deviation`)

        if (category.total > budget) {
            let deviationSpan;
            let deviationAmount = category.total - budget;
            if (!deviationDiv) {
                let categoryBodyDiv = categoryElement.querySelector('.category-body');
                deviationDiv = document.createElement('div');
                deviationDiv.className = 'category-body-content deviation';
                deviationDiv.id = `category_${category.id}_deviation`;

                deviationSpan = document.createElement('span');
                deviationSpan.id = `category_${category.id}_deviationText`;
                deviationSpan.className = 'deviation-text';

                deviationDiv.appendChild(deviationSpan);
                categoryBodyDiv.insertBefore(deviationDiv, balanceElement.parentElement);
            } else {
                deviationSpan = deviationDiv.querySelector(`#category_${category.id}_deviationText`);
            }
            deviationSpan.textContent = category.categoryType === 1
                ? `Windfall: ${window.userNumberFormat.format(deviationAmount)}`
                : `Overspending: ${window.userNumberFormat.format(deviationAmount)}`;

        } else if (deviationDiv) {
            deviationDiv.remove();
        }

        var progressBarPercentage = Math.min(100, Math.floor(category.total * 100 / category.budget));
        var color = "bg-success";
        if (category.categoryType == 2) {
            color = progressBarPercentage < 50 ? "bg-success" : progressBarPercentage < 85 ? "bg-warning" : "bg-danger";
        }

        progressBarElement.className = `progress-bar progress-bar-striped ${color}`;
        progressBarElement.style.width = `${progressBarPercentage}%`;
        progressBarElement.ariaValuenow = `${progressBarPercentage}`;

        return true;
    }    

    #createCategoryElement(category) {
        var mainDiv = document.createElement('div');
        mainDiv.id = `category_${category.id}`;
        mainDiv.className = 'category';
        mainDiv.dataset.id = `${category.id}`;
        mainDiv.dataset.type = `${category.categoryType}`;
        mainDiv.dataset.name = `${category.name}`;
        mainDiv.dataset.budget = `${category.budget}`;
        mainDiv.dataset.fiscalplanid = `${category.fiscalPlanId}`;  

        var menu = this.#menu;
        mainDiv.addEventListener("click", function (event) {            
            if (menu.dataset.categoryid != 0) {
                var borderBox = document.getElementById(`category_${menu.dataset.categoryid}`).querySelector('.border-animation');
                borderBox.classList.remove('border-rotate');
            }
            var y = Math.max(Math.min(event.pageY - 100, window.innerHeight - 200), 66);
            menu.dataset.categoryid = `${category.id}`;
            menu.dataset.type = `${ category.categoryType }`;
            menu.style.left = `${mainDiv.style.left + event.pageX - 100}px`;
            menu.style.top = `${y}px`;
            menu.classList.add('active');

            this.querySelector('.border-animation').classList.add('border-rotate');
        });

        var borderContainerDiv = document.createElement('div');
        borderContainerDiv.className = 'border-container';

        var contentDiv = document.createElement('div');
        contentDiv.className = 'content';

        var categoryBodyDiv = document.createElement('div');
        categoryBodyDiv.className = 'category-body';

        var categoryNameDiv = document.createElement('div');
        categoryNameDiv.id = `category_${category.id}_name`;
        categoryNameDiv.className = "category-body-content";
        categoryNameDiv.textContent = decodeURIComponent(category.name);

        var budget = category.budgetLimit?.budget ?? category.budget;

        var categoryBalanceDiv = document.createElement('div');
        categoryBalanceDiv.className = "category-body-content balance";

        var categoryBalanceSpan = document.createElement('span');
        categoryBalanceSpan.id = `category_${category.id}_balance`;
        categoryBalanceSpan.className = 'balance-text';
        categoryBalanceSpan.textContent = `${window.userNumberFormat.format(category.total)} / 
            ${window.userNumberFormat.format(budget)}`;

        categoryBalanceDiv.appendChild(categoryBalanceSpan);
        categoryBodyDiv.appendChild(categoryNameDiv);

        if (category.total > category.budget) {
            let deviationAmount = category.total - category.budget;
            let deviationDiv = document.createElement('div');
            deviationDiv.className = 'category-body-content deviation';
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

        var progressDiv = document.createElement('div');
        progressDiv.className = 'progress';

        var progressBarDivPercentage = Math.floor(category.total * 100 / budget);
        var color = "bg-success";
        if (category.categoryType == 2) {
            color = progressBarDivPercentage < 50 ? "bg-success" : progressBarDivPercentage < 85 ? "bg-warning" : "bg-danger";
        }

        var progressBarDiv = document.createElement('div');
        progressBarDiv.id = `category_${category.id}_progressbar`;
        progressBarDiv.className = `progress-bar progress-bar-striped ${color}`;
        progressBarDiv.role = 'progressbar';
        progressBarDiv.style.width = `${progressBarDivPercentage}%`;
        progressBarDiv.ariaValuenow = `${progressBarDivPercentage}`;
        progressBarDiv.ariaValuemin = '0';
        progressBarDiv.ariaValuemax = '100';
        progressBarDiv.setAttribute('aria-labelledby', `category_${category.id}_balance`); 

        progressDiv.appendChild(progressBarDiv);

        contentDiv.appendChild(categoryBodyDiv);
        contentDiv.appendChild(progressDiv);

        var borderAnimationDiv = document.createElement('div');
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

    editCategory(formData, month) {        
        var type = parseInt(formData.get('type'));
        var id = parseInt(formData.get('Id'));
        var name = formData.get("Name");
        var budget = parseFloat(formData.get("Budget"));
        var array;

        switch (type) {
            case 1:
                array = this.#data.incomeCategories;
                break;
            case 2:
                array = this.#data.expenseCategories;
                break;
            default:
                return false;
        }

        var category = array.find(c => c.id === id);

        if (!category) {
            return false;
        }

        var oldBudget = category.budgetLimit?.budget ?? category.budget;

        if (category.name == name && oldBudget == budget) {
            return;
        }

        if (oldBudget != budget) {
            let overspending = Math.max(0, category.total - oldBudget);
            let newOverspending = Math.max(0, category.total - budget);
            let diff = newOverspending - overspending;
            this.#data.overspending += diff;
            debugger;
            switch (type) {
                case 1:
                    this.#data.incomeBudget += budget - oldBudget;
                    break;
                case 2:
                    this.#data.expensesBudget += budget - oldBudget;
                    break;
                default:
                    return false;
            }

            this.#formatHeaders();
            category.budgetLimit = { budget, month }
        }

        category.name = name;

        this.#updateCategory(category);
    }

    removeCategory(id, type) {        
        var categoryElement = document.getElementById(`category_${id}`);
        categoryElement.remove();

        if (!this.#data) {
            return false;
        }

        switch (type) {
            case 1:
                array = this.#data.incomeCategories;
                break;
            case 2:
                array = this.#data.expenseCategories;
                break;
            default:
                return false;
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
        var currentDate = this.getCurrentMonth();
        var currentYear = currentDate.getYear();
        var currentMonth = currentDate.getMonth();

        if (transactionYear === currentYear && transactionMonth == currentMonth) {
            let array = this.#data.incomeCategories.concat(this.#data.expenseCategories);
            let category = array.find((element) => element.id === transaction.categoryId);  
            let overspending = Math.max(0, category.total - category.budget);   
            
            category.total += transaction.amount;
            this.#data.overspending += Math.max(0, category.total - category.budget - overspending);
            if (category.categoryType == 2) {
                this.#data.expensesTotal += transaction.amount;

                if (transaction.isHappy) {
                    this.#data.expensesHappyTotal += transaction.amount;

                }
                if (transaction.isNecessary) {
                    this.#data.expensesNecessaryTotal += transaction.amount;
                }
            }
            else if (category.categoryType == 1) {
                this.#data.incomeTotal += transaction.amount;
            }
            
            this.#formatHeaders();
            this.#formatCharts();
            this.#updateCategory(category);
        }        
    }

    getCurrentMonth = () => this.#monthPicker.datepicker('getUTCDate');

    getCategory(id) {
        var categories = this.#data.incomeCategories.concat(this.#data.expenseCategories);
        var category = categories.find(c => c.id == id);
        return category;
    }
}