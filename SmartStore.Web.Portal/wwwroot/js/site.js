function setActiveMenu(activeMenuLink) {
    if (activeMenuLink)
        $(`#${activeMenuLink}`).parent().addClass('active');
}

function objectifyForm(form) {//serialize data function
    return objectifyFormArray(form.serializeArray());
}

function objectifyFormArray(formArray) {//serialize data function

    var returnArray = {};
    var currIndex = -1;
    var currObj = {};
    for (var i = 0; i < formArray.length; i++) {
        var matchedArrayObject = formArray[i]['name'].match(/^([a-zA-Z]+)(\[(\d)\]+.)([a-zA-Z]*)$/);
        if (matchedArrayObject) {
            var objInfo = {
                name: matchedArrayObject[1],
                index: matchedArrayObject[3],
                prop: matchedArrayObject[4]
            };

            if (currIndex == -1) {
                currIndex = objInfo.index;
                currObj = { name: objInfo.name };
            }

            if (currIndex != objInfo.index) {
                if (returnArray[currObj.name] == undefined)
                    returnArray[currObj.name] = [];
                returnArray[currObj.name].push(currObj);
                currObj = { name: objInfo.name };
                currIndex = objInfo.index;
            }
            currObj[objInfo.prop] = formArray[i]['value'];
        } else {
            returnArray[formArray[i]['name']] = formArray[i]['value'];
        }
    }

    if (currIndex != -1 && Object.keys(currObj).length > 1) {
        if (returnArray[currObj.name] == undefined)
            returnArray[currObj.name] = [];
        returnArray[currObj.name].push(currObj);
    }

    return returnArray;
}
