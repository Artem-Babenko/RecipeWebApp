
let allCategories;
let recipesOnPage;

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
        setPathToCategory(result.category);
        recipes = result.recipes;
        recipesOnPage = recipes;
    }
    else {
        // Виведення всіх цепептів.
        const response = await fetch('/recipes', {
            method: "GET",
            headers: { "Accept": "application/json" }
        });

        // Приховування панелей інформації.
        document.querySelector('.content-name').style.display = "none";
        document.querySelector('.content .path').style.display = "none";

        recipes = await response.json();
        recipesOnPage = recipes;
    }

    // Прибирання клавіш сортування при завантажені рецептів з серверу.
    document.querySelectorAll('.sortbar .sort-btn').forEach(sortbtn => {
        sortbtn.classList.remove('active');
        sortbtn.classList.remove('from-top');
        sortbtn.classList.remove('from-bottom');
        sortbtn.querySelector('i').classList.remove('fa-caret-down');
        sortbtn.querySelector('i').classList.remove('fa-caret-up');
    });

    const main = document.querySelector('main .recipes');
    main.textContent = "";
    // На основі кожного рецепту створити контейнер з його короткою інформацією.
    recipes.forEach(recipe => main.append(recipeItem(recipe)));
}

/** Виведення рецептів відносно налаштувань сорт бару. */
function showSortedRecipes(sortMethod, sortDirection) {
    // Сортування.
    const sortedRecipes = recipesOnPage;
    if (sortMethod && sortDirection) {
        switch (sortMethod) {
            case 'датою':
                sortedRecipes.sort((a, b) => {
                    return sortDirection === 'from-bottom' ? a.createDate - b.createDate : b.createDate - a.createDate;
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

    const main = document.querySelector('main .recipes');
    main.textContent = "";
    // На основі кожного рецепту створити контейнер з його короткою інформацією.
    sortedRecipes.forEach(recipe => main.append(recipeItem(recipe)));

    // Функція для обчислення середнього рейтингу коментарів.
    function calculateAverageRating(comments) {
        if (comments.length === 0) {
            return 0; // Якщо коментарів немає, повертаємо 0.
        }

        const totalRating = comments.reduce((sum, comment) => sum + comment.rating, 0);
        return totalRating / comments.length;
    }
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
                showSortedRecipes(sortBy, 'from-bottom');
            }
            // Якщо клавіша сортує з низу у верх.
            else if (button.classList.contains('from-bottom')) {
                // Встановлюєм сортування з верху у низ.
                button.classList.remove('from-bottom')
                button.classList.add('from-top');
                icon.classList.remove('fa-caret-up');
                icon.classList.add('fa-caret-down');
                showSortedRecipes(sortBy, 'from-top');
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
                showSortedRecipes(sortBy, 'from-top');
            }
        });
    });
}


