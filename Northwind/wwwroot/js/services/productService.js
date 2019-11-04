var rSelectedItem = 0;

const resultCallBackProducts = (json) => {
    tmpJson = JSON.stringify(json);
}

const userActionProducts = async (url) => {
    return await fetch(url).then((r) => r.json());
}

document.getElementById("tblModalProduct").addEventListener("click", function () {
    highlight_rowProducts();
});


function highlight_rowProducts() {
    var table = document.getElementById('tblModalProduct');
    var cells = table.getElementsByTagName('td');

    for (var i = 0; i < cells.length; i++) {
        // Take each cell
        var cell = cells[i];
        // do something on onclick event for cell
        cell.onclick = function () {
            // Get the row id where the cell exists
            var rowId = this.parentNode.rowIndex;

            var rowsNotSelected = table.getElementsByTagName('tr');
            for (var row = 0; row < rowsNotSelected.length; row++) {
                rowsNotSelected[row].style.backgroundColor = "";
                rowsNotSelected[row].classList.remove('selected');
            }
            var rowSelected = table.getElementsByTagName('tr')[rowId];
            rowSelected.className += " selected";

            //alert(rSelectedItem);

            var divContainer = document.getElementById("inputProductId[" + rSelectedItem.toString() + "]");
            divContainer.value = rowSelected.cells[0].innerHTML;

            divContainer = document.getElementById("inputProductName[" + rSelectedItem.toString() + "]");
            divContainer.value = rowSelected.cells[1].innerHTML;

            divContainer = document.getElementById("inputQuantityPerUnit[" + rSelectedItem.toString() + "]");
            divContainer.value = rowSelected.cells[4].innerHTML;

            divContainer = document.getElementById("inputUnitPrice[" + rSelectedItem.toString() + "]");
            divContainer.value = rowSelected.cells[5].innerHTML;

            divContainer = document.getElementById("inputDiscount[" + rSelectedItem.toString() + "]");
            divContainer.value = "0";

            $('#modalProduct').modal('toggle');
        }
    }
}

const showProducts = async (baseUrl, remoteSelectedItem) => {

    rSelectedItem = remoteSelectedItem;

    let url = baseUrl + '/api/products/getproducts';
    //alert(remoteSelectedItem);
    //alert(await userAction(url).then((json) => { return JSON.stringify(json) }));
    let alertaVar = JSON.parse(await userActionProducts(url).then((json) => { return JSON.stringify(json) }));

    // EXTRACT VALUE FOR HTML HEADER.
    // ('Book ID', 'Book Name', 'Category' and 'Price')
    var col = Object.keys(alertaVar[0]);


    // CREATE DYNAMIC TABLE.
    var table = document.createElement("table");
    table.setAttribute('id', 'tblProducts');
    table.setAttribute('class', 'table table-striped');

    // CREATE HTML TABLE HEADER ROW USING THE EXTRACTED HEADERS ABOVE.

    var tr = table.insertRow(-1);                   // TABLE ROW.

    for (var i = 0; i < 6; i++) {
        var th = document.createElement("th");      // TABLE HEADER.
        if ((i == 2) || (i == 3) ){
            th.innerHTML = "";
        } else {
            th.innerHTML = col[i];
        }

        tr.appendChild(th);
    }

    //Empty Column
    var th = document.createElement("th");
    th.innerHTML = "";
    tr.appendChild(th);

    // ADD JSON DATA TO THE TABLE AS ROWS.
    for (var i = 0; i < alertaVar.length; i++) {

        tr = table.insertRow(-1);

        for (var j = 0; j < 6; j++) {
            var tabCell = tr.insertCell(-1);
            if ((j == 2) || (j == 3)) {
                tabCell.innerHTML = "";
            } else {
                tabCell.innerHTML = alertaVar[i][col[j]];
            }
        }

        var tabCell = tr.insertCell(6);
        tabCell.innerHTML = '<i class="fa fa-check-square"></i>'
        //tabCell.addEventListener("click", funcAlert, false); //where func is your function name
        tabCell.setAttribute('id', 'imgCheck');
    }

    // FINALLY ADD THE NEWLY CREATED TABLE WITH JSON DATA TO A CONTAINER.
    var divContainer = document.getElementById("tblModalProduct");
    divContainer.innerHTML = "";
    divContainer.appendChild(table);
    document.getElementById("tblModalProduct");

}

const searchProducts = async (baseUrl) => {
    alert("searchProducts ()");
}
