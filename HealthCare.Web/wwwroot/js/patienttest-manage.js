(function ($) {
    function PatientTest() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-patienttest").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new PatientTest();
        self.init();
    })
}(jQuery))