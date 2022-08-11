
///<reference path = "typings/globals/jquery/index.d.ts"/>

const $wrapper = document.querySelector(".wrapper");
let tokens;

$(".sub-but").click(async function (e)
{
    let login = $(".inp-email").val();

    let pass = $(".inp-pass").val();

    let resp;

    if (!validateInput(login, pass))
        return;

    resp = await fetch("https://localho.st:7001/authorize/admin", {
        method: "POST",
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json'
        },
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
        body: JSON.stringify({ Login: $(".inp-email").val(), Password: $(".inp-pass").val() })
    })

    tokens = await resp.json();

    if (!resp.ok)
        return;
    else
    {
        displayWelcomePage(login);
    }

});

$(".res-but").click(async function (e)
{
    $(".inp-email").val("");
    $(".inp-pass").val("");
})

function validateInput(login, pass)
{
    var validRegex = new RegExp(/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/);

    if (login.match(validRegex) && pass.length >= 5)
    {
        return true;

    } else
    {
        return false;
    }
}
function displayWelcomePage(login)
{
    let $welcomePage = $("<div>", { class: "welcome-page" });

    let $welcomeText = $("<p>", { class: "welcome-text" });

    let $continueBut = $("<div>", { class: "continue-but" });

    $welcomeText.text(`Welcome back ${login.split("@")[0]}`);

    $continueBut.text("Continue");

    $(".wrapper").append($welcomePage);

    $($welcomePage).append($welcomeText);

    $($welcomePage).append($continueBut);

    $(".continue-but").click(redirectToAuthorizedPage);

    $(".form").remove();
}



function redirectToAuthorizedPage(e)
{
    const res = fetch("https://localhost:7292/admin/person/manage/authorized.html", {
        method: "GET",
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
            'Token': `${tokens.token}`,
            'RefreshToken': `${tokens.refreshToken}`
        },
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
    }).then(window.location.replace("https://localhost:7292/admin/person/manage/authorized.html"));
}