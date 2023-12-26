
/** Встановлення рецептів на головній сторінці. Також відбувається пошук рецептів, відповідно до категорії. */
async function setRecipes(categoryId) {
    let recipes;
    if (categoryId) {
        // Якщо це виведення відносно обраної категорії.
        const response = await fetch(`/categories/${categoryId}`, {
            method: "GET",
            headers: { "Accept": "application/json" }
        });

        const result = await response.json();
        // Встановлення інформації про категорію рецептів.
        setContentName(result.category, result.recipes.length);
        recipes = result.recipes;
    }
    else {
        // Виведення всіх цепептів.
        const response = await fetch('/recipes', {
            method: "GET",
            headers: { "Accept": "application/json" }
        });

        recipes = await response.json();
    }

    const main = document.querySelector('main .recipes');
    main.textContent = "";
    // На основі кожного рецепту створити контейнер з його короткою інформацією.
    recipes.forEach(recipe => main.append(recipeItem(recipe)));
}


/** Контейнер рецепту для відобращення короткої інформації про нього. */
function recipeItem(recipe) {
    const item = document.createElement('div');
    item.classList.add("item");

    // Картинка
    const img = document.createElement('img');
    img.src = "photos/" + recipe.photoName;
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
    comments.innerHTML = `<i class="fa-regular fa-comments"></i> ${recipe.comments.length}`
    footer.appendChild(comments);

    // Час приготування
    const timeSpanElement = document.createElement('div');
    const timeSpan = formatTime(recipe.cookingTime);
    timeSpanElement.innerHTML = `<i class="fa-regular fa-clock"></i> ${timeSpan}`
    footer.appendChild(timeSpanElement);

    item.appendChild(footer);

    // Івенти
    item.addEventListener('click', () => {
        window.location.href = `/recipe?id=${recipe.id}`;
    });

    return item;
}


/** Отримання та встановлення головних категорій на верх сайту. */
async function setMainCategories() {
    const mainCategoriesContainer = document.querySelector('.main-categories');

    // Запит на отримання головних категорій.
    const response = await fetch('/categories/main', {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok) {
        const categories = await response.json();

        categories.forEach(category => {
            const button = document.createElement('button');
            button.textContent = category.name;
            mainCategoriesContainer.appendChild(button);
        });
    }
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


/** Встановленя списку категорії. */
async function setCategories() {
    // Запит на отримання всіх категорій.
    const response = await fetch('/categories', {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    if (response.ok) {
        const categories = await response.json();
        const mainCategories = categories.filter(category => category.parentCategoryId === null);
        // Створення списку категорії.
        renderCategories(mainCategories, categories);
    }
}


/** Встановлення інформації, які рецепти ми шукаєм по катерогіях. */
function setContentName(category, recipesCount) {
    // Посилання на елементи.
    const contentName = document.querySelector('.content-name');
    const categoryName = contentName.querySelector('.top h2');
    const countOfFint = contentName.querySelector('.top span');
    const subcategoriesList = contentName.querySelector('.bottom');

    contentName.style.display = 'flex';
    categoryName.textContent = category.name;
    countOfFint.textContent = `Знайдено ${recipesCount} рецептів`
    subcategoriesList.style.display = 'none';
    subcategoriesList.textContent = '';

    // Якщо є дочірні категорії. 
    if (category.subcategories.length > 0) {
        subcategoriesList.style.display = 'flex';
        // Створення клавіш-посилань на дочірні категорії.
        category.subcategories.forEach(subcategory => {
            const container = document.createElement('div');
            container.classList.add('subcategory-button');

            // Евент-посилання.
            container.addEventListener('click', async () => setRecipes(subcategory.id));

            // Назва дочірньої категорії.
            const name = document.createElement('p');
            name.textContent = subcategory.name;
            container.appendChild(name);

            // Кількість рецептів у категорії.
            const span = document.createElement('span');
            span.textContent = subcategory.recipeCount === 0 ? "" : subcategory.recipeCount;
            container.appendChild(span);

            subcategoriesList.appendChild(container);
        });
    }
}


/** Створення категорій. */
function renderCategories(mainCategories, allCategories) {
    const container = document.querySelector('nav .categories');

    // Створення елементів для головних категорій.
    mainCategories.forEach(category => {
        renderCategory(category, container);
    });

    // Функція створення елементів для всіх категорій.
    function renderCategory(category, container) {
        const categoryDiv = document.createElement('div');
        categoryDiv.classList.add('category');

        // Назва категорії.
        const categoryName = document.createElement('div');
        categoryName.classList.add('name');
        categoryName.textContent = category.name;
        // Евент-посилання на рецепти.
        categoryName.addEventListener('click', async () => setRecipes(category.id));
        categoryDiv.appendChild(categoryName);

        // Знаходження дочірніх категорій.
        const subcategories = allCategories.filter(subcategory => subcategory.parentCategoryId === category.id);

        // Яйщо є дочірні категорії.
        if (subcategories.length > 0) {
            // Клавіша відкриття списку дочірніх категорій.
            const toggleBtn = document.createElement('span');
            toggleBtn.classList.add('toggle-btn');
            toggleBtn.textContent = '+';

            // Елемент списку дочірніх категорій.
            const subcategoriesDiv = document.createElement('div');
            subcategoriesDiv.style.display = 'none';

            // Створення елементів дочірніх категорій.
            renderSubcategories(subcategories, allCategories, subcategoriesDiv);

            // Евент для клавіші на закриття та відкриття списку дочірніх категорій.
            toggleBtn.addEventListener('click', (event) => {
                event.stopPropagation();
                subcategoriesDiv.style.display = subcategoriesDiv.style.display === 'none' ? 'block' : 'none';
                toggleBtn.textContent = toggleBtn.textContent === '+' ? '-' : '+';
            });

            categoryName.appendChild(toggleBtn);
            categoryDiv.appendChild(subcategoriesDiv);
        }

        container.appendChild(categoryDiv);
    }

    // Рекурсивна функція для створення дочірніх категорій.
    function renderSubcategories(subcategories, allCategories, container) {
        subcategories.forEach(subcategory => {
            renderCategory(subcategory, container);
        });
    }
}


setRecipes();
setMainCategories();
setCategories();