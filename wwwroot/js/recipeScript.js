
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
let recipeId;

setInfo();
setUser();

/** Головна функція встановлення всієї інформації про рецепт. */
async function setInfo() {
    // Отримання ідентифікатору рецепта з запиту.
    const currentUrl = window.location.href;
    const { searchParams } = new URL(currentUrl);
    const id = searchParams.get('id');

    // Запит на сервер
    const response = await fetch(`/recipes/${id}`, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    const recipe = await response.json();
    recipeId = recipe.id;

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

    diagramInfo(recipe);
};

/** Встановлення користувача для написання коментаря. */
function setUser() {

    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
    }
    
    const userNameEncoded = getCookie('UserName');
    const userSurnameEncoded = getCookie('UserSurname');

    if (userNameEncoded && userSurnameEncoded) {
        const userName = decodeURIComponent(userNameEncoded);
        const userSurname = decodeURIComponent(userSurnameEncoded);

        document.getElementById('userName').value = `${userName} ${userSurname}`;
    }
}

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
    ratingNum.textContent = ratingData ? ratingData.toFixed(1) : 0;
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
    name.textContent = ingredient.product.name;
    row.appendChild(name);

    // Кількість та одиниці вимірювання.
    const amount = document.createElement('div');
    amount.classList.add('amount');

    const amountText = (ingredient.amount === 0) ? "" : ingredient.amount;
    const unitText = (ingredient.product.unit === null) ? "" : ingredient.product.unit;

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
        document.querySelectorAll('#recipeRating i').forEach(star => star.classList.replace('fa-solid', 'fa-regular'));

        // Заповнити відповідну кількість зірочок
        for (let i = 1; i <= ratingValue; i++) {
            document.querySelector(`.rating i[data-value="${i}"]`).classList.replace('fa-regular', 'fa-solid');
        }

        // Зберегти значення у змінній JS (тут ви можете використовувати змінну за вашим вибором)
        const selectedRating = ratingValue;
        document.querySelector('#recipeRating span').textContent = selectedRating;
    }
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

/** Відправлення коментаря на сервер. */
async function sendComment() {
    const creatorName = document.getElementById('userName');
    if (creatorName.length < 4) return;

    const commentText = document.getElementById('comment');
    if (commentText.length < 4) return;

    const rating = document.querySelector('#recipeRating span');
    if (rating.textContent == '') return;

    const response = await fetch(`/recipe/comments/${recipeId}`, {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            userName: creatorName.value,
            title: commentText.value,
            rating: rating.textContent
        })
    });

    if (response.ok) {
        const comment = await response.json();
        commentsList.appendChild(commentRow(comment));
        commentText.value = '';
        rating.value = '';
    }
}

document.querySelector('.create-comment button').addEventListener('click', () => sendComment());

