(function (os) {
    var HistoryModel = function () {

        var self = this;

        self.CartId = '';
        self.Created = '';
        self.Approved = '';
        self.Canceled = '';
        self.Shipped = '';
        self.ShippingCost = '';
        self.Total = true;
    }
    os.HistoryModel = HistoryModel;
}(window.Demo));