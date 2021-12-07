(function ($) {
    function ProductBrand() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-productbrand").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new ProductBrand();
        self.init();
    })
}(jQuery))