const getDataFromRegister = () => {
    const username = document.querySelector("#username").value;
    const password = document.querySelector("#password2").value;
    const firstname = document.querySelector("#firstname").value;
    const lastname = document.querySelector("#lastname").value;
    return { username, password, firstname, lastname };
}

const getDataFromUpdate = () => {
    const username = document.querySelector("#usernameOnUpdate").value;
    const password = document.querySelector("#passwordOnUpdate").value;
    const firstname = document.querySelector("#firstnameOnUpdate").value;
    const lastname = document.querySelector("#lastnameOnUpdate").value;
    const userId = sessionStorage.getItem("id");
    return { userId, username, password, firstname, lastname };
}

const getDataFromLogin = () => {
    const username = document.querySelector("#nameInput").value;
    const password = document.querySelector("#passwordInput").value;
    const firstname = "no-name";
    const lastname = "no-name";
    return { username, password, firstname, lastname };
}

const login = async () => {
    const user = getDataFromLogin();

    try {
        const data = await fetch(`api/Users/login/?username=${user.username}&password=${user.password}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (data.status === 204) {
            throw new Error("user not found");
        }
        if (data.status === 400) {
            throw new Error("all fields are required");
        }

        const dataLogin = await data.json();
        sessionStorage.setItem("id", dataLogin.id);
        window.location.href = 'Products.html';
    } catch (error) {
        console.error(error);
        alert(error);
    }
}

const newUser = () => {
    const container = document.querySelector(".container");
    container.classList.remove("container");
}

const seeTheUpdateUser = () => {
    const container = document.querySelector(".containerOfUpdate");
    container.classList.remove("containerOfUpdate");
}

const register = async () => {
    const user = getDataFromRegister();

    try {
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailPattern.test(user.username)) {
            alert("The username must be a valid email address.");
            return;
        }
        if (!user.username || !user.password) {
            throw new Error("username and password are required");
        }
        if (user.username.length > 50) {
            throw new Error("the username must be smaller than 50 characters");
        }
        if (user.password.length > 20) {
            throw new Error("the password must be smaller than 20 characters");
        }

        const postFromData = await fetch("api/Users", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        });
        if (postFromData.status == 409) {
            alert("duplicate user")
        }
        else if (postFromData.status === 404) {
            alert('all fields are required');
        } else if (postFromData.status === 400) {
            alert('your password is not strong enough');
        } else {
            alert("user registered successfully!!!");
        }
    } catch (error) {
        alert(error);
    }
}

const checkScore = async () => {
    const password = document.querySelector('#password2').value;
    const scoreFromData = await fetch("api/Users/password", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(password)
    });

    const dataPost = await scoreFromData.json();
    let score = document.querySelector("#score");
    score.value = dataPost;
}

const updateUser = async () => {
    const user = getDataFromUpdate();
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(user.username)) {
        alert("The username must be a valid email address.");
        return;
    }

    try {
        if (!user.username || !user.password) {
            throw new Error("username and password are required");
        }
        if (user.username.length > 50) {
            throw new Error("the username must be smaller than 50 characters");
        }
        if (user.password.length > 20) {
            throw new Error("the password must be smaller than 20 characters");
        }

        const updateFromData = await fetch(`api/Users/${sessionStorage.getItem("id")}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        });
         if (updateFromData.status === 400) {
              alert('your password is not strong enough');
        } else if (updateFromData.status === 404) {
            alert('all fields are required');
        } else {
            alert(`user ${sessionStorage.getItem("id")} updated`);
            window.location.href = "Products.html";
        }
    } catch (error) {
        alert(error);
    }
}

const logOut = () => {
    sessionStorage.removeItem("id");
    sessionStorage.setItem("Cart", JSON.stringify([]));
    window.location.href = "Products.html";
}