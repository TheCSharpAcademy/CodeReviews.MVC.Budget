const incomeCategoriesAPI = "https://localhost:7246/api/IncomeCategories";
const expenseCategoriesAPI = "https://localhost:7246/api/ExpenseCategories";
const transactionsAPI = "https://localhost:7246/api/Transactions";
const fiscalPlanAPI = "https://localhost:7246/api/FiscalPlan";
const homeController = "https://localhost:7246/Country";

export async function getCountryCookie(countryISOCode, token) {
    try {
        var response = await fetch(homeController, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            },
            body: JSON.stringify(countryISOCode)
        });

        if (response.ok) {
            return await response.json();
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
            return null;
        }
    } catch (error) {
        console.error(error);
        return null;
    }    
}
export async function getFiscalPlanDataByMonth(id, date) {
    try {
        var response = await fetch(`${fiscalPlanAPI}/${id}/MonthlyData?Month=${date.toISOString() ?? new Date().toISOString()}`, {
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

export async function getFiscalPlanDataByYear(id, year) {
    try {
        var response = await fetch(`${fiscalPlanAPI}/${id}/${year.getFullYear()}`, {
            method: "GET"
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

export async function postTransaction(formData) {
    try {
        var response = await fetch(`${transactionsAPI}`, {
           method: "POST",
           headers: {
               "Content-Type": "application/json",
               "RequestVerificationToken": formData.get('__RequestVerificationToken')
           },
           body: JSON.stringify({
               Title: formData.get("Title"),
               Amount: parseFloat(formData.get("Amount")),
               DateTime: formData.get("DateTime"),
               IsHappy: formData.get("IsHappy") === "true" ? true : false,
               IsNecessary: formData.get("IsNecessary") === "true" ? true : false,
               CategoryId: parseInt(formData.get("CategoryId"))
           })
        });

        if (response.ok) {
            return await response.json();
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
            return null;
        }

    } catch (error) {
        console.error(error);
        return null;
    }
}

export async function getTransactions(formData) {
    try {
        var params = new URLSearchParams();

        for (let [key, value] of formData.entries()) {
            if (value !== undefined && value !== '') {
                params.append(key, value);
            }
        }

        var query_string = params.toString();

        var response = await fetch(`${transactionsAPI}?${query_string}`, {
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

export async function patchTransactionEvaluation(formData, previousIsHappy, previousIsNecessary) {
    try {
        var id = parseInt(formData.get("Id"));

        var patchDoc =
            [{
                op: "replace",
                path: "/IsHappy",
                value: formData.get("IsHappy") === "true"
            },
            {
                op: "replace",
                path: "/IsNecessary",
                value: formData.get("IsNecessary") === "true"
            }, {
                op: "replace",
                path: "/PreviousIsHappy",
                value: previousIsHappy
            },
            {
                op: "replace",
                path: "/PreviousIsNecessary",
                value: previousIsNecessary
            },
            {
                op: "replace",
                path: "/Evaluated",
                value: true
            }];

        var response = await fetch(`${transactionsAPI}/${id}`, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json-patch+json"
            },
            body: JSON.stringify(patchDoc)
        });

        if (response.ok) {
            return true;
        } else {
            console.error(`HTTP Patch Error: ${response.status}`);
            return false;
        }

    } catch (error) {
        console.error(error);
        return false;
    };
}

export async function postCategory(formData) {
    try {
        var response = await fetch(`${formData.get("type") == 1 ? incomeCategoriesAPI : expenseCategoriesAPI}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": formData.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: formData.get("Name"),
                Budget: parseFloat(formData.get("Budget")),
                FiscalPlanId: parseInt(formData.get("FiscalPlanId"))
            })
        });

        if (response.ok) {
            return await response.json();
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
            return null;
        }

    } catch (error) {
        console.error(error);
        return null;
    }
}

export async function putCategory(formData, month) {
    try {
        var id = parseInt(formData.get("Id"));
        let queryParams = new URLSearchParams({
            Month: month
        });        

        var response = await fetch(`${formData.get("type") == 1 ? incomeCategoriesAPI : expenseCategoriesAPI}/${id}?${queryParams}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": formData.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: formData.get("Name"),
                Budget: parseFloat(formData.get("Budget")),
                GroupId: parseInt(formData.get("GroupId")),
                Id: id,
                FiscalPlanId: parseInt(formData.get("FiscalPlanId"))
            })
        });

        if (response.ok) {
            return true;
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
            return false;
        }

    } catch (error) {
        console.error(error);
        return false;
    }
}

export async function deleteCategory(id, type, token) {
    try {
        var response = await fetch(`${type == 1 ? incomeCategoriesAPI : expenseCategoriesAPI}/${id}`, {
            method: "DELETE",
            headers: {
                "RequestVerificationToken": token
            }
        });

        if (response.ok) {
            return true;
        } else {
            console.error(`HTTP Delete Error: ${response.status}`);
            return false;
        }

    } catch (error) {
        console.error(error);
        return false;
    }
}

export async function getCategoriesWithUnevaluatedTransactions(id) {
    try {
        var queryParams = new URLSearchParams({
            FiscalPlanId: id
        });
        var response = await fetch(`${expenseCategoriesAPI}/filteredByEvaluation?${queryParams}`, {
            method: "GET"
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
    };
}
