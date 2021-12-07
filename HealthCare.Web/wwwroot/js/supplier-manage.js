(function ($) {
    function Supplier() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-supplier").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();            
        }     
    }
    $(function () {
        var self = new Supplier();
        self.init();
        
    })
}(jQuery))