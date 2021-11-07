jQuery(document).ready(function () {
    setTimeout(function () {
        $(".alertMessage").fadeOut(3400);
    }, 5000)
})

$(function () {
    setActiveClass();
})

function setActiveClass() {
    var path = window.location.pathname;
    var links = $(".nav-link");
    for (let link of links) {
        if (link.attributes.href.value == path) {
            link.classList.add("active")
        }
    }
}