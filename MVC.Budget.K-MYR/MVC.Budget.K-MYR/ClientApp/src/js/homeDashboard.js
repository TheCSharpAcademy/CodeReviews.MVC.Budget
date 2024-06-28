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


    constructor(menu, id, date, apiUrl) {
        this.#data = null;
        this.#menu = menu;      
        this.#initPromise = this.#init(id, date);
    }

    async #init(id, date) {
        try {
            this.#isLoading = true;
            this.#initializeDatePicker(id, date);
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

            let data = await this.#getData(id, date);
            this.#data = data;            

            if (data) {                
                let hasUpdated = this.#renderData(data);
                let hasCreated = this.#createCategoryElements(data);        
            }

            $('.monthPicker .calendar-button').on('click', function () {
                let input = $(this).siblings('.monthSelector');
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

    async #initializeDatePicker(id, date) {
        let self = this;
        this.#monthPicker = await getDatePicker("#home-monthSelector", "month")
        this.#monthPicker.datepicker('setDate', date.toISOString());
        this.#monthPicker.on('changeDate', async function () {
            self.refresh(id, self.#monthPicker.datepicker('getUTCDate'))
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
            this.#updateCategories(data.incomeCategories)
            this.#updateCategories(data.expenseCategories)
        } finally {
            this.#isLoading = false;
        }
    }   

    async #getData(id, date) {
        var categories = await getFiscalPlanDataByMonth(id, date);
        return categories;
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
            console.log(`Category element with id ${category.id} not found`);
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

        var array;
        var accordion;

        switch (categoryDTO.categoryType) {
            case 1:
                array = this.#data.incomeCategories;
                accordion = this.#incomeAccordionBody
                break;
            case 2:
                array = this.#data.expenseCategories;
                accordion = this.#expenseAccordionBody;
                break;            
        }

        var insertIndex = array.findIndex((object) => object.name.localeCompare(categoryDTO.name) > 0);

        if (insertIndex === -1) {
            insertIndex = 0;
        }

        array.splice(insertIndex, 0, categoryDTO);      
        
        var categoryElement = this.#createCategoryELement(categoryDTO);
        accordion.insertBefore(categoryElement, accordion.children[Math.max(0, insertIndex - 1)])
    }

    removeCategory(id, type) {        
        var categoryElement = document.getElementById(`category_${id}`);
        categoryElement.remove();

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
}