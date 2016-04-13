using Core.Common.UI.Core;
using System.Collections.Generic;
using Demo.Client.Contracts;
using GalaSoft.MvvmLight.Messaging;
using Demo.Admin.Messages;
using System.ComponentModel.Composition;
using Demo.Client.Entities;
using Core.Common.Core;

namespace Demo.Admin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class EditProductDialogViewModel : ViewModelBase
    {
        #region Fields

        private bool _isOpen;
        private string _title;
        private Product _model;
        private readonly IInventoryService _inventoryService;
        private string _productName;
        private string _productDescription;
        private decimal _productPrice;
        private Product _currentModel;
        private bool _productIsActive;
        private int _productId;
        private string _productArticleNumber;

        #endregion

        #region Properties

        public bool IsOpen
        {
            get { return this._isOpen; }
            set
            {
                this._isOpen = value;
                OnPropertyChanged(() => this.IsOpen);
            }
        }

        public string Title
        {
            get { return this._title; }
            set
            {
                this._title = value;
                OnPropertyChanged(() => this.Title);
            }
        }

        public Product Model
        {
            get { return this._model; }
            set
            {
                if (this._model == value) return;
                this._model = value;
                OnPropertyChanged(() => this.Model);
                this.SetCurrentProduct(this._model);
            }
        }

        public Product CurrentProduct
        {
            get { return this._currentModel; }
            set
            {
                if (this._currentModel == value) return;
                this._currentModel = value;
                OnPropertyChanged(() => this.CurrentProduct);
            }
        }

        #endregion

        #region CurrentProduct

        //public int ProductId
        //{
        //    get { return this._productId; }
        //    set
        //    {
        //        if (this._productId == value) return;
        //        this._productId = value;
        //        OnPropertyChanged(() => this.ProductId);
        //    }
        //}

        //public string ProductArticleNumber
        //{
        //    get { return this._productArticleNumber; }
        //    set
        //    {
        //        if (this._productArticleNumber == value) return;
        //        this._productArticleNumber = value;
        //        OnPropertyChanged(() => this.ProductArticleNumber);
        //    }
        //}

        //public string ProductName
        //{
        //    get { return this._productName; }
        //    set
        //    {
        //        if (this._productName == value) return;
        //        this._productName = value;
        //        this.CurrentProduct.Name = value;
        //        OnPropertyChanged(() => this.ProductName);
        //    }
        //}

        //public string ProductDescription
        //{
        //    get { return this._productDescription; }
        //    set
        //    {
        //        if (this._productDescription == value) return;
        //        this._productDescription = value;
        //        this.CurrentProduct.Description = value;
        //        OnPropertyChanged(() => this.ProductDescription);
        //    }
        //}

        //public decimal ProductPrice
        //{
        //    get { return this._productPrice; }
        //    set
        //    {
        //        if (this._productPrice == value) return;
        //        this._productPrice = value;
        //        this.CurrentProduct.Price = value;
        //        OnPropertyChanged(() => this.ProductPrice);
        //    }
        //}

        //public bool ProductIsActive
        //{
        //    get { return this._productIsActive; }
        //    set
        //    {
        //        if (this._productIsActive == value) return;
        //        this._productIsActive = value;
        //        this.CurrentProduct.IsActive = value;
        //        OnPropertyChanged(() => this.ProductIsActive);
        //    }
        //}

        #endregion

        #region Commands

        public DelegateCommand<object> OkCommand { get; private set; }
        public DelegateCommand<object> SaveCommand { get; private set; }
        public DelegateCommand<object> CancelCommand { get; private set; }

        #endregion

        #region C-Tor

        [ImportingConstructor]
        public EditProductDialogViewModel(IInventoryService inventoryService)
        {
            this._inventoryService = inventoryService;
            this.RegisterCommands();
            this.CurrentProduct = null;
        }

        #endregion

        #region Overrides

        protected override void AddModels(List<ObjectBase> models)
        {
            models.Add(this.CurrentProduct);
        }

        #endregion

        #region Methods

        private void RegisterCommands()
        {
            this.OkCommand = new DelegateCommand<object>(OnOkCommand);
            this.SaveCommand = new DelegateCommand<object>(OnSaveCommand);
            this.CancelCommand = new DelegateCommand<object>(OnCancelCommand);
        }

        private void Close()
        {
            this.IsOpen = false;
        }

        private void SetCurrentProduct(Product product)
        {
            this.CurrentProduct = new Product();

            this.CurrentProduct.ProductId       = product.ProductId;
            this.CurrentProduct.ArticleNumber   = product.ArticleNumber;
            this.CurrentProduct.Name            = product.Name;
            this.CurrentProduct.Description     = product.Description;
            this.CurrentProduct.IsActive        = product.IsActive;
            this.CurrentProduct.Price           = product.Price;
        }

        #endregion

        #region On...Command

        private void OnOkCommand(object obj)
        {
            this.Close();
        }

        private void OnCancelCommand(object obj)
        {
            this.CurrentProduct = null;
            this.OkCommand.Execute(null);
        }

        private void OnSaveCommand(object obj)
        {
            ValidateModel();

            if (!IsValid) return;

            var p = this._inventoryService.GetProductById(this.CurrentProduct.ProductId, true) ?? new Product();

            p.Name = this.CurrentProduct.Name;
            p.Description = this.CurrentProduct.Description;
            p.Price = this.CurrentProduct.Price;
            p.IsActive = this.CurrentProduct.IsActive;

            var result = this._inventoryService.UpdateProduct(p);
            Messenger.Default.Send(new ProductChangedMessage());
            this.OkCommand.Execute(null);
        }

        #endregion        
    }
}
