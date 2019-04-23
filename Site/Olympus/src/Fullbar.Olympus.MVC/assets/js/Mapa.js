
function Initialize() {

    var latitude = parseFloat($('#latitude').val());
    var longitude = parseFloat($('#longitude').val());
    var endereco = $('#endereco').val() + ', nº' + $('#numero').val();
    var local = $('#local').val();


    google.maps.visualRefresh = true;
    var coordenada = new google.maps.LatLng(latitude, longitude);

    // These are options that set initial zoom level, where the map is centered globally to start, and the type of map to show
    var mapOptions = {
        zoom: 16,
        center: coordenada,
        mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
    };

    // This makes the div with id "map_canvas" a google map
    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);


    var myLatlng = new google.maps.LatLng(latitude, longitude);

    var marker = new google.maps.Marker({
        position: myLatlng,
        map: map,
        title: ''
    });

    var marker = new google.maps.Marker({
        'position': new google.maps.LatLng(latitude, longitude),
        'map': map,
        'title': ''
    });

    // Make the marker-pin blue!
    //marker.setIcon('http://maps.google.com/mapfiles/ms/icons/blue-dot.png')


    var infowindow = new google.maps.InfoWindow({
        content: "<div class='infoDiv'>" + endereco + "</div>"
    });

    google.maps.event.addListener(marker, 'click', function () {
        infowindow.open(map, marker);
    });

}


