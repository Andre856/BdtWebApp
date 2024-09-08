
function setItem(key, value) {
    localStorage.setItem(key, value);
}

function getItem(key, value) {
    localStorage.getItem(key, value);
}

function removeItem(key, value) {
    localStorage.removeItem(key, value);
}

function detectMobile() {
    if (window.innerWidth <= 800) {
        return true;
    } else {
        return false;
    }
};