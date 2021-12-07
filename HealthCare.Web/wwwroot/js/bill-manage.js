(function ($) {
    function Bill() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-bill").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Bill();
        self.init();
    })
}(jQuery))