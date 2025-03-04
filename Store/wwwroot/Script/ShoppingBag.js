let cart = JSON.parse(sessionStorage.getItem("Cart")) || [];
let totalPrice = 0;

addEventListener("load", () => {
    drawProductsInCart();
});

const drawProductsInCart = () => {
    cart.forEach((p) => {
        totalPrice += p.price;
        drawOneProductInCart(p);
    });
    totalAmountAndPrice(totalPrice);
};

const totalAmountAndPrice = (total) => {
    document.getElementById("totalAmount").innerHTML = `$${total}`;
    document.getElementById("itemCount").innerHTML = cart.length;
};

const drawOneProductInCart = (product) => {
    const url = `./Image/bags/${product.picture}`;
    const tmp = document.getElementById("temp-row");
    const cloneProductInCart = tmp.content.cloneNode(true);

    cloneProductInCart.querySelector(".image").style.backgroundImage = `url(${url})`;
    cloneProductInCart.querySelector(".itemName").innerText = product.productName;
    cloneProductInCart.querySelector(".price").innerText = `$${product.price}`;
    cloneProductInCart.querySelector(".expandoHeight").addEventListener("click", () => {
        deleteProductInCart(product.id);
    });

    document.querySelector("tbody").appendChild(cloneProductInCart);
};

const deleteProductInCart = (id) => {
    const pid = cart.findIndex(c => c.id === id);
    cart.splice(pid, 1);
    sessionStorage.setItem("Cart", JSON.stringify(cart));
    document.querySelector("tbody").innerHTML = "";
    totalPrice = 0;
    drawProductsInCart();
};

const placeOrder = async () => {
    if (!JSON.parse(sessionStorage.getItem("id"))) {
        alert("you have not login yet");
        window.location.href = "login.html";
    } else {
        const order = createOrder();
        if (order.orderItems.length === 0) {
            alert("your cart is empty");
            window.location.href = "products.html";
        }

        try {
            const data = await fetch("api/Orders", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(order)
            });

            const orderData = await data.json();
            if (data.status === 401) {
                alert("YOU CAN NOT COMPLETE THIS ORDER 😣😬");
            } else if (data.status === 204) {
                alert("Your cart is empty");
            } else if (data.status === 400) {
                alert("Your order cannot be completed");
            } else {
                alert(`your order ${orderData.id} was placed successfully!!!`);
                sessionStorage.setItem("Cart", JSON.stringify([]));
                window.location.href = "products.html";
            }
        } catch (error) {
            console.error(error);
            alert("An error occurred while placing the order.");
        }
    }
};

const createOrder = () => {
    const orderItemsList = cart.map(c => ({ productId: c.id, quantity: 1 }));

    return {
        orderDate: new Date(),
        orderSum: totalPrice,
        userId: JSON.parse(sessionStorage.getItem("id")) || "",
        orderItems: orderItemsList
    };
};

const signOut = () => {
    sessionStorage.removeItem("id");
    sessionStorage.setItem("Cart", JSON.stringify([]));
    window.location.href = "Products.html";
};