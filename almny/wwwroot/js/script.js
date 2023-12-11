let menu = document.querySelector("#menu");
let navbar = document.querySelector(".navbar");
let select_level = document.querySelector("#select_level");
let select_dept = document.querySelector("#select_dept");
let select_dept_class = document.querySelector(".select_dept");
let label_dept = document.querySelector("#label_dept");
let list_sub = document.querySelector(".list-sub");
let themeBtn = document.querySelector("#theme-btn");
let profile = document.querySelector("header .profile");

document.querySelector("#user-btn").onclick = () => {
  profile.classList.toggle("active");
};

menu.onclick = () => {
  menu.classList.toggle("fa-times");
  navbar.classList.toggle("active");
};

window.onscroll = () => {
  menu.classList.remove("fa-times");
  navbar.classList.remove("active");
};

let body = document.body;
let toggleBtn = document.querySelector("#toggle-btn");
let darkMode = localStorage.getItem("dark-mode");

const enabelDarkMode = () => {
  toggleBtn.classList.replace("fa-sun", "fa-moon");
  body.classList.add("dark");
  localStorage.setItem("dark-mode", "enabled");
};

const disableDarkMode = () => {
  toggleBtn.classList.replace("fa-moon", "fa-sun");
  body.classList.remove("dark");
  localStorage.setItem("dark-mode", "disabled");
};

if (darkMode === "enabled") {
  enabelDarkMode();
}

toggleBtn.onclick = (e) => {
  let darkMode = localStorage.getItem("dark-mode");
  if (darkMode === "disabled") {
    enabelDarkMode();
  } else {
    disableDarkMode();
  }
};

//function myFunction() {
//    var result = select_level.value;
//    if (result === 1) {
//        select_dept_class.style.display = "none";
//    } else if (
//        result == 2 ||
//        result == 3
//    ) {
//        select_dept_class.style.display = "block";
//    }else {
//            select_dept_class.style.display = "none";
//        }
//    }
//}

