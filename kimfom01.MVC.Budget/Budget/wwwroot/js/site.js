"use strict";

const populateModal = (transaction, action) => {
    const id = document.getElementById(`${action}TransactionId`);
    const name = document.getElementById(`${action}TransactionName`);
    const description = document.getElementById(`${action}TransactionDescription`);
    const cost = document.getElementById(`${action}TransactionCost`);
    const month = document.getElementById(`${action}TransactionMonth`);
    const categoryId = document.getElementById(`${action}TransactionCategoryId`);
    const walletId = document.getElementById(`${action}TransactionWalletId`);

    id.value = transaction.id;
    name.value = transaction.name;
    description.value = transaction.description;
    cost.value = transaction.cost;
    month.value = transaction.month;
    categoryId.value = transaction.categoryId;
    walletId.value = transaction.walletId;
}

const months = {
    "January": 1,
    "February": 2,
    "March": 3,
    "April": 4,
    "May": 5,
    "June": 6,
    "July": 7,
    "August": 8,
    "September": 9,
    "October": 10,
    "November": 11,
    "December": 12
}

const extractTransaction = (e) => {
    e.preventDefault();

    const row = e.target.closest("tr");

    const transactionRow = row.children;

    const transaction = {
        id: transactionRow[0].innerText,
        name: transactionRow[1].innerText,
        description: transactionRow[2].innerText,
        cost: transactionRow[5].innerText,
        month: months[transactionRow[6].innerText],
        categoryId: transactionRow[7].innerText,
        walletId: transactionRow[9].innerText,
    };

    return transaction;
}

const editButtons = document.querySelectorAll("#editButtons");

editButtons.forEach(btn => btn.addEventListener("click", (e) => {

    const transaction = extractTransaction(e);

    populateModal(transaction, "update");
}));

const deleteButtons = document.querySelectorAll("#deleteButtons");

deleteButtons.forEach(btn => btn.addEventListener("click", (e) => {
    const transaction = extractTransaction(e);

    populateModal(transaction, "delete");
}))

const viewButtons = document.querySelectorAll("#viewButtons");

viewButtons.forEach(btn => btn.addEventListener("click", (e) => {

    const transaction = extractTransaction(e);

    populateModal(transaction, "view");
}));