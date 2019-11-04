
function fillRegionSelect(viewBag) {

    var result = viewBag.replace(/(&quot\;)/g, "\"")
    var select = document.getElementById("Select-Region");
    JSON.parse(result).forEach(function (element) {
        select.options[select.options.length] = new Option(element.RegionDescription, element.RegionId);
    });
}

function setRegion() {
    var select = document.getElementById("Select-Region");
    if (select.options.length > 0) {
        document.getElementById('Region').value = select.options[select.selectedIndex].text;
        //document.getElementById('Region').value = select.options[select.selectedIndex].value;
    }
}

function setRegionIndex() {
    var select = document.getElementById("Select-Region");
    if (select.options.length > 0) {
        document.getElementById('RegionId').value = select.options[select.selectedIndex].value;
        //document.getElementById('Region').value = select.options[select.selectedIndex].value;
        //window.alert("Text: " + select.options[select.selectedIndex].text + "\nValue: " + select.options[select.selectedIndex].value);
    }
}

function retrieveRegion(dropDownList, valueToSet) {
    var option = dropDownList.firstChild;
    for (var i = 0; i < dropDownList.length; i++) {
        if (option.text.trim().toLowerCase() == valueToSet.trim().toLowerCase()) {
            option.selected = true;
            return;
        }
        option = option.nextElementSibling;
    }
}

function retrieveRegionFromIndex(dropDownList, valueToSet) {
    var option = dropDownList.firstChild;
    for (var i = 0; i < dropDownList.length; i++) {
        if (option.index == valueToSet) {
            option.selected = true;
            return;
        }
        option = option.nextElementSibling;
    }
}

function fillRegionShipViaSelect(viewBag) {
    var result = viewBag.replace(/(&quot\;)/g, "\"")
    var select = document.getElementById("Select-ShipVia");
    JSON.parse(result).forEach(function (element) {
        select.options[select.options.length] = new Option(element.CompanyName, element.ShipperId);
    });
}
function setShipVia() {
    var select = document.getElementById("Select-ShipVia");
    if (select.options.length > 0) {
        document.getElementById('ShipVia').value = select.selectedIndex;
        //document.getElementById('ShipVia').value = select.options[select.selectedIndex].text;
        //document.getElementById('Region').value = select.options[select.selectedIndex].value;
    }
}

function setShipViaIndex() {
    var select = document.getElementById("Select-ShipVia");
    if (select.options.length > 0) {
        document.getElementById('ShipVia').value = select.options[select.selectedIndex].value;
        //document.getElementById('Region').value = select.options[select.selectedIndex].value;
        window.alert("Text: " + select.options[select.selectedIndex].text + "\nValue: " + select.options[select.selectedIndex].value);
    }
}

function retrieveShipVia(dropDownList, valueToSet) {
    var option = dropDownList.firstChild;
    for (var i = 0; i < dropDownList.length; i++) {
        if (option.text.trim().toLowerCase() == valueToSet.trim().toLowerCase()) {
            option.selected = true;
            return;
        }
        option = option.nextElementSibling;
    }
}

function retrieveShipViaFromIndex(dropDownList, valueToSet) {
    var option = dropDownList.firstChild;
    for (var i = 0; i < dropDownList.length; i++) {
        if (option.index == valueToSet) {
            option.selected = true;
            return;
        }
        option = option.nextElementSibling;
    }
}