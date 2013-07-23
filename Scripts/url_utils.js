function getUrl(action, controller) {
    var protocol = window.location.protocol;
    var host = window.location.host;

    var url = (protocol + '//' + host + '/');

    if ((controller != null && controller != 'undefined') && (action != null && action != 'undefined'))
        url += controller + '/' + action;

    return url;
}