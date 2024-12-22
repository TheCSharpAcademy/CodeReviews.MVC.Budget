
var categories = document.getElementById("categories");
var transactions = document.getElementById("transactions");
function showCategories() {
    categories.style.display = "block";
    if (transactions.style.display === "block") {
        transactions.style.display = "none";
    }
}

function hideCategories() {
    categories.style.display = "none";
}

function showTransactions() {
    transactions.style.display = "block";
    if (categories.style.display === "block") {
        categories.style.display = "none";
    }
}

function hideTransactions() {
    transactions.style.display = "none";
}
function showModal(title, formContent) {
    // Sæt titel og dynamisk indhold i modal
    document.getElementById("modal-title").textContent = title;
    document.getElementById("modal-form").innerHTML = formContent;

    // Vis modal
    document.getElementById("modal").classList.remove("hidden");
}

function hideModal() {
    document.getElementById("modal").classList.add("hidden");
}

function submitForm(event) {
    event.preventDefault(); // Forhindrer standard handling

    // Hent data fra formularen
    const formData = new FormData(event.target);

    // Behandl CRUD-operation baseret på kontekst (eksempel: gem data)
    console.log(Object.fromEntries(formData.entries()));

    // Skjul modal efter submission
    hideModal();
}

// Eksempel på at åbne modalen
function showCategoriesModal() {
    showModal("Add/Edit Category", `
        <label for="categoryName">Category Name:</label>
        <input type="text" id="categoryName" name="categoryName" required>
        <button type="submit">Save</button>
    `);
}

function showTransactionsModal() {
    showModal("Add/Edit Transaction", `
        <label for="transactionName">Transaction Name:</label>
        <input type="text" id="transactionName" name="transactionName" required>
        
        <label for="transactionDate">Date:</label>
        <input type="date" id="transactionDate" name="transactionDate" required>
        
        <label for="transactionAmount">Amount:</label>
        <input type="number" id="transactionAmount" name="transactionAmount" required>
        
        <label for="transactionCategory">Category:</label>
        <select id="transactionCategory" name="transactionCategory">
            <option value="Food">Food</option>
            <option value="Transport">Transport</option>
        </select>
        
        <button type="submit">Save</button>
    `);
}

