document.querySelector(".sendButton").addEventListener("click", async function e()
{
    let email = document.querySelector(".emailInput").value;

    let password = document.querySelector(".passwordInput").value;

    let resp = await fetch("https://localhost:7001/authorize/employee", {
        method: "POST",
        body: JSON.stringify({
            Login: email,
            Password: password
        }),
        headers: {
            "Content-Type": "application/json"
        }
    })

    if (resp.ok)
    {
        let tokens = await resp.json();

        let date = new Date(Date.now())

        date = addHours(0.25, date);

        document.cookie = `Token=${tokens.token}; expires=${date.toUTCString()}`;

        date = addHours(5, date);

        document.cookie = `RefreshToken=${tokens.refreshToken}; expires=${date.toUTCString()}`;

        window.location.replace("https://localhost:7230/authorized.html")
    }
})

document.querySelector(".resetButton").addEventListener("click", function ()
{
    document.querySelector(".emailInput").value = ""

    document.querySelector(".passwordInput").value = "";
})

function addHours(numOfHours, date = new Date())
{
    date.setTime(date.getTime() + numOfHours * 60 * 60 * 1000);

    return date;
}