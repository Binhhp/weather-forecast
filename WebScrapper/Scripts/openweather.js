
$(document).ready(function () {
    $("#inputdate").datepicker({
        format: "mm/dd/yyyy",
        autoclose: true,
        todayHighlight: true
    });
    window.setTimeout(function () {
        myposition.init();
        getnew();
    }, 50);
    window.setInterval(function () {
        myposition.init();
    }, 360000);
    $('#btnSearch').click(function () {
        $('#ftco-loader').addClass("show");
        getWeather();
        getWeatherDAY();
    });
    $("#btnsearchdate").click(function () {
        search5day();
        searchcurent();
    })
    $('#btnposition').click(function () {
        myposition.init();
    });
    toastr.options = {
        "closeButton": true
    };
});
function search5day() {
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
    postData('POST', '/WebAPI/SearchDatetime5Day', formData).then(function (data) {
        
        if (data != null && data != "Error") {
            
            var html = '';
            var forecast = '';
            html += '<div class="col-md-12"><h2>Dự báo thời tiết 5 ngày / 3 giờ</h2></div>';
            $.each(data, function (key, item) {
                if (item.NgayTao == null || item.NgayTao == undefined) {
                    toastr["error"]("Không tìm thấy dữ liệu!");
                    return;
                }
                html += '<div class="forecast-list-card">';
                html += '<div class="date" id = "datedaily"><p class="dow">' + item.NgayTao + '</p></div>';
                html += '<img class="weather-icon icon" width="80" height="80" src="https://openweathermap.org/img/wn/' + item.Anh + '.png"/>';
                html += '<div class="temps"><span class="high">' + item.Nhietdocao + '<sup>o</sup>C </span>' + '<span class="low">/ ' + item.Nhietdothap + '<sup>o</sup>C</span></div>';
                html += '<div class="phrase"> Mô tả:  ' + item.MoTa + ' .Mây:  ' + item.May + ' %<br/>Tốc độ gió: ' + item.Tocdogio + ' m/s</div>';
                html += '<div class="info precip"><p>Độ ẩm</p><p>' + item.Doam + ' %</p></div>';
                html += '</div>';
                forecast += '<div class="forecast">';
                forecast += '<div class="forecast-header"><div class="day">' + item.NgayTao + '</div></div>';
                forecast += '<div class="forecast-content"><div class="forecast-icon"><img class="weather-icon icon" width="58" src="https://openweathermap.org/img/wn/' + item.Anh + '.png"/>';
                forecast += '</div><div class="degree">' + item.Nhietdocao + '<sup>o</sup>C</div>';
                forecast += '<small>' + item.Nhietdothap + '<sup>o</sup></small>';
                forecast += '</div></div>';
            })
            document.getElementById('forecast-daily').innerHTML = html;
            document.getElementById('forecastdaily').innerHTML = forecast;
            $('#ftco-loader').removeClass("show");
        }
    })
};
function searchcurent() {
    $('#ftco-loader').addClass("show");
    let formData = new FormData();
    formData.append('date', $("#inputdate").val());
    formData.append('address', $("#inputaddress").val());
    postData('POST', '/WebAPI/SearchCurent', formData).then(function (data) {
        
        if (data != null && data != "Error") {
            if (data.NgayTao == null || data.NgayTao == undefined) {
                return;
            }
            document.getElementById('day').innerText = data.NgayTao;
            document.getElementById('latlong').innerText = 'Lat: ' + data.Lat + ' °N - Lon: ' + data.Long + ' °W';
            document.getElementById('namecity').innerText = data.Ten + ' , ' + data.Datnuoc;
            document.getElementById('temp').innerHTML = data.Nhietdo + '<sup>o</sup>C';
            document.getElementById('icon').innerHTML = '<img src="https://openweathermap.org/img/wn/' + data.Anh + '.png" width="120"/>';
            document.getElementById('humidity').innerHTML = '<img src="/Content/weather/images/icon-umberella.png" alt="">' + data.Doam;
            document.getElementById('windspeed').innerHTML = '<img src="/Content/weather/images/icon-wind.png" alt="">' + data.Tocdogio + ' m/s';
            document.getElementById('pressure').innerHTML = '<img src="/Content/weather/images/icon-compass.png" alt="">' + data.Apsuat + ' Hpa';
            document.getElementById('clouds').innerHTML = '<img src="/Content/weather/images/icon-compass.png" alt="">' + data.May + ' %';
            document.getElementById('description').innerText = data.MoTa;
        }
        else {
            toastr["error"]("Lỗi xảy ra trong quá trình thực hiện");
        }
    })
}
function getWeather() {
    $('#ftco-loader').addClass("show");
    let formData = new FormData();
    formData.append('namecity', $('#txtCity').val());
    postData('POST', '/WebAPI/APIWeather', formData).then(function (data) {
        if (data != null && data != "Error") {
            if (data.coord.lat == null) {
                toastr["error"]("Lỗi xảy ra!");
                return;
            }
            var date = new Date();
            var today = date.getDay() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
            document.getElementById('day').innerText = today;
            document.getElementById('latlong').innerText = 'Lat: ' + data.coord.lat + ' °N - Lon: ' + data.coord.lon + ' °W';
            document.getElementById('namecity').innerText = data.name + ' , ' + data.Sys.country;
            document.getElementById('temp').innerHTML = (Math.ceil(data.main.temp - 273.15)) + '<sup>o</sup>C';
            document.getElementById('icon').innerHTML = '<img src="https://openweathermap.org/img/wn/' + data.weather[0].icon + '.png" width="120"/>';
            document.getElementById('humidity').innerHTML = '<img src="/Content/weather/images/icon-umberella.png" alt="">' + data.main.humidity;
            document.getElementById('windspeed').innerHTML = '<img src="/Content/weather/images/icon-wind.png" alt="">' + data.wind.speed + ' m/s';
            document.getElementById('pressure').innerHTML = '<img src="/Content/weather/images/icon-compass.png" alt="">' + data.main.pressure + ' Hpa';
            document.getElementById('clouds').innerHTML = '<img src="/Content/weather/images/icon-compass.png" alt="">' + data.clouds.all + ' %';
            document.getElementById('description').innerText = data.weather[0].description;
        }
        else {
            toastr["error"]("Lỗi xảy ra trong quá trình thực hiện");
        }
    });
};
function getWeatherDAY() {
    let formData = new FormData();
    formData.append('namecity', $('#txtCity').val());
    postData('POST', '/WebAPI/APIWeatherDAY', formData).then(function (data) {
        $('#ftco-loader').removeClass("show");
        if (data != null && data != "Error") {
            var html = '';
            var forecast = '';
            html += '<div class="col-md-12"><h2>Dự báo thời tiết 5 ngày / 3 giờ</h2></div>';
            $.each(data.list, function (key, item) {
                if (item.dt_txt == null) {
                    toastr["error"]("Lỗi xảy ra!");
                    return;
                }
                html += '<div class="forecast-list-card">';
                html += '<div class="date" id="datedaily"><p class="dow">' + item.dt_txt + '</p></div>';
                html += '<img class="weather-icon icon" width="80" height="80" src="https://openweathermap.org/img/wn/' + item.weather[0].icon + '.png"/>';
                html += '<div class="temps"><span class="high">' + (Math.ceil(item.main.temp_max - 273.15)) + '<sup>o</sup>C </span>' + '<span class="low">/ ' + (Math.ceil(item.main.temp_min - 273.15)) + '<sup>o</sup>C</span></div>';
                html += '<div class="phrase"> Mô tả:  ' + item.weather[0].description + ' .Mây:  ' + item.clouds.all + ' %<br/>Tốc độ gió: ' + item.wind.speed + ' m/s</div>';
                html += '<div class="info precip"><p>Độ ẩm</p><p>' + item.main.humidity + ' %</p></div>';
                html += '</div>';
                forecast += '<div class="forecast">';
                forecast += '<div class="forecast-header"><div class="day">' + item.dt_txt + '</div></div>';
                forecast += '<div class="forecast-content"><div class="forecast-icon"><img class="weather-icon icon" width="58" src="https://openweathermap.org/img/wn/' + item.weather[0].icon + '.png"/>';
                forecast += '</div><div class="degree">' + (Math.ceil(item.main.temp_max - 273.15)) + '<sup>o</sup>C</div>';
                forecast += '<small>' + (Math.ceil(item.main.temp_min - 273.15)) + '<sup>o</sup></small>';
                forecast += '</div></div>';
            })
            document.getElementById('forecast-daily').innerHTML = html;
            document.getElementById('forecastdaily').innerHTML = forecast;
            $('#ftco-loader').removeClass("show");
        }
        else {
            return;
        }
    })
};
var myposition = {
    init: function () {
        navigator.geolocation.getCurrentPosition(function (position) {
            let lat = position.coords.latitude;
            let long = position.coords.longitude;
            getWeatherMyposition(lat, long);
            getWeatherMypositionDAY(lat, long);
        });
    }
};
function getWeatherMyposition(lat, long) {
    $('#ftco-loader').addClass("show");
    let formData = new FormData();
    formData.append('lat', lat);
    formData.append('lng', long);
    postData('POST', '/WebAPI/WeatherMyposition', formData).then(function (data) {
        $('#ftco-loader').removeClass("show");
        if (data != null) {
            if (data.coord.lat == null) {
                toastr["error"]("Lỗi xảy ra!");
                return;
            }
            var date = new Date();
            var today = date.getDay() + '-' + (date.getMonth() + 1) + '-' + date.getFullYear();
            document.getElementById('day').innerText = today;
            document.getElementById('latlong').innerText = 'Lat: ' + data.coord.lat + ' °N - Lon: ' + data.coord.lon + ' °W';
            document.getElementById('namecity').innerText = data.name + ' , ' + data.Sys.country;
            document.getElementById('temp').innerHTML = (Math.ceil(data.main.temp - 273.15)) + '<sup>o</sup>C';
            document.getElementById('icon').innerHTML = '<img src="https://openweathermap.org/img/wn/' + data.weather[0].icon + '.png" width="120"/>';
            document.getElementById('humidity').innerHTML = '<img src="/Content/weather/images/icon-umberella.png" alt="">' + data.main.humidity;
            document.getElementById('windspeed').innerHTML = '<img src="/Content/weather/images/icon-wind.png" alt="">' + data.wind.speed + ' m/s';
            document.getElementById('pressure').innerHTML = '<img src="/Content/weather/images/icon-compass.png" alt="">' + data.main.pressure + ' Hpa';
            document.getElementById('clouds').innerHTML = '<img src="/Content/weather/images/icon-compass.png" alt="">' + data.clouds.all +' %';
            document.getElementById('description').innerText = data.weather[0].description;
        }
    });
};
function getWeatherMypositionDAY(lat, long) {
    let formData = new FormData();
    formData.append('lat', lat);
    formData.append('lng', long);
    postData('POST', '/WebAPI/WeatherMypositionDAY', formData).then(function (data) {
        $('#ftco-loader').removeClass("show");
        if (data != null && data != "Error") {
            var html = '';
            var forecast = '';
            html += '<div class="col-md-12"><h2>Dự báo thời tiết 5 ngày / 3 giờ</h2></div>';
            $.each(data.list, function (key, item) {
                if (item.dt_txt == null) {
                    toastr["error"]("Lỗi xảy ra!");
                    return;
                }
                html += '<div class="forecast-list-card">';
                html += '<div class="date" id="datedaily"><p class="dow">' + item.dt_txt + '</p></div>';
                html += '<img class="weather-icon icon" width="80" height="80" src="https://openweathermap.org/img/wn/' + item.weather[0].icon + '.png"/>';
                html += '<div class="temps"><span class="high">' + (Math.ceil(item.main.temp_max - 273.15)) + '<sup>o</sup>C </span>' + '<span class="low">/ ' + (Math.ceil(item.main.temp_min - 273.15)) + '<sup>o</sup>C</span></div>';
                html += '<div class="phrase"> Mô tả:  ' + item.weather[0].description + ' .Mây:  ' + item.clouds.all + ' %<br/>Tốc độ gió: ' + item.wind.speed + ' m/s</div>';
                html += '<div class="info precip"><p>Độ ẩm</p><p>' + item.main.humidity + ' %</p></div>';
                html += '</div>';
                forecast += '<div class="forecast">';
                forecast += '<div class="forecast-header"><div class="day">' + item.dt_txt + '</div></div>';
                forecast += '<div class="forecast-content"><div class="forecast-icon"><img class="weather-icon icon" width="58" src="https://openweathermap.org/img/wn/' + item.weather[0].icon + '.png"/>';
                forecast += '</div><div class="degree">' + (Math.ceil(item.main.temp_max - 273.15)) + '<sup>o</sup>C</div>';
                forecast += '<small>' + (Math.ceil(item.main.temp_min - 273.15)) + '<sup>o</sup></small>';
                forecast += '</div></div>';
            })
            document.getElementById('forecast-daily').innerHTML = html;
            document.getElementById('forecastdaily').innerHTML = forecast;
            $('#ftco-loader').removeClass("show");
        }
        else {
            toastr["error"]("Lỗi xảy ra trong quá trình thực hiện");
        }
    })
};
function getnew() {
    postData('GET', '/WebAPI/getNew', null).then(function(data){
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