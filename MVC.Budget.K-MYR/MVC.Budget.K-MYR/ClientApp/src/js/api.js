import { API_ROUTES } from './config';
export async function getCountryCookie(countryISOCode, token) {
    try {
        var response = await fetch(API_ROUTES.COUNTRY, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
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

export async function postFiscalPlan(formData) {
    try {
        var response = await fetch(API_ROUTES.fiscalPlans.BASE, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': formData.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: formData.get('Name'),
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

export async function putFiscalPlan(formData) {
    try {
        var id = formData.get('Id');
        var response = await fetch(API_ROUTES.fiscalPlans.BY_ID(id), {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': formData.get('__RequestVerificationToken'),
            },
            body: JSON.stringify({
                Id: id,
                Name: formData.get('Name'),
            }),
        });

        if (response.ok) {
            return true;
        } else {
            console.error(`HTTP PUT Error: ${response.status}`);
            return false;
        }
    } catch (error) {
        console.error(error);
        return false;
    }
}

export async function deleteFiscalPlan(id, token) {
    try {
        var response = await fetch(API_ROUTES.fiscalPlans.BY_ID(id), {
            method: 'DELETE',
            headers: {
                'RequestVerificationToken': token
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

export async function getFiscalPlanDataByMonth(id, date = new Date()) {
    try {
        var formattedDate = date instanceof Date ? date.toISOString() : new Date().toISOString();
        var queryParams = new URLSearchParams({
            Month: formattedDate
        });   
        var response = await fetch(API_ROUTES.fiscalPlans.GET_MONTHLY_DATA(id, queryParams), {
            method: 'GET',
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

export async function getFiscalPlanDataByYear(id, date = new Date()) {
    try {
        var year = date instanceof Date ? date.getFullYear() : new Date().getFullYear();
        var queryParams = new URLSearchParams({
            Year: year
        });
        var response = await fetch(API_ROUTES.fiscalPlans.GET_YEARLY_DATA(id, queryParams), {
            method: 'GET'
        });

        if (response.ok) {
            return await response.json();
        } else {
            console.error(`HTTP GET Error: ${response.status}`);
            return null;
        }
    } catch (error) {
        console.error(error.message);
        return null;
    }
}

export async function postTransaction(formData) {
    try {
        var response = await fetch(API_ROUTES.transactions.BASE, {
           method: 'POST',
           headers: {
               'Content-Type': 'application/json',
               'RequestVerificationToken': formData.get('__RequestVerificationToken')
           },
           body: JSON.stringify({
               Title: formData.get('Title'),
               Amount: parseFloat(formData.get('Amount')),
               DateTime: formData.get('DateTime'),
               IsHappy: formData.get('IsHappy') === 'true',
               IsNecessary: formData.get('IsNecessary') === 'true',
               CategoryId: parseInt(formData.get('CategoryId'))
           })
        });

        if (response.ok) {
            return await response.json();
        } else {
            console.error(`HTTP Post Error: ${response.status}`);
            var response = await response.json();
            return null;
        }

    } catch (error) {
        console.error(error);
        return null;
    }
}

export async function putTransaction(formData) {
    try {
        var id = parseInt(formData.get('Id'));
        var response = await fetch(API_ROUTES.transactions.BY_ID(id), {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': formData.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Id: id,
                Title: formData.get('Title'),
                Amount: parseFloat(formData.get('Amount')),
                DateTime: formData.get('DateTime'),
                IsHappy: formData.get('IsHappy') === 'true',
                IsNecessary: formData.get('IsNecessary') === 'true',
                IsEvaluated: formData.get('IsEvaluated') === 'true',
                PreviousIsHappy: formData.get('PreviousIsHappy') === 'true',
                PreviousIsNecessary: formData.get('PreviousIsNecessary') === 'true',
                CategoryId: parseInt(formData.get('CategoryId'))
            })
        });

        if (response.ok) {
            return true;
        } else {
            console.error(`HTTP PUT Error: ${response.status}`);
            return false;
        }

    } catch (error) {
        console.error(error);
        return false;
    }
}

export async function deleteTransaction(id, token) {
    try {        
        var response = await fetch(API_ROUTES.transactions.BY_ID(id), {
            method: 'DELETE',
            headers: {
                'RequestVerificationToken': token
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

export async function patchTransactionEvaluation(formData, previousIsHappy, previousIsNecessary, token) {
    try {
        var id = formData.get('Id');

        var patchDoc =
            [{
                op: 'replace',
                path: '/IsHappy',
                value: formData.get('IsHappy') === 'true'
            },
            {
                op: 'replace',
                path: '/IsNecessary',
                value: formData.get('IsNecessary') === 'true'
            }, {
                op: 'replace',
                path: '/PreviousIsHappy',
                value: previousIsHappy
            },
            {
                op: 'replace',
                path: '/PreviousIsNecessary',
                value: previousIsNecessary
            },
            {
                op: 'replace',
                path: '/IsEvaluated',
                value: true
            }];

        var response = await fetch(API_ROUTES.transactions.BY_ID(id), {
            method: 'PATCH',
            headers: {
                'Content-Type': 'application/json-patch+json',
                'RequestVerificationToken': token
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

export async function getUnevaluatedTransactions(categoryId, lastDate, lastId, pageSize) {
    try {
        var queryParams = new URLSearchParams({
            categoryId: categoryId,
            pageSize: pageSize
        });

        if (lastDate) {
            queryParams.append('lastDate', lastDate)
        }

        if (lastId) {
            queryParams.append('lastId', lastId)
        }

        var response = await fetch(API_ROUTES.transactions.GET_UNEVALUATED(queryParams), {
            method: 'GET'
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

export async function getCategoryDataByMonth(id, date = new Date(), type) {
    try {
        var formattedDate = date instanceof Date ? date.toISOString() : new Date().toISOString();
        var queryParams = new URLSearchParams({
            Month: formattedDate
        });
        var url;
        switch (type) {
            case 1:           
                url = API_ROUTES.incomeCategories.GET_MONTHLY_DATA(id, queryParams);
                break;
            case 2:
                url = API_ROUTES.expenseCategories.GET_MONTHLY_DATA(id, queryParams);
                break;
            default:
                console.error(`Invalid value '${type}' for 'type' field`);
                return null;
        }
        var response = await fetch(url, {
            method: 'GET',
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

export async function postCategory(formData) {
    try {
        var type = parseInt(formData.get('Type'));
        var url;
        switch (type) {
            case 1:
                url = API_ROUTES.incomeCategories.BASE;
                break;
            case 2:
                url = API_ROUTES.expenseCategories.BASE;
                break;
            default:
                console.error(`Invalid value '${type}' for 'type' field`);
                return false;
        }        
        var response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': formData.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: formData.get('Name'),
                Budget: parseFloat(formData.get('Budget')),
                FiscalPlanId: parseInt(formData.get('FiscalPlanId'))
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

export async function putCategory(formData, date = new Date()) {
    try {
        var id = parseInt(formData.get('Id'));
        var formattedDate = date instanceof Date ? date.toISOString() : new Date().toISOString();
        var queryParams = new URLSearchParams({
            Month: formattedDate
        });      
        var type = parseInt(formData.get('Type'));
        var url;
        switch (type) {
            case 1:
                url = API_ROUTES.incomeCategories.BY_ID_PARAMS(id, queryParams);
                break;
            case 2:
                url = API_ROUTES.expenseCategories.BY_ID_PARAMS(id, queryParams);
                break;
            default:
                console.error(`Invalid value '${type}' for 'type' field`);
                return false;
        }

        var response = await fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': formData.get('__RequestVerificationToken')
            },
            body: JSON.stringify({
                Name: formData.get('Name'),
                Budget: parseFloat(formData.get('Budget')),
                GroupId: parseInt(formData.get('GroupId')),
                Id: id,
                FiscalPlanId: parseInt(formData.get('FiscalPlanId'))
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
        var url;
        switch (type) {
            case 1:
                url = API_ROUTES.incomeCategories.BY_ID(id);
                break;
            case 2:
                url = API_ROUTES.expenseCategories.BY_ID(id);
                break;
            default:
                console.error(`Invalid value '${type}' for 'type' field`);
                return false;
        }
        var response = await fetch(url, {
            method: 'DELETE',
            headers: {
                'RequestVerificationToken': token
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
