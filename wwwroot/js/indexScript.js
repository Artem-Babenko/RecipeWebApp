
async function getRecipes() {
    const response = await fetch('/recipes', {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    const recipes = await response.json();
    const main = document.querySelector('main');
    recipes.forEach(recipe => main.append(recipeItem(recipe)));
}

function recipeItem(recipe) {
    const item = document.createElement('div');
    item.classList.add("item");

    // Картинка
    const img = document.createElement('img');
    img.src = "photos/" + recipe.photoPath;
    item.append(img);

    // Назва
    const name = document.createElement('div');
    name.classList.add("name");
    name.append(recipe.name);
    item.append(name);

    // Категорія
    const category = document.createElement('div');
    category.classList.add('category');
    category.textContent = recipe.category.name;
    item.appendChild(category);

    // Створення блоку для зірок
    const stars = document.createElement('div');
    stars.classList.add('stars');

    // Визначення кількості повних, напів-зірок та порожніх зірок на основі рейтингу
    let ratingData = recipe.rating;
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
        stars.appendChild(starIcon);
    }

    // Створення блоку для відображення рейтингу
    const rating = document.createElement('div');
    rating.classList.add('rating');
    rating.append(ratingData);
    stars.append(rating);

    // Додавання блоку зірок до основного елементу
    item.append(stars);

    // Короткий опис
    const maxWords = 25;

    const descriptionText = recipe.description;
    const words = descriptionText.split(' ');
    const truncatedWords = words.slice(0, maxWords);
    const truncatedDescription = truncatedWords.join(' ');

    // Перевірка, чи останнє слово не є крапкою
    const lastWord = truncatedWords[truncatedWords.length - 1];
    const endsWithPeriod = /\.$/.test(lastWord);

    // Додавання три крапки, якщо останнє слово не є крапкою
    const finalDescription = endsWithPeriod ? truncatedDescription : truncatedDescription + '...';

    const description = document.createElement('div');
    description.classList.add('description');
    description.textContent = finalDescription;
    item.append(description);

    // Колонтитул (низ)
    const footer = document.createElement('div');
    footer.classList.add('footer');

    // Перегляди
    const views = document.createElement('div');
    views.innerHTML = `<i class="fa-regular fa-eye"></i> ${recipe.views}`
    footer.appendChild(views);

    // Коментарі
    const comments = document.createElement('div');
    comments.innerHTML = `<i class="fa-regular fa-comments"></i> ${5}`
    footer.appendChild(comments);

    // Час приготування
    const timeSpanElement = document.createElement('div');
    const timeSpan = formatTime(recipe.cookingTime);
    timeSpanElement.innerHTML = `<i class="fa-regular fa-clock"></i> ${timeSpan}`
    footer.appendChild(timeSpanElement);

    item.appendChild(footer);

    return item;
}

// Функція для перетворення часу
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

async function getAndSetMainCategories() {
    const response = await fetch('/categories', {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    const categories = await response.json();
    const mainCategoriesElement = document.querySelector('.main-categories');
    categories.forEach(category => {
        const button = document.createElement('div');
        button.textContent = category.pluralName;
        mainCategoriesElement.appendChild(button);
    });
}

getRecipes();
getAndSetMainCategories();