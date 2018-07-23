
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

function ChangeImage(files) {
    var img = document.getElementById('profile-image');
    img.src = window.URL.createObjectURL(files[0]);
}

$('#film-rating').rating();
$('#datetimepicker4').datetimepicker({
    format: 'L',
    locale: 'ru',
    useCurrent: false
});

//$('#history-table').footable({
//    "empty": "Заказы отсутсвтуют",
//    "columns": $.get('/manage/HistoryColumns'),
//    "rows": $.get('/manage/HistoryRows')
//});
//var results = $("#Results");
//var onBegin = function () {
//    results.html("<img src=\"/images/ajax-loader.gif\" alt=\"Loading\" />");
//};

//var onComplete = function () {
//    results.html("");
//};

//var onSuccess = function (context) {
//    alert(context);
//};

//var onFailed = function (context) {
//    alert("Failed");
//};

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
    var id = "ticket-row" + row + "-number" + number;
    var value = row * 1000 + Number.parseInt(number);
    var ticketRow = $('#row-' + row);
    if ($(ticketRow).hasClass('d-none'))
        $(ticketRow).removeClass('d-none');
    var content = "<span id='" + id + "'>" +" " + number + "<input type='hidden' name='" + id + "' value=" + value + "></span>";
    $(ticketRow).append(content);
    seatsIds.push(id);
    var cost = $(".session-cost").attr("value");
    var totalCost = Number($('#total-cost').text()) + Number(cost);
    $('#total-cost').text(totalCost);
        
}
function onDeactivateSeat(row, number) {
    var id = "ticket-row" + row + "-number" + number;
    var index = seatsIds.indexOf(id);
    id = "#" + id;
    jQuery(id).detach();
    seatsIds.splice(index);
    if ($('#row-' + row).find('span').length == 0)
        $('#row-' + row).addClass('d-none');
    var cost = $(".session-cost").attr("value");
    var totalCost = $('#total-cost').text() - cost;
    $('#total-cost').text(totalCost);
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


// отправка get запроса без параметров
$('#favorite').click(function (e) {
    e.preventDefault();
    $.get('/Home/ToggleFavorite/' + $('#flag').data("film-id"));
    $('#flag').toggleClass('fa-bookmark').toggleClass('fa-bookmark-o');
    $('.fa-bookmark').attr('title', 'Убрать из избранного');
    $('.fa-bookmark-o').attr('title', 'Добавить в избранное');
});

$('.session-date').click(function (e) {
    e.preventDefault();
    $('#' + $(this).data("target")).load($(this).attr("href"));
    $('.session-date').removeClass('selected');
    $(this).addClass('selected');
    $('#datetimepicker4').datetimepicker('hide');
    if ($('#session-drop').hasClass('selected'))
        $('#session-drop').removeClass('selected');
    if ($(this).parent().hasClass('session-dropdown'))
        $('#session-drop').addClass('selected');
    $('.date-picker').removeClass('selected');
    //$('#datetimepicker4').datetimepicker('viewDate', "01.01.1990 0:00:00");
});

$('.session-link').click(function (e) {
    if ($(this).hasClass('disabled')) {
        console.log('enter');
        e.preventDefault();
    }
});

$('#session-drop').click(function (e) {
    $('#datetimepicker4').datetimepicker('hide');
});

$('#datetimepicker4').on("change.datetimepicker", function (e) {
    if (e != null) {
        console.log(e.date.format("D, M, YYYY"));
        $('#' + $(this).closest('a').data("target")).load($(this).closest('a').attr("href"), { date: e.date.format("M, D, YYYY") });
}
});

$('#msg-sender').click(function () {
    $('input[name=replyid]').val(-1);
    $('.richText-editor').empty();
});

//$(function () {
//    $('.comment-rate').click(function (e) {
//        e.preventDefault(); 
//        var self = this;
//        if ($(this).parent().attr('disabled')) {
//            return;
//        }
//        if ($(this).hasClass('like')) {
//            flag = true;
//        }
//        else {
//            flag = false;
//        }
//        $.get('/Home/RateComment/' + $(this).parent().data('comment-id'), { value: flag }).done(function (data) {
//            if (data.item1 == "OK") {
//                $(self).parent().find('span').text(data.item2);
//            }
//        });
//        $(this).find('i').addClass('comment-rate-selected')
//    });
//});

$(document).on("click", '.comment-rate', function (e) {
    e.preventDefault();
    var self = this;
    if ($(this).parent().attr('disabled')) {
        return;
    }
    if ($(this).hasClass('like')) {
        flag = true;
    }
    else {
        flag = false;
    }
    $.get('/Home/RateComment/' + $(this).parent().data('comment-id'), { value: flag }).done(function (data) {
        if (data.item1 == "OK") {
            $(self).parent().find('span').text(data.item2);
            $(this).addClass('comment-rate-selected');
        }
    });
    
});
//$(function () {
//    $('.comment-reply').click(function (e) {
//        e.preventDefault(); 
//        $('input[name=replyid]').val($(this).next().data('comment-id'));
//        $('.richText-editor').empty();
//        $('.richText-editor').focus();
//        $('.richText-editor').text('ИНВАН, ');
//    });
//});

$(function () {
    $('.comment-rate').ready(function () {
        if ($('.comment-rate').parent().attr('disabled')) {
            $('.comment-rate').parent().find('a').prop('disabled', true);
        }
    });
});

$(".date-picker").click(function (e) {
    e.preventDefault();
    $('#datetimepicker4').datetimepicker('toggle');
    $('.session-date').removeClass('selected');
    $('#session-drop').removeClass('selected');
    $(this).addClass('selected');
    //$(this).addClass('selected');
    //$('#datetimepicker4').datetimepicker('hide');
    //if ($('#session-drop').hasClass('selected'))
    //    $('#session-drop').removeClass('selected');
    //if ($(this).parent().hasClass('session-dropdown'))
    //    $('#session-drop').addClass('selected');
});

$(".date-picker").blur(function (e) {
    $('#datetimepicker4').datetimepicker('hide');
});
//(function ($) {
//    $.fn.focusToEnd = function () {
//        return this.each(function () {
//            var v = $(this).val();
//            $(this).focus().val("").val(v);
//        });
//    };
//})(jQuery);

$('#msg-sender').click(function () {
    $(this).parent().find('textarea').text("");
    $(this).parent().find('textarea').html("");
});


$(document).ready(function () {
    
});
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

//$(function () {

//    $("#prThird").click(function (e) {
//        $(this).blur();
//        $(this).addClass("menu-item-active");
//        e.preventDefault();
//        $('#' + $(this).data("target")).load($(this).attr("href"));
//        $(".profile-menu").removeClass("active");
//        $(".third").addClass("active");
//        //$("#prThird").addClass("menu-item-active");
//        //$("#prThird").blur();
//    });

//});

//function OnLinkClick(e) {
//    e.preventDefault();
//    $('#' + $(this).data("target")).load($(this).attr("href"));
//    $(".profile-menu").removeClass("active");
//}

$('#message').richText({

    // text formatting
    bold: true,
    italic: true,
    underline: true,

    // text alignment
    leftAlign: false,
    centerAlign: false,
    rightAlign: false,

    // lists
    ol: true,
    ul: true,

    // title
    heading: false,

    fonts: false,

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
    imageHTML: '',

    id: "testing",
    useParagraph: true
});

//$("textarea[maxlength]").on("propertychange input", function () {
//    if (this.value.length > this.maxlength) {
//        this.value = this.value.substring(0, this.maxlength);
//    }
//});

//$(document).ready(function () {
//    $('form').find("input[type=textarea], input[type=password], textarea").each(function (ev) {
//        if (!$(this).val()) {
//            $(this).attr("placeholder", "Type your answer here");
//        }
//    });
//});

$(document).ready(function () {
    //initialize swiper when document ready
    var mySwiper = new Swiper('.swiper-container-main', {
        // Optional parameters
        loop: true,
        slidesPerView: 1,
        grabCursor: true,
        speed: 500,

        navigation: {
            nextEl: '.swiper-main-btn-next',
            prevEl: '.swiper-main-btn-prev'
        }
        //// If we need pagination
        //pagination: {
        //    el: '.swiper-pagination-featured',
        //    clickable: true
        //}
    });
});


$(document).ready(function () {
    //initialize swiper when document ready
    var mySwiper = new Swiper('.swiper-container-selected', {
        // Optional parameters
        slidesPerView: 2,
        slidesPerColumn: 2,
        spaceBetween: 30,
        pagination: {
            el: '.swiper-pagination-selected',
            clickable: true,
        },
    })
});


$(document).ready(function () {

    $('.swiper-slide-featured').each(function () {
        var image = new Image();
        image.src = $(this).find('img').attr("src");

        if (image.width / image.height != 2 / 3) {

            $(this).find('a').addClass('col').addClass('px-0');
            $(this).find('img').css('object-fit', 'cover').addClass('h-100');
        }
    });

    $('.img-fixed').each(function () {

        console.log(123);
        var img = new Image();
        img.src = $(this).attr("src");


        if (img.width / img.height != 2 / 3) {
            $(this).css('height', $(this).width() * 3 / 2);
            $(this).css('object-fit', 'cover');
        }
    });

});


//$(window).resize(function () {
//    $('.img-fixed').each(function () {

//        console.log(123);
//        var img = new Image();
//        img.src = $(this).attr("src");


//        if (img.width / img.height != 2 / 3) {
//            $(this).css('height', $(this).width() * 3 / 2);
//            $(this).css('object-fit', 'cover');
//        }
//    });
//});

//function myFunction(x) {
//    if (x.matches) { // If media query matches
//        $('.session-nav').children('a').removeClass('border-orange');
//    } else {
//        $('.session-nav').children('a').addClass('border-orange');
//    }
//}


$(document).ready(function () {

    //var x = window.matchMedia("(max-width: 767px)")
    //myFunction(x) // Call listener function at run time
    //x.addListener(myFunction) // Attach listener function on state changes


    $('#make-order').submit(function (e) {
        if ($('*[id^="ticket-row"]').length == 0) {
            e.preventDefault();
            alert('Не выбрано место');
        }
    });
});

$(document).ajaxStart(function () {
    $("#loading").show();
    $("#loading-2").show();
});

$(document).ajaxStop(function () {
    $("#loading").hide();
    $("#loading-2").hide();
});
