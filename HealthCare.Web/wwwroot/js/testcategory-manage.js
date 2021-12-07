(function ($) {
    function DeleteTestCategory() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-delete-testcategory").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new DeleteTestCategory();
        self.init();
    })

    function AddEditTestCategory() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-addedit-testcategory").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new AddEditTestCategory();
        self.init();
    })
}(jQuery))