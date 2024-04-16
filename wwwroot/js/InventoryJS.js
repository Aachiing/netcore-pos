$(document).ready(function () {
    ActiveMenu("liInventory");
    $("#tbl-inventory-list").load("inventory-list");
    $("#btn-save").click(function () {

        var obj = {
            product_name: $("#product_name").val(),
            product_code: $("#product_code").val(),
            barcode: $("#barcode").val(),
            brand: $("#brand").val(),
            quantity: $("#quantity").val(),
            unit: $("#unit").val(),
            price: $("#price").val(),
            discount_rate: $("#discount_rate").val(),
            is_discounted: $("#chkIsDiscounted").is(":checked")
        }

        $.ajax({
            url: "create",
            type: "POST",
            data: { dto: obj },
            success: function (response) {
                alert(response);
                window.location.reload();
            },
            failure: function (response) {
                alert(response);
            }

        });
    });

    $("#btn-update").click(function () {

        var obj = {
            product_id: $("#frmUpdateProduct #product_id").val(),
            product_name: $("#frmUpdateProduct #product_name").val(),
            product_code: $("#frmUpdateProduct #product_code").val(),
            barcode: $("#frmUpdateProduct #barcode").val(),
            brand: $("#frmUpdateProduct #brand").val(),
            quantity: $("#frmUpdateProduct #quantity").val(),
            unit: $("#frmUpdateProduct #unit").val(),
            price: $("#frmUpdateProduct #price").val(),
        }

        $.ajax({
            url: "update",
            type: "POST",
            data: { dto: obj },
            async: true,
            success: function (response) {
                alert(response);
                window.location.reload();
            },
            failure: function (response) {
                alert(response);
            }

        });
    });

    $("#txt-search").on("keyup", function () {
        $("#tbl-inventory-list").load("inventory-list?keyword=" + $(this).val())
    })
});

function GetById(id) {
    $.ajax({
        url: "get-by-id/" + id,
        type: "GET",
        success: function (response) {
            ClearFields("frmUpdateProduct", response);
            $("#mdlUpdateProduct").modal("show");
        },
        failure: function (response) {
            alert(response);
        }
    });
}
function PaginationClick(pageNo) {
    $("#tbl-inventory-list").load("inventory-list?page=" + pageNo)
}
