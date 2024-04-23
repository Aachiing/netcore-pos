$(document).ready(function () {
    $('#txtSearchItem').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "dropdown-list-product/" + $("#txtSearchItem").val(),
                type: "GET",
                success: function (data) {
                    response(JSON.parse(data))
                },
                failure: function (response) {
                    alert(response)
                }
            })

        },
        select: function (event, ui) {
            debugger
            AddItem(ui.item.product_id);
            setTimeout(function () {
                $('#txtSearchItem').val("");
            }, 100);
        }
    }).autocomplete('instance')._renderItem = function (ul, item) {
        return $('<li>')
            .append(`<div class="row li-product">
                        <span class="li-product-description">${item.product_name}</span><br />
                        <span class="li-product-details"> Code: ${item.product_code} || Available: ${item.quantity} ||  Price: Php ${item.price.toFixed(2)}</span> <br />
                    </div>`)
            .appendTo(ul)
    };

    shortcut.add("F1", function () {
        $('#txtSearchItem').focus();
    });
    shortcut.add("F2", function () {
        $("#btn-qty").trigger('click');
    });
    shortcut.add("F3", function () {
        $("#btn-void").trigger('click');
    });
    shortcut.add("F4", function () {
        $("#btn-cash").trigger('click');
    });
    shortcut.add("F5", function () {
        $("#btn-credit").trigger('click');
    });
    shortcut.add("F6", function () {
        $("#btn-discount").trigger('click');
    });

    $("#btn-qty").on('click', function () {
        var activeRow = $("#tblOrders").find("tr.active");
        if (activeRow.length > 0) {
            var itemName = activeRow.find("td:eq(1)").text();

            $("#txtItemName").val(itemName);
            $("#mdlItemQty").modal("show");
            $('#mdlItemQty').on('shown.bs.modal', function () {
                setTimeout(function () {
                    $('#txtQty').focus();
                }, 100);
            })
        }
        else {
            alert("Please select an item!")
        }
    });
    $("#btn-void").on('click', function () {
        var activeRow = $("#tblOrders").find("tr.active");

        if (activeRow.length > 0) {
            $("#mdlVoidItem").modal("show");
            $('#mdlVoidItem').on('shown.bs.modal', function () {
                setTimeout(function () {
                    $('#txtPassword').focus();
                }, 100);
            })
        }
        else {
            alert("Please select an item!")
        }
    });
    $("#btn-cash").on('click', function () {
        if (ValidOrder()) {
            $("#txtPaymentType").val("CASH");
            $("#divCheckPayment,#divCardPayment,#divddlCustomerName").addClass("d-none");
            $("#divTRA").addClass("d-none");
            $("#divCreditPaymentType,#divtxtCustomerName").removeClass("d-none");
            $("#txtTransactionType").val("");
            $("#mdlPaymentDetails").modal("show");

            $('#mdlPaymentDetails').on('shown.bs.modal', function () {
                setTimeout(function () {
                    $('#txtCustomerName').focus();
                }, 100);
            })
        }
    });
    $("#btn-credit").on('click', function () {
        if (ValidOrder()) {
            $("#txtTransactionType").val("CREDIT");
            $("#divCreditPaymentType,#divddlCustomerName").removeClass("d-none");
            $("#divTRA").removeClass("d-none");
            $("#divCheckPayment,#divCardPayment,#divtxtCustomerName").addClass("d-none");
            $("#mdlPaymentDetails").modal("show");
        }
    });
    $("#btn-discount").on('click', function () {
        if (ValidOrder()) {
            $("#mdlDiscount").modal("show");
        }
    });

    $("#ddlPaymentType").on('change', function () {
        var paymentType = $(this).val();

        $("#txtPaymentType").val(paymentType)

        if (paymentType == "CASH") {
            $("#divCheckPayment,#divCardPayment").addClass("d-none");
        }
        else if (paymentType == "CARD") {
            $("#divCardPayment").removeClass("d-none");
            $("#divCheckPayment").addClass("d-none");
        }
        else if (paymentType == "CHECK") {
            $("#divCheckPayment").removeClass("d-none");
            $("#divCardPayment").addClass("d-none");
        }
    });

    $("#mdlDiscount .btn-success").on('click', function () {
        AddDiscount();
    });
    $("#mdlDiscount").enterKey(function () {
        $("#mdlDiscount .btn-success").trigger("click");
    })
    $("#mdlItemQty .btn-success").on('click', function () {
        AddItemQuantity();
    });
    $("#mdlItemQty").enterKey(function () {
        $("#mdlItemQty .btn-success").trigger("click");
    })
    $("#mdlVoidItem .btn-success").on('click', function () {
        VoidItem($("#txtPassword").val())
    });
    $("#mdlVoidItem").enterKey(function () {
        $("#mdlVoidItem .btn-success").trigger("click");
    })
    $("#mdlPaymentDetails").enterKey(function () {
        $("#mdlPaymentDetails .btn-secondary").trigger("click")
    })
    $("#mdlPaymentDetails .btn-secondary").on('click', function () {
        PostPayment();
    });
    $("#mdlItemQty .btn-success").on('click', function () {
        AddItemQuantity();
    });
})

