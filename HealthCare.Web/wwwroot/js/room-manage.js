(function ($) {
    function Room() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-room").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new Room();
        self.init();
    })
}(jQuery))