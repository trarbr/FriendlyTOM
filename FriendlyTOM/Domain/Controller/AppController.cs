using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Controller
{
    public class AppController
    {

        public static AppController Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new AppController();
                }

                return _instance; 
            }
        }
        private static AppController _instance;

        public CustomerController CustomerController
        {
            get { return _customerController; }

        }
        public SupplierController SupplierController
        {
            get { return _supplierController; }
        }
        public PaymentController PaymentController
        {
            get { return _paymentController; }
        }
        public BookingController BookingController
        {
            get { return _bookingController; }
        }
        public SettingsController SettingsController
        {
            get { return _settingsController; }
        }
        private CustomerController _customerController;
        private SupplierController _supplierController;
        private PaymentController _paymentController;
        private BookingController _bookingController;
        private SettingsController _settingsController;

        private AppController()
        {
        }

        public void Initialize()
        {
            try
            {
                var dataAccessFacade = DataAccess.DataAccessFacade.Instance;

                _settingsController = new SettingsController(dataAccessFacade);
                _settingsController.FirstRunSetup();

                _customerController = new CustomerController(dataAccessFacade);
                _supplierController = new SupplierController(dataAccessFacade);
                _paymentController = new PaymentController(dataAccessFacade);
                _bookingController = new BookingController(dataAccessFacade, 
                    _paymentController, _customerController);

                _customerController.bookingController = _bookingController;
                _customerController.paymentController = _paymentController;
                _supplierController.bookingController = _bookingController;
                _supplierController.paymentController = _paymentController;
            }
            catch
            {
                throw;
            }
        }
    }
}
