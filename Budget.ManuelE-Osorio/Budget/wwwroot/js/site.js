document.getElementById("nav-link-transactions").addEventListener('click', event => SwitchToTransactions(event))
document.getElementById("nav-link-categories").addEventListener('click', event => SwitchToCategories(event))
document.getElementById('transaction-modal').querySelector('form').addEventListener('submit', event => transactionFetch(event))
document.getElementById('category-modal').querySelector('form').addEventListener('submit', event => categoryFetch(event))
document.getElementById('delete-modal').querySelector('form').addEventListener('submit', event => deleteFetch(event))

function SwitchToTransactions(event){
    location.reload() 
}

function SwitchToCategories(event){
    const transactions = document.getElementById("nav-link-transactions")
    const transactionsTable = document.getElementById("transactions-container")
    const categories = document.getElementById("nav-link-categories")
    const categoriesTable = document.getElementById("categories-container")
    const attrib = categories.getAttribute('class')
    if (!attrib.includes('active'))
    {
        categories.setAttribute('class', 'nav-link active')
        transactions.setAttribute('class', 'nav-link')
        transactionsTable.hidden = true
        categoriesTable.hidden = false
    }
}

function transactionFetch(event){
    event.preventDefault()
    if($("#transaction-form").valid()){
        const formData = new FormData(event.target)
        const transaction = Object.fromEntries(formData);
        if(transaction.Id == 0)
        {
            createTransaction(transaction)
        }
        else
        {
            updateTransaction(transaction)
        }
    }
}

function categoryFetch(event){
    event.preventDefault()
    if($("#category-form").valid()){
        const formData = new FormData(event.target)
        const category = Object.fromEntries(formData);
        if(category.Id == 0)
        {
            createCategory(category)
        }
        else
        {
            updateCategory(category)
        }
    }
}

function deleteFetch(event){
    event.preventDefault()
    if($("#delete-form").valid()){
        const formData = new FormData(event.target)
        const deleteObject = Object.fromEntries(formData);
        if(deleteObject.type == 'transaction'){
            deleteTransaction(deleteObject)
        }
        else if(deleteObject.type == 'category'){
            deleteCategory(deleteObject)
        }
    }
}

function createTransaction(transaction){
    const apiAddress = `Budget/Transactions/Create/`  
    fetch(apiAddress,
    {
        method: 'POST',
        headers: {
            'Accept' : '*/*' ,
            'Content-Type' : 'application/json',
            'RequestVerificationToken': `${transaction.__RequestVerificationToken}`},
        body: JSON.stringify(transaction)
    })
    .then( response => {
        if(response.status == 201){
            return response.json()
        }
        else if (response.status == 400)
        {
            throw new Error('Bad request, please check the form values.')
        }
        else{
            throw new Error('Server error, please try again later.')
        }
    })
    .then( body => {
        insertTransaction(body)
        const element = document.getElementById('transaction-modal')
        const modal = bootstrap.Modal.getOrCreateInstance(element)
        modal.hide()
    })
    .catch( e => {
        window.alert(e)
        console.log('Catch', e)
    })
}

function updateTransaction(transaction)
{
    const apiAddress = `Budget/Transactions/Update/${transaction.Id}`  
    fetch(apiAddress,
    {
        method: 'PUT',
        headers: {
            'Accept' : '*/*' ,
            'Content-Type' : 'application/json',
            'RequestVerificationToken': `${transaction.__RequestVerificationToken}`},
        body: JSON.stringify(transaction)
    })
    .then( response => {
        if(response.status == 200){
            return response.json()
        }
        else if (response.status == 400)
        {
            throw new Error('Bad request, please check the form values.')
        }
        else{
            throw new Error('Server error, please try again later.')
        }
    })
    .then( body => {
        insertTransaction(body)
        const element = document.getElementById('transaction-modal')
        const modal = bootstrap.Modal.getOrCreateInstance(element)
        modal.hide()
    })
    .catch( e => {
        window.alert(e)
        console.log('Catch', e)
    })
}

function deleteTransaction(transaction)
{
    const apiAddress = `Budget/Transactions/Delete/${transaction.id}`
    fetch(apiAddress,
    {
        method: 'DELETE',
        headers: {
            'Accept' : '*/*' ,
            'Content-Type' : 'application/json',
            'RequestVerificationToken': `${transaction.__RequestVerificationToken}`}
    })
    .then( response => {
        if(response.status == 200){
            return
        }
        else if (response.status == 404)
        {
            throw new Error('Bad request, the element does not exist.')
        }
        else{
            throw new Error('Server error, please try again later.')
        }
    })
    .then( () => {
        document.getElementById(`transaction-${transaction.id}`).remove()
        const element = document.getElementById('delete-modal')
        const modal = bootstrap.Modal.getOrCreateInstance(element)
        modal.hide()
    })
    .catch( e => {
        window.alert(e)
        console.log('Catch', e)
    })
}

function createCategory(category){
    const apiAddress = `Budget/Categories/Create/`  
    fetch(apiAddress,
    {
        method: 'POST',
        headers: {
            'Accept' : '*/*' ,
            'Content-Type' : 'application/json',
            'RequestVerificationToken': `${category.__RequestVerificationToken}`},
        body: JSON.stringify(category)
    })
    .then( response => {
        if(response.status == 201){
            return response.json()
        }
        else if (response.status == 400)
        {
            throw new Error('Bad request, please check the form values.')
        }
        else{
            throw new Error('Server error, please try again later.')
        }
    })
    .then( body => {
        insertCategory(body)
        const element = document.getElementById('category-modal')
        const modal = bootstrap.Modal.getOrCreateInstance(element)
        modal.hide()
    })
    .catch( e => {
        window.alert(e)
        console.log('Catch', e)
    })    
}

