(function ($) {
    function AddEditPatients() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-addedit-patients").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new AddEditPatients();
        self.init();
    })

    function DeletePatients() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-delete-patients").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new DeletePatients();
        self.init();
    })
}(jQuery))