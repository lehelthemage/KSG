//script for functionality of layout

$(function () {
    $("#create_object").button().text("Create Object!");
    $("#create_object").click(function () {
        location.href = "<?php echo base_url();?>index.php/objects/create/";
    });
    $("#thumbup").button();
    $("#thumbdown").button();
    $("#toolbox").buttonset();

    $("#thumbup").click(function () {
        runMarquee('You like this!');
    });

    $("#thumbdown").click(function () {
        runMarquee('You dislike this!');
    });


});

var removeMarqueeTimeout = null;
function runMarquee(text) {
    removeMarquee();
    $("#toolbox_marquee").text(text);
    clearTimeout(removeMarqueeTimeout);
    setTimeout('showMarquee()', 100);
}

function showMarquee() {
    $("#toolbox_marquee").css('opacity', '1');
    $("#toolbox_marquee").trigger('start');
    removeMarqueeTimeout = setTimeout('removeMarquee()', 3000);
}

function removeMarquee() {
    $("#toolbox_marquee").text('');
    $("#toolbox_marquee").trigger('stop');
    $("#toolbox_marquee").css('opacity', '0');
}