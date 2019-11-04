
const resultCallBackSuppliers = (json) => {
    tmpJson = JSON.stringify(json);
}

const userActionSuppliers = async (url) => {
    return await fetch(url).then((r) => r.json());
}

document.getElementById("tblModalSupplier").addEventListener("click", function () {
    highlight_rowSuppliers();
});


function highlight_rowSuppliers() {
    var table = document.getElementById('tblModalSupplier');
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

            var divContainer = document.getElementById("inputSupplierId");
            divContainer.value = rowSelected.cells[0].innerHTML;

            divContainer = document.getElementById("inputSupplierDescription");
            divContainer.value = rowSelected.cells[1].innerHTML;

            $('#modalSupplier').modal('toggle');
        }
    }
}

const showSuppliers = async (baseUrl) => {

    let url = baseUrl + '/api/suppliers/getsuppliers';

    //alert(await userAction(url).then((json) => { return JSON.stringify(json) }));
    let alertaVar = JSON.parse(await userActionSuppliers(url).then((json) => { return JSON.stringify(json) }));

    // EXTRACT VALUE FOR HTML HEADER.
    // ('Book ID', 'Book Name', 'Category' and 'Price')
    var col = Object.keys(alertaVar[0]);


    // CREATE DYNAMIC TABLE.
    var table = document.createElement("table");
    table.setAttribute('id', 'tblSuppliers');
    table.setAttribute('class', 'table table-striped');

    // CREATE HTML TABLE HEADER ROW USING THE EXTRACTED HEADERS ABOVE.

    var tr = table.insertRow(-1);                   // TABLE ROW.

    for (var i = 0; i <= 3; i++) {
        var th = document.createElement("th");      // TABLE HEADER.
        if (i == 3) {
            th.innerHTML = "";
        } else {
            th.innerHTML = col[i];
        }

        tr.appendChild(th);
    }

    //Empty Column
    //var th = document.createElement("th");
    //th.innerHTML = "";
    //tr.appendChild(th);

    // ADD JSON DATA TO THE TABLE AS ROWS.
    for (var i = 0; i < alertaVar.length; i++) {

        tr = table.insertRow(-1);

        for (var j = 0; j < 3; j++) {
            var tabCell = tr.insertCell(-1);
            tabCell.innerHTML = alertaVar[i][col[j]];
        }

        var tabCell = tr.insertCell(3);
        tabCell.innerHTML = '<i class="fa fa-check-square"></i>'
        //tabCell.addEventListener("click", funcAlert, false); //where func is your function name
        tabCell.setAttribute('id', 'imgCheck');
    }

    // FINALLY ADD THE NEWLY CREATED TABLE WITH JSON DATA TO A CONTAINER.
    var divContainer = document.getElementById("tblModalSupplier");
    divContainer.innerHTML = "";
    divContainer.appendChild(table);
    document.getElementById("tblModalSupplier");

}

const searchSuppliers = async (baseUrl) => {
    alert("searchSuppliers ()");
}
