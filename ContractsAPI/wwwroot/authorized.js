let cookies = document.cookie;

let refreshToken = "";

let token = "";

let employeeData;

cookies = cookies.split(";");

if (cookies[0].includes("RefreshToken"))
{
    refreshToken = cookies[0].split("RefreshToken=")[1];

    token = cookies[1].split("Token=")[1];
} else
{
    refreshToken = cookies[1].split("RefreshToken=")[1];

    token = cookies[0].split("Token=")[1];
}

getEmployeeData();


document.querySelector(".send-button").addEventListener("click", async function ()
{
    let file = document.querySelector(".file-upload").files[0];

    let formData = new FormData();

    let inn = parseInt(document.querySelector(".company-inn-input").value);

    let price = parseFloat(document.querySelector(".price-input").value);

    formData.append(".pdf", file);

    let resp = await fetch(`https://localhost:7230/contracts/add/company_inn=${inn}/employee_group=${employeeData.Group}/price=${price}`, {
        headers: {
            "Authorization": `Bearer ${token}`,
        },
        method: "POST",
        body: formData
    })

    if (resp.status == 401)
    {
        //todo
    }

    if (resp.ok == 200)
    {
        document.querySelector(".header-text").textContent = "File uploaded";
    }
})

document.querySelector(".get-button").addEventListener("click", async function ()
{
    let resp = await fetch("https://localhost:7230/contracts/get", {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${token}`
        }
    })
})

async function getEmployeeData()
{
    let resp = await fetch("https://localhost:7230/getEmployeeData", {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    })

    employeeData = await resp.json();

    document.querySelector(".header-text").textContent = `Welcome back ${employeeData.FirstName} ${employeeData.LastName}`;
}