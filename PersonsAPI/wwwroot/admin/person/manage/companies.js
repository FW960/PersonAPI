let token = "";
let refreshToken = "";

getCookies();

let authorizedTwoTimes = false;

$(".form-auth-wrapper").attr("style", "display:none");

$(".company-add-button").click(async function e()
{
    let companyInnInput = $(".companyInnInputAdd ").val();
    let companyNameInput = $(".companyNameInputAdd").val();

    let resp = fetch("https://localhost:7292/company/add", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({
            Name: companyNameInput, Inn: companyInnInput
        })
    })

    if (resp.status == 401 && !authorizedTwoTimes)
    {
        authorizeAgain();
        return;
    } else if (resp.status == 401)
    {
        getNewToken();
        resp = fetch("https://localhost:7292/company/add", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                Name: companyNameInput, Inn: companyInnInput
            })
        })
    }

    let companyTextArea = $(".companieOutputText");

    if (resp.status == 200)
        companyTextArea.val(`Company ${companyInnInput} added`);
})

$(".company-get-button").click(async function e()
{
    let companyInnInput = $(".companyInnInputGet").val();

    let resp = await fetch(`https://localhost:7292/company/get/by/inn=${companyInnInput}`, {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${token}`
        },
    })

    if (resp.status == 401 && !authorizedTwoTimes)
    {
        authorizeAgain();
        return;
    } else if (resp.status == 401)
    {
        getNewToken();

        resp = await fetch(`https://localhost:7292/company/get/by/inn=${companyInnInput}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            },
        })
    }

    if (resp.status == 200)
    {

        let companyDto = await resp.json();

        let companyTextArea = $(".companieOutputText");

        let text = "";

        for (const key in companyDto)
        {
            if (key == "ceo")
            {
                for (let ceoKey in companyDto[key])
                {
                    text += `${ceoKey} - ${companyDto[key][ceoKey]}` + "\n";
                }
                continue;
            }

            text += `${key} - ${companyDto[key]}` + "\n";
        }

        companyTextArea.val(text);
    }
})
$(".company-update-button").click(async function e()
{
    let companyInnInput = $(".companyInnInputUpdate").val();
    let companyNameInput = $(".companyNameInputUpdate").val();
    let companyCeoInput = $(".companyCeoInputUpdate").val();

    let resp = await fetch(`https://localhost:7292/company/update/new-ceo/id=${companyCeoInput}`, {
        method: "PUT",
        headers: {
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({
            Name: companyNameInput, Inn: companyInnInput
        })
    })

    if (resp.status == 401 && !authorizedTwoTimes)
    {
        authorizeAgain();
        return;
    } else if (resp.status == 401)
    {
        getNewToken();

        resp = await fetch(`https://localhost:7292/company/update/new-ceo/id=${companyCeoInput}`, {
            method: "PUT",
            headers: {
                'Content-Type': 'application/json',
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({
                Name: companyNameInput, Inn: companyInnInput
            })
        })
    }

    let companyTextArea = $(".companieOutputText");

    if (resp.status == 200)
        companyTextArea.val(`Company ${companyInnInput} updated`);

})

$(".company-delete-button").click(async function e()
{
    let companyInnInput = $(".companyInnInputDelete").val();

    let resp = await fetch(`https://localhost:7292/company/delete/by/inn=${companyInnInput}`, {
        method: "DELETE"
    })

    if (resp.status == 401 && !authorizedTwoTimes)
    {
        authorizeAgain();
        return;
    } else if (resp.status == 401)
    {
        getNewToken();

        resp = await fetch(`https://localhost:7292/company/delete/by/inn=${companyInnInput}`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        })

    }

    let companyTextArea = $(".companieOutputText");

    if (resp.status = 200)
        companyTextArea.val(`Company ${companyInnInput} updated`);
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