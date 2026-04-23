const userEmail = document.querySelector("#userName")
const firstName = document.querySelector("#firstName")
const lastName = document.querySelector("#lastName")
const password = document.querySelector("#password")
const show = document.querySelector(".show");
show.style.display = "none"

function toggleForms() {
    const newUser = document.querySelector('.newUser');
    const existUser = document.querySelector('.existUser');
    newUser.classList.toggle('hidden');//מחליף את ההגדרה של ה קלאס שתיהיה מוסתרת או גלויה ברגע שלוחצים מחליף בינהם אם היתה מוסתרת תיהיה גלויה והשני להיפך - ולהיפך
    existUser.classList.toggle('hidden');
}
function validateEmail(email) {
    return email.includes("@") && email.includes(".");
}
const ShowSrength = (level) => {
    show.style.display = "block"
    show.value = level * 25;
}
const CheckPassword = async () => {
    try {
        const Password = {
            ThePassword: password.value,
            Level: 0
        };
        const response = await fetch('https://localhost:44324/api/Password', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(Password)
        });
        if (response.ok) {
            const thePassword = await response.json();
            return thePassword
        }
        else {
            alert("בדיקת הססמא אינה עובדת כעת, מצטערים😒");
        }
    } catch (err) {
        alert(err);
    }
}
const PasswordStrength = async () => {
    if (password.value != "") {
        const passwordAfterChack = await  CheckPassword();
        ShowSrength(passwordAfterChack.level)

    }
}

const register = async () => {
    try {
            if (!validateEmail(userEmail.value)) {
                    alert("כתובת האימייל אינה תקינה");
                    return;
            }

            if (password.value.length < 4 || password.value.length > 20) {
                alert("הסיסמה חייבת להיות בין 4 ל-20 תווים");
                return;
            }
            const newUser = {
                UserId: 0,
                UserEmail: userEmail.value,
                UserFirstName: firstName.value,
                UserLastName: lastName.value,
                UserPassword: password.value
            }
            const response = await fetch('https://localhost:44324/api/Users', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newUser)
            });
            if (response.status == 400) {
                alert("כנראה ססמתך חלשה 😒 מצטערים, הרשמתך לא נקלטה. זה רק לטובתך כדי להגן על החשבון שלך")
            }
            else {
                const dataPost = await response.json();
                alert("הרשמתך נקלטה בהצלחה")
            }
    } catch (err) {
        alert(err)
    }
}


const loginUserEmail = document.querySelector("#userNameR")
const loginUserPassword = document.querySelector("#passwordR")

const login = async () => {
    try {
        const loginUser = {
            UserEmail: loginUserEmail.value,
            UserPassword: loginUserPassword.value
        }
        const response = await fetch('https://localhost:44324/api/Users/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(loginUser)
        });
        if (response.status == 204) { 
            alert("אינך קיים במערכת נא הרשם")
        }

        else {
            alert("התחברת בהצלחה")
            const currentUser = await response.json();
            sessionStorage.setItem("currentUser", JSON.stringify(currentUser))
            window.location.href = "update.html"
        }
    } catch (err) {
        alert(err)
    }
}


