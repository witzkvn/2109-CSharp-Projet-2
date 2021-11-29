let mobileNavOpenBtn = document.getElementById("mobileNavOpen");
let mobileNavCloseBtn = document.getElementById("mobileNavClose");
let navMenu = document.getElementById("nav-menu");

mobileNavOpenBtn.addEventListener("click", () => {
    navMenu.classList.add("active");
    console.log("click");
})

mobileNavCloseBtn.addEventListener("click", () => {
    navMenu.classList.remove("active");
})