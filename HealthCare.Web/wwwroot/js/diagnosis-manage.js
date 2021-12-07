(function ($) {
    function Diagnosis() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-diagnosis").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Diagnosis();
        self.init();
    })
}(jQuery))