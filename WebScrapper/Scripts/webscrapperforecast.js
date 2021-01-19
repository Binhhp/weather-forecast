$('#menuwebapi').removeClass('current-menu-item');
$('#menuwebscrapper').addClass('current-menu-item');
$(document).ready(function () {
    $("#inputdate").datepicker({
        format: "mm/dd/yyyy",
        autoclose: true,
        todayHighlight: true
    });
    window.setTimeout(function () {
        getWeatherScrapperMyposition();
        getnew();
    }, 50);
    window.setInterval(function () {
        getWeatherScrapperMyposition();
    }, 3600000);
    $("#btnSearch").click(function () {
        getWeatherScrapperMyposition();
    });
    $("#btnsearchdate").click(function () {
        searchdate();
    });
    toastr.options = {
        "closeButton": true
    };
});
function searchdate() {
    $("#ftco-loader").addClass('show');
    var date = $("#inputdate").val();
    var address = $("#inputaddress").val();
    if (date.length == 0) {
        toastr["warning"]("Bạn cần nhập ngày tìm kiếm!");
        return;
    }
    if (address.length == 0) {
        toastr["warning"]("Bạn cần nhập địa điểm tìm kiếm!");
        return;
    }
    let formData = new FormData();
    formData.append('date', date);
    formData.append('address', address);
    postData('POST', '/Scrapper/Search', formData).then(function (data) {
        if (data != null) {
            if (data.Tencurent == null) {
                toastr["error"]("Không tìm thấy thành phố!");
                return;
            }
            var headingcurent = '';
            headingcurent += '<h2 class="panel-title">' + data.Tencurent;
            headingcurent += '</h2>';
            $("#day").html(headingcurent);
            $("#date").html(data.smallTxt);
            var summy = '';
            summy += '<img src="' + data.Anh + '" class="pull-left"/><p class="myforecast-current">' + data.Motacurent;
            summy += '<p class="myforecast-current-lrg">' + data.DoF + '</p>';
            summy += '<p class="myforecast-current-sm">' + data.DoC + '</p></div>';
            $("#current_conditions-summary").html(summy);
            $("#current_conditions_detail").html(data.Chitietcurent);
            var sevenday = '';
            sevenday += '<b>Dự báo mở rộng cho:</b><h2 class="panel-title">' + data.Ten7day;
            $("#seven-day-forecast .panel-heading").html(sevenday);
            $("#seven-day-forecast-container").html(data.Noidung7day);
            $("#detailed-forecast-body").html(data.Noidungdetail);
            $("#ftco-loader").removeClass('show');
        }
        else {
            toastr["error"]("Error 404!");
            $("#ftco-loader").removeClass('show');
        }
    })
}
function getWeatherScrapperMyposition() {
    $("#ftco-loader").addClass('show');
    let formData = new FormData();
    formData.append('namecity', $("#namecity").val());
    postData('POST', '/Scrapper/Scrappingforecast', formData).then(function (data) {
      
        if (data != null && data != "Error") {
            if (data.nameconditions == null) {
                toastr["error"]("Không tìm thấy thành phố!");
                $("#ftco-loader").removeClass('show');
                return;
            }
            var htmlheading = '';
            var htmlbodysummy = '';
            htmlheading += '<h2 class="panel-title">' + data.nameconditions + '</h2>';
            htmlbodysummy += '<div class="">' + data.currentconditions + '</div>';
            $("#current_conditions-summary").html(htmlbodysummy);
            $("#current_conditions_detail").html(data.table);
            $("#seven-day-forecast-heading").html(data.sevendayheading);
            $("#seven-day-forecast-container").html(data.sevendaybody);
            $("#detailed-forecast-body").html(data.detailbody);
            $("#date").html(data.smallTXT);
            $("#day").html(htmlheading);
            $("#ftco-loader").removeClass('show');
        }
        else {
            $("#ftco-loader").removeClass();
            toastr["error"]("Lỗi không tìm thấy thành phố");
        }
    })
};
function getnew() {
    postData('GET', '/WebAPI/getNew', null).then(function (data) {
        var html = '';
        $.each(data, function (key, item) {
            html += '<a class="right-rail-article" href="' + item.href + '">';
            html += '<div class="right-rail-article-image"><img src="' + item.img + '" width="308" height="170"/></div>';
            html += '<p class="right-rail-article-title">' + item.title + '</p>';
            html += '<p class="right-rail-article-description">' + item.description + '</p>';
            html += '<a href="' + item.href + '" class="right-rail-cta-text"><div class="right-rail-cta-more">Xem thêm</div></a>';
            html += '</a>';
        })
        $('#postnew').html(html);
    })
};
//fetchAPI
async function postData(verb, url, data) {
    const response = await fetch(url, {
        method: verb,
        mode: 'cors',
        cache: 'default',
        credentials: 'same-origin',
        redirect: 'follow',
        referrerPolicy: 'no-referrer',
        body: data
    }).catch(error => console.error('Error', error));
    return response.json();
};