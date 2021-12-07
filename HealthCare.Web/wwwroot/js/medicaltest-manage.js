(function ($) {
    function DeleteMedicalTest() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-delete-medicaltest").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new DeleteMedicalTest();
        self.init();
    })

    function AddEditMedicalTest() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-addedit-medicaltest").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new AddEditMedicalTest();
        self.init();
    })
}(jQuery))