var slideIndex = 0;
var slidesCount = 0;
var timerId = 0
// остановка и включение автопереключения слайдов при наведении мыши
function onMouseEnter() {
    clearInterval(timerId);
    console.log("clear "+timerId)
}
function onMouseLeave() {
    timerId = setInterval(SlideUp, 5000);
    console.log("set " + timerId)
}

// инициализация параметров слайдшоу
function onSlideshowInit() {
    $(".slide").each(function (index) {
        slidesCount++;
    });
    slideshow.onmouseenter = onMouseEnter;
    slideshow.onmouseleave = onMouseLeave;
}
document.addEventListener("DOMContentLoaded", onSlideshowInit);

// обновление слайдшоу, по таймеру
function onSlideUpdate() {
    
    $(".slide").each(function (index) {
        if (index == slideIndex) {
            $(this).css("display", "block");
        }
        else {
            $(this).css("display", "none");
        }
    });
    
}
document.addEventListener("DOMContentLoaded", onSlideUpdate);
timerId = setInterval(SlideUp, 5000);
console.log("set " + timerId)


// функции для ручного переключения слайдов вперед и назад
function SlideUp() {
    if (slideIndex + 1 >= slidesCount) {
        slideIndex = 0;
    }
    else {
        slideIndex++;
    }
    onSlideUpdate();
}
function SlideDown() {
    console.log(slideIndex);
    if (slideIndex <= 0) {
        slideIndex = slidesCount - 1;
    }
    else {
        slideIndex--;
    }
    onSlideUpdate();
}
