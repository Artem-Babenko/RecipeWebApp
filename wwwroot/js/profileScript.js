
/** Об'єкт користувача. */
let user;
/** Список категорій. */
let categories;
/** Список рецептів. */
let recipes;
let totalViews;
let averageRating;
let totalComments;

let categoryIdToShow = null;

/** Отримує користувача. */
async function getUser() {
    const response = await fetch(`/user`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    const result = await response.json();
    user = result.user;
    categories = result.categories;
    recipes = result.recipes;
    totalViews = result.totalViews;
    averageRating = result.averageRating.toFixed(2);
    totalComments = result.totalComments;
}


/** Встановлює інформацію про користувача. */
function setUserInfo() {
    const name = document.querySelector('.name');
    const surname = document.querySelector('.surname');
    const email = document.querySelector('.email');
    const password = document.querySelector('.password');
    const age = document.querySelector('.age');
    const gender = document.querySelector('.gender');
    const totalCommentsElement = document.querySelector('.total-comments');
    const totalViewsElement = document.querySelector('.total-views');
    const averageRatingElement = document.querySelector('.average-rating');

    name.textContent = user.name;
    surname.textContent = user.surname;
    email.textContent = user.email;
    age.textContent = user.age == null ? 'Не вказано' : user.age;
    gender.value = user.gender ?? 'none';
    password.value = user.password;

    totalCommentsElement.innerHTML = `<span>Отримано коментарів:</span> ${totalComments}`;
    totalViewsElement.innerHTML = `<span>Отримано переглядів:</span> ${totalViews}`;
    averageRatingElement.innerHTML = `<span>Мій рейтинг</span>: ${averageRating} <span>/ 5</span>`;
}

/** Обробник редагування користувача. */
function editUserInfo() {
    const userPanel = document.querySelector('.user');

    const editButton = userPanel.querySelector('.edit-button');
    const name = userPanel.querySelector('.name');
    const surname = userPanel.querySelector('.surname');
    const age = userPanel.querySelector('.age');
    const gender = userPanel.querySelector('.gender');
    const email = userPanel.querySelector('.email');
    const password = userPanel.querySelector('.password');

    editButton.addEventListener('click', async () => {
        // Відкриваєм редагування.
        if (!editButton.classList.contains('edit')) {
            editButton.classList.add('edit');
            editButton.classList.remove('fa-pencil');
            editButton.classList.add('fa-floppy-disk');

            name.contentEditable = true;
            name.classList.add('active');

            surname.contentEditable = true;
            surname.classList.add('active');

            age.contentEditable = true;
            age.classList.add('active');

            gender.removeAttribute('disabled');
            gender.classList.add('active');

            email.contentEditable = true;
            email.classList.add('active');

            password.removeAttribute('disabled');
            password.setAttribute('type', 'text');
            password.classList.add('active');
        }
        // Закриваєм редагування.
        else {
            if (password.value.trim().length < 4 ||
                email.textContent.trim().length < 6 ||
                (isNaN(parseInt(age.textContent.trim())) && age.textContent != 'Не вказано') ||
                name.textContent.trim() == '' ||
                surname.textContent.trim() == ''
            ) return;

            editButton.classList.add('fa-pencil');
            editButton.classList.remove('fa-floppy-disk');

            name.contentEditable = false;
            name.classList.remove('active');

            surname.contentEditable = false;
            surname.classList.remove('active');

            age.contentEditable = false;
            age.classList.remove('active');

            gender.setAttribute('disabled', '');
            gender.classList.remove('active');

            email.contentEditable = false;
            email.classList.remove('active');

            password.setAttribute('disabled', '');
            password.setAttribute('type', 'password');
            password.classList.remove('active');

            const response = await fetch(`/user`, {
                method: "PUT",
                headers: { "Accept": "application/json", "Content-Type": "application/json" },
                body: JSON.stringify({
                    id: user.id,
                    name: name.textContent.trim(),
                    surname: surname.textContent.trim(),
                    age: parseInt(age.textContent.trim()),
                    gender: gender.value,
                    email: email.textContent.trim(),
                    password: password.value.trim()
                })
            });

            if (response.ok) {
                editButton.classList.remove('edit');
                console.log('success user edit');
            }
        }
    });
}

/** Встановлє інформацію про рецепти та спиток рецептів. */
function setRecipesInfo() {
    const recipeCount = document.querySelector('.recipes-info span');
    const recipeCategories = document.querySelector('.recipes-info .categories');

    // Встановлення кількості доданих рецептів.
    recipeCount.textContent = `Додано ${recipes.length} рецептів`;

    // Створення елементів посилань на категорії.
    categories.forEach(category => recipeCategories.appendChild(categoryLinkButton(category)));

    // Створення списку рецептів.
    showRecipes();
}

/** Обробник сортбару. */
function sortBar() {
    const sortbar = document.querySelector('.sortbar');
    const sortButtons = sortbar.querySelectorAll('.sort-btn');

    // Для кожної клавіши сортування.
    sortButtons.forEach(button => {
        const icon = document.createElement('i');
        icon.classList.add('fa-solid');
        button.appendChild(icon);

        const sortBy = button.textContent.split('<')[0].trim().toLowerCase();

        // Якщо відбудеть клік по клавіші сортування.
        button.addEventListener('click', () => {

            // Якщо клавіша сортує з верху у них.
            if (button.classList.contains('from-top')) {
                // Встановлюєм сортування з низу у верх.
                button.classList.remove('from-top');
                button.classList.add('from-bottom');
                icon.classList.remove('fa-caret-down');
                icon.classList.add('fa-caret-up');
                showRecipes(categoryIdToShow, sortBy, 'from-bottom');
            }
            // Якщо клавіша сортує з низу у верх.
            else if (button.classList.contains('from-bottom')) {
                // Встановлюєм сортування з верху у низ.
                button.classList.remove('from-bottom')
                button.classList.add('from-top');
                icon.classList.remove('fa-caret-up');
                icon.classList.add('fa-caret-down');
                showRecipes(categoryIdToShow, sortBy, 'from-top');
            }
            // Якщо це перше натиснення на клавішу.
            else {
                // Прибираєм натиснення з інших клавіш.
                sortButtons.forEach(sortbtn => {
                    sortbtn.classList.remove('active');
                    sortbtn.classList.remove('from-top');
                    sortbtn.classList.remove('from-bottom');
                    sortbtn.querySelector('i').classList.remove('fa-caret-down');
                    sortbtn.querySelector('i').classList.remove('fa-caret-up');
                });

                // Встановлюєм активність та сортування з верху у них.
                button.classList.add('active');
                button.classList.add('from-top');
                icon.classList.remove('fa-caret-up');
                icon.classList.add('fa-caret-down');

                showRecipes(categoryIdToShow, sortBy, 'from-top');
            }
        });
    });
}

/** Клавіша-посилання на категоїю. */
function categoryLinkButton(category) {
    const categoryContainer = document.createElement('div');
    categoryContainer.classList.add('category');

    // Евент на клавішу-посилання на категорію.
    categoryContainer.addEventListener('click', () => {
        
        // Якщо клавіша вже натиснута.
        if (categoryContainer.classList.contains('active')) {
            // То вимикаєм її та виводим усі рецепти.
            categoryContainer.classList.remove('active');
            showRecipes();
        }
        else {
            // Прибираєм візуалізацію натиснення у віх клавіш.
            const recipeCategories = document.querySelectorAll('.recipes-info .categories .category');
            recipeCategories.forEach(button => button.classList.remove('active'));
            // Активуєм ту яку натиснули.
            categoryContainer.classList.add('active');

            // Виклик метода показу рецептів, але вже відносно категорії.
            showRecipes(category.category.id);
            categoryIdToShow = category.category.id;
        }
    });

    // Клавіша категорії.
    const button = document.createElement('div');
    button.textContent = category.category.name;
    categoryContainer.appendChild(button);

    // Кількість рецептів у категорії.
    const span = document.createElement('span');
    span.textContent = category.recipeCount;
    categoryContainer.appendChild(span);

    return categoryContainer;
}

/** Функція відкриття панелі підтвердження видалення рецепта. */
function openDeletePanel(container, recipeId) {
    const deletePanel = document.querySelector('.delete-message');
    deletePanel.style.opacity = 1;
    deletePanel.style.zIndex = 1;

    // Елемент з клавішами вибору.
    const buttons = deletePanel.querySelector('.buttons');
    buttons.textContent = "";

    // Клавіша так.
    const yesButton = document.createElement('p');
    yesButton.textContent = 'Так';
    yesButton.addEventListener('click', async () => {
        container.remove();
        const response = await fetch(`/recipes/${recipeId}`, {
            method: "DELETE"
        });
        if (response.ok) closeDeletePanel();
    });
    buttons.appendChild(yesButton);

    // Клавіша ні.
    const noButton = document.createElement('p');
    noButton.textContent = "Ні";
    noButton.addEventListener('click', () => {
        closeDeletePanel();
    });
    buttons.appendChild(noButton);
}

/** Функція закриття панелі підтвердження видалення рецепта. */
function closeDeletePanel() {
    const deletePanel = document.querySelector('.delete-message');
    deletePanel.style.opacity = 0;
    setTimeout(() => deletePanel.style.zIndex = -1, 500);
}

/** Показує всі рецепти користувача або рецепти які належать категорії. */
function showRecipes(categoryId, sortMethod, sortDirection) {
    const recipeList = document.querySelector('.recipes-info .recipes-list');
    recipeList.textContent = '';

    // Сортування.
    const sortedRecipes = recipes;
    if (sortMethod && sortDirection) {
        switch (sortMethod) {
            case 'датою':
                sortedRecipes.sort((a, b) => {
                    const dateA = new Date(a.createDate);
                    const dateB = new Date(b.createDate);
                    return sortDirection === 'from-bottom' ? dateA - dateB : dateB - dateA;
                });
                break;
            case 'популярністю':
                sortedRecipes.sort((a, b) => {
                    return sortDirection === 'from-bottom' ? a.views - b.views : b.views - a.views;
                });
                break;
            case 'коментарями':
                sortedRecipes.sort((a, b) => {
                    return sortDirection === 'from-bottom' ? a.comments.length - b.comments.length : b.comments.length - a.comments.length;
                });
                break;
            case 'рейтингом':
                sortedRecipes.sort((a, b) => {
                    const averageRatingA = calculateAverageRating(a.comments);
                    const averageRatingB = calculateAverageRating(b.comments);
                    return sortDirection === 'from-bottom' ? averageRatingA - averageRatingB : averageRatingB - averageRatingA;
                });
                break;
        }
    }

    if (categoryId) {
        const filteredRecipes = sortedRecipes.filter(recipe => recipe.category.id === categoryId);
        filteredRecipes.forEach(recipe => recipeList.append(recipeContainer(recipe)));
    }
    else {
        sortedRecipes.forEach(recipe => recipeList.append(recipeContainer(recipe)));
    }

    // Функція для обчислення середнього рейтингу коментарів.
    function calculateAverageRating(comments) {
        if (comments.length === 0) {
            return 0; // Якщо коментарів немає, повертаємо 0.
        }

        const totalRating = comments.reduce((sum, comment) => sum + comment.rating, 0);
        return totalRating / comments.length;
    }
}

/** Створює контейнер з інформаціює про рецепт. */
function recipeContainer(recipe) {
    const container = document.createElement('div');
    container.classList.add('container');

    // Зображення рецепту.
    const img = document.createElement('img');
    img.src = `/photos/${recipe.photoName}`;
    container.appendChild(img);

    // Блок інформації.
    const infoBlock = document.createElement('div');
    infoBlock.classList.add('recipe-info');

    // Назва рецепту.
    const name = document.createElement('div');
    name.classList.add('name');
    name.textContent = recipe.name;
    infoBlock.appendChild(name);

    // Створення блоку для зірок.
    const stars = document.createElement('div');
    stars.classList.add('stars');

    // Визначення кількості повних, напів-зірок та порожніх зірок на основі рейтингу.
    const totalRatingSum = recipe.comments.reduce((sum, comment) => sum + comment.rating, 0);
    let ratingData = totalRatingSum / recipe.comments.length;
    const fullStars = Math.floor(ratingData);
    const hasHalfStar = ratingData % 1 !== 0;

    // Створення зірок відповідно до рейтингу.
    for (let i = 1; i <= 5; i++) {
        const starIcon = document.createElement('i');
        starIcon.classList.add('fa-star', i <= fullStars ? 'fa-solid' : 'fa-regular');

        // Додавання напів-зірки, якщо вона присутня.
        if (hasHalfStar && i === Math.ceil(ratingData)) {
            starIcon.classList.remove('fa-star', 'fa-regular');
            starIcon.classList.add('fa-star-half-stroke', 'fa-solid');
        }

        // Додавання зірки до блоку.
        stars.appendChild(starIcon);
    }

    // Створення блоку для відображення рейтингу.
    const rating = document.createElement('div');
    rating.classList.add('rating');
    rating.textContent = ratingData ? ratingData.toFixed(1) : 0;
    stars.append(rating);
    infoBlock.appendChild(stars);

    // Короткий опис.
    const maxWords = 20;

    const descriptionText = recipe.description;
    const words = descriptionText.split(' ');
    const truncatedWords = words.slice(0, maxWords);
    const truncatedDescription = truncatedWords.join(' ');

    // Перевірка, чи останнє слово не є крапкою.
    const lastWord = truncatedWords[truncatedWords.length - 1];
    const endsWithPeriod = /\.$/.test(lastWord);

    // Додавання три крапки, якщо останнє слово не є крапкою.
    const finalDescription = endsWithPeriod ? truncatedDescription : truncatedDescription + '...';

    const description = document.createElement('div');
    description.classList.add('description');
    description.textContent = finalDescription;
    infoBlock.append(description);

    // Колонтитул (низ).
    const footer = document.createElement('div');
    footer.classList.add('footer');

    // Перегляди.
    const views = document.createElement('div');
    views.innerHTML = `<i class="fa-regular fa-eye"></i> ${recipe.views}`
    footer.appendChild(views);

    // Коментарі.
    const comments = document.createElement('div');
    comments.innerHTML = `<i class="fa-regular fa-comments"></i> ${recipe.comments.length}`
    footer.appendChild(comments);

    // Дата створення.
    const createDate = document.createElement('div');
    createDate.innerHTML = `<i class="fa-regular fa-calendar"></i> ${formatCSharpDate(recipe.createDate)}`;
    footer.appendChild(createDate);

    infoBlock.appendChild(footer);

    // Кінець блоку інформації та додавання його у контейнер.
    container.appendChild(infoBlock);

    // Блок взаємодії.
    const interactionsBlock = document.createElement('div');
    interactionsBlock.classList.add('interactions');

    // Клавіша перегляду рецепту.
    const viewButton = document.createElement('button');
    viewButton.classList.add('view-button');
    viewButton.innerHTML = '<i class="fa-solid fa-arrow-up-right-from-square"></i>';
    viewButton.addEventListener('click', () => {
        window.location.href = `/recipe?id=${recipe.id}`;
    });
    interactionsBlock.appendChild(viewButton);

    // Клавіша редагування рецепту.
    const editButton = document.createElement('button');
    editButton.classList.add('edit-button');
    editButton.innerHTML = '<i class="fa-solid fa-pen-to-square"></i>';
    editButton.addEventListener('click', () => {
        window.location.href = `/edit?id=${recipe.id}`;
    });
    interactionsBlock.appendChild(editButton);

    // Клавіша видалення рецепту.
    const deleteButton = document.createElement('button');
    deleteButton.classList.add('delete-button');
    deleteButton.innerHTML = '<i class="fa-solid fa-trash-can"></i>';
    deleteButton.addEventListener('click', () => {
        openDeletePanel(container, recipe.id);
    });
    interactionsBlock.appendChild(deleteButton);

    // Кінець блоку взаємодії та додавання його у контейнер.
    container.appendChild(interactionsBlock);

    return container;
}

/** Функція перетворення дати з DateTime C#. */
function formatCSharpDate(csharpDate) {
    // Перетворення рядка на об'єкт Date
    var date = new Date(csharpDate);

    // Отримання компонентів дати
    var day = date.getDate();
    var month = date.getMonth() + 1; // Місяці в JavaScript починаються з 0
    var year = date.getFullYear() % 100; // Отримання останніх двох цифр року

    // Додавання нуля перед числами меншими за 10
    day = (day < 10) ? '0' + day : day;
    month = (month < 10) ? '0' + month : month;

    // Формування рядка з датою у форматі dd.mm.yy
    var formattedDate = day + '.' + month + '.' + year;

    return formattedDate;
}

/** Функція для перетворення часу на "h год m хв". */
function formatTime(time) {
    const [hours, minutes] = time.split(':');
    let result = '';

    if (parseInt(hours) > 0) {
        result += parseInt(hours) + ' год ';
    }

    const formattedMinutes = parseInt(minutes) > 0 ? parseInt(minutes) + ' хв' : '';
    result += formattedMinutes;

    return result.trim(); // Видаляємо можливі пробіли в кінці
}

document.querySelector('.logout-button').addEventListener('click', () => {
    Cookies.remove('UserName');
    Cookies.remove('UserSurname');
    window.location.href = '/user/logout';
});

async function initializePage() {
    await getUser();
    setUserInfo();
    setRecipesInfo();
    sortBar();
    editUserInfo()
}

initializePage();