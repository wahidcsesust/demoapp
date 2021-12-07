
$(document).ready(function () {
    $('#ImageData').change(function () {       
        readURL(this);
    });

    var angle = 0,
        img = document.getElementById('container');
    document.getElementById('button').onclick = function () {
        angle = (angle + 90) % 360;
        img.className = "rotate" + angle;
    }

});// end of document ready

function readURL(input) {
    //alert('ok');
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#image').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }

    
}



