(function ($) {
    function Doctor() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-doctor").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Doctor();
        self.init();
    })
}(jQuery))