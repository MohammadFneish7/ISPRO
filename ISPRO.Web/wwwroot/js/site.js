// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getCardTemplate(array) {
    return '<div style="width:250px; height:267px;padding: 20px;display: flex;flex-direction: column;align-items: center;align-content: center;border: 1px solid lightgray;border-radius: 15px;">' +
        '<i class="bi bi-browser-edge"></i><h4>' + array[2] + '</h4></br>' +
        '<div>Code:</div>' +
        '<div style="text-decoration: underline; font-size: 12px; font-weight:bold">' + $(array[1]).text().match(/.{1,4}/g).join(" ") + '</div>' +
        '<div>Subscription: ' + array[3] + '</div>' +
        '<div>Recharge Period: ' + array[4] + ' Days</div>' +
        '<div>Price: ' + numberWithCommas(array[5]) + ' ' + array[6] + '</div>' +
        '<div>Expiry Date: ' + array[7] + '</div>' +
        '</div>'
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function printCards() {
    let data = null;
    if (IndexTable.rows({ filter: 'applied', selected: true }).count() > 0) {
        data = IndexTable.rows({ filter: 'applied', selected: true }).data()
    } else if (IndexTable.rows({ filter: 'applied'}).count() > 0) {
        data = IndexTable.rows({ filter: 'applied'}).data()
    } else if (IndexTable.rows({ selected: true }).count() > 0) {
        data = IndexTable.rows({ selected: true }).data()
    } else {
        data = IndexTable.rows().data()
    }

    let printdiv = '<div class="print-div" style="break-after:page;display:flex; flex-direction: row; gap: 10px;flex-wrap: wrap;position:absolute; left: 10000px;">'

    for (i = 0; i < data.length; i++) {
        if (i > 0 && i % 12 == 0)
            printdiv += '</div><div class="print-div" style="margin-top:10px;break-after:page;display:flex; flex-direction: row; gap: 10px;flex-wrap: wrap;position:absolute; left: 10000px;">';
        printdiv += getCardTemplate(data[i]);
    }
    printdiv += '</div>';

    //let printWindow = window.open("", "PRINT", ',"width=0,height=0,left=10000,top=10000,status=no,toolbar=no,scrollbars=no,resizable=no,directories=no,location=no,menubar=no,fullscreen=no,visible=none');
    //printWindow.document.write("<html><head><title>Print Cards</title>");
    //printWindow.document.write("</head><body>");
    //printWindow.document.write(printdiv);
    //printWindow.document.write("</body></html>");
    //printWindow.document.close();
    //printWindow.focus();
    $('.print-div').remove();
    $('body').css('overflow-x', 'hidden');
    $('body').css('overflow-y', 'hidden');
    $('body').prepend(printdiv);
    window.print();
    $('.print-div').remove();
    $('body').css('overflow-x', 'auto');
    $('body').css('overflow-y', 'auto');
    //printWindow.close();
}