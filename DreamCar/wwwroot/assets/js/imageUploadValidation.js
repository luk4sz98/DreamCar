$.validator.addMethod("maximumcollectionsize", function (value, element, params) {
    var photosLimit = params[1];
    if (element.files.length > photosLimit) {
        return false;
    }
    return true;
});

$.validator.addMethod("validfiletype", function (value, element, params) {
    var validTypes = params[1].split(',');
    var fileTypeFound = false;
    for (var i = 0; i < element.files.length; i++) {
        var fileType = element.files[i].name.split('.')[element.files[i].name.split('.').length - 1];
        for (var j = 0; j < validTypes.length; j++) {
            if (fileType == validTypes[j]) {
                fileTypeFound = true;
                break;
            }
            else {
                fileTypeFound = false;
            }
        }
        if (!fileTypeFound)
            return fileTypeFound;
    }
    return fileTypeFound;
});

$.validator.addMethod("maximumfilesize", function (value, element, params) {
    var maxFileSize = params[1];
    for (var i = 0; i < element.files.length; i++) {
        if (convertBytesToMegabytes(element.files[i].size) > parseFloat(maxFileSize)) {
            return false;
        }
    }
    return true;
});

$.validator.addMethod("minimumfilesize", function (value, element, params) {
    var minFileSize = params[1];
    for (var i = 0; i < element.files.length; i++) {
        if (convertBytesToMegabytes(element.files[i].size) < parseFloat(minFileSize)) {
            return false;
        }
    }
    return true;
});

$.validator.unobtrusive.adapters.add("maximumcollectionsize", ['limit'], function (options) {
    var element = $(options.form).find('input#fileUpload')[0];
    options.rules['maximumcollectionsize'] = [element, parseFloat(options.params['limit']) - $('img').length];
    options.messages["maximumcollectionsize"] = options.message;
});

$.validator.unobtrusive.adapters.add("validfiletype", ['validTypes'], function (options) {
    var element = $(options.form).find('input#fileUpload')[0];
    options.rules['validfiletype'] = [element, options.params['validTypes']];
    options.messages["validfiletype"] = options.message;
});

$.validator.unobtrusive.adapters.add("maximumfilesize", ['maximumSize'], function (options) {
    var element = $(options.form).find('input#fileUpload')[0];
    options.rules['maximumfilesize'] = [element, parseFloat(options.params['maximumSize'])];
    options.messages["maximumfilesize"] = options.message;
});

$.validator.unobtrusive.adapters.add("minimumfilesize", ['minimumSize'], function (options) {
    var element = $(options.form).find('input#fileUpload')[0];
    options.rules['minimumfilesize'] = [element, parseFloat(options.params['minimumSize'])];
    options.messages["minimumfilesize"] = options.message;
});

function convertBytesToMegabytes(bytes) {
    return (bytes / 1024) / 1024;
}