function updateCategory(category){
    const apiAddress = `Budget/Categories/Update/${category.Id}`  
    fetch(apiAddress,
    {
        method: 'PUT',
        headers: {
            'Accept' : '*/*' ,
            'Content-Type' : 'application/json',
            'RequestVerificationToken': `${category.__RequestVerificationToken}`},
        body: JSON.stringify(category)
    })
    .then( response => {
        if(response.status == 200){
            return response.json()
        }
        else if (response.status == 400)
        {
            throw new Error('Bad request, please check the form values.')
        }
        else{
            throw new Error('Server error, please try again later.')
        }
    })
    .then( body => {
        insertCategory(body)
        const element = document.getElementById('category-modal')
        const modal = bootstrap.Modal.getOrCreateInstance(element)
        modal.hide()
    })
    .catch( e => {
        window.alert(e)
        console.log('Catch', e)
    })
}

function deleteCategory(category){
    const apiAddress = `Budget/Categories/Delete/${category.id}`
    fetch(apiAddress,
    {
        method: 'DELETE',
        headers: {
            'Accept' : '*/*' ,
            'Content-Type' : 'application/json',
            'RequestVerificationToken': `${category.__RequestVerificationToken}`},
    })
    .then( response => {
        if(response.status == 200){
            return
        }
        else if (response.status == 404)
        {
            throw new Error('Bad request, the element does not exist.')
        }
        else{
            throw new Error('Server error, please try again later.')
        }
    })
    .then( () => {
        document.getElementById(`category-${category.id}`).remove()
        const element = document.getElementById('delete-modal')
        const modal = bootstrap.Modal.getOrCreateInstance(element)
        modal.hide()
    })
    .catch( e => {
        window.alert(e)
        console.log('Catch', e)
    })    
}

function insertTransaction(transaction){
    let data = document.getElementById(`transaction-${transaction.id}`)
    if (data == null)
    {
        data = document.getElementById("transaction-template").cloneNode(true)
        data.hidden = false
        data.setAttribute('id', `transaction-${transaction.id}`)
        buttons = data.getElementsByTagName('button')
        buttons[0].setAttribute('onclick', `transactionModal(${transaction.id})`)
        buttons[1].setAttribute('onclick', `deleteModal(${transaction.id}, 'transaction')`)
        document.getElementById('transactions-table-body').appendChild(data)
    }

    const values = data.getElementsByTagName('td')
    const formatter = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
    });
    values[0].innerText = transaction.name
    values[1].innerText = transaction.description
    values[2].innerText = new Date(Date.parse(transaction.date)).toLocaleString()
    values[3].innerText = formatter.format(transaction.amount)
    values[4].innerText = transaction.category.name
}

function insertCategory(category){
    let data = document.getElementById(`category-${category.id}`)
    if (data == null)
    {
        data = document.getElementById('category-template').cloneNode(true)
        data.hidden = false
        data.setAttribute('id', `category-${category.id}`)
        buttons = data.getElementsByTagName('button')
        buttons[0].setAttribute('onclick', `categoryModal(${category.id})`)
        buttons[1].setAttribute('onclick', `deleteModal(${category.id}, 'category')`)
        document.getElementById('categories-table-body').appendChild(data)
    }
    const values = data.getElementsByTagName('td')
    values[0].innerText = category.name
}

function transactionModal(id){
    const element = document.getElementById('transaction-modal')
    const modal = bootstrap.Modal.getOrCreateInstance(element)
    const input = element.getElementsByTagName('input')
    const category = element.querySelector('#Category')

    input[0].value = id

    if(id == 0){
        element.querySelector('#modalLabel').innerText = 'Create Transaction'
        element.querySelector('#inputButton').innerText = 'Create'
        input[1].value = ''
        input[2].value = ''
        input[3].valueAsDate = null
        input[5].value = ''
        category.value = ''
    }
    else{
        element.querySelector('#modalLabel').innerText = 'Edit Category'
        element.querySelector('#inputButton').innerText = 'Update'
        const data = document.getElementById(`transaction-${id}`)
        const values = data.getElementsByTagName('td')
        input[1].value = values[0].innerText
        input[2].value = values[1].innerText
        input[3].value = new Date(Date.parse(values[2].innerText)).toISOString().slice(0, 16)
        input[5].value = values[3].innerText.replace("$", "")
        category.value = values[4].innerText
    }

    modal.show()
}

function categoryModal(id){
    const element = document.getElementById('category-modal')
    const modal = bootstrap.Modal.getOrCreateInstance(element)
    const input = element.getElementsByTagName('input')

    input[0].value = id

    if(id == 0){
        element.querySelector('#modalLabel').innerText = 'Create Category'
        element.querySelector('#inputButton').innerText = 'Create'
        input[1].value = ''
    }
    else{
        element.querySelector('#modalLabel').innerText = 'Edit Category'
        element.querySelector('#inputButton').innerText = 'Update'
        const data = document.getElementById(`category-${id}`)
        const values = data.getElementsByTagName('td')
        input[1].value = values[0].innerText
    }
    modal.show()
}

function deleteModal(id, type){
    const element = document.getElementById('delete-modal')
    const modal = bootstrap.Modal.getOrCreateInstance(element)
    const deleteId = element.querySelector('#delete-id')
    const deleteType = element.querySelector('#delete-type')
    deleteId.value = id
    deleteType.value = type
    const text = element.querySelector('#delete-modal-text')
    text.firstChild.textContent = `Are you sure you want to delete the ${type}?`
    modal.show();
}
