function setMenuItemActive() {
    jQuery("li>a").each(function () {
        if (window.location.pathname == jQuery(this).attr("href")) {
            jQuery(this).addClass("menu-item-active");
        }
    });
}
document.addEventListener("DOMContentLoaded", setMenuItemActive);

function SetNewEntry(files) {
    var img = document.getElementById('filmimage');
    img.src = window.URL.createObjectURL(files[0]);
    //img.onload = function () {
        //window.URL.revokeObjectURL(this.src);
    //}
}


var results = $("#Results");
var onBegin = function () {
    results.html("<img src=\"/images/ajax-loader.gif\" alt=\"Loading\" />");
};

var onComplete = function () {
    results.html("");
};

var onSuccess = function (context) {
    alert(context);
};

var onFailed = function (context) {
    alert("Failed");
};

var seatsIds = []
function setSeatsClickable() {
    jQuery(".seat").click(function () {
        var element = jQuery(this);
        if (!element.hasClass("booked")) {
            if (!element.hasClass("active-seat")) {
                element.addClass("active-seat");
                var row = element.attr("row");
                var number = element.attr("number");
                onClickSeat(row, number);
            }
            else {
                element.removeClass("active-seat");
                var row = element.attr("row");
                var number = element.attr("number");
                onDeactivateSeat(row, number);
            }
        }
    });
}
function onClickSeat(row, number) {
    var cost = jQuery(".session-cost").attr("value");
    console.log(cost);
    var content = "<p>" + row + " ряд</p>";
    content += "<p>" + number + " место</p>";
    content += "<p>" + cost + "р</p>";
    var id = "ticket-row" + row + "-number" + number;
    var value = row * 1000 + Number.parseInt(number);
    jQuery(".make-order-button").before("<div class='ticket' id='" + id + "'>" + content + "<input type='hidden' name='"+id+"' value=" + value + "></div>");
    seatsIds.push(id);
}
function onDeactivateSeat(row, number) {
    var id = "ticket-row" + row + "-number" + number;
    var index = seatsIds.indexOf(id);
    id = "#" + id;
    jQuery(id).detach();
    seatsIds.splice(index);
}
document.addEventListener("DOMContentLoaded", setSeatsClickable);





function onUpButtonClick() {
    jQuery("html, body").animate({ scrollTop: 0 }, 400);
}
function hideUpButtonClick() {
    var scroll = jQuery(document).scrollTop();
    if (scroll <= 150) {
        jQuery(".up-button").addClass("up-button-hidden");
        jQuery(".up-button-mobile").addClass("up-button-hidden");
    }
    else {
        jQuery(".up-button").removeClass("up-button-hidden");
        jQuery(".up-button-mobile").removeClass("up-button-hidden");
    }
}
document.addEventListener("scroll", hideUpButtonClick);
document.addEventListener("DOMContentLoaded", hideUpButtonClick);


//$(function () {

//    $("#prSecond").click(function (e) {
//        e.preventDefault();
//        $('#' + $(this).data("target")).load($(this).attr("href"));
//        $(".profile-menu").removeClass("active");
//        $(".second").addClass("active");
//        $("#prSecond").addClass("menu-item-active");
//        $("#prSecond").blur();
//    });
    
//});

$(function () {

    $("#prThird").click(function (e) {
        e.preventDefault();
        $('#' + $(this).data("target")).load($(this).attr("href"));
        $(".profile-menu").removeClass("active");
        $(".third").addClass("active");
        $("#prThird").addClass("menu-item-active");
        $("#prThird").blur();
    });

});

function OnLinkClick(e) {
    e.preventDefault();
    $('#' + $(this).data("target")).load($(this).attr("href"));
    $(".profile-menu").removeClass("active");
}

$('#message').richText({

    // text formatting
    bold: true,
    italic: true,
    underline: true,

    // text alignment
    leftAlign: true,
    centerAlign: true,
    rightAlign: true,

    // lists
    ol: true,
    ul: true,

    // title
    heading: true,

    // colors
    fontColor: true,

    // uploads
    imageUpload: true,
    fileUpload: false,

    // link
    urls: true,

    // tables
    table: false,

    // code
    removeStyles: false,
    code: false,

    // colors
    colors: [],

    // media
    videoEmbed: false,

    translations: {
        'title': 'Title',
        'white': 'White',
        'black': 'Черный',
        'brown': 'Brown',
        'beige': 'Beige',
        'darkBlue': 'Dark Blue',
        'blue': 'Blue',
        'lightBlue': 'Light Blue',
        'darkRed': 'Dark Red',
        'red': 'Red',
        'darkGreen': 'Dark Green',
        'green': 'Green',
        'purple': 'Purple',
        'darkTurquois': 'Dark Turquois',
        'turquois': 'Turquois',
        'darkOrange': 'Dark Orange',
        'orange': 'Orange',
        'yellow': 'Yellow',
        'imageURL': 'Image URL',
        'fileURL': 'File URL',
        'linkText': 'Link text',
        'url': 'URL',
        'size': 'Size',
        'responsive': 'Responsive',
        'text': 'Text',
        'openIn': 'Open in',
        'sameTab': 'Same tab',
        'newTab': 'New tab',
        'align': 'Align',
        'left': 'Left',
        'center': 'Центр',
        'right': 'Right',
        'rows': 'Rows',
        'columns': 'Columns',
        'add': 'Add',
        'pleaseEnterURL': 'Please enter an URL',
        'videoURLnotSupported': 'Video URL not supported',
        'pleaseSelectImage': 'Please select an image',
        'pleaseSelectFile': 'Please select a file',
        'bold': 'Bold',
        'italic': 'Italic',
        'underline': 'Underline',
        'alignLeft': 'Align left',
        'alignCenter': 'Align centered',
        'alignRight': 'Align right',
        'addOrderedList': 'Add ordered list',
        'addUnorderedList': 'Add unordered list',
        'addHeading': 'Add Heading/title',
        'addFont': 'Выбрать шрифт',
        'addFontColor': 'Выбрать цвет шрифта',
        'addImage': 'Прикрепить изображение',
        'addVideo': 'Прикрепить видео',
        'addFile': 'Прикрепить файл',
        'addURL': 'Добавить ссылку',
        'addTable': 'Вставить таблицу',
        'removeStyles': 'Убрать стили',
        'code': 'Показать HTML код',
        'undo': 'Отменить',
        'redo': 'Вернуть',
        'close': 'Закрыть'
    },

    // dropdowns
    fileHTML: '',
    imageHTML: ''

    
});

//$(document).ready(function () {
//    $('form').find("input[type=textarea], input[type=password], textarea").each(function (ev) {
//        if (!$(this).val()) {
//            $(this).attr("placeholder", "Type your answer here");
//        }
//    });
//});