async function login() {
    const emailInput = document.getElementById("email");
    const passwordInput = document.getElementById("password");
    const errorMessage = document.getElementById("error");

    const email = emailInput.value;
    const password = passwordInput.value;

    // Перевірка чи обидва введені поля пусті
    if (!email && !password) {
        emailInput.style.outline = "2px solid red";
        passwordInput.style.outline = "2px solid red";
        errorMessage.style.display = "block";
        errorMessage.innerText = "Невведена пошта та пароль";
    }
    // Перевірка чи ввід пошти пустий
    else if (!email) {
        emailInput.style.outline = "2px solid red";
        passwordInput.style.outline = "none";
        errorMessage.style.display = "block";
        errorMessage.innerText = "Невведена пошта";
    }
    // Перевірка чи ввід паролю пустий
    else if (!password) {
        emailInput.style.outline = "none";
        passwordInput.style.outline = "2px solid red";
        errorMessage.style.display = "block";
        errorMessage.innerText = "Невведений пароль";
    }
    // якщо всі поля заповненні
    else {
        const response = await fetch("/user/login", {
            method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify({
                email: emailInput.value,
                password: passwordInput.value
            })
        });

        // якщо користувача Не знайдено
        if (response.status === 404) {
            emailInput.style.outline = "2px solid red";
            passwordInput.style.outline = "2px solid red";
            passwordInput.value = "";
            errorMessage.style.display = "block";
            errorMessage.innerText = "Неправильна пошта або пароль";
        }
        // якщо користувача Знайдено
        else if (response.status === 200) {
            const urlParams = new URLSearchParams(window.location.search);
            const returnUrl = urlParams.get('returnUrl');
            const redirectUrl = returnUrl ? returnUrl : "/";
            window.location.href = redirectUrl;
        }
    }
}

document.getElementById("login").addEventListener("click", async () => login());



document.getElementById("password").addEventListener('input', function () {
    this.style.outline = "none";
});

document.getElementById("email").addEventListener("click", function () {
    this.style.outline = "none";
});