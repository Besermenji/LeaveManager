$(document).ready(function () {

    
    //$(".clickableLink").click(function () {
    //    var tmp = $("#deliveryManagerComment").val();
    //    var id = $(this.id).val().split;
    //    var comm = tmp[1];
    //    alert(id);
    //    alert(comm);
       
    //    return false;
    //});
    $('.clickableLink').on('click', function () {
        var tmp = this.id.val().split(" ");
        var id = tmp[0];
        var comm = $("#deliveryManagerComment").val();
       
        //TODO ajax request koji apdejtuje tabelu i u povratku brise tr tabele
        return false;
    });





});