function SearchProduct() {
    $("#mdlSearchProduct").modal("show");

    $('#mdlSearchProduct').on('shown.bs.modal', function () {
        setTimeout(function () {
            $('#txtSearch').focus();
        }, 100);
    })
}
function AddItem(id) {
    $.ajax({
        type: "GET",
        url: "../../inventory/get-by-id/" + id,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            AppendSelectedItem(response);
            ComputeTotalAmount();
        },
        failure: function (response) {
            alert(response);
        }
    });

}
function AppendSelectedItem(obj) {
    $("#tblOrders").parent().find("tr").removeClass("active");
    $('#tblOrders > tbody').append('<tr class="active"><td class="d-none">' + obj.product_id + '</td><td>' + obj.product_name + '</td><td>' + obj.product_code + '</td><td>' + obj.barcode + '</td><td>' + obj.price + '</td><td>1</td><td>' + obj.price + '</td></tr>');
    $("#mdlSearchProduct").modal("hide");
}
function ComputeTotalAmount() {
    var amount = 0;
    var itemCount = 0;
    var vat = 1.12;
    var discount = $("#txtDiscount").val();

    $('#tblOrders > tbody > tr').each(function () {
        amount += parseFloat($(this).find("td:eq(6)").text());
        itemCount += parseInt($(this).find("td:eq(5)").text());
    })

    $("#spnItemCount").text(itemCount);
    $("#spnSubTotal").text((amount).toFixed(2));
    $("#spnVat").text(((amount / vat) * 0.12).toFixed(2));
    $("#spnTotal").text(parseFloat(amount).toFixed(2));
}
function AddItemQuantity() {

    var activeRow = $("#tblOrders").find("tr.active");

    var qty = parseInt($('#txtQty').val());
    var price = parseFloat(activeRow.find("td:eq(4)").text());

    if (!isNaN(qty) && qty > 0) {
        activeRow.find("td:eq(5)").text(qty);
        activeRow.find("td:eq(6)").text(qty * price);

        $("#mdlItemQty").modal("hide");
        $('#txtQty').val("");
        ComputeTotalAmount();
    }
    else {
        alert("Please enter quantity");
    }
}
function VoidItem(password) {
    var activeRow = $("#tblOrders").find("tr.active");

    var itemID = activeRow.find("td:eq(0)").text();
    var qty = parseInt(activeRow.find("td:eq(5)").text());

    if (activeRow.length > 0) {
        var password = $("#txtPassword").val();

        if (password != "admin") {
            alert("Unauthorize to void item.")
            return false;
        }

        $("#mdlVoidItem").modal("hide");
        activeRow.remove();
        ComputeTotalAmount();
    }
    else {
        alert("Please select an item!");
    }

}
function GetPaymentDetails() {

    var obj_order_details = [];

    $('#tblOrders > tbody > tr').each(function () {

        var obj = {
            product_id: $(this).find("td:eq(0)").text(),
            item_name: $(this).find("td:eq(1)").text(),
            barcode: $(this).find("td:eq(3)").text(),
            quantity: parseInt($(this).find("td:eq(5)").text()),
            unit_price: parseFloat($(this).find("td:eq(4)").text()),
            total_amount: parseFloat($(this).find("td:eq(6)").text())
        }

        obj_order_details.push(obj);
    })

    var obj_order_header = {
        customer_name: $("#txtTransactionType").val() == "CREDIT" ? $("#ddlCustomerName").val() : $("#txtCustomerName").val(),
        payment_type: $("#txtPaymentType").val(),
        card_no: $("#txtCardNo").val(),
        reference_no: $("#txtReferenceNo").val(),
        check_no: $("#txtCheckNo").val(),
        check_amount: $("#txtCheckAmt").val(),
        check_date: $("#dtCheckDate").val(),
        amount_paid: $("#txtAmountPaid").val(),
        discount: $("#txtDiscount").val(),
        remarks: $("#txtRemarks").val(),
        transaction_type: $("#txtTransactionType").val(),
        order_no: $("#txtTransactionType").val() == "CREDIT" ? $("#txtTRANo").val() : $("#txtORNo").val(),
        order_details: obj_order_details,
    }

    return obj_order_header;
}
function ValidatePaymentDetails(payment_type,transaction_type) {
    var invalid_input = 0;

    if (payment_type == "CARD" && transaction_type != "CREDIT") {

        if ($("#txtCustomerName").val() == '') {
            $("#txtCustomerName").css("border", "1px solid red");
            invalid_input++;
        }

        if ($("#txtCardNo").val() == '') {
            $("#txtCardNo").css("border", "1px solid red");
            invalid_input++;
        }

        if ($("#txtReferenceNo").val() == '') {
            $("#txtReferenceNo").css("border", "1px solid red");
            invalid_input++;
        }

        if ($("#txtAmountPaid").val() == '') {
            $("#txtAmountPaid").css("border", "1px solid red");
            invalid_input++;
        }

        return invalid_input;
    }
    else if (payment_type == "CHECK" && transaction_type != "CREDIT") {

        if ($("#txtCustomerName").val() == '') {
            $("#txtCustomerName").css("border", "1px solid red");
            invalid_input++;
        }

        if ($("#txtCheckNo").val() == '') {
            $("#txtCheckNo").css("border", "1px solid red");
            invalid_input++;
        }

        if ($("#txtCheckAmt").val() == '') {
            $("#txtCheckAmt").css("border", "1px solid red");
            invalid_input++;
        }

        if ($("#dtCheckDate").val() == '') {
            $("#dtCheckDate").css("border", "1px solid red");
            invalid_input++;
        }

        if ($("#txtAmountPaid").val() == '') {
            $("#txtAmountPaid").css("border", "1px solid red");
            invalid_input++;
        }

        return invalid_input;
    }
    else if (payment_type == "CASH" && transaction_type != "CREDIT") {

        if ($("#txtCustomerName").val() == '') {
            $("#txtCustomerName").css("border", "1px solid red");
            invalid_input++;
        }

        if ($("#txtAmountPaid").val() == '') {
            $("#txtAmountPaid").css("border", "1px solid red");
            invalid_input++;
        }
        return invalid_input;
    }
    return invalid_input;
}
function PostPayment() {
    var count = ValidatePaymentDetails($("#txtPaymentType").val(), $("#txtTransactionType").val());

    if (count == 0) {
        $.ajax({
            url: "pay",
            type: "POST",
            data: { dto: GetPaymentDetails() },
            success: function (response) {
                alert(response.msg);

                if (response.status == 200)
                    window.location.reload();
            },
            failure: function (response) {
                alert(response);
            }

        });
    }
    else {
        alert("Please complete payment details")
    }
}
function ValidOrder() {
    var activeRow = $("#tblOrders").find("tr.active");

    if (activeRow.length == 0) {
        alert("Please select an item!");
        return false;
    }

    return true;
}
function AddDiscount() {
    ComputeTotalAmount();

    var discount = parseFloat($("#txtDiscount").val());
    var total = parseFloat($("#spnTotal").text());

    $("#spnDiscount").text(discount.toFixed(2));
    $("#spnTotal").text((total - discount).toFixed(2));

    $("#mdlDiscount").modal("hide");
}