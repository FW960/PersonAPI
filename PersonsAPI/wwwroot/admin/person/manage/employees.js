let token = "";
let refreshToken = "";

getCookies();

let authorizedTwoTimes = false;

$(".form-auth-wrapper").attr("style", "display:none");

document.querySelector(".employee-add-button").addEventListener("click", async function e()
{
    let employeeFirstNameInputAdd = document.querySelector(".employeeFirstNameInputAdd").value;

    let employeeLastnameInputAdd = document.querySelector(".employeeLastnameInputAdd").value;

    let employeeEmailInputAdd = document.querySelector(".employeeEmailInputAdd").value;

    let employeeAgeInputAdd = document.querySelector(".employeeAgeInputAdd").value;

    let employeeGroupInputAdd = document.querySelector(".employeeGroupInputAdd").value;

    let employeeInputPasswordAdd = document.querySelector(".employeeInputPasswordAdd").value;

    let resp = await fetch("https://localhost:7292/employees/add/agent", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`,
            "password": employeeInputPasswordAdd
        },
        body: JSON.stringify(
            {
                FirstName: employeeFirstNameInputAdd,
                LastName: employeeLastnameInputAdd,
                Email: employeeEmailInputAdd,
                Age: employeeAgeInputAdd,
                Group: employeeGroupInputAdd
            }
        )
    })

    if (resp.status == 401 && !authorizedTwoTimes)
    {
        authorizeAgain();
        return;
    } else if (resp.status == 401)
    {
        getNewToken();

        resp = await fetch("https://localhost:7292/employees/add/agent", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
                "password": employeeInputPasswordAdd
            },
            body: JSON.stringify(
                {
                    FirstName: employeeFirstNameInputAdd,
                    LastName: employeeLastnameInputAdd,
                    Email: employeeEmailInputAdd,
                    Age: employeeAgeInputAdd,
                    Group: employeeGroupInputAdd
                }
            )
        })
    }

    if (resp.ok)
    {
        document.querySelector(".employeesOutputText").textContent = "Employee added";
    }

})

document.querySelector(".employee-get-by-id-button").addEventListener("click", async function e()
{
    let employeeIdInputGetById = document.querySelector(".employeeIdInputGetById").value;

    let resp = await fetch(`https://localhost:7292/employees/get/by_id/agent/id=${employeeIdInputGetById}`, {
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

        resp = await fetch(`https://localhost:7292/employees/get/by_id/agent/id=${employeeIdInputGetById}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        })
    }

    if (resp.ok)
    {
        let employeeDto = await resp.json();

        let text = "";

        for (const key in employeeDto)
        {
            text += `${key} ${employeeDto[key]}\n`;
        }

        document.querySelector(".employeesOutputText").textContent = text;
    }
})

document.querySelector(".employee-get-by-name-button").addEventListener("click", async function e()
{
    let employeeFirstNameInputGetByName = document.querySelector(".employeeFirstNameInputGetByName").value;

    let employeeLastnameInputGetByName = document.querySelector(".employeeLastnameInputGetByName").value;

    let resp = await fetch(`https://localhost:7292/employees/get/by_full_name/agent/first_name=${employeeFirstNameInputGetByName}/last_name=${employeeLastnameInputGetByName}`, {
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

        resp = await fetch(`https://localhost:7292/employees/get/by_full_name/agent/first_name=${employeeFirstNameInputGetByName}/last_name=${employeeLastnameInputGetByName}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        })
    }

    if (resp.ok)
    {
        let employeeDto = await resp.json();

        let text = "";

        for (const key in employeeDto)
        {
            text += `${key} ${employeeDto[key]}\n`;
        }

        document.querySelector(".employeesOutputText").textContent = text;
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