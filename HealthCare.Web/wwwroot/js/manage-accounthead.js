(function ($) {
    function AddEdit() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-addedit-accounthead").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new AddEdit();
        self.init();
    })

    function Delete() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-delete-accounthead").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Delete();
        self.init();
    })
}(jQuery))