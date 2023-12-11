


async function getRecipes(){
    const response = await fetch('/recipes', {
        method: "GET",
        headers: {"Accept" : "application/json"}
    });

    const recipes = await response.json();
    recipes.forEach(recipe => document.querySelector('body').append(recipe.name));
}

getRecipes();