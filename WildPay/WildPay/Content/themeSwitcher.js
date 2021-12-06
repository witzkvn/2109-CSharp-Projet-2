const root = document.documentElement;

const getBlackColors = () => {
    const bgBlackLight = getComputedStyle(root).getPropertyValue("--bg-black-light");
    const bgBlackMedium = getComputedStyle(root).getPropertyValue("--bg-black-medium");
    const bgBlackDark = getComputedStyle(root).getPropertyValue("--bg-black-dark");
    const bgBlackDarker = getComputedStyle(root).getPropertyValue("--bg-black-darker");
    return [bgBlackLight, bgBlackMedium, bgBlackDark, bgBlackDarker];
}

const getWhiteColors = () => {
    const bgWhiteLight = getComputedStyle(root).getPropertyValue("--bg-white-light");
    const bgWhiteMedium = getComputedStyle(root).getPropertyValue("--bg-white-medium");
    const bgWhiteDark = getComputedStyle(root).getPropertyValue("--bg-white-dark");
    const bgWhiteDarker = getComputedStyle(root).getPropertyValue("--bg-white-darker");
    return [bgWhiteLight, bgWhiteMedium, bgWhiteDark, bgWhiteDarker];
}

const getTextColors = () => {
    const textWhite = getComputedStyle(root).getPropertyValue("--text-white");
    const textGrey = getComputedStyle(root).getPropertyValue("--text-grey");
    const textBlack = getComputedStyle(root).getPropertyValue("--text-black");
    return [textWhite, textGrey, textBlack]
}

const toggleDarkTheme = (theme) => {
    const [bgWhiteLight, bgWhiteMedium, bgWhiteDark, bgWhiteDarker] = getWhiteColors();
    const [bgBlackLight, bgBlackMedium, bgBlackDark, bgBlackDarker] = getBlackColors();
    const [textWhite, textGrey, textBlack] = getTextColors();

    if (theme && theme === "light") {
        root.style.setProperty("--bg-light", bgWhiteLight);
        root.style.setProperty("--bg-medium", bgWhiteMedium);
        root.style.setProperty("--bg-dark", bgWhiteDark);
        root.style.setProperty("--bg-darker", bgWhiteDarker);
        root.style.setProperty("--text-color", textBlack);
    } else if (theme && theme === "dark") {
        root.style.setProperty("--bg-light", bgBlackLight);
        root.style.setProperty("--bg-medium", bgBlackMedium);
        root.style.setProperty("--bg-dark", bgBlackDark);
        root.style.setProperty("--bg-darker", bgBlackDarker);
        root.style.setProperty("--text-color", textWhite);

    } else {
        return
    }
}

let moonBtn = document.getElementById("moonBtn");
let sunBtn = document.getElementById("sunBtn");
let bodyEl = document.getElementsByTagName("body")[0];

const handleThemeSwitch = () => {
    const theme = localStorage.getItem("theme") || "light";
    if (theme == "light") {
        localStorage.setItem("theme", "dark");
        bodyEl.classList.add("dark");
        moonBtn.style.display = "none";
        sunBtn.style.display = "flex";
        toggleDarkTheme("dark");
    } else {
        localStorage.setItem("theme", "light");
        bodyEl.classList.remove("dark");
        moonBtn.style.display = "flex";
        sunBtn.style.display = "none";
        toggleDarkTheme("light");
    }
}

// init
if (localStorage.getItem("theme")) {
    let theme = localStorage.getItem("theme");
    if (theme == "dark") {
        bodyEl.classList.add("dark");
        toggleDarkTheme("dark");
        moonBtn.style.display = "none";
    } else {
        bodyEl.classList.remove("dark");
        toggleDarkTheme("light");
        sunBtn.style.display = "none";
    }
} else {
    localStorage.setItem("theme", "light");
    sunBtn.style.display = "none";
}


let themeBtn = document.getElementById("themeBtn");
themeBtn.addEventListener("click", handleThemeSwitch);