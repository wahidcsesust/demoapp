(function ($) {
    function AddEditMembers() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-addedit-members").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new AddEditMembers();
        self.init();
    })

    function DeleteMembers() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-delete-members").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new DeleteMembers();
        self.init();
    })
}(jQuery))