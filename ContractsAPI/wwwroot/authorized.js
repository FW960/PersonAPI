let cookies = document.cookie;

let refreshToken = "";

let token = "";

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