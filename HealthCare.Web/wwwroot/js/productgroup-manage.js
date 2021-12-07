(function ($) {
    function ProductGroup() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-productgroup").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new ProductGroup();
        self.init();
    })
}(jQuery))