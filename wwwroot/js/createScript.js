
const recipeCateroryElement = document.getElementById('recipeCaterory');
const productPanel = document.querySelector('.product-panel');
const productList = productPanel.querySelector('.product-list');
const ingredientList = document.querySelector('.ingredient-list');
const addProductButton = document.querySelector('.ingredients-panel .add-button');
const productSearch = document.getElementById('productSearch');
/** Масив вибраних продуктів. */
let selectedProducts = [];
/** Всі продукти завантажені з серверу. */
let allProducts = [];

/** Функція отримання можливих категорій для створення рецепту. */
async function getPossibleToSetCategories() {
    const response = await fetch('/categories/last', {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    const categories = await response.json();
    // Додавання категорій на сторінку.
    categories.forEach(category => {
        const option = document.createElement('option');
        option.setAttribute('value', category.name);
        option.textContent = category.name;
        recipeCateroryElement.appendChild(option);
    });
}

/** Функція отримання продуктів. */
async function getProducts() {
    const response = await fetch('/products', {
        method: "GET",
        headers: { "Accept": "application/json" }
    });

    allProducts = await response.json();
    // Сортування за назвою.
    allProducts.sort((a, b) => a.name.localeCompare(b.name));
    // Занесення продуктів у елемент-список вибору продуктів.
    setProducts(allProducts);
}

/** Функція запису продуктів у елемент-список. */
async function setProducts(products) {
    for (const product of products) {
        productList.appendChild(productRow(product));
    }
}

/** Евент-інпут, при вводі у який будуть сортуватись продукти за назвою. */
productSearch.addEventListener('input', () => {
    // Очистка списку продуктів
    productList.textContent = '';
    // Введені символи.
    const searchTerm = productSearch.value.trim().toLowerCase();

    // Якщо поле пошуку порожнє, вивести весь список.
    if (searchTerm.length <= 0) {
        setProducts(allProducts);
    }
    else {
        // Вибірка продуктів, які містять введений символ.
        const filteredProducts = allProducts.filter(product =>
            product.name.toLowerCase().includes(searchTerm)
        );

        setProducts(filteredProducts);
    }
});

/** Функція відкриття панелі продуктів. */
function openProductPanel() {
    productPanel.style.zIndex = "1";
    productPanel.style.opacity = "1";
}

/** Функція закриття панелі продуктів. */
function closeProductPanel() {
    productPanel.style.opacity = "0";
    setTimeout(() => productPanel.style.zIndex = "-1", 500);
}

/** Елемент-рядок у списку продуктів. */
function productRow(product) {
    const row = document.createElement('div');
    row.classList.add('product');
    row.setAttribute('product-id', product.id);

    // Назва продукту.
    const name = document.createElement('p');
    name.textContent = product.name;
    name.classList.add('name');
    row.appendChild(name);

    // Кількість та одиниці вимірювання продукту.
    const amount = document.createElement('p');
    amount.textContent = `${product.amount} ${product.unit}`;
    amount.classList.add('amount');
    row.appendChild(amount);

    // Клавіша додавання продукту до інгредієнтів.
    const addButton = document.createElement('p');
    addButton.innerHTML = `<i class="fa-solid fa-plus"></i>`;
    addButton.classList.add('add-button');
    addButton.addEventListener('click', () => {
        // Якщо продукт вибраний, забороняємо його вибрати ще раз.
        if (row.classList.contains('selected')) return;
        // Занесення продукту до списку вибраних.
        selectedProducts.push(product);
        // Закриття панелі продуктів.
        closeProductPanel();
        // Додавання рядка Інгредієнта.
        ingredientList.appendChild(ingredientRow(product));
        // Встановлення опції вибрано для рядка продукта.
        row.classList.toggle('selected');
        addButton.innerHTML = '<i class="fa-solid fa-check"></i>';
        addButton.classList.toggle('active');
    });
    row.appendChild(addButton);

    // Якщо цей рядок вже вибраний то:
    selectedProducts.forEach(selectedProduct => {
        if (selectedProduct.id == product.id) {
            // Встановлення неможливості вибрати його.
            row.classList.toggle('selected');
            addButton.innerHTML = '<i class="fa-solid fa-check"></i>';
            addButton.classList.toggle('active');
        }
    });

    return row;
}

/** Елемент-рякод інредієнт рецепта. */
function ingredientRow(product) {
    const row = document.createElement('div');
    row.setAttribute('ingredient-id', product.id);
    row.classList.add('ingredient');

    // Назва інгредієнта.
    const name = document.createElement('p');
    name.classList.add('name');
    name.textContent = product.name;
    row.appendChild(name);

    // Елемент-інпут для встановлення кількості інгредієнта.
    const amount = document.createElement('input');
    amount.setAttribute('placeholder', '...');
    amount.setAttribute('type', 'number');
    amount.classList.add('amount');
    amount.value = product.amount;
    if (product.amount != 0)
        row.appendChild(amount);

    // Одиниці вимірювання інгредієнта.
    const unit = document.createElement('p');
    unit.classList.add('unit');
    unit.textContent = product.unit;
    if (product.amount != 0)
        row.appendChild(unit);

    // Клавіша видалення інгредієнта зі списку інгредієнтів рецепта.
    const removeButton = document.createElement('p');
    removeButton.innerHTML = `<i class="fa-solid fa-xmark"></i>`;
    removeButton.classList.add('remove');
    removeButton.addEventListener('click', () => {
        // Видалення елемента-інгредієнт на сторінці.
        row.remove();
        // Видалення зі списку обраних продуктів.
        for (let i = 0; i < selectedProducts.length; i++) {
            if (selectedProducts[i].id === product.id) {
                selectedProducts.splice(i, 1);
                break; 
            }
        }
        console.log(selectedProducts);
        // Встановлення продукту (у списку) активним для вибору так, як і раніше.
        const productRowInList = productList.querySelector(`.product[product-id='${product.id}']`);
        const addButton = productRowInList.querySelector('.add-button')
        addButton.innerHTML = `<i class="fa-solid fa-plus"></i>`;
        addButton.classList.toggle('active');
        productRowInList.classList.toggle('selected');
    });
    row.appendChild(removeButton);

    return row;
}

/** Евенти вставлення фотографії у поле. */
function dragAndDropEvents() {
    const dropContainer = document.getElementById("dropcontainer");
    const fileInput = document.getElementById("photo");

    dropContainer.addEventListener("dragover", (e) => {
        // prevent default to allow drop
        e.preventDefault();
    }, false);

    dropContainer.addEventListener("dragenter", () => {
        dropContainer.classList.add("drag-active");
    });

    dropContainer.addEventListener("dragleave", () => {
        dropContainer.classList.remove("drag-active");
    });

    dropContainer.addEventListener("drop", (e) => {
        e.preventDefault();
        dropContainer.classList.remove("drag-active");
        fileInput.files = e.dataTransfer.files;
        upload();
    });
}

/** Функція завантаження фото на сервер. */
async function upload() {
    const data = new FormData();
    data.append('FileName', document.getElementById('photo').value);
    data.append('Image', document.getElementById('photo').files[0]);

    // Створення запиту, який несе у собі фото.
    const response = await fetch('/recipes/photo', {
        method: "POST",
        body: data
    });

    if (response.ok) {
        const photo = await response.json();
        document.getElementById('photo').style.display = "none";
        document.getElementById('dropcontainer').style.display = "none";
        document.getElementById('photoImg').setAttribute('src', "/photos/" + photo.fileName);
        document.getElementById("double-click").style.display = "flex";
    }
}

/** Після подвійного кліку на завантажене фото, воно прибирається. */
function deleteTemporaryPhoto() {
    document.getElementById('photo').style.display = "none";
    document.getElementById('dropcontainer').style.display = "flex";
    document.getElementById('photoImg').setAttribute('src', "");
    document.getElementById("double-click").style.display = "none";
}

// Виклик функцій. 
getPossibleToSetCategories();
getProducts();
dragAndDropEvents();