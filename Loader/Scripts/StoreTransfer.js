
$(document).on("change", "#TrQnty", function () {
    currentTr = $(this).closest('tr');
    var TQuantity = parseInt($(this).val());
    var RemQnty = parseInt($(this).closest("tr").find("#RemQnty").val());
    var ReqDetailId = $(this).closest("tr").find("#ReqDetailId").val();
    var ItemName = $(this).closest("tr").find("#ItemSelect option:selected").text();

    if (TQuantity >= 0) {
        //$(this).closest("tr").find(".StoreRemarksValdn").removeClass("hidden");
        if (TQuantity == 0) {
            $(this).closest("tr").find('#StoreRemarks').val("");
            $(this).closest("tr").find('#StoreRemarks').rules('add', {
                required: false  // set a new rule
            });
            $(this).closest("tr").find('#StoreRemarks').next("span").find("span").remove();
            var tableCount = $(this).closest('tr').next().has("table").length;
            if (tableCount != 0) {
                $(this).closest('tr').next().remove();
            }  
        }

       if (TQuantity <= RemQnty) {
        $('.ItemDetailMsg').text("Is serial or Mac Numbers required?");
        $('.ItemDetailConfirm').modal('show');
        $(".hiddenRDId").val(ReqDetailId);
        $(".hiddenTRId").val(TQuantity);
        $(".hiddenItemName").val(ItemName);
    }

    }

    if (TQuantity > RemQnty) {
        $('.ItemMsg').text("Transfer Quantity Can't be greater then Remainging Transfer Quantity.");
        $('.ItemOverflow').modal('show');
        $(this).val(0);
        var abc = currentTr.next().has("table").length;
        if (abc != 0) {
            currentTr.next().remove();
        }
        $(this).closest("tr").find('#StoreRemarks').rules('add', {
            required: false // set a new rule
        });
        $(this).closest("tr").find('#StoreRemarks').next("span").find("span").remove();
    }
});

$(document).on("click", "#DetailsBtnNo", function () {
    var abc = currentTr.next().has("table").length;
    if (abc != 0) {
        currentTr.next().remove();
    }
});


$(document).on("click", "#DetailBtnYes", function () {
    debugger;
    var numberOfRows = $("#TQnty").val();
    var ItemId = $("#ItemId").val();
   

    if (!ItemId) {
        ErrorAlert("Please Select Item First", 1000);

        return false;
    }

    var url = "/Transfer/GetItemDetailView";
    var data = { numberOfRows: numberOfRows, ItemId: ItemId };

   
    $.get(url, data, function (data) {
        debugger;
      
        var abc = currentTr.next().has("table").length;
        if (abc != 0) {
            currentTr.next().remove();
        }
        currentTr.after(data);
     
    });
});



