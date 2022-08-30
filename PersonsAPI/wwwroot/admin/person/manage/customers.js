let token = "";
let refreshToken = "";

getCookies();

let authorizedTwoTimes = false;

$(".form-auth-wrapper").attr("style", "display:none");

$(".customers-add-button").click(async function e()
{
    let customerPostInputAdd = $(".customerPostInputAdd").val();
    let customerCompanyInnInputAdd = $(".customerCompanyInnInputAdd").val();
    let customerEmailInputAdd = $(".customerEmailInputAdd").val();
    let customerFirstNameInputAdd = $(".customerFirstNameInputAdd").val();
    let customerLastnameInputAdd = $(".customerLastnameInputAdd").val();

    let resp = await fetch("https://localhost:7292/customers/add/agent", {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json"
        },
        body: JSON.stringify(
            {
                FirstName: customerFirstNameInputAdd,
                LastName: customerLastnameInputAdd,
                CompanyInn: customerCompanyInnInputAdd,
                Post: customerPostInputAdd,
                Email: customerEmailInputAdd
            })
    })
    if (resp.status == 401 && !authorizedTwoTimes)
    {
        authorizeAgain();
        return;
    } else if (resp.status == 401)
    {
        getNewToken();

        resp = await fetch("https://localhost:7292/customers/add/agent", {
            method: "POST",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(
                {
                    FirstName: customerFirstNameInputAdd,
                    LastName: customerLastnameInputAdd,
                    CompanyInn: customerCompanyInnInputAdd,
                    Post: customerPostInputAdd,
                    Email: customerEmailInputAdd
                })
        })
    }

    let customersOutputText = $(".customersOutputText");

    if (resp.ok)
    {
        customersOutputText.val("Customer added");
    }

})

$(".customer-get-by-id-button").click(async function e()
{
    let customerInputGetById = $(".customerInputGetById").val();

    let resp = await fetch(`https://localhost:7292/customers/get/by_id/agent/id=${customerInputGetById}`, {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${token}`
        }
    })

    if (resp.status == 401 && !authorizedTwoTimes)
    {
        authorizeAgain();
        return;
    } else if (resp.status == 401)
    {
        getNewToken();

        resp = await fetch(`https://localhost:7292/customers/get/by_id/agent/id=${customerInputGetById}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        })
    }


    if (resp.ok)
    {
        let customersOutputText = $(".customersOutputText");
        let text = "";

        let customerDto = await resp.json();

        for (const key in customerDto)
        {
            text += `${key} ${customerDto[key]} \n`;
        }

        customersOutputText.val(text);
    }
})

document.querySelector(".customers-get-by-name-button").addEventListener("click", async function e()
{

    let firstName = document.querySelector(".customerFirstNameInputGetByName").value;

    let lastName = document.querySelector(".customerLastnameInputGetByName").value;

    let resp = await fetch(`https://localhost:7292/customers/get/by_full_name/agent/first_name=${firstName}/last_name=${lastName}`, {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${token}`
        }
    })

    if (resp.status == 401 && !authorizedTwoTimes)
    {
        authorizeAgain();
        return;
    } else if (resp.status == 401)
    {
        getNewToken();

        resp = await fetch(`https://localhost:7292/customers/get/by_full_name/agent/first_name=${firstName}/last_name=${lastName}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        })
    }

    if (resp.ok)
    {
        let customerDto = await resp.json();

        let text = "";

        for (const key in customerDto)
        {
            text += `${key} ${customerDto[key]} \n`;
        }

        document.querySelector(".customersOutputText").textContent = text;
    }
})

function authorizeAgain()
{
    $(".form-auth-wrapper").attr("style", "display:block");

    $(".formsWrapper").addClass("hide");

    document.querySelector(".form-auth-wrapper").style.height = document.body.offsetHeight + 20;

    $(".sub-but").click(getNewToken)

}

async function getNewToken()
{
    let resp = await fetch("https://localhost:7292/admin/person/manage/authorized.html", {
        method: "GET",
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
            "RefreshToken": refreshToken,
        }
    })

    if (resp.ok)
    {
        getCookies();

        if (!authorizedTwoTimes)
        {
            document.querySelector(".form-auth-wrapper").remove();
            document.querySelector(".formsWrapper").classList.remove("hide");

        }

        authorizedTwoTimes = true;
    }
}

function getCookies()
{
    let cookies = document.cookie.split(";")

    for (let i = 0; i < cookies.length; i++)
    {
        if (cookies[i].includes("RefreshToken="))
        {
            refreshToken = cookies[i].split("RefreshToken=")[1];
            continue;
        } else if (cookies[i].includes("Token"))
        {
            token = cookies[i].split("Token=")[1];
        }
    }

}