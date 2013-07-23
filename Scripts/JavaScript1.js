
    var google_geocoder;

$("#geocoder_dialog").dialog({ autoOpen: false });

$(document).ready(function () {
        

    $("#btnFind").click(function () {
        google_geocoder = new google.maps.Geocoder();
        var full_address;
        full_address = $("#address").val;
        full_address += " " + $("#city").val;
        full_address += " " + $("#state").val;

        google_geocoder.geocode({ "address": full_address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                $("#resource_value").val(results[0].geometry.location);
            }

            $("#resource_value").blur();
            $("#geocoder_dialog").dialog("close");
        });
    });

    $(".resource_value").each(function () {
        $(this).focus(function () {
            $("#geocoder_dialog").dialog("open");
        });
    });
});
