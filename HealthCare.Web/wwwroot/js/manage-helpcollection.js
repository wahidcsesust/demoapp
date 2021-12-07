(function ($) {
    function AddEditHelpCollection() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-addedit-helpcollection").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new AddEditHelpCollection();
        self.init();
    })

    function DeleteHelpCollection() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-delete-helpcollection").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new DeleteHelpCollection();
        self.init();
    })
}(jQuery))