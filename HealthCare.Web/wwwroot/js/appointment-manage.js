(function ($) {
    function Appointment() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-appointment").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Appointment();
        self.init();
    })
}(jQuery))