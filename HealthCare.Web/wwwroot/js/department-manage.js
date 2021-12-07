(function ($) {
    function DeleteDepartment() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-delete-department").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new DeleteDepartment();
        self.init();
    })

    function AddEditDepartment() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-addedit-department").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new AddEditDepartment();
        self.init();
    })
}(jQuery))