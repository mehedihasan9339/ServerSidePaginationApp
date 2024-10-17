function initializeTable(tableId, url, columns) {
    $('.loader').show(); // Show loader during table initialization

    $('#' + tableId).DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": url,
            "type": "POST",
            "contentType": "application/json",
            "data": function (d) {
                // Construct the data object including search parameters
                return JSON.stringify({
                    draw: d.draw,
                    start: d.start,
                    length: d.length,
                    sortColumn: d.order.length > 0 ? d.columns[d.order[0].column].data : null,
                    sortDirection: d.order.length > 0 ? d.order[0].dir : null,
                    filterColumn: d.search.column >= 0 ? d.columns[d.search.column].data : null,
                    filterValue: d.search.value
                });
            },
            "complete": function () {
                $('.loader').hide(); // Hide loader when data is fetched
            }
        },
        "columns": columns,
        "paging": true,
        "ordering": true,
        "lengthChange": true,
        "info": true,
        "searching": true // Make sure searching is enabled
    });
}