/* Обробник діаграми та таблиці харчової цінності. */
function diagramInfo(recipe) {

    // Всі білки.
    let proteinSum = 0;
    for (const ingredient of recipe.ingredients) {
        if (ingredient.amount == 0) break;
        proteinSum += (ingredient.amount / ingredient.product.amount) * ingredient.product.proteins;
    }
    const totalProteins = proteinSum.toFixed(2);
    console.log(`Кількість білків у страві: ${totalProteins} г`);

    // Всі вуглеводи.
    let carbohydrateSum = 0;
    for (const ingredient of recipe.ingredients) {
        if (ingredient.amount == 0) break;
        carbohydrateSum += (ingredient.amount / ingredient.product.amount) * ingredient.product.carbohydrate;
    }
    const totalCarbohydrates = carbohydrateSum.toFixed(2);
    console.log(`Кількість вуглеводів у страві: ${totalCarbohydrates} г`);

    // Всі жири.
    let fatsSum = 0;
    for (const ingredient of recipe.ingredients) {
        if (ingredient.amount == 0) break;
        fatsSum += (ingredient.amount / ingredient.product.amount) * ingredient.product.fats;
    }
    const totalFats = fatsSum.toFixed(2);
    console.log(`Кількість жирів у страві: ${totalFats} г`);

    // Всі калорії.
    let caloriesSum = 0;
    for (const ingredient of recipe.ingredients) {
        if (ingredient.amount == 0) break;
        caloriesSum += (ingredient.amount / ingredient.product.amount) * ingredient.product.calories;
    }
    const totalCalories = caloriesSum.toFixed(0);
    console.log(`Кількість калорій у страві: ${totalCalories} ккал`);

    // Вага страви.
    let totalWeight = 0;
    for (const ingredient of recipe.ingredients) {
        const weightInGrams = ingredient.product.weight ? ingredient.product.weight * ingredient.amount : ingredient.amount;
        totalWeight += weightInGrams;
    }
    console.log(`Вага страви: ${totalWeight.toFixed(2)} г`);

    // Калогій на 100 г.
    const totalCaloriesPer100g = (totalCalories / totalWeight * 100).toFixed(0);
    console.log(`Калорій на 100 г: ${totalCaloriesPer100g} ккал`);

    // Білків на 100 г.
    const totalProteinsPer100g = (totalProteins / totalWeight * 100).toFixed(2);
    console.log(`Білків на 100 г: ${totalProteinsPer100g} г`);

    // Вуглеводів на 100 г.
    const totalCarbohydratesPer100g = (totalCarbohydrates / totalWeight * 100).toFixed(2);
    console.log(`Вуглеводів на 100 г: ${totalCarbohydratesPer100g} г`);

    // Жири на 100 г.
    const totalFatsPer100g = (totalFats / totalWeight * 100).toFixed(2);
    console.log(`Вуглеводів на 100 г: ${totalFatsPer100g} г`);

    // Сума БЖВ
    const bjvSum = parseFloat(totalCarbohydratesPer100g) + parseFloat(totalFatsPer100g) + parseFloat(totalProteinsPer100g);

    // Отримайте елементи сегментів
    const segments = document.querySelectorAll('.donut-segment');

    // Задайте значення відсотків для кожного сегменту
    const proteinPercentage = (parseFloat(totalProteinsPer100g) / bjvSum) * 100;
    const carbsPercentage = (parseFloat(totalCarbohydratesPer100g) / bjvSum) * 100;
    const fatPercentage = (parseFloat(totalFatsPer100g) / bjvSum) * 100;

    // Встановлення даних на сторінку
    const proteinsPer100gElement = document.getElementById('proteinsFor100g');
    const carbohydratesPer100gElement = document.getElementById('carbohydratesFor100g');
    const fatsPer100gElement = document.getElementById('fatsFor100g');
    const caloriesElement = document.querySelector('.diagram-text span');

    proteinsPer100gElement.textContent = `Білки - ${totalProteinsPer100g} г (${proteinPercentage.toFixed(2)} %)`;
    carbohydratesPer100gElement.textContent = `Вуглеводи - ${totalCarbohydratesPer100g} г (${carbsPercentage.toFixed(2)} %)`;
    fatsPer100gElement.textContent = `Жири - ${totalFatsPer100g} г (${fatPercentage.toFixed(2)} %)`;
    caloriesElement.textContent = totalCaloriesPer100g;

    // Встановлення даних слайдерів.
    const proteinSlider = document.getElementById('proteinSlider');
    const carbohydrateSlider = document.getElementById('carbohydrateSlider');
    const fatsSlider = document.getElementById('fatsSlider');
    proteinSlider.style.width = proteinPercentage + "%";
    carbohydrateSlider.style.width = carbsPercentage + "%";
    fatsSlider.style.width = fatPercentage + "%";

    // Встановлення даних у таблицю.
    const table = document.querySelector('.calories-table tbody');
    recipe.ingredients.forEach(ingredient => table.append(productRow(ingredient.product, ingredient.amount)));

    // Дані у таблиці "Разом"
    document.getElementById('totalWeigth').textContent = formatNumber(totalWeight);
    document.getElementById('totalProteins').textContent = formatNumber(totalProteins);
    document.getElementById('totalCarbohydogates').textContent = formatNumber(totalCarbohydrates);
    document.getElementById('totalFats').textContent = formatNumber(totalFats);
    document.getElementById('totalCalories').textContent = formatNumber(totalCalories);

    // Дані у таблиці "На 100 г"
    document.getElementById('proteinsPer100g').textContent = formatNumber(totalProteinsPer100g);
    document.getElementById('carbohydratesPer100g').textContent = formatNumber(totalCarbohydratesPer100g);
    document.getElementById('fatsPer100gElement').textContent = formatNumber(totalFatsPer100g);
    document.getElementById('caloriesPer100g').textContent = formatNumber(totalCaloriesPer100g);

    // Функція для встановлення величини сегментів
    function setSegmentSizes() {
        let offset = 0;

        segments.forEach((segment, index) => {
            const percentage = (index === 0) ? proteinPercentage : (index === 1) ? carbsPercentage : fatPercentage;
            const dasharray = `${percentage} ${100 - percentage}`;

            segment.setAttribute('stroke-dasharray', dasharray);
            segment.setAttribute('stroke-dashoffset', offset);

            // Збільшуйте зміщення для кожного сегменту
            offset -= percentage;
        });
    }

    // Викликати функцію при завантаженні сторінки
    setSegmentSizes();
}

/** Елемент-рядок таблиці харчової цінності. */
function productRow(ingredient, amount) {
    const tr = document.createElement('tr');

    // Назва продукту.
    const name = document.createElement('td');
    name.textContent = ingredient.name;
    tr.appendChild(name);

    // Міра (кількість).
    const amountCell = document.createElement('td');
    amountCell.textContent = `${amount} ${ingredient.unit ?? ""}`;
    tr.appendChild(amountCell);

    // Вага.
    const weight = document.createElement('td');
    weight.textContent = formatNumber(ingredient.weight ? ingredient.weight * amount : amount) || 0;
    tr.appendChild(weight);

    // Білки.
    const proteins = document.createElement('td');
    proteins.textContent = formatNumber(ingredient.proteins * (amount / ingredient.amount)) || 0;
    tr.appendChild(proteins);

    // Вуглеводи.
    const carbohydrates = document.createElement('td');
    carbohydrates.textContent = formatNumber(ingredient.carbohydrate * (amount / ingredient.amount)) || 0;
    tr.appendChild(carbohydrates);

    // Жири.
    const fats = document.createElement('td');
    fats.textContent = formatNumber(ingredient.fats * (amount / ingredient.amount)) || 0;
    tr.appendChild(fats);

    // Калорії.
    const calories = document.createElement('td');
    calories.textContent = formatNumber(ingredient.calories * (amount / ingredient.amount)) || 0;
    tr.appendChild(calories);

    return tr;
}

/** Повертає число округлене до одного знака після коми. Якщо чисно не дробове то нулів після коми немає. */
function formatNumber(value) {
    if (isNaN(value)) {
        // Виводимо порожній рядок замість "NaN".
        return "";  
    }

    // Округлення до двох знаків після коми.
    const roundedValue = Math.round(value * 10) / 10;  

    // Перевіряємо, чи є дробова частина необхідною для виведення.
    const formattedValue = roundedValue % 1 !== 0 ? roundedValue.toFixed(1) : String(Math.round(roundedValue));

    return formattedValue;
}
