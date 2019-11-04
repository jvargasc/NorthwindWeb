var rowCounter = 1;
var rowExists = 1;
//var viewDataUrl = "";

//function sendViewData(valueParameter) {
//    viewDataUrl = valueParameter;
//}

$(function () {
    $('#btnAdd').click(function () {

    });
});

function addTr(Url) {
    $('<tr id="tablerow' + rowExists + '">' +
        '<td>' +
        '   <div class="editor-field">' +
        '       <input type="text" class="form-control" name="inputQuantity[' + rowExists + ']" id="inputQuantity[' + rowExists + ']" value="" required="required" />' +
        '   </div>' +
        '</td>' +
        '<td>' +
        '   <div class="editor-field">' +
        '       <input class="form-control" name="inputProductId[' + rowExists + ']" id="inputProductId[' + rowExists + ']" type="hidden" value="" required="required" />' +
        '       <input type="text" class="form-control col-lg-5" name="inputProductName[' + rowExists + ']" id="inputProductName[' + rowExists + ']" value="" required="required" readonly />' +
        '       <button type="button" class="btn btn-info" onclick="loadModalProduct(\'' + Url + '\', ' + rowExists + ')\" >' +
        '           <span class="glyphicon glyphicon-search"></span> Search' +
        '       </button >' +
        '   </div>' +
        '</td>' +
        '<td>' +
        '   <div class="editor-field">' +
        '       <input type="text" class="form-control" name="inputQuantityPerUnit[' + rowExists + ']" id="inputQuantityPerUnit[' + rowExists + ']" value="" required="required" readonly/>' +
        '   </div>' +
        '</td>' +
        '<td>' +
        '   <div class="editor-field">' +
        '       <input type="text" class="form-control" name="inputUnitPrice[' + rowExists + ']" id="inputUnitPrice[' + rowExists + ']" value="" required="required" />' +
        '   </div>' +
        '</td>' +
        '<td>' +
        '   <div class="editor-field">' +
        '       <input type="text" class="form-control" name="inputDiscount[' + rowExists + ']" id="inputDiscount[' + rowExists + ']" value="" required="required" />' +
        '   </div>' +
        '</td>' +
        '<td>' +
        '   <button type="button" class="btn btn-primary" onclick="removeTr(' + rowExists + ');">Delete</button>' +
        '</td>' +
        '</tr>').appendTo('#orderDetailTable');

    document.body.addEventListener('click', function (event) {
        if (event.srcElement.id == '#btnProduct') {
            //someFunc();
        };
    });

    rowCounter++;
    rowExists++;
    divContainer = document.getElementById("rowCounter");
    divContainer.value = rowCounter;
    return false;
}

function removeTr(index) {
    if (rowCounter > 1) {
        $('#tablerow' + index).remove();
        rowCounter--;
    }
    divContainer = document.getElementById("rowCounter");
    divContainer.value = rowCounter;
    return false;
}

function loadModalProduct(Url, remoteSelectedItem) {

    $("#modalProduct").modal();
    showProducts(Url, remoteSelectedItem);
}
