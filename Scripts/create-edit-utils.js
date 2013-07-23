function TogglePropertyValues() {
    if ($("#IsCategory").is(':checked')) {
        $(".values").hide();
    }
    else {
        $(".values").show();
    }
}

function HidePropertyValues() {
    $(".values").hide();
}