/** Контейнер рецепту для відобращення короткої інформації про нього. */
function recipeItem(recipe) {
    const item = document.createElement('div');
    item.classList.add("item");

    // Картинка.
    const img = document.createElement('img');
    img.src = "photos/" + recipe.photoName;
    item.append(img);

    // Назва.
    const name = document.createElement('div');
    name.classList.add("name");
    name.append(recipe.name);
    item.append(name);

    // Категорія.
    const category = document.createElement('div');
    category.classList.add('category');
    category.textContent = recipe.category.name;
    item.appendChild(category);

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

    // Додавання блоку зірок до основного елементу.
    item.append(stars);

    // Короткий опис.
    const maxWords = 25;

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
    item.append(description);

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

    // Час приготування.
    const timeSpanElement = document.createElement('div');
    const timeSpan = formatTime(recipe.cookingTime);
    timeSpanElement.innerHTML = `<i class="fa-regular fa-clock"></i> ${timeSpan}`
    footer.appendChild(timeSpanElement);

    item.appendChild(footer);

    // Івенти.
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
            button.addEventListener('click', (event) => {
                event.preventDefault();
                setRecipes(category.id);
            })
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
        allCategories = categories;

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


/** Встановлення шляху по категоріям. */
function setPathToCategory(category) {
    // Початково додаємо ім'я поточної категорії до шляху
    const path = [category.name];

    findPathToRoot(category);

    // Рекурсивно визначає шлях до кореневої категорії.
    function findPathToRoot(currentCategory) {
        const parentCategory = allCategories.find(c => c.id === currentCategory.parentCategoryId);

        // Якщо я батьківська то: 
        if (parentCategory) {
            // Додає ім'я батьківської категорії до початку шляху.
            path.unshift(parentCategory.name); 
            // Рекурсивний виклик для батьківської категорії.
            findPathToRoot(parentCategory); 
        }
    }

    // Знаходження та відображення елемента ці шляхом.
    const pathContainer = document.querySelector('.content .path');
    pathContainer.style.display = 'flex';
    pathContainer.textContent = '';

    // Клавіша "Головна"
    const mainSpan = document.createElement('span');
    mainSpan.classList.add('category-link');
    mainSpan.innerHTML = 'Головна';
    // Евент-клік на перезавантаження сторінки.
    mainSpan.addEventListener('click', () => setRecipes());

    pathContainer.append(mainSpan);

    // Для кожної назви категорії.
    path.forEach(word => {
        // Елемент-назва категорії.
        const span = document.createElement('span');
        span.classList.add('category-link');
        span.innerHTML = word;
        span.addEventListener('click', (event) => {
            event.preventDefault();
            setRecipes(allCategories.find(c => c.name === word).id);
        });

        // Символ стрілки.
        const arrowIcon = document.createElement('i');
        arrowIcon.classList.add('fa-solid', 'fa-angle-right');

        pathContainer.append(arrowIcon);
        pathContainer.append(span);
    });
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

/** Встановлення користувача на клавішу профілю. */
function setUser() {

    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
    }

    const userInfoElement = document.querySelector('.user-info');
    const userNameEncoded = getCookie('UserName');
    const userSurnameEncoded = getCookie('UserSurname');

    if (userNameEncoded && userSurnameEncoded) {
        const userName = decodeURIComponent(userNameEncoded);
        const userSurname = decodeURIComponent(userSurnameEncoded);

        userInfoElement.innerHTML = `<i class="fa-regular fa-user"></i> ${userName} ${userSurname}`;
        userInfoElement.href = '/profile'
    } else {
        userInfoElement.innerHTML = '<i class="fa-regular fa-user"></i> Увійти';
        userInfoElement.href = '/login'
    }
}

/** Обробник пошуку. */
function searchHandler() {
    const searchInput = document.getElementById('search');

    searchInput.addEventListener('input', () => {
        const searchTerm = searchInput.value.toLowerCase();

        // Фільтрація рецептів за введеним користувачем текстом.
        const filteredRecipes = recipesOnPage.filter(recipe => {
            // Порівняння з назвою, описом, складністю, інгредієнтами та іншими властивостями рецепту.
            return (
                recipe.name.toLowerCase().includes(searchTerm) ||
                recipe.description.toLowerCase().includes(searchTerm) ||
                recipe.difficulty.toLowerCase().includes(searchTerm) ||
                recipe.ingredients.some(ingredient => ingredient.name.toLowerCase().includes(searchTerm)) ||
                recipe.cookingSteps.some(cookingStep => cookingStep.title.toLowerCase().includes(searchTerm))
            );
        });

        // Прибирання клавіш сортування при завантажені рецептів з серверу.
        document.querySelectorAll('.sortbar .sort-btn').forEach(sortbtn => {
            sortbtn.classList.remove('active');
            sortbtn.classList.remove('from-top');
            sortbtn.classList.remove('from-bottom');
            sortbtn.querySelector('i').classList.remove('fa-caret-down');
            sortbtn.querySelector('i').classList.remove('fa-caret-up');
        });

        // Встановлення відфільтрованих рецептів на сторінці.
        const main = document.querySelector('main .recipes');
        main.textContent = "";
        filteredRecipes.forEach(recipe => main.append(recipeItem(recipe)));
    });
}


setRecipes();
setMainCategories();
setCategories();
setUser();
sortBar();
searchHandler();