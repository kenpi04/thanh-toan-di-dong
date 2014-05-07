var HomeViewModel = function () {
    this.phonenumber = ko.observable("");
    this.validPhone = ko.computed(function () {
        if (this.phonenumber != "" && /([0-9]+)/.test(this.phonenumber))
            this.phonenumber = "";
      
    }, this);
}
ko.applyBindings(new HomeViewModel());