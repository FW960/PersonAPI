let token = document.cookie.split("Token=")[2].split(";")[0];
let refreshToken = document.cookie.split("RefreshToken=")[1].split(";")[0];
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
    }
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
    }

    let companyDto = await resp.json();

    let textArea = $(".companieOutputText");

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

    textArea.val("");

    textArea.val(text);
})
$(".company-update-button").click(async function e()
{
    let companyInnInput = $(".companyInnInputUpdate").val();
    let companyNameInput = $(".companyNameInputUpdate").val();
    let companyCeoInput = $(".companyCeoInputUpdate").val();

    let resp = await fetch("https://localhost:7292/company/update", {
        method: "PUT",
        headers: {
            "Authorization": `Bearer ${token}`
        }
    })

    if (resp.status == 401 && !authorizedTwoTimes)
    {
        authorizeAgain();
        return;
    }

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
    }
})

function authorizeAgain()
{
    $(".form-auth").attr("style", "display:block");

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
        token = document.cookie.split("Token=")[2].split(";")[0];

        if (!authorizedTwoTimes)
        {
            document.querySelector(".form-auth-wrapper").remove();
            document.querySelector(".formsWrapper").classList.remove("hide");

        }

        authorizedTwoTimes = true;
    }
}

