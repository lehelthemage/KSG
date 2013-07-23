
var google_geocoder;
var focused_geocode_box;

$("#geocoder_dialog").dialog({ autoOpen: false });
/*
    $(".resource_value").each(function () {
        $(this).focus(function () {
            focused_geocode_box = $(this).attr('id');
            $("#geocoder_dialog").dialog("open");
        });
    });
*/

        

    $("#btnFind").click(function () {
        
        google_geocoder = new google.maps.Geocoder();

        var full_address;
        full_address = $("#address").val();
        alert("address = " + full_address);
        full_address += " " + $("#city").val();
        full_address += " " + $("#state").val();

        google_geocoder.geocode({ "address": full_address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                $("#" + focused_geocode_box).val(results[0].geometry.location);
            }

            $("#" + focused_geocode_box).blur();
            $("#geocoder_dialog").dialog("close");
        });
    });

    

