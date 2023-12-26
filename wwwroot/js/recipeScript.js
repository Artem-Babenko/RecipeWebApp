
// Посилання на елементи сторінки.
const recipeName = document.querySelector('.name');
const img = document.querySelector('img');
const description = document.querySelector('.description');
const infoTable = document.querySelector('table');
const ingredientList = document.querySelector('.ingredient-list');
const stepList = document.querySelector('.step-list');
const viewsSpan = document.querySelector('.create-comment h2 span');
const commentsList = document.querySelector('.comments');
const footer = document.querySelector('footer');

setInfo();

/** Головна функція встановлення всієї інформації про рецепт. */
async function setInfo() {
    // Отримання ідентифікатору рецепта з запиту.
    const currentUrl = window.location.href;
    const { searchParams } = new URL(currentUrl);
    const id = searchParams.get('id');

    // Запит на сервер
    const response = await fetch(`/recipe/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    const recipe = await response.json();

    // Встановлення даних на сторінку.
    recipeName.textContent = recipe.name;
    img.src = `/photos/${recipe.photoName}`;
    description.textContent = recipe.description;

    // Таблиця даних.
    infoTable.append(dataTBody(recipe));

    // Список інгередієнтів.
    recipe.ingredients.forEach(ingredient => ingredientList.appendChild(ingredientRow(ingredient)));

    // Список кроків приготування.
    recipe.cookingSteps.forEach(step => stepList.appendChild(stepRow(step)));

    // Перегляди.
    viewsSpan.innerHTML = `<i class="fa-regular fa-eye"></i> ${recipe.views}`;

    // Список коментарів.
    recipe.comments.forEach(comment => commentsList.appendChild(commentRow(comment)));

    // Футер з датою створення.
    footer.textContent = `Рецепт доданий ${formatCSharpDate(recipe.createDate)}`;
};


/** Формування tboby для таблиці даних. */
function dataTBody(recipe) {
    const tbody = document.createElement('tbody');

    // Рядок таблиці.
    const tr = document.createElement('tr');

    // Автор.
    const author = document.createElement('td');
    author.textContent = `${recipe.user.name} ${recipe.user.surname}`;
    tr.appendChild(author);

    // Рейтинг.
    const rating = document.createElement('td');
    rating.classList.add("rating");
    // Визначення кількості повних, напів-зірок та порожніх зірок на основі рейтингу


    const totalRatingSum = recipe.comments.reduce((sum, comment) => sum + comment.rating, 0);

    let ratingData = totalRatingSum / recipe.comments.length;
    const fullStars = Math.floor(ratingData);
    const hasHalfStar = ratingData % 1 !== 0;

    // Створення зірок відповідно до рейтингу
    for (let i = 1; i <= 5; i++) {
        const starIcon = document.createElement('i');
        starIcon.classList.add('fa-star', i <= fullStars ? 'fa-solid' : 'fa-regular');

        // Додавання напів-зірки, якщо вона присутня
        if (hasHalfStar && i === Math.ceil(ratingData)) {
            starIcon.classList.remove('fa-star', 'fa-regular');
            starIcon.classList.add('fa-star-half-stroke', 'fa-solid');
        }

        // Додавання зірки до блоку
        rating.appendChild(starIcon);
    }

    // Створення блоку для відображення рейтингу
    const ratingNum = document.createElement('span');
    ratingNum.textContent = ratingData;
    rating.append(ratingNum);
    tr.appendChild(rating);

    // Складність рецепту.
    const difficulty = document.createElement('td');
    difficulty.textContent = recipe.difficulty;
    tr.appendChild(difficulty);

    // Час приготування.
    const cookingTime = document.createElement('td');
    cookingTime.textContent = formatTime(recipe.cookingTime);
    tr.appendChild(cookingTime);

    tbody.appendChild(tr);
    return tbody;
}


/** Функція перетворення часу. */
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


/** Поветає елемент-рядок списку інгредієнтів. */
function ingredientRow(ingredient) {
    const row = document.createElement('div');
    row.classList.add('row');

    // Назва інгредієнту.
    const name = document.createElement('div');
    name.classList.add('ingredient-name');
    name.textContent = ingredient.name;
    row.appendChild(name);

    // Кількість та одиниці вимірювання.
    const amount = document.createElement('div');
    amount.classList.add('amount');

    const amountText = (ingredient.amount === 0) ? "" : ingredient.amount;
    const unitText = (ingredient.unit === null) ? "" : ingredient.unit;

    amount.textContent = `${amountText} ${unitText}`;
    row.appendChild(amount);

    return row;
}


/** Повертає елемент-рядок для списку кроків приготування. */
function stepRow(step) {
    const row = document.createElement('div');
    row.classList.add('row');

    // Номер кроку.
    const index = document.createElement('div');
    index.classList.add('index');
    index.textContent = step.stepIndex;
    row.appendChild(index);

    // Опис кроку.
    const title = document.createElement('div');
    title.classList.add('title');
    title.textContent = step.title;
    row.appendChild(title);

    return row;
}


/** Повертає елемент-рядок для списку коментарів. */
function commentRow(comment) {
    const container = document.createElement('div');
    container.classList.add('comment');

    // Інформаційний блок.
    const infoBlock = document.createElement('div');
    infoBlock.classList.add('info');
    container.appendChild(infoBlock);

    // Ім'я користувача.
    const userName = document.createElement('p');
    userName.classList.add('user-name');
    userName.textContent = comment.userName;
    infoBlock.appendChild(userName);

    // Оцінка користувача.
    const rating = document.createElement('p');
    rating.classList.add('rating');
    let ratingData = comment.rating;
    const fullStars = Math.floor(ratingData);
    const hasHalfStar = ratingData % 1 !== 0;

    // Створення зірок відповідно до рейтингу
    for (let i = 1; i <= 5; i++) {
        const starIcon = document.createElement('i');
        starIcon.classList.add('fa-star', i <= fullStars ? 'fa-solid' : 'fa-regular');

        // Додавання напів-зірки, якщо вона присутня
        if (hasHalfStar && i === Math.ceil(ratingData)) {
            starIcon.classList.remove('fa-star', 'fa-regular');
            starIcon.classList.add('fa-star-half-stroke', 'fa-solid');
        }

        // Додавання зірки до блоку
        rating.appendChild(starIcon);
    }
    infoBlock.appendChild(rating);

    // Текст коментаря.
    const commentText = document.createElement('p');
    commentText.classList.add('title');
    commentText.textContent = comment.title;
    infoBlock.appendChild(commentText);

    // Час створення.
    const createTime = document.createElement('p');
    createTime.classList.add('create-time');
    createTime.textContent = formatCSharpDate(comment.createTime);
    infoBlock.appendChild(createTime);

    // Кількість вподобань коментаря.
    const countOfLikes = document.createElement('div');
    countOfLikes.classList.add('count-of-likes');
    countOfLikes.innerHTML = `<i class="fa-regular fa-heart"></i> ${comment.countOfLikes}`
    container.appendChild(countOfLikes);

    return container;
}


document.getElementById('recipeRating').addEventListener('click', rateRecipe);

function rateRecipe(event) {
    if (event.target.classList.contains('fa-star')) {
        // Отримати значення з data-атрибута
        const ratingValue = event.target.getAttribute('data-value');

        // Очистити всі зірочки
        document.querySelectorAll('.rating i').forEach(star => star.classList.replace('fa-solid', 'fa-regular'));

        // Заповнити відповідну кількість зірочок
        for (let i = 1; i <= ratingValue; i++) {
            document.querySelector(`.rating i[data-value="${i}"]`).classList.replace('fa-regular', 'fa-solid');
        }

        // Зберегти значення у змінній JS (тут ви можете використовувати змінну за вашим вибором)
        const selectedRating = ratingValue;
        document.querySelector('#recipeRating span').textContent = selectedRating;
    }
}

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