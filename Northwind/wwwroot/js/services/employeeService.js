//const getTodo = () => {
//    return new Promise((resolve, reject) => {
//        setTimeout(() => {
//            let error = false;
//            if (!error)
//                resolve({ text: 'Complete Code Example' })
//            else
//                reject()
//        }, 2000)
//    })
//}

//const searchCustomers = (baseUrl) => {
//    getTodo(todo => {
//        console.log(todo.text)
//    })
//}

const resultCallBackEmployees = (json) => {
    tmpJson = JSON.stringify(json);
}

const userActionEmployees = async (url) => {
    return await fetch(url).then((r) => r.json());
}

//function onRowClick(tableId, callback) {
//    var table = document.getElementById(tableId),
//        rows = table.getElementsByTagName("tr"),
//        i;
//    for (i = 0; i < rows.length; i++) {
//        table.rows[i].onclick = function (row) {
//            return function () {
//                callback(row);
//            };
//        }(table.rows[i]);
//    }
//};

//document.getElementById("tblModalCustomer").addEventListener("click", function (row) {
//    //var value = row.getElementsByTagName("td")[0].innerHTML;
//    alert(this);
//    //document.getElementById('click-response').innerHTML = value + " clicked!";
//    console.log("value>>", value);
//});


document.getElementById("tblModalEmployee").addEventListener("click", function () {
    //    var divContainer = document.getElementById("inputCustomerId");
    //    divContainer.value = "inputCustomerId";

    //    divContainer = document.getElementById("inputCustomerDescription");
    //    divContainer.value = "inputCustomerDescription";

    //    const row = event.target.parentNode;
    //    alert(this.rowIndex);

    //    $('#modalCustomer').modal('toggle');

    //    //var d = document.getElementById('inputCustomerId')
    //    //undefined
    //    //d.value = 'sdfsd'
    highlight_rowEmployees();
});

function highlight_rowEmployees() {
    var table = document.getElementById('tblModalEmployee');
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

            var divContainer = document.getElementById("inputEmployeeId");
            divContainer.value = rowSelected.cells[0].innerHTML;

            divContainer = document.getElementById("inputEmployeeName");
            divContainer.value = rowSelected.cells[2].innerHTML + ' ' + rowSelected.cells[1].innerHTML;

            $('#modalEmployee').modal('toggle');
        }
    }
}

const showEmployees = async (baseUrl) => {

    let url = baseUrl + '/api/employees/getemployees';

    //alert(await userAction(url).then((json) => { return JSON.stringify(json) }));
    let alertaVar = JSON.parse(await userActionEmployees(url).then((json) => { return JSON.stringify(json) }));

    //alert(alertaVar);

    // EXTRACT VALUE FOR HTML HEADER.
    // ('Book ID', 'Book Name', 'Category' and 'Price')
    var col = Object.keys(alertaVar[0]);

    //for (var i = 0; i < alertaVar.length; i++) {
    //    for (var key in alertaVar[i]) {
    //        if (col.indexOf(key) === -1) {
    //            col.push(key);
    //        }
    //    }
    //}

    // CREATE DYNAMIC TABLE.
    var table = document.createElement("table");
    table.setAttribute('id', 'tblEmployees');
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
    var divContainer = document.getElementById("tblModalEmployee");
    divContainer.innerHTML = "";
    divContainer.appendChild(table);
    document.getElementById("tblModalEmployee");

}

const searchEmployees = async (baseUrl) => {
    alert("searchEmployees ()");
}
