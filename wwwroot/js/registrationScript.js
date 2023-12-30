async function registration() {
    const nameInput = document.getElementById("name");
    const surnameInput = document.getElementById("surname");
    const emailInput = document.getElementById("email");
    const passwordFirstInput = document.getElementById("password1");
    const passwordSecondInput = document.getElementById("password2");
    const errorMessage = document.getElementById("error");

    // Перевірка чи введені всі поля вводу
    const fields = ["name", "surname", "email", "password1", "password2"];
    let isValid = true;
    for (const fieldName of fields) {
        const input = document.getElementById(fieldName);
        // Скинути попередні стилі та повідомлення про помилку
        input.style.outline = "none";
    }

    // Перевірка для кожного поля
    for (const fieldName of fields) {
        const input = document.getElementById(fieldName);
        if (!input.value) {
            input.style.outline = "2px solid red";
            isValid = false;
        }
    }

    if (!isValid) {
        errorMessage.style.display = "block";
        errorMessage.innerText = "Не всі поля введені";
    }
    // Кінець перевірки полів вводу

    // Перевірка чи паролі мають 8 символів
    else if (passwordFirstInput.value.length < 8) {
        errorMessage.style.display = "block";
        errorMessage.innerText = "Пароль має менше 8 символів";
        passwordFirstInput.style.outline = "2px solid red";
    }

    // Перевірка чи однакові введені паролі
    else if (passwordFirstInput.value != passwordSecondInput.value) {
        passwordFirstInput.style.outline = "2px solid red";
        passwordSecondInput.style.outline = "2px solid red";
        errorMessage.style.display = "block";
        errorMessage.innerText = "Паролі не співпали";
    }
    // якщо всі помилкові умови не виконані то буде надісланий запит на сервер
    else {
        errorMessage.style.display = "none";

        const response = await fetch("/user/registration", {
            method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify({
                name: nameInput.value,
                surname: surnameInput.value,
                email: emailInput.value,
                password: passwordSecondInput.value
            })
        });

        if (response.status === 400) {
            emailInput.style.outline = "2px solid red";
            errorMessage.style.display = "block";
            errorMessage.innerText = "Ця пошта вже зареєстрована";
        } else if (response.status === 200) {
            window.location.href = "/";
        }
    }

}

document.getElementById("registrationButton").addEventListener("click", async () => registration());

