ymaps.ready(init);

function init() {
    var myPlacemark,
        myMap = new ymaps.Map('map', {
            center: [55.753994, 37.622093],
            zoom: 13
        }, {
            searchControlProvider: 'yandex#search'
        });


    myMap.events.add('click', function (e) {
        var coords = e.get('coords');

        if (myPlacemark) {
            myPlacemark.geometry.setCoordinates(coords);
        }

        else {
            myPlacemark = createPlacemark(coords);
            myMap.geoObjects.add(myPlacemark);

            myPlacemark.events.add('dragend', function () {
                getAddress(myPlacemark.geometry.getCoordinates());
            });
        }
        getAddress(coords);
    });


    function createPlacemark(coords) {
        return new ymaps.Placemark(coords, {
            iconCaption: 'поиск...'
        }, {
            preset: 'islands#violetDotIconWithCaption',
            draggable: true
        });
    }

    function getAddress(coords) {
        myPlacemark.properties.set('iconCaption', 'поиск...');
        var url = "http://suggestions.dadata.ru/suggestions/api/4_1/rs/geolocate/address";
        var token = "b39b45a2ac15a8f8eb139365a70c67e73c345ea6";
        var query = { lat: coords[0], lon: coords[1], count: 1 };

        var options = {
            type: "POST",
            url: url,
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(query),
            headers: {
                "Accept": "application/json",
                "Authorization": "Token " + token
            },
            success: function (result) {
                console.log(result);
                if (result.suggestions.length > 0) {
                    let parts = result.suggestions[0].value.split(',');
                    console.log(parts);
                    $('#PlaceName').val(result.suggestions[0].value);
                    $('#Longitude').val(coords[1]);
                    $('#Latitude').val(coords[0]);

                    $('#Latitude').val($('#Latitude').val().replace('.', ','));
                    $('#Longitude').val($('#Longitude').val().replace('.', ','));
                    myPlacemark.properties
                        .set({
                            iconCaption: [
                                parts[0] || '',
                                parts[1] || '', parts[2] || ''
                            ].filter(Boolean).join(', '),
                            // В качестве контента балуна задаем строку с адресом объекта.
                            balloonContent: result.suggestions[0].value
                        });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log("error", textStatus, errorThrown);
            }
        };

        $.ajax(options);
    }

    let longitude = parseFloat($('#Longitude').val().replace(',', '.')), latintude = parseFloat($('#Latitude').val().replace(',', '.'));
    if (longitude == 0  && latintude == 0) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                console.log(position);
                myMap.setCenter([position.coords.latitude, position.coords.longitude]);
            }, function () {
                alert("Geolocation is not supported by this browser or was denied by the user.");
            });
        } else {
            alert("Geolocation is not supported by this browser.");
        }
    } else {
        let coords = [latintude, longitude];
        myMap.setCenter(coords);
        myPlacemark = createPlacemark(coords);
        myMap.geoObjects.add(myPlacemark);
        getAddress(coords);
        
    }
}
