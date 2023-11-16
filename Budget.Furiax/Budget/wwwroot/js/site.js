const uriTransaction = 'api/TransactionModels';
const uriCategory = 'api/Category';
let transactions = [];

function getTransactions() {
    fetch(uriTransaction)
        .then(response => response.json())
        .then(data => displayTransactions(data))
        .catch(error => console.error('Unable to get items.', error));
}
function addTransaction() {
    const addDate = document.getElementById('add-date');
    const addSource = document.getElementById('add-source');
    const addAmount = document.getElementById('add-amount');
    const addCategoryId = document.getElementById('add-categoryId');
    const item = {
        TransactionDate: addDate.value,
        TransactionSource: addSource.value.trim(),
        TransactionAmount: parseFloat(addAmount.value),
        CategoryId: parseInt(addCategoryId.value),
    };

    fetch(uriTransaction, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getTransactions();
            addDate.value = '';
            addSource.value = '';
            addAmount.value = '';
            addCategoryId.value = '';
        })
        .catch(error => console.error('Unable to add item.'));
}
function addCategory() {
    const addCategory = document.getElementById('add-category');
    const item = { CategoryName: addCategory.value.trim() }
    fetch(uriCategory, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            addCategory.value = '';
        })
        .catch(error => console.error('Unabale to add category.'));
}
function displayTransactions(data) {
    const tList = document.getElementById('transactionList');
    tList.innerHTML = '';

    data.forEach(item => {

        let tr = tList.insertRow();

        let td1 = tr.insertCell(0);
        let textNodeId = document.createTextNode(item.TransactionId);
        td1.appendChild(textNodeId);

        let td2 = tr.insertCell(1);
        let date = new Date(item.TransactionDate);
        let dateString = formatDate(date);
        let textNodeDate = document.createTextNode(dateString);
        td2.appendChild(textNodeDate);

        let td3 = tr.insertCell(2);
        let textNodeSource = document.createTextNode(item.TransactionSource);
        td3.appendChild(textNodeSource);

        let td4 = tr.insertCell(3);
        let decimalAmount = item.TransactionAmount;
        let formattedAmount = formatAmount(decimalAmount);
        let textNodeAmount = document.createTextNode(formattedAmount);
        td4.appendChild(textNodeAmount);

    });

    transactions = data;
}
function formatDate(date) {
    const options = { day: 'numeric', month: 'short', year: 'numeric' };
    return date.toLocaleDateString('nl-BE', options);
}
function formatAmount(decimalAmount) {
    return decimalAmount.toFixed(2);
}