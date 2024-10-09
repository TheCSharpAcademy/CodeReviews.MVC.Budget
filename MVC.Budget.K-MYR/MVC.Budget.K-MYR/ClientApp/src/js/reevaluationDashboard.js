import { getUnevaluatedTransactions, patchTransactionEvaluation } from './api';
export default class ReevaluationDashboard {
    #data;
    #isLoading;
    #initPromise;
    #container;
    #infoDiv;

    constructor() {
        this.#data = null;   
        this.#initPromise = this.#init();
    }

    async #init() {
        try {
            this.#isLoading = true;
            this.#container = document.getElementById("reevalCategories-container");
            this.#infoDiv = document.getElementById("reevalInfo");
            this.#attachEventHandlers();  
            this.#toggleReevaluationInfo();
           
        } finally {
            this.#isLoading = false;
            this.formatDashboard();
        }
    }

    async #reevaluateTransaction(form) {
        var formData = new FormData(form);        
        var id = formData.get('Id');
        var element = document.getElementById(`reeval_transaction_${id}`);
        var previousIsHappy = element.dataset.ishappy === 'true';
        var previousIsNecessary = element.dataset.isnecessary === 'true';
        var IsPatched = await patchTransactionEvaluation(formData, previousIsHappy, previousIsNecessary);        

        if (IsPatched) {
            let accordionBody = form.closest(".accordion-body");
            let accordion = accordionBody.closest(".accordion");
            let categoryId = parseInt(accordion.dataset.categoryid);
            let spinner = document.getElementById(`spinner_${categoryId}`);

            element.removeEventListener("submit", this.#onReevaluate);
            element.remove();

            if (accordionBody.childElementCount < 5 && spinner) {
                let lastTransaction = accordionBody.children[accordionBody.childElementCount - 2];
                let lastDate = lastTransaction.dataset.date;
                let transactions = await getUnevaluatedTransactions(categoryId, lastDate, parseInt(lastTransaction.dataset.id), 6);

                if (transactions && transactions.length > 0) {
                    this.#createTransactionElements(transactions, accordionBody, spinner);
                } else {
                    spinner.remove();
                }
            }

            if (accordionBody.childElementCount === 0) {
                accordionBody.closest(".accordion").remove();
            }
            this.#toggleReevaluationInfo();
        }
    }

    #attachEventHandlers() {
        $(".reevaluate-transaction-form").on("submit", this.#onReevaluate.bind(this));
    }

    #onReevaluate(event) {
        event.preventDefault();
        this.#reevaluateTransaction(event.target);
    }

    formatDashboard() {
        try {
            if (this.#isLoading) {
                console.log("Dashboard is loading...")
                return false;
            }
            this.#isLoading = true;

            if (this.#data) {
                for (let i = 0; i < this.#data.length; i++) {
                    let category = this.#data[i];
                    for (let i = 0; i < category.transactions.length; i++) {
                        let transaction = category.transactions[i];
                        let element = document.getElementById(`transaction_date_${transaction.id}`);
                        element.textContent = new Date(transaction.dateTime).toLocaleDateString(window.userLocale);
                        element = document.getElementById(`transaction_amount_${transaction.id}`);
                        element.textContent = window.userNumberFormat.format(transaction.amount);
                    }
                }
            }
            else {
                let transactions = $(".transaction-body", this.#container);

                for (let i = 0; i < transactions.length; i++) {
                    let transaction = transactions[i];
                    let id = parseInt(transaction.dataset.id);
                    let date = new Date(transaction.dataset.date);
                    let amount = parseFloat(transaction.dataset.amount);
                    
                    let element = document.getElementById(`transaction_date_${id}`);
                    element.textContent = date.toLocaleDateString(window.userLocale);
                    element = document.getElementById(`transaction_amount_${id}`);
                    element.textContent = window.userNumberFormat.format(amount);                    
                }
            }

        } finally {
            this.#isLoading = false;
        }
    }

    #createTransactionElement(transaction) {
        var transactionBody = document.createElement('div');

        transactionBody.id = `reeval_transaction_${transaction.id}`;
        transactionBody.className = 'transaction-body';
        transactionBody.setAttribute('data-id', transaction.id);
        transactionBody.setAttribute('data-ishappy', transaction.isHappy);
        transactionBody.setAttribute('data-isnecessary', transaction.isNecessary);
        transactionBody.setAttribute('data-amount', transaction.amount);
        transactionBody.setAttribute('data-date', transaction.dateTime);

        var titleDiv = document.createElement('div');
        titleDiv.textContent = decodeURIComponent(transaction.title);

        var dateDiv = document.createElement('div');
        dateDiv.id = `transaction_date_${transaction.id}`;
        dateDiv.textContent = new Date(transaction.dateTime).toLocaleDateString(window.userLocale);

        var amountDiv = document.createElement('div');
        amountDiv.id = `transaction_amount_${transaction.id}`;
        amountDiv.textContent = window.userNumberFormat.format(transaction.amount);

        var transactionForm = document.createElement('form');
        transactionForm.id = `reevaluate-transaction-form_${transaction.id}`;
        transactionForm.setAttribute('novalidate', 'novalidate');
        transactionForm.addEventListener("submit", this.#onReevaluate.bind(this));

        var hiddenInput = document.createElement('input');
        hiddenInput.type = 'hidden';
        hiddenInput.value = transaction.id;
        hiddenInput.id = `reeval_transaction_${transaction.id}`;
        hiddenInput.name = 'Id';

        var wrapperDiv = document.createElement('div');
        wrapperDiv.className = 'reevalIconsContainer';

        var innerWrapper1 = document.createElement('div');
        innerWrapper1.className = 'd-flex align-items-center';

        var isHappyTrueInput = document.createElement('input');
        isHappyTrueInput.type = 'radio';
        isHappyTrueInput.value = 'true';
        isHappyTrueInput.id = `isHappyTrue_${transaction.id}`;
        isHappyTrueInput.className = 'iconRadioButton';
        isHappyTrueInput.name = 'IsHappy';

        var isHappyTrueLabel = document.createElement('label');
        isHappyTrueLabel.className = 'reevalIconLabel';
        isHappyTrueLabel.htmlFor = `isHappyTrue_${transaction.id}`;

        var isHappySvgContainer = document.createElement('div');
        isHappySvgContainer.className = 'reevalIconContainer';

        var isHappyImg = document.createElement('img');
        isHappyImg.src = "/dist/img/happy-emote.svg";
        isHappyImg.height = 30;
        isHappyImg.width = 30;
        isHappyImg.className = "reevalIcon";

        var isHappyFalseInput = document.createElement('input');
        isHappyFalseInput.type = 'radio';
        isHappyFalseInput.value = 'false';
        isHappyFalseInput.id = `isHappyFalse_${transaction.id}`;
        isHappyFalseInput.className = 'iconRadioButton';
        isHappyFalseInput.checked = true;
        isHappyFalseInput.name = 'IsHappy';

        var isHappyFalseLabel = document.createElement('label');
        isHappyFalseLabel.className = 'reevalIconLabel';
        isHappyFalseLabel.htmlFor = `isHappyFalse_${transaction.id}`;

        var isUnhappySvgContainer = document.createElement('div');
        isUnhappySvgContainer.className = 'reevalIconContainer';

        var isUnhappyImg = document.createElement('img');
        isUnhappyImg.src = "/dist/img/sad-emote.svg";
        isUnhappyImg.height = 30;
        isUnhappyImg.width = 30;
        isUnhappyImg.className = "reevalIcon";

        var innerWrapper2 = document.createElement('div');
        innerWrapper2.className = 'd-flex align-items-center';

        var isNecessaryTrueInput = document.createElement('input');
        isNecessaryTrueInput.type = 'radio';
        isNecessaryTrueInput.value = 'true';
        isNecessaryTrueInput.id = `isNecessaryTrue_${transaction.id}`;
        isNecessaryTrueInput.className = 'iconRadioButton';
        isNecessaryTrueInput.name = 'IsNecessary';

        var isNecessaryTrueLabel = document.createElement('label');
        isNecessaryTrueLabel.className = 'reevalIconLabel';
        isNecessaryTrueLabel.htmlFor = `isNecessaryTrue_${transaction.id}`;

        var isNecessarySvgContainer = document.createElement('div');
        isNecessarySvgContainer.className = 'reevalIconContainer';

        var isNecessaryImg = document.createElement('img');
        isNecessaryImg.src = "/dist/img/chart-growth.svg";
        isNecessaryImg.height = 30;
        isNecessaryImg.width = 30;
        isNecessaryImg.className = "reevalIconNecessity";

        var isNecessaryFalseInput = document.createElement('input');
        isNecessaryFalseInput.type = 'radio';
        isNecessaryFalseInput.value = 'false';
        isNecessaryFalseInput.checked = true;
        isNecessaryFalseInput.id = `isNecessaryFalse_${transaction.id}`;
        isNecessaryFalseInput.className = 'iconRadioButton';
        isNecessaryFalseInput.name = 'IsNecessary';

        var isNecessaryFalseLabel = document.createElement('label');
        isNecessaryFalseLabel.className = 'reevalIconLabel';
        isNecessaryFalseLabel.htmlFor = `isNecessaryFalse_${transaction.id}`;

        var isUnnecessarySvgContainer = document.createElement('div');
        isUnnecessarySvgContainer.className = 'reevalIconContainer';

        var isUnnecessaryImg = document.createElement('img');
        isUnnecessaryImg.src = "/dist/img/chart-decrease.svg";
        isUnnecessaryImg.height = 30;
        isUnnecessaryImg.width = 30;
        isUnnecessaryImg.className = "reevalIconNecessity";

        var buttonDiv = document.createElement('div');

        var submitButton = document.createElement('button');
        submitButton.className = 'reeval-submit-button';
        submitButton.type = 'submit';

        var submitButtonSvg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        submitButtonSvg.setAttribute('class', 'reeval-submit-svg');
        submitButtonSvg.setAttribute('viewBox', '0 0 24 24');
        submitButtonSvg.setAttribute('height', '40');
        submitButtonSvg.setAttribute('width', '40');
        submitButtonSvg.innerHTML = `<circle cx="12" cy="12" r="11.5" stroke-width="1"></circle>
                            <path d="M8.5 12.5L10.5 14.5L15.5 9.5" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>`;

        isHappySvgContainer.appendChild(isHappyImg);
        isHappyTrueLabel.appendChild(isHappySvgContainer);

        isUnhappySvgContainer.appendChild(isUnhappyImg);
        isHappyFalseLabel.appendChild(isUnhappySvgContainer);

        innerWrapper1.appendChild(isHappyTrueInput);
        innerWrapper1.appendChild(isHappyTrueLabel);
        innerWrapper1.appendChild(isHappyFalseInput);
        innerWrapper1.appendChild(isHappyFalseLabel);

        isNecessarySvgContainer.appendChild(isNecessaryImg);
        isNecessaryTrueLabel.appendChild(isNecessarySvgContainer);

        isUnnecessarySvgContainer.appendChild(isUnnecessaryImg);
        isNecessaryFalseLabel.appendChild(isUnnecessarySvgContainer);

        innerWrapper2.appendChild(isNecessaryTrueInput);
        innerWrapper2.appendChild(isNecessaryTrueLabel);
        innerWrapper2.appendChild(isNecessaryFalseInput);
        innerWrapper2.appendChild(isNecessaryFalseLabel);

        submitButton.appendChild(submitButtonSvg);
        buttonDiv.appendChild(submitButton);

        wrapperDiv.appendChild(innerWrapper1);
        wrapperDiv.appendChild(innerWrapper2);
        wrapperDiv.appendChild(buttonDiv);

        transactionForm.appendChild(hiddenInput);
        transactionForm.appendChild(wrapperDiv);

        transactionBody.appendChild(titleDiv);
        transactionBody.appendChild(dateDiv);
        transactionBody.appendChild(amountDiv);
        transactionBody.appendChild(transactionForm);

        return transactionBody;
    }   

    #createTransactionElements(data, accordion, beforeElement) { 
        if (data && accordion) {
            let frag = document.createDocumentFragment();

            for (let i = 0; i < data.length; i++) {
                frag.appendChild(this.#createTransactionElement(data[i]));

                if (beforeElement) {
                    accordion.insertBefore(frag, beforeElement);
                }
                else {
                    accordion.appendChild(frag);
                }
            }
        }
    }

    #toggleReevaluationInfo() {
        if (this.#container.childElementCount == 0) {
            this.#infoDiv.style.display = 'block';
        } else {
            this.#infoDiv.style.display = 'none';
        }
    }
}
