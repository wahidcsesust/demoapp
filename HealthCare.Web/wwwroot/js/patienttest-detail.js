
function MedicalTestChangeEvent(guid) {
    var medicalTestId = $("#PatientTestDetails_" + guid + "__MedicalTestId").val();
    $.ajax({
        cache: false,
        url: "/PatientTest/GetMedicalTestRate/" + medicalTestId,
        type: "GET",
        success: function (result) {
            if (result.isResultOk) {
                $("#PatientTestDetails_" + guid + "__TestRate").val(result.testRate);
                $("#PatientTestDetails_" + guid + "__Discount").val("0");
                $("#PatientTestDetails_" + guid + "__Amount").val(result.totalAmount);
            }
        }
    });
}

function DiscountChangeEvent(data) {
    var discount = data.value;
    var id = data.id.split("_");
    var guid = id[1];
    var medicalTestId = $("#PatientTestDetails_" + guid + "__MedicalTestId").val();
    $.ajax({
        cache: false,
        url: "/PatientTest/GetMedicalTestRateWithDiscount/" + medicalTestId + "?discount="+discount,
        type: "GET",
        success: function (result) {
            if (result.isResultOk) {
                $("#PatientTestDetails_" + guid + "__Amount").val(result.totalAmount);
            }
        }
    });
}

function LessAmountOnChangeEvent(data) {
    var lessAmount = data.value;
    var dueAmount = $("#DueAmountHide").val();
    if (parseInt(lessAmount) > parseInt(dueAmount)) {
        toastr.warning("Warning : Less amount can not be greater than due amount!");
        $("#LessAmount").val("0");
        $("#DueAmount").val(dueAmount);
        return;
    }
    $("#DueAmount").val(dueAmount - lessAmount);
}

function PaidAmountOnChangeEvent(data) {
    var paidAmount = data.value;
    var dueAmount = $("#DueAmount").val();
    if (parseInt(paidAmount) > parseInt(dueAmount)) {
        toastr.warning("Warning : Paid amount can not be greater than due amount!");
        $("#PaidAmount").val("0");
        return;
    }
}

var DeletePatientTestDetail = function (row, parentId, childId) {
    bootbox.confirm({
        message: "Are you sure?",
        className: 'rubberBand animated',
        size: 'medium',
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    cache: false,
                    url: "/PatientTest/DeletePatientTestItem?parentId=" + parentId + "&childId=" + childId,
                    type: "GET",
                    success: function (result) {
                        if (result.isResultOk) {
                            $(row).remove();
                            toastr.success("Notification : Test item delete successfully");
                            location.reload();
                        } else {
                            toastr.warning(result.message);
                            return;
                        }
                    }
                });
            }
        }
    });
}