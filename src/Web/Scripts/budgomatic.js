/// <reference path="jquery-1.5.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.11.js" />

$(document).ready(function () {
    $(":input[data-datepicker]").not(".balanceDate").datepicker({
        dateFormat: 'yy-mm-dd'
        }
    );
})

$(document).ready(function () {
    $(":input[data-datepicker]").datepicker({
        dateFormat: 'yy-mm-dd',
        onSelect: function (dateText, inst) {
                $.post(
                "Home/Balances",
                {
                    date: dateText
                },
                function (data) {
                    $('#balances').html(data);
                },
                "html"
                )
            }
        }
    );
})
