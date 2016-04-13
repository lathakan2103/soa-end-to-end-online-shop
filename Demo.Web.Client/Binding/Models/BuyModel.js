(function (os) {
    var BuyModel = function () {

        var self = this;

        self.initialized = false;
        self.ArticleNumber = '';
        self.Name = '';
        self.Description = '';
        self.Price = '';
        self.Image = '';
        self.IsActive = true;
    }
    os.BuyModel = BuyModel;
}(window.Demo));