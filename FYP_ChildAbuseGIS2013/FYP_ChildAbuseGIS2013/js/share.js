var resultTable;
var addressResult = false;

$(document).ready(function () {
    resultTable = $("#example").dataTable({
        "bProcessing": true,
        "sAjaxSource": "http://localhost:27020/api/json/file",
        "sAjaxDataProp": "File",
        "aoColumns": [
                                { "sTitle": "ID", "mData": "ID" },
                                { "sTitle": "Title", "mData": "title" },
                                { "sTitle": "Date", "mData": "date" },
                                { "sTitle": "Description", "mData": "description" },
                                { "sTitle": "Type", "mData": "type" }
                            ],
        "bPaginate": true,
        "bFilter": true,
        "bInfo": true,
        "bAutoWidth": true
    });

    $("#example tbody").delegate("tr", "click", function () {
        var iPos = resultTable.fnGetPosition(this);
        if (iPos != null) {
            window.location.href = "result.aspx?ID=" + iPos + "";
        }
    });
});

function skm_LockScreen(str) {
    var lock = document.getElementById('skm_LockPane');
    if (lock)
        lock.className = 'LockOn';

    lock.innerHTML = str + "<br/><img src=@'../../image/loading.gif' alt='loading'/>";
}

function initialize() {
    var countryRestrict = { 'country': 'sg' };
    autocomplete = new google.maps.places.Autocomplete(
    /** @type {HTMLInputElement} */(document.getElementById('autocomplete')),
                    {
                        types: ['geocode'],
                        componentRestrictions: countryRestrict
                    }
                );
    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        var place = autocomplete.getPlace();
        if (!place.geometry) {
            addressResult = false;
            return;
        }
        else {
            addressResult = true;
            $("#tbLocation").value = document.getElementById('autocomplete').value;
            $("#tbX").value = place.geometry.location.lat();
            $("#tbY").value = place.geometry.location.lng();
        }
    });
}