$(document).ready(function () {


    $('.clickableLink').on('click', function () {
        var tmp = this.id.split(" ");
        var id = tmp[0];
        var comm = $("#deliveryManagerComment").val();
        //json that we are sending to ajax
        var dataRequest = new Object();
        dataRequest.id = id;
        dataRequest.comm = comm;
       
        
        if (tmp[1] == "deliveryManagerApprove") {
            //ajax for approved action
            $.ajax({
                data: JSON.stringify(dataRequest),
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "DeliveryManagerAjax/Approved",
                success: function (data) {
                    alert(data);
                }
            });

            return false;
        }
        else {
            //ajax for denied action
            alert("other");
            return false;
        }
        
        
        return false;
    });





});