function setActiveMenu(activeMenuLink) {
    if (activeMenuLink)
        $(`#${activeMenuLink}`).parent().addClass('active');
}

function objectifyForm(form) {//serialize data function
    return objectifyFormArray(form.serializeArray());
}

function objectifyFormArray(formArray) {//serialize data function

    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        returnArray[formArray[i]['name']] = formArray[i]['value'];
    }
    return returnArray;
}
