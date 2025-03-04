const categories = [];
sessionStorage.setItem("Categories", JSON.stringify(categories));

addEventListener("load", async () => {
    getProductsList();
    getCategories();
    const updateCart = JSON.parse(sessionStorage.getItem("Cart")) || [];
    document.querySelector("#ItemsCountText").innerHTML = updateCart.length;
});

const getAllFilters = () => {
    document.getElementById('ProductList').innerHTML = "";
    const filter = {
        position: 0,
        skip: 0,
        minPrice: document.querySelector('#minPrice').value,
        maxPrice: document.querySelector('#maxPrice').value,
        desc: document.querySelector('#nameSearch').value,
        categoryIds: JSON.parse(sessionStorage.getItem("Categories")) || []
    };
    return filter;
};

const getProductsList = async () => {
    const filters = getAllFilters();
    let url = `api/Products/?position=${filters.position}&skip=${filters.skip}`;

    if (filters.desc) {
        url += `&desc=${filters.desc}`;
    }
    if (filters.minPrice) {
        url += `&minPrice=${filters.minPrice}`;
    }
    if (filters.maxPrice) {
        url += `&maxPrice=${filters.maxPrice}`;
    }
    if (filters.categoryIds.length) {
        filters.categoryIds.forEach(id => {
            url += `&categoryIds=${id}`;
        });
    }

    try {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (response.status === 204) {
            alert("there are no products");
        } else {
            const productData = await response.json();
            console.log(productData);
            drawProducts(productData);
        }
    } catch (error) {
        console.log(error);
    }
};

const drawProducts = (products) => {
    products.forEach(drawOneProduct);
};

const drawOneProduct = (product) => {
    const tmp = document.getElementById('temp-card');
    const cloneProduct = tmp.content.cloneNode(true);
    cloneProduct.querySelector('img').src = `./Image/bags/${product.picture}`;
    cloneProduct.querySelector('h1').textContent = product.productName;
    cloneProduct.querySelector('.price').innerText = `$${product.price}`;
    cloneProduct.querySelector('.description').innerText = product.description;
    cloneProduct.querySelector('button').addEventListener("click", () => { addToCart(product) });
    document.getElementById('ProductList').appendChild(cloneProduct);
};

const getCategories = async () => {
    try {
        const response = await fetch('api/Categories', {
            method: 'GET',
            headers: {
                "Content-Type": "application/json"
            }
        });

        const categories = await response.json();

        if (response.status === 204) {
            alert("There are no categories");
        } else {
            drawCategories(categories);
        }
    } catch (error) {
        console.log(error);
    }
};

const drawCategories = (categories) => {
    categories.forEach(drawOneCategory);
};

const drawOneCategory = (category) => {
    console.log(category);
    const tmp = document.getElementById('temp-category');
    const cloneCategory = tmp.content.cloneNode(true);
    cloneCategory.querySelector('.opt').addEventListener("change", () => { chooseCategories(category.id) });
    cloneCategory.querySelector('.OptionName').innerText = category.categoryName;
    document.getElementById('categoryList').appendChild(cloneCategory);
};

const chooseCategories = (cId) => {
    let currCategories = JSON.parse(sessionStorage.getItem("Categories"));
    const cindex = currCategories.indexOf(cId);
    if (cindex === -1) {
        currCategories.push(cId);
    } else {
        currCategories.splice(cindex, 1);
    }
    sessionStorage.setItem("Categories", JSON.stringify(currCategories));
    getProductsList();
};

const addToCart = (product) => {
    let updateCart = JSON.parse(sessionStorage.getItem("Cart")) || [];
    updateCart.push(product);
    sessionStorage.setItem("Cart", JSON.stringify(updateCart));
    document.querySelector("#ItemsCountText").innerHTML = updateCart.length;
};

const enterToMyAccount = () => {
    if (!JSON.parse(sessionStorage.getItem("id"))) {
        alert("you have not logged in yet");
        window.location.href = "login.html";
    } else {
        window.location.href = "userDetails.html";
    }
};