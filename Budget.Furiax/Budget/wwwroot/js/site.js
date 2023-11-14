const uriTransaction = 'api/TransactionModels';
let transactions = [];

function getTransactions() {
    fetch(uriTransaction)
        .then(response => response.json())
        .then(data => displayTransactions(data))
        .catch(error => console.error('Unable to get items.', error));
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
        td4.appendChild(textNodeAmount):

    });

    transactionList = data;
}
function formatDate(date) {
    const options = { day: 'numeric', month: 'short', year: 'numeric' };
    return date.toLocaleDateString('nl-BE', options);
}
function formatAmount(decimalAmount) {
    return decimalAmount.toFixed(2);
}